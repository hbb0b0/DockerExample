using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ThirdPartOrderProcess.DB;
using ThirdPartOrderProcess.Model;

namespace ThirdPartOrderProcess.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {

        protected ILogger m_Logger = null;
        protected virtual string TemplateName
        {
            get; set;
        }

        protected readonly List<OrderTemplateModel> OrderTemplateList = null;
        protected OrderTemplateModel CurrentOrderTemplateModel = null;
        public BaseController(ILogger<BaseController> logger)
        {
            m_Logger = logger;
            OrderTemplateList = DBHelper.GetOrderTemplateList();
        }
    }
}
