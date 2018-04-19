using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzDemo
{


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
        public List<string[]>  s { get; set; }
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


}
