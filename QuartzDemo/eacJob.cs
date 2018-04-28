using Quartz;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace QuartzDemo
{
    [DisallowConcurrentExecution]
    class eacJob : IJob
    {

        private static string tag = "eac";
        private static string origin = "https://ceo.bi";
        private static string url_index = $"{origin}/trade/index_json?market={tag}_cny";
        private static string url_buy = $"{origin}/trade/up.html";
        private static string url_sell = $"{origin}/trade/up.html";
        private static string url_chexiao = $"{origin}/trade/chexiao.html";
        private static string refer = $"{origin}/t/cny_{tag}.jsp";

        private static string url_referprice = "";
        private static string sessionid = "onri4qmn5ls8rein0tqg8e1ep5";

        public CEO getIndex(out string sec_)
        {

            var client = new RestClient(url_index);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", "aliyungf_tc=AQAAABcs4VQ2wwoAzi2M0ufA9uQkjTEh; JSPSESSID=nkn1qpraha5vcev59mcg21mqg7");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", $"{origin}/t/cny_{tag}.jsp");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("origin", origin);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");

            IRestResponse response = client.Execute(request);
            sec_ = response.Cookies != null && response.Cookies.Count > 0 ? response.Cookies[0].Value : "";
            return JsonHelper.DeserializeJsonToObject<CEO>(response.Content.Replace(",\"s\":\"no\"", ""));
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
            request.AddHeader("cookie", $"aliyungf_tc=AQAAABcs4VQ2wwoAzi2M0ufA9uQkjTEh; JSPSESSID={sessionid}");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", refer);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("origin", origin);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            

            request.AddParameter("market", $"{tag}_cny");
            request.AddParameter("num", num);
            request.AddParameter("paypassword", "660218");
            request.AddParameter("price", price);
            request.AddParameter("type", "1");
            IRestResponse response = client.Execute(request);
        }

        public void sell(string sec_, string num, string price)
        {
            var client = new RestClient(url_sell);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", $"aliyungf_tc=AQAAABcs4VQ2wwoAzi2M0ufA9uQkjTEh; JSPSESSID={sessionid}");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer",refer);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("origin", origin);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddParameter("market", $"{tag}_cny");
            request.AddParameter("num", num);
            request.AddParameter("paypassword", "660218");
            request.AddParameter("price", price);
            request.AddParameter("type", "2");
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
                

                string sec_ = "";
                var ceo = getIndex(out sec_);
                Console.WriteLine(ceo.finance[0]);
               
                if (ceo.depth.s!=null)
                {
                    double maxprice = double.Parse(ceo.depth.b[0][0]);
                    double maxcount = double.Parse(ceo.depth.b[0][1]);

                    double minprice = double.Parse(ceo.depth.s[9][0]);
                    double mincount = double.Parse(ceo.depth.s[9][1]);
                    double totalmoney = maxprice * maxcount;
                    double minmoney = minprice * mincount;
                    Console.WriteLine(minmoney+"---"+minprice/maxprice+"---"+maxprice);

                    if (ceo.order != null)
                    {
                        foreach (var o in ceo.order)
                        {

                            if (double.Parse(o.price) != maxprice || totalmoney < 1800)
                            {
                                
                                    chexiao(sec_, o.id);
                                
                               
                            }
                          
                        }
                    }

                    if (maxprice >= 0.009)
                    {

                        double maxsellcount = ceo.finance[2] - 1;
                        sell(sec_, maxsellcount.ToString("0.00"), maxprice.ToString());
                    }

                    //if (ceo.order != null && ceo.order.Count > 0)
                    //{
                    //    //Console.WriteLine("2:" + totalmoney);
                    //}
                    //else
                    //{
                    //    if (totalmoney > 1000 && maxprice < 0.007810)
                    //    {
                    //        //if (minprice / maxprice < 1.02)
                    //        //{
                    //        //    double buymoney = minmoney <= 1000 ? minmoney : 1000;
                    //        //    buy(sec_, (buymoney / minprice).ToString("0.00"), minprice.ToString());
                    //        //}

                    //        buy(sec_, (1000 / maxprice).ToString("0.00"), maxprice.ToString());
                    //    }
                        
                       
                    //}
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
