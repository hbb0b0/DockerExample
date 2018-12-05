using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ThirdPartOrderProcess.Common
{
    public class HttpTool
    {

        protected ILogger m_Logger = null;
        public HttpTool(ILogger logger)
        {
            m_Logger = logger;
        }
        /// <summary>
        /// 异步请求post（键值对形式,可等待的）
        /// </summary>
        /// <param name="uri">网络基址("http://localhost:59315")</param>
        /// <param name="url">网络的地址("/api/UMeng")</param>
        /// <param name="formData">键值对List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();formData.Add(new KeyValuePair<string, string>("userid", "29122"));formData.Add(new KeyValuePair<string, string>("umengids", "29122"));</param>
        /// <param name="charset">编码格式</param>
        /// <param name="mediaType">头媒体类型</param>
        /// <returns></returns>

        public  async Task<string> HttpPostAsync(string uri, string url, List<KeyValuePair<string, string>> formData = null, string charset = "UTF-8", string mediaType = "application/x-www-form-urlencoded")
        {
            string result = null;
            string tokenUri = url;
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(uri)
                };
                HttpContent content = new FormUrlEncodedContent(formData);
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                content.Headers.ContentType.CharSet = charset;
                for (int i = 0; i < formData.Count; i++)
                {
                    content.Headers.Add(formData[i].Key, formData[i].Value);
                }

                HttpResponseMessage resp = await client.PostAsync(tokenUri, content);
                resp.EnsureSuccessStatusCode();
                result = await resp.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {
                m_Logger.LogError("HttpPostAsync:Exception{0}",ex.Message);
                //Console.WriteLine($"HttpPostAsync:Exception{ex.Message}");
                
            }
            return result;
        }
    
    }
}