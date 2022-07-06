using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarSalesAPI.Models
{
    public class GetBrandsResponse: BaseApiResponse
    {
        public List<BrandModel> GetBrands { get; set; }
    }

    public class GetCasesResponse: BaseApiResponse
    {
        public List<CaseModel> GetCases { get; set; }
    }

    public class GetFuelsResponse : BaseApiResponse
    {
        public List<FuelModel> GetFuels { get; set; }
    }

    public class GetGearsResponse : BaseApiResponse
    {
        public List<GearModel> GetGears { get; set; }
    }

    public class GetModelsResponse : BaseApiResponse
    {
        public List<ModelModel> GetModels { get; set; }
    }

    public class GetSeriesResponse : BaseApiResponse
    {
        public List<SeriesModel> GetSeries { get; set; }
    }

    public class GetPlatesResponse : BaseApiResponse
    {
        public List<PlateModel> GetPlates { get; set; }
    }

    public class GetTractionsResponse : BaseApiResponse
    {
        public List<TractionModel> GetTractions { get; set; }
    }

    public class GetUsersResponse : BaseApiResponse
    {
        public List<UserModel> GetUsers { get; set; }
    }

    public class GetUserResponse : BaseApiResponse
    {
        public UserModel GetUser { get; set; }
    }

    public class GetCarResponse : BaseApiResponse
    {
        public CarModel Car { get; set; }
        public CarDetailsModel CarDetails { get; set; }
        public List<CarDetailPhotosModel> CarPhotos { get; set; }
    }

    public class GetCarsResponse : BaseApiResponse
    {
        public List<GetCarRequest> GetCars { get; set; }
    }

    public class GetSoldCarsResponse : BaseApiResponse
    {
        public List<GetCarRequest> GetCars { get; set; }
        public int PageCount { get; set; }
        public int ItemCount { get; set; }
    }

    public class GetLastAddedResponse : BaseApiResponse
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class GetDetailsResponse : BaseApiResponse
    {
        public GetDetailsRequest GetDetails { get; set; }
    }
}