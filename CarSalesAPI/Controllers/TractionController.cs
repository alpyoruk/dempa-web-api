using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class TractionController : ApiController
    {
        [HttpGet, Route("api/Traction/GetTractions")]
        public async Task<GetTractionsResponse> GetTractions()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetTractionsResponse res = new GetTractionsResponse()
                    {
                        GetTractions = new List<TractionModel>()
                    };

                    var data = await context.Traction.Where(x => x.isDeleted == false).ToListAsync();

                    foreach (var item in data)
                    {
                        TractionModel model = new TractionModel()
                        {
                            ID = item.ID,
                            Name = item.Name
                        };

                        res.GetTractions.Add(model);
                    }

                    res.GetTractions = res.GetTractions.OrderBy(x => x.Name).ToList();

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetTractionsResponse res = new GetTractionsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Traction/AddOrEditTraction")]
        public async Task<GetLastAddedResponse> AddOrEditTraction(AddOrEditRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetLastAddedResponse res = new GetLastAddedResponse();

                    if (input.isAdd)
                    {
                        Traction data = new Traction()
                        {
                            Name = input.Name
                        };

                        var dataRes = context.Traction.Add(data);
                        await context.SaveChangesAsync();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = await context.Traction.SingleOrDefaultAsync(x => x.ID == input.ID);

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