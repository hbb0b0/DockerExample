using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ThirdPartOrderProcess.Entity.XiaMengQBi;
using ThirdPartOrderProcess.DB;
using ThirdPartOrderProcess.Entity;
using ThirdPartOrderProcess.Model;
using ThirdPartOrderProcess.Common;
using System.Collections.Concurrent;
namespace ThirdPartOrderProcess.Controllers
{
    //厦门Q币
    [Route("api/[controller]")]
    public class XiaMenQBiController : BaseController
    {

        private static ConcurrentQueue<int> m_OrderStatusQueue = new ConcurrentQueue<int>();
        private static object m_sync_object = new object();
        private int GetOrderStaus()
        {
            int MAXNUMBER = 10;
            int result = 0;
            int currentFailCount = 0;
            int maxFailCount = (int)(MAXNUMBER * CurrentOrderTemplateModel.SubmitFailRate);
            if (!m_OrderStatusQueue.TryPeek(out result))
            {
                lock (m_sync_object)
                {
                    Random rand = new Random((int)DateTime.Now.Ticks);
                    List<int> currentList=new List<int>();
                    for (int i = 0; i < MAXNUMBER; i++)
                    {
                        int currentOrderStatus = rand.Next(2);
                        if (currentOrderStatus == 0)
                        {
                            currentFailCount++;
                        }
                        
                        if (currentFailCount > maxFailCount)
                        {
                            currentOrderStatus=1;
                        }
                        currentList.Add(currentOrderStatus);
                    }
                    int sumFail = currentList.Where(p=>p==0).Count();
                    if(currentFailCount<sumFail)
                    {
                        int needFailCount= sumFail-currentFailCount;
                        int updateCounte=0;
                        for(int i=0;i<currentList.Count;i++)
                        {

                            if(currentList[i]==1)
                            {
                                if(updateCounte<needFailCount)
                                {
                                  currentList[i]=0;
                                  updateCounte++;
                                }
                                else
                                {
                                    break;
                                }

                            }
                        }
                        
                    }
                    foreach(var item in currentList)
                    {
                        m_OrderStatusQueue.Enqueue(item);
                    }
                }
            }
            m_OrderStatusQueue.TryDequeue(out result);

            return result;
        }
        public XiaMenQBiController(ILogger<XiaMenQBiController> logger) : base(logger)
        {
            m_Logger=logger;
            TemplateName = "XiaMengQBi";
            CurrentOrderTemplateModel = OrderTemplateList.Where(p => p.TemplateName == TemplateName).First();
        }

        //Q币充值接口 
        [Route("[action]")]
        [HttpPost]
        public RequestOK jRecharge([FromBody]XiaMengQBiOrder value)
        {

            OrderModel model = new OrderModel();
            int orderStatus= GetOrderStaus();
            //int orderStatus=0;
            value.status =  orderStatus.ToString()=="1"?"01":"00";
            //value.status="00";
            String str = JsonConvert.SerializeObject(value);
            model.JSON = str;
            model.OrderNum = value.outOrderId;
            model.OrderTemplateID = CurrentOrderTemplateModel.ID;
            model.OrderStatus = orderStatus;
            int result = DBHelper.Insert(model);
            //m_Logger.LogInformation(str);
            RequestOK requestResult=new RequestOK(0,result.ToString(),"OK");
            //return CurrentOrderTemplateModel.SubmitSuccess;
            return requestResult;
        }

        //订单查询接口 
        [Route("[action]")]
        [HttpPost]
        public XiaMengQBiOrderQueryResult orderQuery([FromBody]XiaMengQBiOrderQuery query)
        {
            XiaMengQBiOrderQueryResult result = null;
            if (query == null || string.IsNullOrEmpty(query.transId))
            {
                return null;
            }

            OrderModel model = DBHelper.GetOrderByID(query.transId);
            if (model != null)
            {
                XiaMengQBiOrder orderEntity = JsonConvert.DeserializeObject<XiaMengQBiOrder>(model.JSON);
                result = new XiaMengQBiOrderQueryResult();
                result.msg = model.OrderStatus.ToString();
                result.nonceStr = Guid.NewGuid().ToString().ToLower().Replace("-", "");
                result.outOrderId = orderEntity.outOrderId;
                result.rechargeNum = orderEntity.goodsNum;
                //充值结果 0计费失败 1计费成功 2未回报
                if (model.Status == 0)
                {
                    result.rechargeResult = "2";
                }
                else
                {
                    result.rechargeResult = model.OrderStatus.ToString();
                }
                result.userInfo = orderEntity.userName;
                result.result = "0";
                result.transId = model.ID.ToString();

                string strSign = orderEntity.mchCode + orderEntity.mchProductCode
                + orderEntity.outOrderId + model.ID.ToString() + orderEntity.userName
                 + result.rechargeNum + result.rechargeResult + result.nonceStr;

                result.sign = MD51.GetMd5Hash(strSign);

            }

            string str=  JsonConvert.SerializeObject(result);
             m_Logger.LogDebug(str);
            
            return result;
        }


        //余额查询接口
        [Route("[action]")]
        [HttpPost]
        public AccountBalanceQueryResult accountBalance([FromBody]AccountBalanceQuery query)
        {
            AccountBalanceQueryResult result = new AccountBalanceQueryResult();
            result.balanceFee = 1000d;
            result.mchCode = query.mchCode;
            result.mchProductCode = query.mchProductCode;
            result.msg = "0";
            result.nonceStr = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            //Md5(mchCode + mchProductCode + nonceStr + status)
            string strSign = query.mchCode + query.mchProductCode + result.nonceStr;
            result.sign = MD51.GetMd5Hash(strSign);
            return result;
        }


    }
}
