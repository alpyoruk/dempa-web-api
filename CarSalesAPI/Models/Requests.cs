using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSalesAPI.Models
{
    public class AddOrEditRequest
    {
        public bool isAdd { get; set; }
        public bool isEdit { get; set; }
        public bool isDelete { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class AddOrEditUserRequest
    {
        public bool isAdd { get; set; }
        public bool isEdit { get; set; }
        public bool isDelete { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
    }

    public class AddOrEditSeriesRequest
    {
        public bool isAdd { get; set; }
        public bool isEdit { get; set; }
        public bool isDelete { get; set; }
        public int BrandID { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class AddOrEditModelRequest
    {
        public bool isAdd { get; set; }
        public bool isEdit { get; set; }
        public bool isDelete { get; set; }
        public int SeriesID { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class GetCarRequest
    {
        public CarModel Car { get; set; }
        public CarDetailsModel CarDetails { get; set; }
    }

    public class AddCarRequest
    {
        public CarModel Car { get; set; }
        public CarDetailsModel CarDetails { get; set; }
        public List<CarDetailPhotosModel> CarPhotos { get; set; }
    }

    public class EditCarRequest
    {
        public CarModel Car { get; set; }
        public CarDetailsModel CarDetails { get; set; }
    }

    public class ChangeCarStatusRequest
    {
        public int CarID { get; set; }
        public bool checkIsDeleted { get; set; }
        public bool checkIsSold { get; set; }
        public DateTime SoldDate { get; set; }
        public int SoldPrice { get; set; }
        public bool checkIsPayed { get; set; }
        public string PayedNumber { get; set; }
        public bool Status { get; set; }
        public bool checkIsRemoved { get; set; }
    }

    public class GetBrandIdOrSeriesId
    {
        public int BrandID { get; set; }
        public int SeriesID { get; set; }
    }

    public class EditCarLabelsRequest
    {
        public int CarID { get; set; }
        public bool isTitle { get; set; }
        public string Title { get; set; }
        public bool isDesc { get; set; }
        public string Desc { get; set; }
    }

    public class EditPhotoRequest
    {
        public int CarID { get; set; }
        public List<CarDetailPhotos> AddPhotos { get; set; }
        public List<CarDetailPhotos> DeletePhotos { get; set; }
        public bool isAdd { get; set; }
        public bool isDelete { get; set; }
    }

    public class GetDetailsRequest
    {
        public string BrandName { get; set; }
        public string SeriesName { get; set; }
        public string ModelName { get; set; }
        public string CaseName { get; set; }
        public string FuelName { get; set; }
        public string GearName { get; set; }
        public string PlateName { get; set; }
        public string TractionName { get; set; }
    }

    public class EditPhotoMainRequest
    {
        public string PhotoLink { get; set; }
        public int CarID { get; set; }
    }

    public class UpdatePasswordRequest
    {
        public int ID { get; set; }
        public string Password { get; set; }
        public string oldPassword { get; set; }
    }

    public class SoldCarsRequest
    {
        public int Page { get; set; }
    }
}