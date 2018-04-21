using Quartz;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace QuartzDemo
{
    [DisallowConcurrentExecution]
    class dogeJob : IJob
    {

        private static string tag = "doge";
        private static string origin = "https://ceo.bi";
        private static string url_index = $"https://ceo.bi/trade/index_json?market={tag}_cny";
        private static string url_buy = "https://ceo.bi/trade/up.html";
        private static string url_chexiao = "https://ceo.bi/trade/chexiao.html";
        private static string refer = $"https://ceo.bi/t/cny_{tag}.jsp";

        private static string url_referprice = "";
        private static string sessionid = "nkn1qpraha5vcev59mcg21mqg7";

        public CEO getIndex(out string sec_)
        {
            var client = new RestClient(url_index);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", $"aliyungf_tc=AQAAABcs4VQ2wwoAzi2M0ufA9uQkjTEh; JSPSESSID={sessionid}");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", $"https://ceo.bi/t/cny_{tag}.jsp");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("origin", origin);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            IRestResponse response = client.Execute(request);
            
            
            sec_ = response.Cookies != null && response.Cookies.Count > 0 ? response.Cookies[0].Value : "";
            return JsonHelper.DeserializeJsonToObject<CEO>(response.Content);
        }

        public reprice getReferPrice(string rptag)
        {
            var client = new RestClient($"https://api.coinmarketcap.com/v1/ticker/{rptag}/?convert=cny");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("upgrade-insecure-requests", "1");
            IRestResponse response = client.Execute(request);
            return JsonHelper.DeserializeJsonToObject<reprice>(response.Content.Substring(1, response.Content.Length - 2));
        }

        public void buy(string sec_, string num, string price)
        {
            var client = new RestClient(url_buy);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", "aliyungf_tc=AQAAABcs4VQ2wwoAzi2M0ufA9uQkjTEh; JSPSESSID=nkn1qpraha5vcev59mcg21mqg7");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", refer);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("origin", origin);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");





            //request.AddHeader("cookie", $"JSPSESSID={sessionid}; sec_={sec_}");



            request.AddParameter("market", $"{tag}_cny");
            request.AddParameter("num", num);
            request.AddParameter("paypassword", "660218");
            request.AddParameter("price", price);
            request.AddParameter("type", "1");
            IRestResponse response = client.Execute(request);
        }

        public void chexiao(string sec_, int id)
        {
            var client = new RestClient(url_chexiao);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", $"JSPSESSID={sessionid}; sec_={sec_}");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", refer);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("origin", origin);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddParameter("id", id);
            IRestResponse response = client.Execute(request);
        }

        public async Task Execute(IJobExecutionContext context)
        {

            try
            {
                reprice rp = getReferPrice("Dogecoin");
                double eosprice = double.Parse(double.Parse(rp.price_cny).ToString("0.0000"));

                string sec_ = "";
                var ceo = getIndex(out sec_);

                double maxprice = double.Parse(ceo.depth.b[0][0]);
                double maxcount = double.Parse(ceo.depth.b[0][1]);

                double minprice = double.Parse(ceo.depth.s[9][0]);
                double mincount = double.Parse(ceo.depth.s[9][1]);
                double totalmoney = maxprice * maxcount;
                double minmoney = minprice * mincount;
                Console.WriteLine(maxprice + ":" + totalmoney + "----" + minprice + ":" + minmoney);
                if (ceo.order != null)
                {
                    foreach (var o in ceo.order)
                    {
                        if (double.Parse(o.price) != eosprice && o.type == 1)
                        {

                            chexiao(sec_, o.id);

                        }
                    }
                }



                if (ceo.order != null && ceo.order.Count > 0)
                {
                    Console.WriteLine("2:" + totalmoney);
                }
                else
                {

                    buy(sec_, "100000", eosprice.ToString());
                    Console.WriteLine("1:" + totalmoney);
                }









            }
            catch (Exception ex)
            {

                ;
            }


            await Task.CompletedTask;
        }
    }
}
