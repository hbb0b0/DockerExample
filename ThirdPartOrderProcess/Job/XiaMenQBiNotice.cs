using System;
using System.Collections.Generic;
using ThirdPartOrderProcess.Entity;
using ThirdPartOrderProcess.Model;
using ThirdPartOrderProcess.DB;
using System.Linq;
using Newtonsoft.Json;
using System.Reflection;
using ThirdPartOrderProcess.Common;
using Microsoft.Extensions.Logging;

namespace ThirdPartOrderProcess.Job
{
    public class XiaMenQBiNotice
    {
        private const string TemplateName = "XiaMengQBi";
        private OrderTemplateModel CurrentOrderTemplateModel = null;
        private readonly List<string> m_ParamKeyList = new List<string>();
        private readonly List<string> m_SignKeyList = new List<string>();
        private readonly string m_signKeyName;
        //private  List<KeyValuePair<string,string>> m_Raw_KeyValuePair;

        private ILoggerFactory m_logFactory;
        private ILogger m_logger;

        HttpTool m_httpTool;
        public XiaMenQBiNotice()
        {
            m_logFactory = new LoggerFactory();
            m_logFactory.AddConsole();
            m_logger = m_logFactory.CreateLogger<XiaMenQBiNotice>();
            m_httpTool = new HttpTool(m_logger);
            CurrentOrderTemplateModel = DBHelper.GetOrderTemplateList().Where(p => p.TemplateName == TemplateName).First();
            String noticeParamn = CurrentOrderTemplateModel.NoticeParam.Replace(CurrentOrderTemplateModel.NoticeSign, "");
            String[] paramArray = noticeParamn.Split('&');
            foreach (var item in paramArray)
            {
                string[] paramPair = item.Split('=');
                m_ParamKeyList.Add(paramPair[0]);
            }

            string[] signPair = CurrentOrderTemplateModel.NoticeSign
            .Replace("&", "")
            .Replace("[", "")
            .Replace("]", "")
            .Split('=');
            if (signPair != null && signPair.Length == 2)
            {
                m_signKeyName = signPair[0];
                string[] signArray = signPair[1].Split('+');
                foreach (var item in signArray)
                {
                    m_SignKeyList.Add(item);
                }
            }


        }


        private List<KeyValuePair<string, string>> GetKeyValuePairList(OrderModel model)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> signList = new List<KeyValuePair<string, string>>();
            XiaMengQBiOrder entity = JsonConvert.DeserializeObject<XiaMengQBiOrder>(model.JSON);

            string value;
            foreach (var key in m_ParamKeyList)
            {
                PropertyInfo p = typeof(XiaMengQBiOrder).GetProperty(key);
                if (p != null)
                {
                    value = p.GetValue(entity)?.ToString();
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(key, value);
                    list.Add(pair);

                }

            }
            //sign
            if (!string.IsNullOrEmpty(m_signKeyName))
            {
                string signString = string.Empty;
                foreach (var key in m_SignKeyList)
                {
                    KeyValuePair<string, string> pair = list.Where(m => m.Key == key).FirstOrDefault();

                    if (!String.IsNullOrEmpty(pair.Key))
                    {
                        signString += pair.Value;
                    }
                }
                //Md5(mchCode + mchProductCode + nonceStr + status+key)

                if (!string.IsNullOrEmpty(CurrentOrderTemplateModel.SecretKey))
                {
                    signString += CurrentOrderTemplateModel.SecretKey;
                }
                Console.WriteLine($"prepare post: orderNum:{model.OrderNum} time:{DateTime.Now.ToString()} signRawValue:{signString}");
                string signValue = MD51.GetMd5Hash(signString);
                KeyValuePair<string, string> p1 = new KeyValuePair<string, string>(m_signKeyName, signValue);
                list.Add(p1);
            }
            return list;
        }

        // This method is called by the timer delegate.
        public async void Notice(Object stateInfo)
        {
            //GetWorkList

            List<OrderModel> list = DBHelper.GetWorkList();
            //string strMsg = string.Format($"Time:{DateTime.Now} XiaMenQBiNotice:Count:{list.Count}");

            foreach (OrderModel orderItem in list)
            {
                m_logger.LogInformation(string.Format($"Notice start:HttpPostAsync:{CurrentOrderTemplateModel.NoticeURI}+{CurrentOrderTemplateModel.NoticeURL}"));
                var result = await m_httpTool.HttpPostAsync(
                      CurrentOrderTemplateModel.NoticeURI,
                      CurrentOrderTemplateModel.NoticeURL,
                      GetKeyValuePairList(orderItem)
                    );
                DBHelper.UpdateOrderNoticeInfo(orderItem.ID, result);
                //Console.WriteLine($"Notice:HttpPostAsync:{result}");
                m_logger.LogInformation($"Notice end:HttpPostAsync:{result}");

            }


        }
    }
}