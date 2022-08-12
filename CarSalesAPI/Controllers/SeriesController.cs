using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class SeriesController : ApiController
    {
        [HttpPost, Route("api/Series/GetSeries")]
        public async Task<GetSeriesResponse> GetSeries(GetBrandIdOrSeriesId input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetSeriesResponse res = new GetSeriesResponse()
                    {
                        GetSeries = new List<SeriesModel>()
                    };

                    var seriesBrandData = await context.SeriesBrand.Where(x => x.BrandID == input.BrandID).ToListAsync();

                    foreach (var item in seriesBrandData)
                    {
                        var data = await context.Series.Where(x => x.ID == item.SeriesID && x.isDeleted == false).ToListAsync();

                        foreach(var seriesItem in data)
                        {
                            SeriesModel model = new SeriesModel()
                            {
                                ID = seriesItem.ID,
                                Name = seriesItem.Name
                            };

                            res.GetSeries.Add(model);
                        }
                    }

                    res.GetSeries = res.GetSeries.OrderBy(x => x.Name).ToList();
                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch(Exception _ex)
            {
                GetSeriesResponse res = new GetSeriesResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Series/AddOrEditSeries")]
        public async Task<GetLastAddedResponse> AddOrEditSeries(AddOrEditSeriesRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetLastAddedResponse res = new GetLastAddedResponse();

                    if (input.isAdd)
                    {
                        Series data = new Series()
                        {
                            Name = input.Name,
                        };

                        var dataRes = context.Series.Add(data);
                        await context.SaveChangesAsync();

                        SeriesBrand seriesBrandData = new SeriesBrand()
                        {
                            BrandID = input.BrandID,
                            SeriesID = dataRes.ID
                        };

                        context.SeriesBrand.Add(seriesBrandData);
                        await context.SaveChangesAsync();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = await context.Series.SingleOrDefaultAsync(x => x.ID == input.ID);
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