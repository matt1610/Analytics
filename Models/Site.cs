using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Analytics.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Owner { get; set; }
    }
}