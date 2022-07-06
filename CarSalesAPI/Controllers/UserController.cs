using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CarSalesAPI.Models;
using CarSalesAPI.Converters;
using System.Net.Mail;
using System.Net;

namespace CarSalesAPI.Controllers
{
    public class UserController : ApiController
    {
        private static UserController _instance;

        public static UserController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserController();
            }

            return _instance;
        }

        [HttpPost, Route("api/User/Login")]
        public GetUserResponse Login(UserModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetUserResponse res = new GetUserResponse()
                    {
                        GetUser = new UserModel()
                    };

                    var data = context.Users.SingleOrDefault(x => x.Mail == input.Mail && x.isDeleted == false);

                    if (data == null)
                    {
                        res.Success = true;
                        res.Status = 1;
                        res.StatusMessage = "Hatalı Mail!";
                    }
                    else
                    {
                        string Password = Crypto.Decrypt(data.Password);

                        if (Password != input.Password)
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
        public BaseApiResponse UpdateUser(UserModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var userData = context.Users.SingleOrDefault(x => x.ID == input.ID);

                    userData.Name = input.Name;
                    userData.Surname = input.Surname;
                    userData.Mail = input.Mail;

                    context.SaveChanges();

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
        public BaseApiResponse UpdatePassword(UpdatePasswordRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var passData = context.Users.SingleOrDefault(x => x.ID == input.ID);

                    string Password = Crypto.Decrypt(passData.Password);

                    BaseApiResponse res = new BaseApiResponse()
                    {
                        Success = true,
                        Status = 1,
                    };

                    if (Password != input.oldPassword)
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
        public BaseApiResponse ForgotPassword(UserModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = context.Users.SingleOrDefault(x => x.Mail == input.Mail && x.isDeleted == false);

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
                    Success = false,
                    Status = 0,
                    StatusMessage = _ex.Message
                };

                return res;
            }
        }

        [HttpPost, Route("api/User/GetUser")]
        public GetUserResponse GetUser(UserModel input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    var data = context.Users.Single(x => x.ID == input.ID);

                    UserModel model = new UserModel()
                    {
                        ID = data.ID,
                        Mail = data.Mail,
                        Name = data.Name,
                        Surname = data.Surname,
                        Password = data.Password,
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
        public GetUsersResponse GetUsers()
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    GetUsersResponse res = new GetUsersResponse()
                    {
                        GetUsers = new List<UserModel>()
                    };

                    var data = context.Users.Where(x => x.isDeleted == false);

                    foreach (var item in data)
                    {
                        UserModel model = new UserModel()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Surname = item.Surname,
                            Mail = item.Mail,
                            Password = item.Password,
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
        public BaseApiResponse AddOrEditUser(AddOrEditUserRequest input)
        {
            try
            {
                using (var context = new CarSalesEntities())
                {
                    if (input.isAdd)
                    {
                        Users data = new Users()
                        {
                            Name = input.Name,
                            Surname = input.Surname,
                            Mail = input.Mail,
                            Password = input.Password
                        };
                    }
                    else
                    {
                        var data = context.Users.Single(x => x.ID == input.ID);

                        if (input.isEdit)
                        {
                            data.Name = input.Name;
                            data.Surname = input.Surname;
                            data.Mail = input.Mail;
                            data.Password = input.Password;
                        }
                        else if (input.isDelete)
                        {
                            data.isDeleted = true;
                        }
                    }

                    context.SaveChanges();

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