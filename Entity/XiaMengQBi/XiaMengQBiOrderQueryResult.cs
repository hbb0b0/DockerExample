
using ThirdPartOrderProcess.OrderEntity;

namespace ThirdPartOrderProcess.Entity
{
    //查询返回
    public class XiaMengQBiOrderQueryResult 
    {
        //0：成功返回；其他失败
        public string result { get; set; }
        //结果描述
        public string msg { get; set; }

        //商家流水号
        public string outOrderId { get; set; }

        //平台流水号
        public string transId { get; set; }

        //充值账户
        public string userInfo { get; set; }

        //充值结果
        public string rechargeResult { get; set; }

        //充值数量
        public string rechargeNum { get; set; }

        //随机字符串
        public string nonceStr { get; set; }
    
        //Md5(mchCode + mchProductCode + userOrderId + transId + userInfo + rechargeNum + rechargeResult
		//		+ nonceStr)
        public string sign{
            get;
            set;
        }

            


    }
}