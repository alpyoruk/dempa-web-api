using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class FuelController : ApiController
    {
        [HttpGet, Route("api/Fuel/GetFuels")]
        public async Task<GetFuelsResponse> GetFuels()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetFuelsResponse res = new GetFuelsResponse()
                    {
                        GetFuels = new List<FuelModel>()
                    };

                    var data = await context.Fuel.Where(x => x.isDeleted == false).ToListAsync();

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
        public async Task<GetLastAddedResponse> AddOrEditFuel(AddOrEditRequest input)
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
                        await context.SaveChangesAsync();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = await context.Fuel.SingleOrDefaultAsync(x => x.ID == input.ID);

                        if (input.isEdit)
                        {
                            data.Name = input.Name;
                        }
                        else if (input.isDelete)
                        {
                            data.isDeleted = true;
                        }

                        await context.SaveChangesAsync();
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