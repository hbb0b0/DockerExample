using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Net;

namespace WebAPIForDocker.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private IConfigOptions m_configOptions;
        private ILogger m_logger;
        public OrdersController(IConfigOptions configOptions, ILogger<OrdersController> logger)
        {
            m_configOptions = configOptions;
            m_logger = logger;
        }
        // GET api/values
        [Route("[action]")]
        [HttpGet]
        public ActionResult<IEnumerable<Tuple<string, string>>> GetOrderList()
        {
            m_logger.LogDebug(" ConnectionString:" + m_configOptions.ConnectionString);
            //Console.WriteLine(" ConnectionString:" + m_configOptions.ConnectionString);
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(m_configOptions.ConnectionString))
                {

                    sqlCon.Open();
                    string sql = "select top 10 * from [dbo].[Order]";

                    using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(new Tuple<string, string>(reader["orderNum"].ToString(), reader["InDate"].ToString()));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                m_logger.LogError(ex, ex.Message, "Get");
            }

            return list;
        }

        // GET api/values/5
        [Route("[action]")]
        [HttpGet]
        public ActionResult<string> GetHostName()
        {
            string hostName = Dns.GetHostName();
            return hostName;
        }

       
    }
}
