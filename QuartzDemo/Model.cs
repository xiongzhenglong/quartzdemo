using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzDemo
{

    #region CEO
    public class CEO
    {

        public string[] top { get; set; }
        public Depth depth { get; set; }
        public List<Order> order { get; set; }
        public float[] finance { get; set; }
    }





    public class Depth
    {
        public List<string[]> b { get; set; }
        public List<string[]> s { get; set; }
    }

    public class Order
    {
        public string time { get; set; }
        public int type { get; set; }
        public string price { get; set; }
        public string num { get; set; }
        public string mum { get; set; }
        public string deal { get; set; }
        public int id { get; set; }
        public string avg_price { get; set; }
        public string ymum { get; set; }
    }
    #endregion

    #region Price


  
    public class reprice
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string rank { get; set; }
        public string price_usd { get; set; }
        public string price_btc { get; set; }
        public string market_cap_usd { get; set; }
        public string available_supply { get; set; }
        public string total_supply { get; set; }
        public string max_supply { get; set; }
        public string percent_change_1h { get; set; }
        public string percent_change_24h { get; set; }
        public string percent_change_7d { get; set; }
        public string last_updated { get; set; }
        public string price_cny { get; set; }
        public string market_cap_cny { get; set; }
    }


    #endregion



}
