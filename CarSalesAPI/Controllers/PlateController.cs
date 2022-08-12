using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class PlateController : ApiController
    {
        [HttpGet, Route("api/Plate/GetPlates")]
        public async Task<GetPlatesResponse> GetPlates()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetPlatesResponse res = new GetPlatesResponse()
                    {
                        GetPlates = new List<PlateModel>()
                    };

                    var data = await context.Plate.Where(x => x.isDeleted == false).ToListAsync();

                    foreach (var item in data)
                    {
                        PlateModel model = new PlateModel()
                        {
                            ID = item.ID,
                            Name = item.Name
                        };

                        res.GetPlates.Add(model);
                    }

                    res.GetPlates = res.GetPlates.OrderBy(x => x.Name).ToList();

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetPlatesResponse res = new GetPlatesResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Plate/AddOrEditPlate")]
        public async Task<GetLastAddedResponse> AddOrEditPlate(AddOrEditRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetLastAddedResponse res = new GetLastAddedResponse();

                    if (input.isAdd)
                    {
                        Plate data = new Plate()
                        {
                            Name = input.Name
                        };

                        var dataRes = context.Plate.Add(data);
                        await context.SaveChangesAsync();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = await context.Plate.SingleOrDefaultAsync(x => x.ID == input.ID);

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