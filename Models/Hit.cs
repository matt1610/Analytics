using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Analytics.Models
{
    public class Hit
    {
        public int Id { get; set; }
        public string Site { get; set; }
        public string Date { get; set; }
        public string Page { get; set; }
    }
}