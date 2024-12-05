using MobileReimbursement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace MobileReimbursement.BusinessClass
{
    public class CustomJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            this.ContentType = "text/html";
            //context.HttpContext.Response.Write("<textarea>");
            base.ExecuteResult(context);
            //context.HttpContext.Response.Write("</textarea>");
            context.HttpContext.Response.End();
        }
    }

    public class HomeController : Controller
    {
        string nexttime = System.Configuration.ConfigurationManager.AppSettings["nexttime"].ToString();
        Int32 applicationcode = Convert.ToInt32(ConfigurationManager.AppSettings["applicationcode"].ToString());
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Login()
        {

            DatabaseRepository dr = new DatabaseRepository();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            SuccessfactorOdataServiceReference.SuccessfactorOdataService service = new SuccessfactorOdataServiceReference.SuccessfactorOdataService();
            bool log = false;
            if (Request.Cookies["SSOCookies"] != null)
            {
                HttpCookie reqCookies = Request.Cookies["SSOCookies"];
                if (reqCookies != null)
                {
                    string token = reqCookies["Token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        //dt = dr.GetSSODetails(token, applicationcode, nexttime);
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                // Valid User Create Sesssion
                                string Employeeid = dt.Rows[0]["VEMPLOYEEID"].ToString();
                                var udl = service.UserDetails(Employeeid);
                                foreach (var item in udl)
                                {
                                    uld = new UserLoginData
                                    {
                                        EMPNO = Employeeid,
                                        EMPNAME = item.Empname,
                                        MailID = item.Emailid,
                                        MobileNo = item.Mobile,
                                        STATUS = 0,
                                        Token = token
                                    };
                                }
                                //CPLHelperService.CPLHelperServiceSoapClient sc = new CPLHelperService.CPLHelperServiceSoapClient();
                                //var rolelist = sc.GetEmployeeApplicationUserRole(Convert.ToInt64(ConfigurationManager.AppSettings["applicationcode"].ToString()), uld.EMPNO);

                                dr.BindRoleCode(uld);

                                string empno = "0";
                                switch (uld.Rolecode)
                                {
                                    //Admin
                                    case 1: empno = uld.EMPNO; break;
                                    //Department
                                    case 2: empno = uld.EMPNO; break;
                                    //Supper Admin
                                    case 3: empno = uld.EMPNO; break;
                                    //General
                                    case 4: empno = "0"; break;
                                    //break;
                                    default: empno = uld.EMPNO; break;
                                }

                                uld.ApplicationCode = applicationcode;
                                List<Applicationmenu> objApplicationmenuList = dr.ApplicationMenuList(applicationcode, empno);

                                var roleid = (from n in objApplicationmenuList
                                              where n.Rolecode == objApplicationmenuList.Min(n2 => n2.Rolecode)
                                              select n.Rolecode).FirstOrDefault();
                                List<Applicationmenu> list = new List<Applicationmenu>();
                                list = (from n in objApplicationmenuList
                                        where n.Rolecode.Equals(roleid)
                                        select n).ToList();


                                if (objApplicationmenuList.Count > 0)
                                {
                                    Session["UserObject"] = uld;
                                    Session["UserMenu"] = list;
                                    Session["LoginMsg"] = "Login Successful.....";
                                }


                                //Set Cookies
                                reqCookies.Expires = DateTime.Now.AddHours(Convert.ToDouble(nexttime));
                                Response.SetCookie(reqCookies);
                                log = true;
                            }
                        }
                        else
                            Response.Cookies.Remove("SSOCookies");
                    }
                    else
                        Response.Cookies.Remove("SSOCookies");


                }

            }


            String t1 = (String)Session["LogOutMessage"];
            ViewBag.IsSessionOut = (String)Session["LogOutMessage"];

            ViewBag.Success = TempData["Success"] != null ? Convert.ToString(TempData["Success"]) : string.Empty;
            ViewBag.Error = TempData["Error"] != null ? Convert.ToString(TempData["Error"]) : string.Empty;
            if (TempData["Success"] != null)
                TempData.Remove("Success");

            if (TempData["Error"] != null)
                TempData.Remove("Error");

            if (log)
                return RedirectToAction("Index", "Home");
            else
                return View();

        }

        [HttpPost]
        public ActionResult Login(LoginModel LM, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                DatabaseRepository dr = new DatabaseRepository();

                string Employeeid = fc["uid"] != null ? fc["uid"].ToString() : string.Empty;
                string Pwd = fc["pwd"] != null ? fc["pwd"].ToString() : string.Empty;

                Int32 nStatus = dr.CheckLogin(Employeeid, Pwd);

                LM.LoginError = (String)Session["LoginMsg"];
                LM.MessageCode = nStatus.ToString();
                UserLoginData uld = new UserLoginData();
                if (nStatus == 0)
                {
                    uld = (UserLoginData)Session["UserObject"];
                    HttpCookie SCookies = new HttpCookie("SSOCookies");
                    SCookies["Token"] = uld.Token;
                    //StudentCookies.Expires = DateTime.Now.AddDays(1);
                    uld.Nexttime = nexttime;
                    SCookies.Expires = DateTime.Now.AddHours(Convert.ToDouble(nexttime));
                    Response.SetCookie(SCookies);
                   // dr.InsertIntoSSO(uld);

                    switch (uld.Rolecode)
                    {
                        //Admin
                        case 1:
                            return RedirectToAction("Index", "Home");

                        //Supper Admin
                        case 3:
                            return RedirectToAction("Index", "Home");
                        //break;
                        default:
                            return RedirectToAction("Index", "Home");
                            //break;
                    }
                }
                else
                {
                    ViewBag.IsSessionOut = LM.LoginError;
                    return View(LM);
                }
            }
            else
            {
                return View(LM);
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection fc)
        {

            DatabaseRepository dr = new DatabaseRepository();
            string Employeeid = fc["c_uid"] != null ? fc["c_uid"].ToString() : string.Empty;
            string OldPwd = fc["c_pwd"] != null ? fc["c_pwd"].ToString() : string.Empty;
            string NewPwd = fc["c_newpwd"] != null ? fc["c_newpwd"].ToString() : string.Empty;
            Int32 nStatus = dr.CheckLogin(Employeeid, OldPwd);

            if (nStatus == 0)
            {
                nStatus = dr.ChangePassword(Employeeid, OldPwd, NewPwd);
                if (nStatus == 0)
                    TempData["Success"] = "Password reset successfuly..";
                else
                    TempData["Error"] = "Sorry something went wrong when updating password..";

                return RedirectToAction("Login", "Home");
            }
            else
            {
                TempData["Error"] = "Invalid Username and Password..";
                return RedirectToAction("Login", "Home");
            }

        }

        public ActionResult Logout()
        {
            DatabaseRepository dr = new DatabaseRepository();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();


            if (Request.Cookies["SSOCookies"] != null)
            {
                HttpCookie reqCookies = Request.Cookies["SSOCookies"];
                if (reqCookies != null)
                {
                    string token = reqCookies["Token"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        dr.DeleteSSODetails(token);
                        reqCookies.Expires = DateTime.Now.AddHours(-4);
                        Response.Cookies.Remove("SSOCookies");
                        //Response.Cookies.Clear();
                    }
                }
            }
            return View();
        }

        public CustomJsonResult ContinueSession()
        {
            Session.Timeout = 20;
            return new CustomJsonResult { Data = new { message = "Session refreshed..........", MessageCode = "0" } };
        }
    }
}