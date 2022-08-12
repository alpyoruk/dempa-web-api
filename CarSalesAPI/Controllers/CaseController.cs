using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class CaseController : ApiController
    {
        [HttpGet, Route("api/Case/GetCases")]
        public async Task<GetCasesResponse> GetCases()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetCasesResponse res = new GetCasesResponse()
                    {
                        GetCases = new List<CaseModel>()
                    };

                    var data = await context.Case.Where(x => x.isDeleted == false).ToListAsync();

                    foreach (var item in data)
                    {
                        CaseModel model = new CaseModel()
                        {
                            ID = item.ID,
                            Name = item.Name
                        };

                        res.GetCases.Add(model);
                    }

                    res.GetCases = res.GetCases.OrderBy(x => x.Name).ToList();

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetCasesResponse res = new GetCasesResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Case/AddOrEditCase")]
        public async Task<GetLastAddedResponse> AddOrEditCase(AddOrEditRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetLastAddedResponse res = new GetLastAddedResponse();

                    if (input.isAdd)
                    {
                        Case data = new Case()
                        {
                            Name = input.Name
                        };

                        var dataRes = context.Case.Add(data);
                        await context.SaveChangesAsync();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = await context.Case.SingleOrDefaultAsync(x => x.ID == input.ID);

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
                    Success = true,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }
    }
}