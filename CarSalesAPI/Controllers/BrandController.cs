﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CarSalesAPI.Models;

namespace CarSalesAPI.Controllers
{
    public class BrandController : ApiController
    {
        [HttpGet, Route("api/Brand/GetBrands")]
        public async Task<GetBrandsResponse> GetBrands()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetBrandsResponse res = new GetBrandsResponse()
                    {
                        GetBrands = new List<BrandModel>()
                    };

                    var data = await context.Brand.Where(x => x.isDeleted == false).ToListAsync();

                    foreach (var item in data)
                    {
                        BrandModel model = new BrandModel()
                        {
                            ID = item.ID,
                            Name = item.Name
                        };

                        res.GetBrands.Add(model);
                    }

                    res.GetBrands = res.GetBrands.OrderBy(x => x.Name).ToList();
                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetBrandsResponse res = new GetBrandsResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/Brand/AddOrEditBrand")]
        public async Task<GetLastAddedResponse> AddOrEditBrand(AddOrEditRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetLastAddedResponse res = new GetLastAddedResponse();

                    if (input.isAdd)
                    {
                        Brand data = new Brand()
                        {
                            Name = input.Name
                        };

                        var dataRes = context.Brand.Add(data);
                        await context.SaveChangesAsync();

                        res.ID = dataRes.ID;
                        res.Name = dataRes.Name;
                    }
                    else
                    {
                        var data = await context.Brand.SingleOrDefaultAsync(x => x.ID == input.ID);

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