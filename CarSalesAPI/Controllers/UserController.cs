using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CarSalesAPI.Models;
using CarSalesAPI.Converters;
using System.Net.Mail;
using System.Net;
using System.Data.Entity;
using System.Threading.Tasks;

namespace CarSalesAPI.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost, Route("api/User/Login")]
        public async Task<GetUserResponse> Login(UserModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetUserResponse res = new GetUserResponse()
                    {
                        GetUser = new UserModel()
                    };

                    var data = await context.Users.SingleOrDefaultAsync(x => x.Mail == input.Mail && x.isDeleted == false);

                    if (data == null)
                    {
                        res.Success = true;
                        res.Status = 1;
                        res.StatusMessage = "Hatalı Mail!";
                    }
                    else
                    {
                        byte[] Password = Crypto.Encrypt(input.StringPassword);

                        if (!ByteCompare.ByteArrayCompare(Password, data.Password))
                        {
                            res.Success = true;
                            res.Status = 1;
                            res.StatusMessage = "Hatalı Şifre";
                        }
                        else
                        {
                            UserModel model = new UserModel()
                            {
                                ID = data.ID,
                                Mail = data.Mail,
                                Name = data.Name,
                                Surname = data.Surname,
                                FullName = data.Name + " " + data.Surname
                            };

                            res.GetUser = model;
                            res.Success = true;
                            res.Status = 1;
                            res.StatusMessage = "OK";
                        }
                    }

                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetUserResponse res = new GetUserResponse
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/User/UpdateUser")]
        public async Task<BaseApiResponse> UpdateUser(UserModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var userData = await context.Users.SingleOrDefaultAsync(x => x.ID == input.ID);

                    userData.Name = input.Name;
                    userData.Surname = input.Surname;
                    userData.Mail = input.Mail;

                    await context.SaveChangesAsync();

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                        StatusMessage = "OK"
                    };

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/User/UpdatePassword")]
        public async Task<BaseApiResponse> UpdatePassword(UpdatePasswordRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var passData = await context.Users.SingleOrDefaultAsync(x => x.ID == input.ID);

                    byte[] Password = Crypto.Encrypt(input.oldPassword);

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                    };

                    if (!ByteCompare.ByteArrayCompare(Password, passData.Password))
                    {
                        res.StatusMessage = "Hatalı Eski Şifre!";
                    }
                    else
                    {
                        passData.Password = Crypto.Encrypt(input.Password);
                        context.SaveChanges();

                        res.StatusMessage = "OK";
                    }

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
                {
                    Success = true,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/User/ForgotPassword")]
        public async Task<BaseApiResponse> ForgotPassword(UserModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = await context.Users.SingleOrDefaultAsync(x => x.Mail == input.Mail && x.isDeleted == false);

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                    };

                    if (data == null)
                    {
                        res.StatusMessage = "Mail Adresi Yanlış";
                    }
                    else
                    {
                        var newPass = RandomStringGenerator.RandomString(6);

                        var senderEmail = "alpyorukyazilim@outlook.com";
                        var recieverEmail = input.Mail;
                        var password = "yazilimalpyoruk2022";
                        var sub = "Şifre Sıfırlama İsteği";
                        var body = "Yeni şifreniz: " + newPass;
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.outlook.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail, password)
                        };
                        using (var mess = new MailMessage(senderEmail, recieverEmail)
                        {
                            Subject = sub,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        };

                        data.Password = Crypto.Encrypt(newPass);
                        await context.SaveChangesAsync();

                        res.StatusMessage = "OK";
                    }

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/User/GetUser")]
        public async Task<GetUserResponse> GetUser(UserModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = await context.Users.SingleOrDefaultAsync(x => x.ID == input.ID);

                    UserModel model = new UserModel()
                    {
                        ID = data.ID,
                        Mail = data.Mail,
                        Name = data.Name,
                        Surname = data.Surname,
                        FullName = data.Name + " " + data.Surname
                    };

                    GetUserResponse res = new GetUserResponse()
                    {
                        GetUser = model,
                        Success = true,
                        Status = 1,
                        StatusMessage = "OK"
                    };

                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetUserResponse res = new GetUserResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpGet, Route("api/User/GetUsers")]
        public async Task<GetUsersResponse> GetUsers()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetUsersResponse res = new GetUsersResponse()
                    {
                        GetUsers = new List<UserModel>()
                    };

                    var data = await context.Users.Where(x => x.isDeleted == false).ToListAsync();

                    foreach (var item in data)
                    {
                        UserModel model = new UserModel()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Surname = item.Surname,
                            Mail = item.Mail,
                            FullName = item.Name + " " + item.Surname
                        };

                        res.GetUsers.Add(model);
                    }

                    res.Success = true;
                    res.Status = 1;
                    res.StatusMessage = "OK";
                    return res;
                }
            }
            catch (Exception _ex)
            {
                GetUsersResponse res = new GetUsersResponse()
                {
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/User/AddOrEditUser")]
        public async Task<BaseApiResponse> AddOrEditUser(AddOrEditUserRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    byte[] Password = Crypto.Encrypt(input.Password);

                    if (input.isAdd)
                    {
                        Users data = new Users()
                        {
                            Name = input.Name,
                            Surname = input.Surname,
                            Mail = input.Mail,
                            Password = Password
                        };
                    }
                    else
                    {
                        var data = await context.Users.SingleOrDefaultAsync(x => x.ID == input.ID);

                        if (input.isEdit)
                        {
                            data.Name = input.Name;
                            data.Surname = input.Surname;
                            data.Mail = input.Mail;
                            data.Password = Password;
                        }
                        else if (input.isDelete)
                        {
                            data.isDeleted = true;
                        }
                    }

                    await context.SaveChangesAsync();

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                        StatusMessage = "OK"
                    };

                    return res;
                }
            }
            catch (Exception _ex)
            {
                BaseApiResponse res = new BaseApiResponse()
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