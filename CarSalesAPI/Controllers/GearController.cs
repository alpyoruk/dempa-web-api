using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class GearController : ApiController
    {
        [HttpGet, Route("api/Gear/GetGears")]
        public async Task<GetGearsResponse> GetGears()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetGearsResponse res = new GetGearsResponse()
                    {
                        GetGears = new List<GearModel>()
                    };

                    var data = await context.Gear.Where(x => x.isDeleted == false).ToListAsync();

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
        public async Task<GetLastAddedResponse> AddOrEditGear(AddOrEditRequest input)
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
                        await context.SaveChangesAsync();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = await context.Gear.SingleOrDefaultAsync(x => x.ID == input.ID);

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