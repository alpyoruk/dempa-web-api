using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSalesAPI.Models
{
    public class BaseApiResponse
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string StatusMessage { get; set; }
    }
}