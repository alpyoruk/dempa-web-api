using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class CarController : ApiController
    {
        [HttpGet, Route("api/Car/GetCars")]
        public GetCarsResponse GetCars()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = context.Car.Where(x => x.isDeleted == false && x.isPayed == false && x.isSold == false && x.isRemoved == false);

                    GetCarsResponse res = new GetCarsResponse()
                    {
                        GetCars = new List<GetCarRequest>()
                    };

                    foreach (var item in data)
                    {
                        GetCarRequest carReq = new GetCarRequest()
                        {
                            Car = new CarModel(),
                            CarDetails = new CarDetailsModel()
                        };

                        CarModel carModel = new CarModel()
                        {
                            ID = item.ID,
                            SalePrice = item.SalePrice,
                            PublishDate = item.PublishDate,
                            Title = item.Title,
                            Description = item.Description,
                        };

                        var photoData = context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true);

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == item.ID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Color = carDetailsData.Color,
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = context.Brand.SingleOrDefault(x => x.ID == carDetailsData.BrandID).Name,
                            GearName = context.Gear.SingleOrDefault(x => x.ID == carDetailsData.GearID).Name,
                            ModelName = context.Model.SingleOrDefault(x => x.ID == carDetailsData.ModelID).Name,
                            SeriesName = context.Series.SingleOrDefault(x => x.ID == carDetailsData.SeriesID).Name,
                        };

                        carReq.CarDetails = carDetailsModel;

                        res.GetCars.Add(carReq);
                    }

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetCarsResponse res = new GetCarsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpGet, Route("api/Car/GetOnSale")]
        public GetCarsResponse GetOnSale()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarsResponse res = new GetCarsResponse()
                    {
                        GetCars = new List<GetCarRequest>()
                    };

                    var data = context.Car.Where(x => x.isDeleted == false && x.isPayed == false && x.isSold == false && x.isRemoved == false);

                    foreach (var item in data)
                    {
                        GetCarRequest carReq = new GetCarRequest()
                        {
                            Car = new CarModel(),
                            CarDetails = new CarDetailsModel()
                        };

                        CarModel carModel = new CarModel()
                        {
                            ID = item.ID,
                            SalePrice = item.SalePrice,
                            PublishDate = item.PublishDate,
                            Title = item.Title,
                            Description = item.Description,
                        };

                        var photoData = context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true);

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == item.ID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Color = carDetailsData.Color,
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = context.Brand.SingleOrDefault(x => x.ID == carDetailsData.BrandID).Name,
                            ModelName = context.Model.SingleOrDefault(x => x.ID == carDetailsData.ModelID).Name,
                            SeriesName = context.Series.SingleOrDefault(x => x.ID == carDetailsData.SeriesID).Name,
                        };

                        carReq.CarDetails = carDetailsModel;

                        res.GetCars.Add(carReq);
                    }

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetCarsResponse res = new GetCarsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/GetOnSold")]
        public GetSoldCarsResponse GetOnSold(SoldCarsRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = context.Car.Where(x => x.isSold == true && x.isRemoved == false);

                    double itemCount = data.ToList().Count;
                    double dpageCount = itemCount / 10;
                    int pageCount = Convert.ToInt32(Math.Ceiling(dpageCount));

                    GetSoldCarsResponse res = new GetSoldCarsResponse()
                    {
                        GetCars = new List<GetCarRequest>(),
                        ItemCount = Convert.ToInt32(itemCount),
                        PageCount = pageCount,
                    };

                    var skip = 10 * (input.Page - 1);

                    var sortedData = data.OrderByDescending(x => x.SoldDate).Skip(skip).Take(10);


                    foreach (var item in sortedData)
                    {
                        GetCarRequest carReq = new GetCarRequest()
                        {
                            Car = new CarModel(),
                            CarDetails = new CarDetailsModel()
                        };

                        CarModel carModel = new CarModel()
                        {
                            ID = item.ID,
                            PublishDate = item.PublishDate,
                            Title = item.Title,
                            SoldDate = item.SoldDate,
                            SoldPrice = item.SoldPrice,
                            BoughtDate = item.BoughtDate,
                            BoughtPrice = item.BoughtPrice
                        };

                        var photoData = context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true);

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == item.ID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = context.Brand.SingleOrDefault(x => x.ID == carDetailsData.BrandID).Name,
                            ModelName = context.Model.SingleOrDefault(x => x.ID == carDetailsData.ModelID).Name,
                            SeriesName = context.Series.SingleOrDefault(x => x.ID == carDetailsData.SeriesID).Name,
                        };

                        carReq.CarDetails = carDetailsModel;

                        res.GetCars.Add(carReq);
                    }

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetSoldCarsResponse res = new GetSoldCarsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpGet, Route("api/Car/GetOnPaid")]
        public GetCarsResponse GetOnPaid()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarsResponse res = new GetCarsResponse()
                    {
                        GetCars = new List<GetCarRequest>()
                    };

                    var data = context.Car.Where(x => x.isPayed == true && x.isSold == false && x.isRemoved == false);

                    foreach (var item in data)
                    {
                        GetCarRequest carReq = new GetCarRequest()
                        {
                            Car = new CarModel(),
                            CarDetails = new CarDetailsModel()
                        };

                        CarModel carModel = new CarModel()
                        {
                            ID = item.ID,
                            SalePrice = item.SalePrice,
                            PublishDate = item.PublishDate,
                            Title = item.Title,
                            PayedNumber = item.PayedNumber,
                            Description = item.Description,
                        };

                        var photoData = context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true);

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == item.ID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Color = carDetailsData.Color,
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = context.Brand.SingleOrDefault(x => x.ID == carDetailsData.BrandID).Name,
                            ModelName = context.Model.SingleOrDefault(x => x.ID == carDetailsData.ModelID).Name,
                            SeriesName = context.Series.SingleOrDefault(x => x.ID == carDetailsData.SeriesID).Name,
                        };

                        carReq.CarDetails = carDetailsModel;

                        res.GetCars.Add(carReq);
                    }

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetCarsResponse res = new GetCarsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpGet, Route("api/Car/GetOnNotSale")]
        public GetCarsResponse GetOnNotSale()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarsResponse res = new GetCarsResponse()
                    {
                        GetCars = new List<GetCarRequest>()
                    };

                    var data = context.Car.Where(x => x.isDeleted == true && x.isPayed == false && x.isSold == false && x.isRemoved == false);

                    foreach (var item in data)
                    {
                        GetCarRequest carReq = new GetCarRequest()
                        {
                            Car = new CarModel(),
                            CarDetails = new CarDetailsModel()
                        };

                        CarModel carModel = new CarModel()
                        {
                            ID = item.ID,
                            SalePrice = item.SalePrice,
                            Description = item.Description,
                            PublishDate = item.PublishDate,
                            Title = item.Title,
                        };

                        var photoData = context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true);

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == item.ID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Color = carDetailsData.Color,
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = context.Brand.SingleOrDefault(x => x.ID == carDetailsData.BrandID).Name,
                            ModelName = context.Model.SingleOrDefault(x => x.ID == carDetailsData.ModelID).Name,
                            SeriesName = context.Series.SingleOrDefault(x => x.ID == carDetailsData.SeriesID).Name,
                        };

                        carReq.CarDetails = carDetailsModel;

                        res.GetCars.Add(carReq);
                    }

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetCarsResponse res = new GetCarsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/GetCarForAdmin")]
        public GetCarResponse GetCarForAdmin(CarModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarResponse res = new GetCarResponse()
                    {
                        Car = new CarModel(),
                        CarDetails = new CarDetailsModel(),
                        CarPhotos = new List<CarDetailPhotosModel>()
                    };

                    var carData = context.Car.SingleOrDefault(x => x.ID == input.ID);

                    CarModel carModel = new CarModel()
                    {
                        ID = carData.ID,
                        SalePrice = carData.SalePrice,
                        Description = carData.Description,
                        PublishDate = carData.PublishDate,
                        Title = carData.Title,
                        UserFullName = context.Users.SingleOrDefault(x => x.ID == carData.UserID).Name + " " + context.Users.SingleOrDefault(x => x.ID == carData.UserID).Surname,
                        BoughtDate = carData.BoughtDate,
                        BoughtPrice = carData.BoughtPrice,
                        isPayed = carData.isPayed,
                        isDeleted = carData.isDeleted
                    };

                    if (carData.isSold)
                    {
                        carModel.SoldDate = carData.SoldDate;
                        carModel.SoldPrice = carData.SoldPrice;
                    }

                    if (carData.isPayed)
                    {
                        carModel.PayedNumber = carData.PayedNumber;
                    }

                    res.Car = carModel;

                    var photoData = context.CarDetailPhotos.Where(x => x.CarID == input.ID);

                    foreach (var photoItem in photoData)
                    {
                        CarDetailPhotosModel carDetailPhotosModel = new CarDetailPhotosModel()
                        {
                            ID = photoItem.ID,
                            PhotoLink = photoItem.PhotoLink,
                            PhotoName = photoItem.PhotoName,
                            isMainPhoto = photoItem.isMainPhoto,
                            PhotoKey = photoItem.PhotoKey
                        };

                        res.CarPhotos.Add(carDetailPhotosModel);
                    }

                    var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == input.ID);

                    CarDetailsModel carDetailsModel = new CarDetailsModel()
                    {
                        ID = carDetailsData.ID,
                        Capacity = carDetailsData.Capacity,
                        Color = carDetailsData.Color,
                        Kilometer = carDetailsData.Kilometer,
                        Power = carDetailsData.Power,
                        Year = carDetailsData.Year,
                        BrandID = carDetailsData.BrandID,
                        BrandName = context.Brand.SingleOrDefault(x => x.ID == carDetailsData.BrandID).Name,
                        CaseID = carDetailsData.CaseID,
                        CaseName = context.Case.SingleOrDefault(x => x.ID == carDetailsData.CaseID).Name,
                        FuelID = carDetailsData.FuelID,
                        FuelName = context.Fuel.SingleOrDefault(x => x.ID == carDetailsData.FuelID).Name,
                        GearID = carDetailsData.GearID,
                        GearName = context.Gear.SingleOrDefault(x => x.ID == carDetailsData.GearID).Name,
                        ModelID = carDetailsData.ModelID,
                        ModelName = context.Model.SingleOrDefault(x => x.ID == carDetailsData.ModelID).Name,
                        PlateID = carDetailsData.PlateID,
                        PlateName = context.Plate.SingleOrDefault(x => x.ID == carDetailsData.PlateID).Name,
                        SeriesID = carDetailsData.SeriesID,
                        SeriesName = context.Series.SingleOrDefault(x => x.ID == carDetailsData.SeriesID).Name,
                        TractionID = carDetailsData.TractionID,
                        TractionName = context.Traction.SingleOrDefault(x => x.ID == carDetailsData.TractionID).Name,
                    };

                    res.CarDetails = carDetailsModel;

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetCarResponse res = new GetCarResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/GetCarForSold")]
        public GetCarResponse GetCarForASold(CarModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarResponse res = new GetCarResponse()
                    {
                        Car = new CarModel(),
                        CarDetails = new CarDetailsModel(),
                        CarPhotos = new List<CarDetailPhotosModel>()
                    };

                    var carData = context.Car.SingleOrDefault(x => x.ID == input.ID);

                    CarModel carModel = new CarModel()
                    {
                        ID = carData.ID,
                        SalePrice = carData.SalePrice,
                        Description = carData.Description,
                        PublishDate = carData.PublishDate,
                        Title = carData.Title,
                        UserFullName = context.Users.SingleOrDefault(x => x.ID == carData.UserID).Name + " " + context.Users.SingleOrDefault(x => x.ID == carData.UserID).Surname,
                        BoughtDate = carData.BoughtDate,
                        BoughtPrice = carData.BoughtPrice,
                        isPayed = carData.isPayed,
                        isDeleted = carData.isDeleted,
                        SoldDate = carData.SoldDate,
                        SoldPrice = carData.SoldPrice
                    };

                    if (carData.isSold)
                    {
                        carModel.SoldDate = carData.SoldDate;
                        carModel.SoldPrice = carData.SoldPrice;
                    }

                    if (carData.isPayed)
                    {
                        carModel.PayedNumber = carData.PayedNumber;
                    }

                    res.Car = carModel;

                    var photoData = context.CarDetailPhotos.Where(x => x.CarID == input.ID);

                    foreach (var photoItem in photoData)
                    {
                        CarDetailPhotosModel carDetailPhotosModel = new CarDetailPhotosModel()
                        {
                            ID = photoItem.ID,
                            PhotoLink = photoItem.PhotoLink,
                            PhotoName = photoItem.PhotoName,
                            isMainPhoto = photoItem.isMainPhoto,
                            PhotoKey = photoItem.PhotoKey
                        };

                        res.CarPhotos.Add(carDetailPhotosModel);
                    }

                    var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == input.ID);

                    CarDetailsModel carDetailsModel = new CarDetailsModel()
                    {
                        ID = carDetailsData.ID,
                        Capacity = carDetailsData.Capacity,
                        Color = carDetailsData.Color,
                        Kilometer = carDetailsData.Kilometer,
                        Power = carDetailsData.Power,
                        Year = carDetailsData.Year,
                        BrandID = carDetailsData.BrandID,
                        BrandName = context.Brand.SingleOrDefault(x => x.ID == carDetailsData.BrandID).Name,
                        CaseID = carDetailsData.CaseID,
                        CaseName = context.Case.SingleOrDefault(x => x.ID == carDetailsData.CaseID).Name,
                        FuelID = carDetailsData.FuelID,
                        FuelName = context.Fuel.SingleOrDefault(x => x.ID == carDetailsData.FuelID).Name,
                        GearID = carDetailsData.GearID,
                        GearName = context.Gear.SingleOrDefault(x => x.ID == carDetailsData.GearID).Name,
                        ModelID = carDetailsData.ModelID,
                        ModelName = context.Model.SingleOrDefault(x => x.ID == carDetailsData.ModelID).Name,
                        PlateID = carDetailsData.PlateID,
                        PlateName = context.Plate.SingleOrDefault(x => x.ID == carDetailsData.PlateID).Name,
                        SeriesID = carDetailsData.SeriesID,
                        SeriesName = context.Series.SingleOrDefault(x => x.ID == carDetailsData.SeriesID).Name,
                        TractionID = carDetailsData.TractionID,
                        TractionName = context.Traction.SingleOrDefault(x => x.ID == carDetailsData.TractionID).Name,
                    };

                    res.CarDetails = carDetailsModel;

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetCarResponse res = new GetCarResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpGet, Route("api/Car/GetCar")]
        public GetCarResponse GetCar(CarModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarResponse res = new GetCarResponse()
                    {
                        Car = new CarModel(),
                        CarDetails = new CarDetailsModel(),
                        CarPhotos = new List<CarDetailPhotosModel>()
                    };

                    var carData = context.Car.SingleOrDefault(x => x.ID == input.ID);

                    CarModel carModel = new CarModel()
                    {
                        SalePrice = carData.SalePrice,
                        Description = carData.Description,
                        PublishDate = carData.PublishDate,
                        Title = carData.Title,
                    };

                    res.Car = carModel;

                    var photoData = context.CarDetailPhotos.Where(x => x.CarID == input.ID);

                    foreach (var photoItem in photoData)
                    {
                        CarDetailPhotosModel carDetailPhotosModel = new CarDetailPhotosModel()
                        {
                            PhotoLink = photoItem.PhotoLink,
                            PhotoName = photoItem.PhotoName
                        };

                        res.CarPhotos.Add(carDetailPhotosModel);
                    }

                    var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == input.ID);

                    CarDetailsModel carDetailsModel = new CarDetailsModel()
                    {
                        Capacity = carDetailsData.Capacity,
                        Color = carDetailsData.Color,
                        Kilometer = carDetailsData.Kilometer,
                        Power = carDetailsData.Power,
                        Year = carDetailsData.Year,
                        BrandName = context.Brand.SingleOrDefault(x => x.ID == carDetailsData.BrandID).Name,
                        CaseName = context.Case.SingleOrDefault(x => x.ID == carDetailsData.CaseID).Name,
                        FuelName = context.Fuel.SingleOrDefault(x => x.ID == carDetailsData.FuelID).Name,
                        GearName = context.Gear.SingleOrDefault(x => x.ID == carDetailsData.GearID).Name,
                        ModelName = context.Model.SingleOrDefault(x => x.ID == carDetailsData.ModelID).Name,
                        PlateName = context.Plate.SingleOrDefault(x => x.ID == carDetailsData.PlateID).Name,
                        SeriesName = context.Series.SingleOrDefault(x => x.ID == carDetailsData.SeriesID).Name,
                        TractionName = context.Traction.SingleOrDefault(x => x.ID == carDetailsData.TractionID).Name
                    };

                    res.CarDetails = carDetailsModel;

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetCarResponse res = new GetCarResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/AddCar")]
        public BaseApiResponse AddCar(AddCarRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    Car carData = new Car()
                    {
                        BoughtDate = input.Car.BoughtDate,
                        BoughtPrice = input.Car.BoughtPrice,
                        Description = input.Car.Description,
                        PublishDate = DateTime.Now,
                        SalePrice = input.Car.SalePrice,
                        Title = input.Car.Title,
                        UserID = input.Car.UserID,
                    };

                    var dataRes = context.Car.Add(carData);
                    context.SaveChanges();
                    int lastAddedCarID = dataRes.ID;

                    CarDetails carDetailsData = new CarDetails()
                    {
                        BrandID = input.CarDetails.BrandID,
                        Capacity = input.CarDetails.Capacity,
                        CarID = lastAddedCarID,
                        CaseID = input.CarDetails.CaseID,
                        Color = input.CarDetails.Color,
                        FuelID = input.CarDetails.FuelID,
                        GearID = input.CarDetails.GearID,
                        ModelID = input.CarDetails.ModelID,
                        Kilometer = input.CarDetails.Kilometer,
                        PlateID = input.CarDetails.PlateID,
                        Power = input.CarDetails.Power,
                        SeriesID = input.CarDetails.SeriesID,
                        TractionID = input.CarDetails.TractionID,
                        Year = input.CarDetails.Year
                    };

                    context.CarDetails.Add(carDetailsData);

                    if (input.CarPhotos.Count > 0)
                    {
                        foreach (var item in input.CarPhotos)
                        {
                            CarDetailPhotos carDetailPhotosData = new CarDetailPhotos()
                            {
                                CarID = lastAddedCarID,
                                PhotoLink = item.PhotoLink,
                                isMainPhoto = item.isMainPhoto,
                                PhotoName = item.PhotoName
                            };

                            context.CarDetailPhotos.Add(carDetailPhotosData);
                        }
                    }

                    context.SaveChanges();

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                        StatusMessage = "OK"
                    };

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/EditCar")]
        public GetDetailsResponse EditCar(EditCarRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetDetailsResponse res = new GetDetailsResponse()
                    {
                        GetDetails = new GetDetailsRequest()
                    };

                    var carData = context.Car.SingleOrDefault(x => x.ID == input.Car.ID);

                    carData.Title = input.Car.Title;
                    carData.Description = input.Car.Description;
                    carData.SalePrice = input.Car.SalePrice;
                    carData.BoughtDate = input.Car.BoughtDate;
                    carData.BoughtPrice = input.Car.BoughtPrice;

                    context.SaveChanges();

                    var carDetailsData = context.CarDetails.SingleOrDefault(x => x.CarID == input.Car.ID);

                    carDetailsData.BrandID = input.CarDetails.BrandID;
                    carDetailsData.CaseID = input.CarDetails.CaseID;
                    carDetailsData.FuelID = input.CarDetails.FuelID;
                    carDetailsData.GearID = input.CarDetails.GearID;
                    carDetailsData.ModelID = input.CarDetails.ModelID;
                    carDetailsData.PlateID = input.CarDetails.PlateID;
                    carDetailsData.SeriesID = input.CarDetails.SeriesID;
                    carDetailsData.TractionID = input.CarDetails.TractionID;
                    carDetailsData.Capacity = input.CarDetails.Capacity;
                    carDetailsData.Color = input.CarDetails.Color;
                    carDetailsData.Kilometer = input.CarDetails.Kilometer;
                    carDetailsData.Power = input.CarDetails.Power;
                    carDetailsData.Year = input.CarDetails.Year;

                    context.SaveChanges();

                    res.GetDetails.BrandName = context.Brand.SingleOrDefault(x => x.ID == input.CarDetails.BrandID).Name;
                    res.GetDetails.SeriesName = context.Series.SingleOrDefault(x => x.ID == input.CarDetails.SeriesID).Name;
                    res.GetDetails.ModelName = context.Model.SingleOrDefault(x => x.ID == input.CarDetails.ModelID).Name;
                    res.GetDetails.CaseName = context.Case.SingleOrDefault(x => x.ID == input.CarDetails.CaseID).Name;
                    res.GetDetails.FuelName = context.Fuel.SingleOrDefault(x => x.ID == input.CarDetails.FuelID).Name;
                    res.GetDetails.GearName = context.Gear.SingleOrDefault(x => x.ID == input.CarDetails.GearID).Name;
                    res.GetDetails.PlateName = context.Plate.SingleOrDefault(x => x.ID == input.CarDetails.PlateID).Name;
                    res.GetDetails.TractionName = context.Traction.SingleOrDefault(x => x.ID == input.CarDetails.TractionID).Name;

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetDetailsResponse res = new GetDetailsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/EditCarLabels")]
        public BaseApiResponse EditCarLabels(EditCarLabelsRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var carData = context.Car.Single(x => x.ID == input.CarID);
                    if (input.isTitle)
                    {
                        carData.Title = input.Title;
                        context.SaveChanges();
                    }

                    if (input.isDesc)
                    {
                        carData.Description = input.Desc;
                        context.SaveChanges();
                    }

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                        StatusMessage = "OK"
                    };

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/ChangeCarStatus")]
        public BaseApiResponse ChangeCarStatus(ChangeCarStatusRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = context.Car.SingleOrDefault(x => x.ID == input.CarID);

                    if (input.checkIsDeleted)
                    {
                        data.isDeleted = input.Status;
                        context.SaveChanges();
                    }
                    else if (input.checkIsPayed)
                    {
                        data.isPayed = input.Status;
                        if (input.Status)
                        {
                            data.PayedNumber = input.PayedNumber;
                        }
                        else
                        {
                            data.PayedNumber = null;
                        }
                        context.SaveChanges();
                    }
                    else if (input.checkIsSold)
                    {
                        data.isSold = input.Status;
                        if (input.Status)
                        {
                            data.SoldPrice = input.SoldPrice;
                            data.SoldDate = input.SoldDate;
                            data.isPayed = false;
                            data.isDeleted = false;
                            data.PayedNumber = null;
                        }
                        else
                        {
                            data.SoldPrice = null;
                            data.SoldDate = null;
                            data.isDeleted = true;
                        }
                        context.SaveChanges();
                    }
                    else if (input.checkIsRemoved)
                    {
                        data.isRemoved = true;
                        context.SaveChanges();
                    }

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                        StatusMessage = "OK"
                    };

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/EditPhoto")]
        public BaseApiResponse EditPhoto(EditPhotoRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    if (input.isAdd)
                    {
                        foreach (var item in input.AddPhotos)
                        {
                            CarDetailPhotos model = new CarDetailPhotos()
                            {
                                PhotoLink = item.PhotoLink,
                                PhotoName = item.PhotoName,
                                isMainPhoto = false,
                                CarID = input.CarID,
                                PhotoKey = item.PhotoKey
                            };

                            context.CarDetailPhotos.Add(model);
                            context.SaveChanges();
                        }
                    }
                    if (input.isDelete)
                    {
                        foreach (var item in input.DeletePhotos)
                        {
                            var deleteData = context.CarDetailPhotos.SingleOrDefault(x => x.ID == item.ID);
                            context.CarDetailPhotos.Remove(deleteData);
                            context.SaveChanges();
                        }
                    }

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                        StatusMessage = "OK"
                    };

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Car/EditPhotoMain")]
        public BaseApiResponse EditPhotoMain(EditPhotoMainRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var mainData = context.CarDetailPhotos.SingleOrDefault(x => x.CarID == input.CarID && x.isMainPhoto == true);
                    mainData.isMainPhoto = false;
                    context.SaveChanges();

                    var data = context.CarDetailPhotos.SingleOrDefault(x => x.PhotoLink == input.PhotoLink);
                    data.isMainPhoto = true;
                    context.SaveChanges();

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                        StatusMessage = "OK"
                    };

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }
    }
}