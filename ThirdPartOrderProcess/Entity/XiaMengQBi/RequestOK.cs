namespace ThirdPartOrderProcess.Entity.XiaMengQBi
{
    public class RequestOK
    {
        public RequestOK(int result,string transId, string msg="OK")
        {
            this.result=0;
            this.msg=msg;
            this.transId= transId;

        }
        public int result
        {
            get;
            set;
        } 
        public string msg
        {
            get;
            set;
        }

        public string transId
        {
            get;
            set;
        }
    }
}