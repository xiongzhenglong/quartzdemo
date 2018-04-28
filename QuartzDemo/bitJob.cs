using Quartz;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace QuartzDemo
{
    [DisallowConcurrentExecution]
    class bitJob : IJob
    {
        private static string tag = "oioc";
        private static string origin = "https://ceo.bi";
        private static string url_index = $"{origin}/trade/index_json?market={tag}_cny";
        private static string url_buy = $"{origin}/trade/up.html";
        private static string url_sell = $"{origin}/trade/up.html";
        private static string url_chexiao = $"{origin}/trade/chexiao.html";
        private static string refer = $"{origin}/t/cny_{tag}.jsp";

        private static string url_referprice = "";
        private static string sessionid = "l0a8e76tii3s7oe6jahll544h0";

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

        public void sell(string sec_, string num, string price)
        {
            var client = new RestClient(url_sell);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", $"aliyungf_tc=AQAAABcs4VQ2wwoAzi2M0ufA9uQkjTEh; JSPSESSID={sessionid}");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", refer);
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
            string sec_ = "";
            var ceo = getIndex(out sec_);
            double bmoney = 0;
            foreach (var b in ceo.depth.b)
            {
                double buy = double.Parse(b[0]);
                double count = double.Parse(b[1]);
                if (buy >= 0.404)
                {
                    bmoney = bmoney + buy * count;
                }
            }
            if (bmoney > 3000)
            {
                double maxsellcount = ceo.finance[2] - 1;
                sell(sec_, maxsellcount.ToString("0.00"), "0.404");
            }
            else
            {
                if (ceo.order != null)
                {
                    foreach (var o in ceo.order)
                    {
                        chexiao(sec_, o.id);                     

                    }
                }
            }
            Console.WriteLine(bmoney);
          
            await Task.CompletedTask;
        }
    }
}
