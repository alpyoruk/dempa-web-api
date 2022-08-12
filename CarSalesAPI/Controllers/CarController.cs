using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class CarController : ApiController
    {
        [HttpGet, Route("api/Car/GetCars")]
        public async Task<GetCarsResponse> GetCars()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = await context.Car.Where(x => x.isDeleted == false && x.isPayed == false && x.isSold == false && x.isRemoved == false).ToListAsync();

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

                        var photoData = await context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true).ToListAsync();

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == item.ID);
                        var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == carDetailsData.BrandID);
                        var gearN = await context.Gear.SingleOrDefaultAsync(x => x.ID == carDetailsData.GearID);
                        var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == carDetailsData.ModelID);
                        var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == carDetailsData.SeriesID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Color = carDetailsData.Color,
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = brandN.Name,
                            GearName = gearN.Name,
                            ModelName = modelN.Name,
                            SeriesName = seriesN.Name,
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
        public async Task<GetCarsResponse> GetOnSale()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarsResponse res = new GetCarsResponse()
                    {
                        GetCars = new List<GetCarRequest>()
                    };

                    var data = await context.Car.Where(x => x.isDeleted == false && x.isPayed == false && x.isSold == false && x.isRemoved == false).ToListAsync();

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

                        var photoData = await context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true).ToListAsync();

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == item.ID);
                        var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == carDetailsData.BrandID);
                        var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == carDetailsData.ModelID);
                        var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == carDetailsData.SeriesID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Color = carDetailsData.Color,
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = brandN.Name,
                            ModelName = modelN.Name,
                            SeriesName = seriesN.Name,
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
        public async Task<GetSoldCarsResponse> GetOnSold(SoldCarsRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = await context.Car.Where(x => x.isSold == true && x.isRemoved == false).ToListAsync();

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

                        var photoData = await context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true).ToListAsync();

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == item.ID);
                        var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == carDetailsData.BrandID);
                        var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == carDetailsData.ModelID);
                        var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == carDetailsData.SeriesID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = brandN.Name,
                            ModelName = modelN.Name,
                            SeriesName = seriesN.Name,
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
        public async Task<GetCarsResponse> GetOnPaid()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarsResponse res = new GetCarsResponse()
                    {
                        GetCars = new List<GetCarRequest>()
                    };

                    var data = await context.Car.Where(x => x.isPayed == true && x.isSold == false && x.isRemoved == false).ToListAsync();

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

                        var photoData = await context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true).ToListAsync();

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == item.ID);
                        var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == carDetailsData.BrandID);
                        var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == carDetailsData.ModelID);
                        var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == carDetailsData.SeriesID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Color = carDetailsData.Color,
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = brandN.Name,
                            ModelName = modelN.Name,
                            SeriesName = seriesN.Name,
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
        public async Task<GetCarsResponse> GetOnNotSale()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCarsResponse res = new GetCarsResponse()
                    {
                        GetCars = new List<GetCarRequest>()
                    };

                    var data = await context.Car.Where(x => x.isDeleted == true && x.isPayed == false && x.isSold == false && x.isRemoved == false).ToListAsync();

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

                        var photoData = await context.CarDetailPhotos.Where(x => x.CarID == item.ID && x.isMainPhoto == true).ToListAsync();

                        foreach (var photoItem in photoData)
                        {
                            carModel.HeaderPhoto = photoItem.PhotoLink;
                            carModel.HeaderPhotoName = photoItem.PhotoName;
                        }

                        carReq.Car = carModel;

                        var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == item.ID);
                        var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == carDetailsData.BrandID);
                        var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == carDetailsData.ModelID);
                        var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == carDetailsData.SeriesID);

                        CarDetailsModel carDetailsModel = new CarDetailsModel()
                        {
                            Color = carDetailsData.Color,
                            Kilometer = carDetailsData.Kilometer,
                            Year = carDetailsData.Year,
                            BrandName = brandN.Name,
                            ModelName = modelN.Name,
                            SeriesName = seriesN.Name,
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
        public async Task<GetCarResponse> GetCarForAdmin(CarModel input)
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

                    var carData = await context.Car.SingleOrDefaultAsync(x => x.ID == input.ID);

                    var user = await context.Users.SingleOrDefaultAsync(x => x.ID == carData.UserID);

                    CarModel carModel = new CarModel()
                    {
                        ID = carData.ID,
                        SalePrice = carData.SalePrice,
                        Description = carData.Description,
                        PublishDate = carData.PublishDate,
                        Title = carData.Title,
                        UserFullName = user.Name + " " + user.Surname,
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

                    var photoData = await context.CarDetailPhotos.Where(x => x.CarID == input.ID).ToListAsync();

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

                    var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == input.ID);

                    var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == carDetailsData.BrandID);
                    var caseN = await context.Case.SingleOrDefaultAsync(x => x.ID == carDetailsData.CaseID);
                    var fuelN = await context.Fuel.SingleOrDefaultAsync(x => x.ID == carDetailsData.FuelID);
                    var gearN = await context.Gear.SingleOrDefaultAsync(x => x.ID == carDetailsData.GearID);
                    var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == carDetailsData.ModelID);
                    var plateN = await context.Plate.SingleOrDefaultAsync(x => x.ID == carDetailsData.PlateID);
                    var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == carDetailsData.SeriesID);
                    var tractionN = await context.Traction.SingleOrDefaultAsync(x => x.ID == carDetailsData.TractionID);

                    CarDetailsModel carDetailsModel = new CarDetailsModel()
                    {
                        ID = carDetailsData.ID,
                        Capacity = carDetailsData.Capacity,
                        Color = carDetailsData.Color,
                        Kilometer = carDetailsData.Kilometer,
                        Power = carDetailsData.Power,
                        Year = carDetailsData.Year,
                        BrandID = carDetailsData.BrandID,
                        BrandName = brandN.Name,
                        CaseID = carDetailsData.CaseID,
                        CaseName = caseN.Name,
                        FuelID = carDetailsData.FuelID,
                        FuelName = fuelN.Name,
                        GearID = carDetailsData.GearID,
                        GearName = gearN.Name,
                        ModelID = carDetailsData.ModelID,
                        ModelName = modelN.Name,
                        PlateID = carDetailsData.PlateID,
                        PlateName = plateN.Name,
                        SeriesID = carDetailsData.SeriesID,
                        SeriesName = seriesN.Name,
                        TractionID = carDetailsData.TractionID,
                        TractionName = tractionN.Name,
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
        public async Task<GetCarResponse> GetCarForASold(CarModel input)
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

                    var carData = await context.Car.SingleOrDefaultAsync(x => x.ID == input.ID);
                    var user = await context.Users.SingleOrDefaultAsync(x => x.ID == carData.UserID);

                    CarModel carModel = new CarModel()
                    {
                        ID = carData.ID,
                        SalePrice = carData.SalePrice,
                        Description = carData.Description,
                        PublishDate = carData.PublishDate,
                        Title = carData.Title,
                        UserFullName = user.Name + " " + user.Surname,
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

                    var photoData = await context.CarDetailPhotos.Where(x => x.CarID == input.ID).ToListAsync();

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

                    var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == input.ID);
                    var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == carDetailsData.BrandID);
                    var caseN = await context.Case.SingleOrDefaultAsync(x => x.ID == carDetailsData.CaseID);
                    var fuelN = await context.Fuel.SingleOrDefaultAsync(x => x.ID == carDetailsData.FuelID);
                    var gearN = await context.Gear.SingleOrDefaultAsync(x => x.ID == carDetailsData.GearID);
                    var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == carDetailsData.ModelID);
                    var plateN = await context.Plate.SingleOrDefaultAsync(x => x.ID == carDetailsData.PlateID);
                    var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == carDetailsData.SeriesID);
                    var tractionN = await context.Traction.SingleOrDefaultAsync(x => x.ID == carDetailsData.TractionID);

                    CarDetailsModel carDetailsModel = new CarDetailsModel()
                    {
                        ID = carDetailsData.ID,
                        Capacity = carDetailsData.Capacity,
                        Color = carDetailsData.Color,
                        Kilometer = carDetailsData.Kilometer,
                        Power = carDetailsData.Power,
                        Year = carDetailsData.Year,
                        BrandID = carDetailsData.BrandID,
                        BrandName = brandN.Name,
                        CaseID = carDetailsData.CaseID,
                        CaseName = caseN.Name,
                        FuelID = carDetailsData.FuelID,
                        FuelName = fuelN.Name,
                        GearID = carDetailsData.GearID,
                        GearName = gearN.Name,
                        ModelID = carDetailsData.ModelID,
                        ModelName = modelN.Name,
                        PlateID = carDetailsData.PlateID,
                        PlateName = plateN.Name,
                        SeriesID = carDetailsData.SeriesID,
                        SeriesName = seriesN.Name,
                        TractionID = carDetailsData.TractionID,
                        TractionName = tractionN.Name,
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
        public async Task<GetCarResponse> GetCar(CarModel input)
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

                    var carData = await context.Car.SingleOrDefaultAsync(x => x.ID == input.ID);

                    CarModel carModel = new CarModel()
                    {
                        SalePrice = carData.SalePrice,
                        Description = carData.Description,
                        PublishDate = carData.PublishDate,
                        Title = carData.Title,
                    };

                    res.Car = carModel;

                    var photoData = await context.CarDetailPhotos.Where(x => x.CarID == input.ID).ToListAsync();

                    foreach (var photoItem in photoData)
                    {
                        CarDetailPhotosModel carDetailPhotosModel = new CarDetailPhotosModel()
                        {
                            PhotoLink = photoItem.PhotoLink,
                            PhotoName = photoItem.PhotoName
                        };

                        res.CarPhotos.Add(carDetailPhotosModel);
                    }

                    var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == input.ID);
                    var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == carDetailsData.BrandID);
                    var caseN = await context.Case.SingleOrDefaultAsync(x => x.ID == carDetailsData.CaseID);
                    var fuelN = await context.Fuel.SingleOrDefaultAsync(x => x.ID == carDetailsData.FuelID);
                    var gearN = await context.Gear.SingleOrDefaultAsync(x => x.ID == carDetailsData.GearID);
                    var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == carDetailsData.ModelID);
                    var plateN = await context.Plate.SingleOrDefaultAsync(x => x.ID == carDetailsData.PlateID);
                    var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == carDetailsData.SeriesID);
                    var traction = await context.Traction.SingleOrDefaultAsync(x => x.ID == carDetailsData.TractionID);

                    CarDetailsModel carDetailsModel = new CarDetailsModel()
                    {
                        Capacity = carDetailsData.Capacity,
                        Color = carDetailsData.Color,
                        Kilometer = carDetailsData.Kilometer,
                        Power = carDetailsData.Power,
                        Year = carDetailsData.Year,
                        BrandName = brandN.Name,
                        CaseName = caseN.Name,
                        FuelName = fuelN.Name,
                        GearName = gearN.Name,
                        ModelName = modelN.Name,
                        PlateName = plateN.Name,
                        SeriesName = seriesN.Name,
                        TractionName = traction.Name
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
        public async Task<BaseApiResponse> AddCar(AddCarRequest input)
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
                    await context.SaveChangesAsync();
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

                    await context.SaveChangesAsync();

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
        public async Task<GetDetailsResponse> EditCar(EditCarRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetDetailsResponse res = new GetDetailsResponse()
                    {
                        GetDetails = new GetDetailsRequest()
                    };

                    var carData = await context.Car.SingleOrDefaultAsync(x => x.ID == input.Car.ID);

                    carData.Title = input.Car.Title;
                    carData.Description = input.Car.Description;
                    carData.SalePrice = input.Car.SalePrice;
                    carData.BoughtDate = input.Car.BoughtDate;
                    carData.BoughtPrice = input.Car.BoughtPrice;

                    await context.SaveChangesAsync();

                    var carDetailsData = await context.CarDetails.SingleOrDefaultAsync(x => x.CarID == input.Car.ID);

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

                    await context.SaveChangesAsync();

                    var brandN = await context.Brand.SingleOrDefaultAsync(x => x.ID == input.CarDetails.BrandID);
                    var seriesN = await context.Series.SingleOrDefaultAsync(x => x.ID == input.CarDetails.SeriesID);
                    var modelN = await context.Model.SingleOrDefaultAsync(x => x.ID == input.CarDetails.ModelID);
                    var caseN = await context.Case.SingleOrDefaultAsync(x => x.ID == input.CarDetails.CaseID);
                    var fuelN = await context.Fuel.SingleOrDefaultAsync(x => x.ID == input.CarDetails.FuelID);
                    var gearN = await context.Gear.SingleOrDefaultAsync(x => x.ID == input.CarDetails.GearID);
                    var plateN = await context.Plate.SingleOrDefaultAsync(x => x.ID == input.CarDetails.PlateID);
                    var tractionN = await context.Traction.SingleOrDefaultAsync(x => x.ID == input.CarDetails.TractionID);

                    res.GetDetails.BrandName = brandN.Name;
                    res.GetDetails.SeriesName = seriesN.Name;
                    res.GetDetails.ModelName = modelN.Name;
                    res.GetDetails.CaseName = caseN.Name;
                    res.GetDetails.FuelName = fuelN.Name;
                    res.GetDetails.GearName = gearN.Name;
                    res.GetDetails.PlateName = plateN.Name;
                    res.GetDetails.TractionName = tractionN.Name;

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
        public async Task<BaseApiResponse> EditCarLabels(EditCarLabelsRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var carData = await context.Car.SingleOrDefaultAsync(x => x.ID == input.CarID);
                    if (input.isTitle)
                    {
                        carData.Title = input.Title;
                        await context.SaveChangesAsync();
                    }

                    if (input.isDesc)
                    {
                        carData.Description = input.Desc;
                        await context.SaveChangesAsync();
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
        public async Task<BaseApiResponse> ChangeCarStatus(ChangeCarStatusRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = await context.Car.SingleOrDefaultAsync(x => x.ID == input.CarID);

                    if (input.checkIsDeleted)
                    {
                        data.isDeleted = input.Status;
                        await context.SaveChangesAsync();
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
                        await context.SaveChangesAsync();
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
                        await context.SaveChangesAsync();
                    }
                    else if (input.checkIsRemoved)
                    {
                        data.isRemoved = true;
                        await context.SaveChangesAsync();
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
        public async Task<BaseApiResponse> EditPhoto(EditPhotoRequest input)
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
                            await context.SaveChangesAsync();
                        }
                    }
                    if (input.isDelete)
                    {
                        foreach (var item in input.DeletePhotos)
                        {
                            var deleteData = await context.CarDetailPhotos.SingleOrDefaultAsync(x => x.ID == item.ID);
                            context.CarDetailPhotos.Remove(deleteData);
                            await context.SaveChangesAsync();
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
        public async Task<BaseApiResponse> EditPhotoMain(EditPhotoMainRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var mainData = await context.CarDetailPhotos.SingleOrDefaultAsync(x => x.CarID == input.CarID && x.isMainPhoto == true);
                    mainData.isMainPhoto = false;
                    context.SaveChanges();

                    var data = await context.CarDetailPhotos.SingleOrDefaultAsync(x => x.PhotoLink == input.PhotoLink);
                    data.isMainPhoto = true;
                    await context.SaveChangesAsync();

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