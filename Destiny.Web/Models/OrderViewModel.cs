using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Destiny.Web.Models
{
    public class OrderViewModel
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string OrderNumber { get; set; }
        public string ProductCode { get; set; }
        public decimal Amount { get; set; }
        public string CreateBy { get; set; }
        public bool IsPaid { get; set; }
        public string AliPayCode { get; set; }
    }
}