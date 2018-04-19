using Quartz;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace QuartzDemo
{
    [DisallowConcurrentExecution]
    class CEOJob:IJob
    {

        private static string tag = "oioc";
        private static string origin = "https://www.ceo.bi";
        private static string url_index = $"https://www.ceo.bi/trade/index_json?market={tag}_cny";
        private static string url_buy = "https://www.ceo.bi/trade/up.html";
        private static string url_chexiao = "https://ceo.bi/trade/chexiao.html";
        private static string refer = $"https://www.ceo.bi/t/cny_{tag}.jsp";
        private static string sessionid = "onri4qmn5ls8rein0tqg8e1ep5";

        public CEO getIndex(out string sec_)
        {
            var client = new RestClient(url_index);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddCookie("JSPSESSID", sessionid);
            IRestResponse<CEO> response = client.Execute<CEO>(request);
            sec_ = response.Cookies[0].Value;
            return JsonHelper.DeserializeJsonToObject<CEO>(response.Content);
        }

        public void buy(string sec_,string num,string price)
        {
            var client = new RestClient(url_buy);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", $"JSPSESSID={sessionid}; sec_={sec_}");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", refer);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("x-devtools-emulate-network-conditions-client-id", "2687BE9BCA71601DA7A9F346038BC9FC");
            request.AddHeader("origin", origin);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddParameter("market", $"{tag}_cny");
            request.AddParameter("num", num);
            request.AddParameter("paypassword", "660218");
            request.AddParameter("price", price);
            request.AddParameter("type", "1");
            IRestResponse response = client.Execute(request);
        }

        public void chexiao(string sec_,int id)
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

                double maxprice = double.Parse(ceo.depth.b[0][0]);
                double maxcount = double.Parse(ceo.depth.b[0][1]);

                double minprice = double.Parse(ceo.depth.s[9][0]);
                double mincount = double.Parse(ceo.depth.s[9][1]);
                double totalmoney = maxprice * maxcount;
                double minmoney = minprice * mincount;
                Console.WriteLine(maxprice+":"+totalmoney+"----"+minprice+":"+minmoney);
                if (ceo.order !=null && ceo.order.Count>0)
                {
                    foreach (var o in ceo.order)
                    {
                        if (double.Parse(o.price) != maxprice)
                        {
                            chexiao(sec_, o.id);
                        }
                    }
                }
                if (totalmoney> 300000 )
                {
                    if (ceo.order != null && ceo.order.Count > 0)
                    {
                        Console.WriteLine("2:" + totalmoney);
                    }
                    else
                    {
                        buy(sec_, maxcount.ToString(), maxprice.ToString());
                        Console.WriteLine("1:"+totalmoney);
                    }
                    
                   
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
