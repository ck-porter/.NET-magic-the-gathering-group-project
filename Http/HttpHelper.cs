using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Http
{
    public class HttpHelper
    {
        static HttpClient client = new HttpClient();

        public static async Task<string> GetXmlAsync(string strRequestUrl)
        {
            string temp = "";
            try
            {
                temp = await client.GetStringAsync(strRequestUrl);
                return temp;
            }
            catch { return ""; }
        }


        public static void CancelHttpRequestAsync()
        {
            try
            {
                client.CancelPendingRequests();
            }
            catch { }
        }
    }
}
