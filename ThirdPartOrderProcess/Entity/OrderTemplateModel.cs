using System;

namespace ThirdPartOrderProcess.Model
{
    public class OrderTemplateModel
    {
        public int ID { get; set; }
        public String TemplateName { get; set; }
        public String ResponseType { get; set; }
        public String SubmitSuccess { get; set; }
        public String SubmitFail { get; set; }
        public double SubmitFailRate { get; set; }
        public String QueryResponse { get; set; }
        public String NoticeParam { get; set; }
        public String NoticeURL { get; set; }
        public String NoticeURI { get; set; }
        public String NoticeSign { get; set; }
        public String QueryBalance { get; set; }
        public DateTime InDate { get; set; }

        public String SecretKey { get; set; }

    }
}