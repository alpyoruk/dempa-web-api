using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSalesAPI.Models
{
    public class CarDetailPhotosModel
    {
        public int ID { get; set; }
        public int CarID { get; set; }
        public string PhotoLink { get; set; }
        public bool isMainPhoto { get; set; }
        public string PhotoName { get; set; }
        public string PhotoKey { get; set; }
    }
}