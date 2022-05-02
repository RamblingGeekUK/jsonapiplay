using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace crest
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            if(!CheckNet()) 
            {
                Console.WriteLine("Check that your online");
                Environment.Exit(0);
            }

            Console.WriteLine("Getting data...");
            
            try 
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://marketdata.tradermade.com/api/v1/live?currency=EURUSD,GBPUSD&api_key=Ky62n7v8IxBBisyol-4b");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CurrencyDetails>(responseBody);

                if (result.quotes != null)
                {
                    foreach (var item in result.quotes)
                    {
                        Console.WriteLine(" Result: " + item.base_currency + "/" + item.quote_currency + " " + item.mid);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Something went wrong... 🤦‍♂️😒");
            }
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool CheckNet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);         
        }

        public class CurrencyDetails
        {
                public string? endpoint { get; set; }
                public quotes[]? quotes { get; set; }
                public string? requested_time { get; set; }
                public long timestamp { get; set; }

        }

        public class quotes
        {
            public double ask { get; set; }
            public double bid { get; set; }
            public string? base_currency { get; set; }
            public double mid { get; set; }
            public string? quote_currency { get; set; }
        }
    }
}