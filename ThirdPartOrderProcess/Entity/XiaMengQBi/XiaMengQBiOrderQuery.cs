
using ThirdPartOrderProcess.OrderEntity;

namespace ThirdPartOrderProcess.Entity
{
    //订单查询实体
    public class XiaMengQBiOrderQuery 
    {
        ///商户编号
        public string mchCode { get; set; }

        ///产品编号
        public string mchProductCode { get; set; }

        //商家流水号
        public string outOrderId { get; set; }

        //平台流水号
        public string transId { get; set; }
       
       //签名
       public string sign { get; set; }

       //随机字符串
        public string nonceStr { get; set; }


    }
}