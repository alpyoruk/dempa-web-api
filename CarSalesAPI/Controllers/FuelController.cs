using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class FuelController : ApiController
    {
        private static FuelController _instance;

        public static FuelController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FuelController();
            }

            return _instance;
        }

        [HttpGet, Route("api/Fuel/GetFuels")]
        public GetFuelsResponse GetFuels()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetFuelsResponse res = new GetFuelsResponse()
                    {
                        GetFuels = new List<FuelModel>()
                    };

                    var data = context.Fuel.Where(x => x.isDeleted == false);

                    foreach (var item in data)
                    {
                        FuelModel model = new FuelModel()
                        {
                            ID = item.ID,
                            Name = item.Name
                        };

                        res.GetFuels.Add(model);
                    }

                    res.GetFuels = res.GetFuels.OrderBy(x => x.Name).ToList();

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetFuelsResponse res = new GetFuelsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Fuel/AddOrEditFuel")]
        public GetLastAddedResponse AddOrEditFuel(AddOrEditRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetLastAddedResponse res = new GetLastAddedResponse();

                    if (input.isAdd)
                    {
                        Fuel data = new Fuel()
                        {
                            Name = input.Name
                        };

                        var dataRes = context.Fuel.Add(data);
                        context.SaveChanges();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = context.Fuel.Single(x => x.ID == input.ID);

                        if (input.isEdit)
                        {
                            data.Name = input.Name;
                        }
                        else if (input.isDelete)
                        {
                            data.isDeleted = true;
                        }

                        context.SaveChanges();
                    }

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetLastAddedResponse res = new GetLastAddedResponse()
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