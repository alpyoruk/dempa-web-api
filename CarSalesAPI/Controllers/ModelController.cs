using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class ModelController : ApiController
    {
        [HttpPost, Route("api/Model/GetModels")]
        public GetModelsResponse GetModels(GetBrandIdOrSeriesId input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetModelsResponse res = new GetModelsResponse()
                    {
                        GetModels = new List<ModelModel>()
                    };

                    var modelSeriesData = context.ModelSeries.Where(x => x.SeriesID == input.SeriesID);

                    foreach (var item in modelSeriesData)
                    {
                        var data = context.Model.Where(x => x.ID == item.ModelID && x.isDeleted == false) ;

                        foreach(var modelItem in data)
                        {
                            ModelModel model = new ModelModel()
                            {
                                ID = modelItem.ID,
                                Name = modelItem.Name
                            };

                            res.GetModels.Add(model);
                        }                    
                    }

                    res.GetModels = res.GetModels.OrderBy(x => x.Name).ToList();
                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetModelsResponse res = new GetModelsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Model/AddOrEditModel")]
        public GetLastAddedResponse AddOrEditModel(AddOrEditModelRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetLastAddedResponse res = new GetLastAddedResponse();

                    if (input.isAdd)
                    {
                        Model data = new Model()
                        {
                            Name = input.Name
                        };

                        var dataRes = context.Model.Add(data);

                        ModelSeries modelSeriesData = new ModelSeries()
                        {
                            SeriesID = input.SeriesID,
                            ModelID = dataRes.ID
                        };

                        context.ModelSeries.Add(modelSeriesData);
                        context.SaveChanges();
                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = context.Model.Single(x => x.ID == input.ID);

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