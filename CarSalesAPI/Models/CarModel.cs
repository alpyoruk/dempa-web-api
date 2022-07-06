using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSalesAPI.Models
{
    public class CarModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime BoughtDate { get; set; }
        public int BoughtPrice { get; set; }
        public int SalePrice { get; set; }
        public System.DateTime PublishDate { get; set; }
        public Nullable<int> SoldPrice { get; set; }
        public Nullable<System.DateTime> SoldDate { get; set; }
        public bool isDeleted { get; set; }
        public bool isSold { get; set; }
        public bool isPayed { get; set; }
        public string PayedNumber { get; set; }
        public int UserID { get; set; }
        public bool isRemoved { get; set; }

        public string HeaderPhoto;
        public string HeaderPhotoName;

        public string UserFullName;
    }
}