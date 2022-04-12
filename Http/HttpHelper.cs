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
        //Create a new HttpClient

        static HttpClient client = new HttpClient();

        public static async Task<string> GetXmlAsync(string strRequestUrl)
        {
            string temp = "";
            try
            {
                //Sends a GET request to the specified Uri

                temp = await client.GetStringAsync(strRequestUrl);
                
                //Return the response body as a string in an asynchronous operation

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
