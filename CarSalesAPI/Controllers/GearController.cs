using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class GearController : ApiController
    {
        private static GearController _instance;

        public static GearController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GearController();
            }

            return _instance;
        }

        [HttpGet, Route("api/Gear/GetGears")]
        public GetGearsResponse GetGears()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetGearsResponse res = new GetGearsResponse()
                    {
                        GetGears = new List<GearModel>()
                    };

                    var data = context.Gear.Where(x => x.isDeleted == false);

                    foreach (var item in data)
                    {
                        GearModel model = new GearModel()
                        {
                            ID = item.ID,
                            Name = item.Name
                        };

                        res.GetGears.Add(model);
                    }

                    res.GetGears = res.GetGears.OrderBy(x => x.Name).ToList();

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetGearsResponse res = new GetGearsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Gear/AddOrEditGear")]
        public GetLastAddedResponse AddOrEditGear(AddOrEditRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetLastAddedResponse res = new GetLastAddedResponse();

                    if (input.isAdd)
                    {
                        Gear data = new Gear()
                        {
                            Name = input.Name
                        };

                        var dataRes = context.Gear.Add(data);
                        context.SaveChanges();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = context.Gear.Single(x => x.ID == input.ID);

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