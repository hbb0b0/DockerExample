using System;

namespace ThirdPartOrderProcess.Model
{
    public class OrderModel
    {
        public int ID
        {
            get; set;
        }
        public String OrderNum
        {
            get;
            set;
        }
        public String JSON
        {
            get;
            set;
        }

        //0:初始 1：已发送通知
        public int Status
        {
            get; set;
        }

        //1： 订单成功 0：订单失败
         public int OrderStatus
        {           
            get; set;
        }
        public DateTime InDate
        {
            get;
            set;
        }
        public int OrderTemplateID
        {
            get;set;
        }
    }
}