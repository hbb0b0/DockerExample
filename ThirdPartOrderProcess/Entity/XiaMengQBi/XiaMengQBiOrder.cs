
using ThirdPartOrderProcess.OrderEntity;

namespace ThirdPartOrderProcess.Entity
{
    //订单实体
    public class XiaMengQBiOrder : BaseOrder
    {
        public string mchCode { get; set; }
        public string mchProductCode { get; set; }
        public string outOrderId { get; set; }
        public string orderIp { get; set; }
        public string userName { get; set; }
        public string gameName { get; set; }
        public string goodsNum { get; set; }
        public string nonceStr { get; set; }

        public string status{get;set;}


    }
}