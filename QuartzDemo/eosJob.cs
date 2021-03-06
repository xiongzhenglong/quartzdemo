﻿using Quartz;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace QuartzDemo
{
    [DisallowConcurrentExecution]
    class eosJob : IJob
    {
        private static string tag = "eos";
        private static string origin = "https://www.ceo.bi";
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
            request.AddHeader("cookie", "title208=1; userid=169533; move_moble=13401454382; sec_=5ad827f13bc73e4f4b351c519e75b7b6154fbd37; Hm_lvt_8ea3f9ee7328affe1c09a675ba5961a6=1523590103,1523600939,1523608784,1524101268; aliyungf_tc=AQAAAPDlSHpbvQ0A9pRLL1bFAoc+xBf6; JSPSESSID=l0a8e76tii3s7oe6jahll544h0; title213=1; title217=1; Hm_lpvt_8ea3f9ee7328affe1c09a675ba5961a6=1524706352; aliyungf_tc=AQAAAPDlSHpbvQ0A9pRLL1bFAoc+xBf6; JSPSESSID=l0a8e76tii3s7oe6jahll544h0; Hm_lvt_8ea3f9ee7328affe1c09a675ba5961a6=1523590103,1523600939,1523608784,1524101268; title208=1; move_moble=13401454382; title213=1; title217=1; userid=169533; Hm_lpvt_8ea3f9ee7328affe1c09a675ba5961a6=1524706352");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.9");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", $"{origin}/t/cny_{tag}.jsp");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("origin", origin);
            request.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            request.AddParameter("market", $"{tag}_cny");

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

        public void buy(string sec_, string num, string price)
        {
            var client = new RestClient(url_buy);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", $"JSPSESSID={sessionid}");
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

        public async Task Execute(IJobExecutionContext context)
        {
            string sec_ = "";
            var ceo = getIndex(out sec_);

            #region buy
            double smoney = 0;
            foreach (var s in ceo.depth.s)
            {
                double sell = double.Parse(s[0]);
                double count = double.Parse(s[1]);
                if (sell <= 90.05)
                {
                    smoney = smoney + sell * count;
                }
            }
            Console.WriteLine(smoney);
            if (smoney > 500)
            {
                buy(sec_, "10", "90.05");
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
            #endregion

            #region sell
            //double bmoney = 0;
            //foreach (var b in ceo.depth.b)
            //{
            //    double buy = double.Parse(b[0]);
            //    double count = double.Parse(b[1]);
            //    if (buy >= 93.5)
            //    {
            //        bmoney = bmoney + buy * count;
            //    }
            //}
            //if (bmoney > 500)
            //{
            //    double maxsellcount = ceo.finance[2];
            //    sell(sec_, maxsellcount.ToString(), "93.5");
            //}
            //else
            //{
            //    if (ceo.order != null)
            //    {
            //        foreach (var o in ceo.order)
            //        {
            //            chexiao(sec_, o.id);

            //        }
            //    }
            //}
            //Console.WriteLine(bmoney);
            #endregion


            await Task.CompletedTask;
        }
    }
}
