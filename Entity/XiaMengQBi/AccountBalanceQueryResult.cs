

using ThirdPartOrderProcess.OrderEntity;

namespace ThirdPartOrderProcess.Entity
{
    //余额查询返回实体
    public class AccountBalanceQueryResult 
    {
        ///商户编号
        public string mchCode { get; set; }

        ///产品编号
        public string mchProductCode { get; set; }

        //商家流水号
        public string result { get; set; }
        
        //余额
        public double balanceFee
        {
            get;
            set;
        }

        //平台流水号
        public string msg { get; set; }
       
       //签名
       public string sign { get; set; }

       //随机字符串
        public string nonceStr { get; set; }


    }
}