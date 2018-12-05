

using ThirdPartOrderProcess.OrderEntity;

namespace ThirdPartOrderProcess.Entity
{
    //余额查询实体
    public class AccountBalanceQuery 
    {
        ///商户编号
        public string mchCode { get; set; }

        ///产品编号
        public string mchProductCode { get; set; }
       
       //签名
       public string sign { get; set; }

       //随机字符串
        public string nonceStr { get; set; }


    }
}