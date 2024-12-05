using MobileReimbursement.BusinessClass;
using MobileReimbursement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;

namespace MobileReimbursement.Controllers
{
    public class AssetController : Controller
    {
        private DatabaseRepository dr = new DatabaseRepository();

        public void DataTableColumnRename(DataTable dt, Dictionary<string, string> d)
        {
            foreach (KeyValuePair<string, string> pair in d)
            {
                dt.Columns[pair.Key].ColumnName = pair.Value;
            }
        }

        private IQueryable<T> SortIQueryableFileData<T>(IQueryable<T> data, string fieldName, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) return data;
            if (string.IsNullOrWhiteSpace(sortOrder)) return data;

            var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "i");
            System.Linq.Expressions.Expression conversion = System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression.Property(param, fieldName), typeof(object));
            var mySortExpression = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(conversion, param);

            return (sortOrder == "desc") ? data.OrderByDescending(mySortExpression)
                : data.OrderBy(mySortExpression);
        }

        public ActionResult MobileMaster()
        {
            return View();
        }

        public ActionResult ReimbursementClaim()
        {
            return View();
        }

        public ActionResult ReimbursementApproval()
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            asset_report objasset_report = new asset_report();

            try
            {
                //objasset_report.Status = Request.QueryString["status"];
                objasset_report.FromDate = !string.IsNullOrEmpty(Request.QueryString["fdate"]) ? Convert.ToString(Request.QueryString["fdate"]) : string.Empty;
                objasset_report.ToDate   = !string.IsNullOrEmpty(Request.QueryString["tdate"]) ? Convert.ToString(Request.QueryString["tdate"]) : string.Empty;

                dtTable = dr.AssetRequestStatus(objasset_report, out status, out returnmessage);

                if (dtTable.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "inline; filename=AssetRequestStatus.xls"); // Download File Name.
                    Response.Write("<table border='1 px' style='width: 941px; cellspacing=0 frame=void rules=none border-color: #C0C0C0;'><tr style='background-color: #FFFFCC'>");

                    for (int i = 0; i < dtTable.Columns.Count; i++)
                    {
                        Response.Write("<td style='height: 22px; font-weight: lighter; background-color: lightcyan; text-align: center;'>");
                        Response.Write(dtTable.Columns[i].ColumnName.ToString());
                        Response.Write("</span></td>");
                    }
                    Response.Write("</tr>");
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        Response.Write("<tr>");
                        for (int j = 0; j < dtTable.Columns.Count; j++)
                        {
                            Response.Write("<td style='height: 22px;'>" + dtTable.Rows[i][j].ToString() + " </td>");
                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</table>");
                    Response.End();
                    status = 0;
                }
            }
            catch
            {
            }
            return View();
        }

        public ActionResult MonthlyReport()
        {
            return View();
        }

        public ActionResult DepartmentwiseReport()
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            asset_report objasset_report = new asset_report();

            try
            {
                objasset_report.Status = Request.QueryString["status"];
                objasset_report.FromDate = !string.IsNullOrEmpty(Request.QueryString["fdate"]) ? Convert.ToString(Request.QueryString["fdate"]) : string.Empty;
                objasset_report.ToDate   = !string.IsNullOrEmpty(Request.QueryString["tdate"]) ? Convert.ToString(Request.QueryString["tdate"]) : string.Empty;

                dtTable = dr.DownloadReport(objasset_report, out status, out returnmessage);

                if (dtTable.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "inline; filename=AssetReport.xls"); // Download File Name.
                    Response.Write("<table border='1 px' style='width: 941px; cellspacing=0 frame=void rules=none border-color: #C0C0C0;'><tr style='background-color: #FFFFCC'>");

                    for (int i = 0; i < dtTable.Columns.Count; i++)
                    {
                        Response.Write("<td style='height: 22px; font-weight: lighter; background-color: lightcyan; text-align: center;'>");
                        Response.Write(dtTable.Columns[i].ColumnName.ToString());
                        Response.Write("</span></td>");
                    }
                    Response.Write("</tr>");
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        Response.Write("<tr>");
                        for (int j = 0; j < dtTable.Columns.Count; j++)
                        {
                            Response.Write("<td style='height: 22px;'>" + dtTable.Rows[i][j].ToString() + " </td>");
                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</table>");
                    Response.End();
                    status = 0;
                }
            }
            catch
            {
            }
            return View();
        }

        public ActionResult selfRequest()
        {
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();

            dtTable = dr.GetHOD();

            var a = (from d in dtTable.AsEnumerable()
                     select new AssetComman
                     {
                         HOD = d.Field<dynamic>("HOD")
                     }).FirstOrDefault();
            ViewBag.hod = a.HOD;

            return View();
        }

        [HttpPost]
        public JsonResult GetItem()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetItem();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NTYPECODE"] != null ? Convert.ToString(row["NTYPECODE"]) : "0",
                    Drptext = row["VTYPEDESCRIPTION"] != null ? Convert.ToString(row["VTYPEDESCRIPTION"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult NewJoiner()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCompany()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetCompany();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["COMPANY"] != null ? Convert.ToString(row["COMPANY"]) : "0",
                    Drptext = row["CMPNAME"] != null ? Convert.ToString(row["CMPNAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        //public JsonResult GetDivision(int? CompanyID)
        public JsonResult GetDivision(string CompanyID)
        {
            if (!string.IsNullOrEmpty(CompanyID))
            {
                var objStatusList = new List<dropdown>();
                dr = new DatabaseRepository();
                DataTable dtTable = dr.GetDivision(CompanyID);
                objStatusList = (from a in dtTable.AsEnumerable()
                                 select new dropdown
                                 {
                                     Drpvalu = a.Field<dynamic>("DIVISION") != null ? Convert.ToString(a.Field<dynamic>("DIVISION")) : 0,
                                     Drptext = a.Field<dynamic>("DIVNAME") != null ? Convert.ToString(a.Field<dynamic>("DIVNAME")) : string.Empty,
                                 }).ToList();
                var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetUnit(string CompanyID, string DivisionID)
        {
            if (!string.IsNullOrEmpty(CompanyID))
            {
                var objStatusList = new List<dropdown>();
                dr = new DatabaseRepository();
                DataTable dtTable = dr.GetUnit(CompanyID, DivisionID);
                objStatusList = (from a in dtTable.AsEnumerable()
                                 select new dropdown
                                 {
                                     Drpvalu = a.Field<dynamic>("UNIT") != null ? Convert.ToString(a.Field<dynamic>("UNIT")) : 0,
                                     Drptext = a.Field<dynamic>("UNTNAME") != null ? Convert.ToString(a.Field<dynamic>("UNTNAME")) : string.Empty,
                                 }).ToList();
                var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDepartment(string CompanyID, string DivisionID, string UnitID)
        {
            if (!string.IsNullOrEmpty(CompanyID))
            {
                var objStatusList = new List<dropdown>();
                dr = new DatabaseRepository();
                DataTable dtTable = dr.GetDepartment(CompanyID, DivisionID, UnitID);
                objStatusList = (from a in dtTable.AsEnumerable()
                                 select new dropdown
                                 {
                                     Drpvalu = a.Field<dynamic>("DEPTCD") != null ? Convert.ToString(a.Field<dynamic>("DEPTCD")) : 0,
                                     Drptext = a.Field<dynamic>("DEPTNAME") != null ? Convert.ToString(a.Field<dynamic>("DEPTNAME")) : string.Empty,
                                 }).ToList();
                var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDesignation(string CompanyID)
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetDesignation(CompanyID);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["MNGTGRP"]  != null ? Convert.ToString(row["MNGTGRP"]) : "0",
                    Drptext = row["MNGTDESC"] != null ? Convert.ToString(row["MNGTDESC"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetLocation()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetLocation();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NLOCATIONCODE"] != null ? Convert.ToString(row["NLOCATIONCODE"]) : "0",
                    Drptext = row["VLOCATIONDESC"] != null ? Convert.ToString(row["VLOCATIONDESC"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAssetHOD()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetAssetHOD();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["EMPNO"] != null ? Convert.ToString(row["EMPNO"]) : "0",
                    Drptext = row["EMPNAME"] != null ? Convert.ToString(row["EMPNAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult AssetRequest(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();

            try
            {
                objam_item_request.Nrequestno      = Convert.ToInt32(fc["hdn_reqno"].ToString());
                objam_item_request.Nreqtype        = Convert.ToInt32(fc["drp_reqtype"].ToString());
                objam_item_request.Nitemno         = Convert.ToInt32(fc["drp_item"].ToString());
                objam_item_request.Vhodempno       = (fc["drp_hod"].ToString());
                objam_item_request.Vreason         = (fc["txt_remarks"].ToString());
                objam_item_request.Nallocationtype = Convert.ToInt32(fc["drp_allocation"].ToString());
                objam_item_request.Dallocationdate = !string.IsNullOrEmpty(fc["txt_allocationdate"].ToString()) ? fc["txt_allocationdate"].ToString() : string.Empty;

                if (fc["txt_empname"] != null)
                    objam_item_request.Vempname = !string.IsNullOrEmpty(fc["txt_empname"].ToString()) ? fc["txt_empname"].ToString() : string.Empty;
                else
                    objam_item_request.Vempname = string.Empty;

                if (fc["drp_company"] != null)
                    objam_item_request.Ncompany = !string.IsNullOrEmpty(fc["drp_company"].ToString()) ? fc["drp_company"].ToString() : string.Empty;
                
                if (fc["drp_division"] != null)
                    objam_item_request.Ndivision = !string.IsNullOrEmpty(fc["drp_division"].ToString()) ? fc["drp_division"].ToString() : string.Empty;

                if (fc["drp_department"] != null)
                    objam_item_request.Ndepartment = !string.IsNullOrEmpty(fc["drp_department"].ToString())? fc["drp_department"].ToString() : string.Empty;

                if (fc["drp_designation"] != null)
                    objam_item_request.Ndesignation = !string.IsNullOrEmpty(fc["drp_designation"].ToString()) ? fc["drp_designation"].ToString() : string.Empty;
                
                if (fc["drp_location"] != null)
                    objam_item_request.Nlocation = !string.IsNullOrEmpty(fc["drp_location"].ToString()) ? Convert.ToInt32(fc["drp_location"].ToString()) : (Int32?)null;
                else
                    objam_item_request.Nlocation = (Int32?)0;

                if (fc["txt_joindate"] != null)
                    objam_item_request.Djoiningdate = !string.IsNullOrEmpty(fc["txt_joindate"].ToString()) ? fc["txt_joindate"].ToString() : string.Empty;
                else
                    objam_item_request.Djoiningdate = string.Empty;

                if (fc["drp_unit"] != null)
                    objam_item_request.Nunit = !string.IsNullOrEmpty(fc["drp_unit"].ToString()) ? fc["drp_unit"].ToString() : string.Empty;
                
                dr.AssetRequest(objam_item_request, out status, out returnmessage);

                status = 0;
                returnmessage = "Asset Request is Submitted Successfully..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public JsonResult GetAssetReqListJQGrid(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;

            Self = Request.QueryString["Self"] != null ? Convert.ToString(Request.QueryString["Self"]) : string.Empty;

            dtTable = dr.GetAssetRequest(Self);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<am_item_request>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new am_item_request
                                     {
                                         Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                                         RequestFor = d.Field<dynamic>("REQUEST"),
                                         ItemName = d.Field<dynamic>("ITEM"),
                                         HODName = d.Field<dynamic>("HOD"),
                                         FCName = d.Field<dynamic>("FC"),
                                         Action = "",
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<am_item_request>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.Nrequestno,
                            cell = new object[] { s.Action, s.Nrequestno, s.RequestFor, s.ItemName, s.HODName, s.FCName }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult fetchAssetReqDetails(String Reqno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            am_item_request a = new am_item_request();
            try
            {
                dtTable = dr.fetchAssetReqDetails(Reqno, out status, out returnmessage);

                a = (from d in dtTable.AsEnumerable()
                     select new am_item_request
                     {
                         //Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                         //Ncompany = d.Field<dynamic>("NCOMPANY") != null ? Convert.ToInt32(d.Field<dynamic>("NCOMPANY")) : 0,
                         //Ndivision = d.Field<dynamic>("NDIVISION") != null ? Convert.ToInt32(d.Field<dynamic>("NDIVISION")) : 0,
                         //Nunit = d.Field<dynamic>("NUNIT") != null ? Convert.ToInt32(d.Field<dynamic>("NUNIT")) : 0,
                         //Ndepartment = d.Field<dynamic>("NDEPARTMENT") != null ? Convert.ToInt32(d.Field<dynamic>("NDEPARTMENT")) : 0,
                         //Ndesignation = d.Field<dynamic>("NDESIGNATION") != null ? Convert.ToInt32(d.Field<dynamic>("NDESIGNATION")) : 0,
                         //Nlocation = d.Field<dynamic>("NLOCATION") != null ? Convert.ToInt32(d.Field<dynamic>("NLOCATION")) : 0,
                         //Nreqtype = d.Field<dynamic>("NREQTYPE") != null ? Convert.ToInt32(d.Field<dynamic>("NREQTYPE")) : 0,
                         //Nitemno = d.Field<dynamic>("NITEMNO") != null ? Convert.ToInt32(d.Field<dynamic>("NITEMNO")) : 0,
                         //Nallocationtype = d.Field<dynamic>("NALLOCATIONTYPE") != null ? Convert.ToInt32(d.Field<dynamic>("NALLOCATIONTYPE")) : 0,
                         //Vempname = d.Field<dynamic>("VEMPNAME") != null ? Convert.ToString(d.Field<dynamic>("VEMPNAME")) : string.Empty,
                         //Djoiningdate = d.Field<dynamic>("DJOININGDATE"),
                         //Vhodempno = d.Field<dynamic>("VHODEMPNO") != null ? Convert.ToString(d.Field<dynamic>("VHODEMPNO")) : string.Empty,
                         //Vreason = d.Field<dynamic>("VREASON") != null ? Convert.ToString(d.Field<dynamic>("VREASON")) : string.Empty,

                         Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                         //Ncompany = d.Field<dynamic>("NCOMPANY") != null ? Convert.ToInt32(d.Field<dynamic>("NCOMPANY")) : string.Empty,
                         Ncompany = d.Field<dynamic>("NCOMPANY") != null ? Convert.ToString(d.Field<dynamic>("NCOMPANY")) : string.Empty,

                         //Ndivision = d.Field<dynamic>("NDIVISION") != null ? Convert.ToInt32(d.Field<dynamic>("NDIVISION")) : string.Empty,
                         Ndivision = d.Field<dynamic>("NDIVISION") != null ? Convert.ToString(d.Field<dynamic>("NDIVISION")) : string.Empty,

                         //Nunit = d.Field<dynamic>("NUNIT") != null ? Convert.ToInt32(d.Field<dynamic>("NUNIT")) : string.Empty,
                         Nunit = d.Field<dynamic>("NUNIT") != null ? Convert.ToString(d.Field<dynamic>("NUNIT")) : string.Empty,

                         //Ndepartment = d.Field<dynamic>("NDEPARTMENT") != null ? Convert.ToInt32(d.Field<dynamic>("NDEPARTMENT")) : string.Empty,
                         Ndepartment = d.Field<dynamic>("NDEPARTMENT") != null ? Convert.ToString(d.Field<dynamic>("NDEPARTMENT")) : string.Empty,

                         //Ndesignation = d.Field<dynamic>("NDESIGNATION") != null ? Convert.ToInt32(d.Field<dynamic>("NDESIGNATION")) : string.Empty,
                         Ndesignation = d.Field<dynamic>("NDESIGNATION") != null ? Convert.ToString(d.Field<dynamic>("NDESIGNATION")) : string.Empty,

                         Nlocation = d.Field<dynamic>("NLOCATION") != null ? Convert.ToInt32(d.Field<dynamic>("NLOCATION")) : 0,
                         Nreqtype = d.Field<dynamic>("NREQTYPE") != null ? Convert.ToInt32(d.Field<dynamic>("NREQTYPE")) : 0,
                         Nitemno = d.Field<dynamic>("NITEMNO") != null ? Convert.ToInt32(d.Field<dynamic>("NITEMNO")) : 0,
                         Nallocationtype = d.Field<dynamic>("NALLOCATIONTYPE") != null ? Convert.ToInt32(d.Field<dynamic>("NALLOCATIONTYPE")) : 0,
                         Vempname = d.Field<dynamic>("VEMPNAME") != null ? Convert.ToString(d.Field<dynamic>("VEMPNAME")) : string.Empty,
                         Djoiningdate = d.Field<dynamic>("DJOININGDATE"),
                         Vhodempno = d.Field<dynamic>("VHODEMPNO") != null ? Convert.ToString(d.Field<dynamic>("VHODEMPNO")) : string.Empty,
                         Vreason = d.Field<dynamic>("VREASON") != null ? Convert.ToString(d.Field<dynamic>("VREASON")) : string.Empty,

                     }).FirstOrDefault();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult DeleteAssetRequest(String Reqno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteAssetRequest(Reqno, out status, out returnmessage);
                status = 0;
                returnmessage = "Asset Request Deleted Successfully..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public JsonResult RequestReminder(String Reqno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.RequestReminder(Reqno, out status, out returnmessage);
                status = 0;
                returnmessage = "Reminder has been sent to approver.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public ActionResult AssetReqHistory(string Reqno, string action)
        {
            ViewBag.Reqno = Reqno;
            ViewBag.action = action;
            return PartialView("_ApprovalHistory");
        }

        public static List<am_item_request> AssetHistory(string Reqno)
        {
            DatabaseRepository dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();

            dtTable = dr.AssetHistory(Reqno);

            List<am_item_request> objlist = new List<am_item_request>(dtTable.Rows.Count);

            objlist = (from d in dtTable.AsEnumerable()
                       select new am_item_request
                       {
                           Vempname = d.Field<dynamic>("APPROVEDBY"),
                           Level = d.Field<dynamic>("CASE"),
                           ApprovalDate = d.Field<dynamic>("DINSERTDATE"),
                           Statusname = d.Field<dynamic>("STATUS"),
                           Vreason = d.Field<dynamic>("VAPPROVEREMARKS") != null ? Convert.ToString(d.Field<dynamic>("VAPPROVEREMARKS")) : string.Empty,
                           Nstatus = d.Field<dynamic>("NSTATUS"),
                           Vempno = d.Field<dynamic>("VEMPNO"),
                       }).ToList();
            return objlist;
        }

        public ActionResult Approval()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAssetApprovalListJQGrid(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;

            dtTable = dr.GetAssetApprovalRequest();

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();
                        
                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<am_item_request>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new am_item_request
                                     {
                                         Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                                         Vempname = d.Field<dynamic>("EMPLOYEE"),
                                         Locationname = d.Field<dynamic>("LOCATION"),
                                         ItemName = d.Field<dynamic>("ITEM"),
                                         Action = "",
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<am_item_request>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.Nrequestno,
                            cell = new object[] { s.Action, s.Nrequestno, s.Vempname, s.Locationname, s.ItemName }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public ActionResult ViewRequest(string Reqno)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();

            dtTable = dr.ViewRequest(Reqno);

            var a = (from d in dtTable.AsEnumerable()
                     select new am_item_request
                     {
                         Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                         RequestFor = d.Field<dynamic>("REQUESTFOR") != null ? Convert.ToString(d.Field<dynamic>("REQUESTFOR")) : string.Empty,
                         ItemName = d.Field<dynamic>("ITEM") != null ? Convert.ToString(d.Field<dynamic>("ITEM")) : string.Empty,
                         Allocation = d.Field<dynamic>("ALLOCATION") != null ? Convert.ToString(d.Field<dynamic>("ALLOCATION")) : string.Empty,
                         Vempname = d.Field<dynamic>("EMPNAME") != null ? Convert.ToString(d.Field<dynamic>("EMPNAME")) : string.Empty,
                         CMPNAME = d.Field<dynamic>("CMPNAME") != null ? Convert.ToString(d.Field<dynamic>("CMPNAME")) : string.Empty,
                         DIVNAME = d.Field<dynamic>("DIVNAME") != null ? Convert.ToString(d.Field<dynamic>("DIVNAME")) : string.Empty,
                         DEPTNAME = d.Field<dynamic>("DEPTNAME") != null ? Convert.ToString(d.Field<dynamic>("DEPTNAME")) : string.Empty,
                         DESIGNATION = d.Field<dynamic>("DESIGNATION") != null ? Convert.ToString(d.Field<dynamic>("DESIGNATION")) : string.Empty,
                         Locationname = d.Field<dynamic>("LOCATION") != null ? Convert.ToString(d.Field<dynamic>("LOCATION")) : string.Empty,
                         Djoiningdate = d.Field<dynamic>("DJOININGDATE") != null ? d.Field<dynamic>("DJOININGDATE") : null,
                         Vreason = d.Field<dynamic>("VREASON") != null ? Convert.ToString(d.Field<dynamic>("VREASON")) : string.Empty,
                         HODCOUNT = d.Field<dynamic>("HODCOUNT") != null ? Convert.ToString(d.Field<dynamic>("HODCOUNT")) : string.Empty,
                     }).FirstOrDefault();

            if (a != null)
            {
                ViewBag.Nrequestno = a.Nrequestno;
                ViewBag.RequestFor = a.RequestFor;
                ViewBag.ItemName = a.ItemName;
                ViewBag.Allocation = a.Allocation;
                ViewBag.Vempname = a.Vempname;
                ViewBag.CMPNAME = a.CMPNAME;
                ViewBag.DIVNAME = a.DIVNAME;
                ViewBag.DEPTNAME = a.DEPTNAME;
                ViewBag.DESIGNATION = a.DESIGNATION;
                ViewBag.Locationname = a.Locationname;
                ViewBag.Djoiningdate = a.Djoiningdate;
                ViewBag.Vreason = a.Vreason;
                ViewBag.HODCOUNT = a.HODCOUNT;
            }

            return PartialView("_ViewAssetRequest");
        }

        public ActionResult userAcceptance()
        {
            ViewBag.ITAdmin = ConfigurationManager.AppSettings["ITAdmin"].ToString();
            return View();
        }

        [HttpPost]
        public JsonResult GetPendingAcceptanceistJQGrid(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;

            dtTable = dr.GetPendingAcceptance();

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<am_item_request>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new am_item_request
                                     {
                                         Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                                         Vempno = d.Field<dynamic>("VALLOCATEDTO"),
                                         Vempname = d.Field<dynamic>("EMPNAME"),
                                         DEPTNAME = d.Field<dynamic>("DEPTNAME"),
                                         Locationname = d.Field<dynamic>("LOCNAME"),
                                         ItemName = d.Field<dynamic>("ITEM"),
                                         SerialNo = d.Field<dynamic>("VSERIALNO"),
                                         AllocationDate = d.Field<dynamic>("DALLOCATIONDATE"),
                                         Engname = d.Field<dynamic>("VALLOCATEDBY"),
                                         Action = "",
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<am_item_request>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.Nrequestno,
                            cell = new object[] { s.Action, s.Nrequestno, s.Vempno, s.Vempname, s.DEPTNAME, s.Locationname, s.ItemName, s.SerialNo, s.AllocationDate, s.Engname }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult UnexpectedDeallocation(String Reqno, string Remarks)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.UnexpectedDeallocation(Reqno, Remarks, out status, out returnmessage);
                status = 0;
                returnmessage = "Asset De Allocation Submitted Successfully..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult pendingAllocation()
        {
            ViewBag.ITAdmin = ConfigurationManager.AppSettings["ITAdmin"].ToString();
            return View();
        }

        [HttpPost]
        public JsonResult GetPendingAllocationListJQGrid(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;

            dtTable = dr.GetpendingAllocation();

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<am_item_request>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new am_item_request
                                     {
                                         Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                                         Dinsertdate = d.Field<dynamic>("DINSERTDATE"),
                                         ItemName = d.Field<dynamic>("ITEM"),
                                         Vempno = d.Field<dynamic>("VEMPNO"),
                                         Vempname = d.Field<dynamic>("EMPNAME"),
                                         DEPTNAME = d.Field<dynamic>("DEPTNAME"),
                                         Engname = d.Field<dynamic>("EMPNAME1"),
                                         DESIGNATION = d.Field<dynamic>("DESIGNATION"),
                                         AllocationDate = d.Field<dynamic>("DJOININGDATE"),
                                         Locationname = d.Field<dynamic>("LOCATION"),
                                         Action = "",
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<am_item_request>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.Nrequestno,
                            cell = new object[] { s.Action, s.Nrequestno, s.Locationname, s.Dinsertdate, s.ItemName, s.Vempno, s.Vempname, s.DEPTNAME, s.Engname, s.DESIGNATION, s.AllocationDate }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult RejectApprovedRequest(String Reqno, string Remarks)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.RejectApprovedRequest(Reqno, Remarks, out status, out returnmessage);
                status = 0;
                returnmessage = "Asset Request Cancelled Successfully..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult Assetallocation()
        {
            ViewBag.Reqno = Request.QueryString["Reqno"] != null ? Convert.ToString(Request.QueryString["Reqno"]) : null;
            ViewBag.Empno = Request.QueryString["Empno"] != null ? Convert.ToString(Request.QueryString["Empno"]) : null;
            return View();
        }

        [HttpPost]
        public JsonResult GetAllocationEmployee(string Search)
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetAllocationEmployee(Search);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["EMPNO"] != null ? Convert.ToString(row["EMPNO"]) : "0",
                    Drptext = row["EMPNAME"] != null ? Convert.ToString(row["EMPNAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public string GetAllocationEmployeeDetails(string Empno)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string jsonstring = string.Empty;
            dtTable = dr.GetAllocationEmployeeDetails(Empno);

            var a = (from d in dtTable.AsEnumerable()
                     select new am_item_request
                     {
                         Vempname = d.Field<dynamic>("EMPNAME"),
                         DEPTNAME = d.Field<dynamic>("DEPTNAME"),
                         DESIGNATION = d.Field<dynamic>("DESGNAME"),
                         Locationname = d.Field<dynamic>("LOCNAME"),
                     }).FirstOrDefault();

            jsonstring = JsonConvert.SerializeObject(a, Formatting.Indented);

            return jsonstring;
        }

        [HttpPost]
        public JsonResult GetItemType()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetItemType();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NITEMNO"] != null ? Convert.ToString(row["NITEMNO"]) : "0",
                    Drptext = row["DESCRIPTION"] != null ? Convert.ToString(row["DESCRIPTION"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetSerialNo(string ItemNo, string Receipt)
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetSerialNo(ItemNo, Receipt);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NASSETNO"] != null ? Convert.ToString(row["NASSETNO"]) : "0",
                    Drptext = row["VSERIALNO"] != null ? Convert.ToString(row["VSERIALNO"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult SubmitAllocationRequest(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();

            try
            {
                objam_item_request.Vempno = (fc["drp_allocate"].ToString());

                if (fc["drp_item"] != "") {
                    objam_item_request.Nitemno = Convert.ToInt32(fc["drp_item"].ToString());
                    objam_item_request.Assetno = Convert.ToString(fc["drp_serialno"].ToString());
                    objam_item_request.Compliance = Convert.ToInt32(fc["drp_compliance"].ToString());
                    objam_item_request.Domain = Convert.ToInt32(fc["drp_domain"].ToString());
                    objam_item_request.OS = Convert.ToInt32(fc["drp_OS"].ToString());
                    objam_item_request.Hostname = Convert.ToString(fc["txt_hostname"].ToString());
                    objam_item_request.Classification = Convert.ToInt32(fc["drp_Classification"].ToString());
                }
                                
                objam_item_request.Dallocationdate = !string.IsNullOrEmpty(fc["txt_allocationdate"].ToString()) ? fc["txt_allocationdate"].ToString() : string.Empty;
                objam_item_request.Vreason = (fc["txt_remarks"].ToString());
                if (fc["hdn_reqno"] != null)
                    objam_item_request.Nrequestno = !string.IsNullOrEmpty(fc["hdn_reqno"].ToString()) ? Convert.ToInt32(fc["hdn_reqno"].ToString()) : (Int32?)null;
                else
                    objam_item_request.Nrequestno = (Int32?)0;

                if (fc["chk_monitor"] != null)
                    objam_item_request.chkbox = 'Y';
                else
                    objam_item_request.chkbox = 'N';

                if (fc["drp_item_monitor"] != null)
                    objam_item_request.Nitemno1 = !string.IsNullOrEmpty(fc["drp_item_monitor"].ToString()) ? Convert.ToInt32(fc["drp_item_monitor"].ToString()) : (Int32?)null;
                else
                    objam_item_request.Nitemno1 = (Int32?)null;

                if (fc["drp_serialno_monitor"] != null)
                    objam_item_request.Assetno1 = !string.IsNullOrEmpty(fc["drp_serialno_monitor"].ToString()) ? Convert.ToInt32(fc["drp_serialno_monitor"].ToString()) : (Int32?)null;
                else
                    objam_item_request.Assetno1 = (Int32?)null;               

                if (fc["chk_printer"] != null)
                    objam_item_request.chkbox1 = 'Y';
                else
                    objam_item_request.chkbox1 = 'N';

                if (fc["drp_item_printer"] != null)
                    objam_item_request.Nitemno2 = !string.IsNullOrEmpty(fc["drp_item_printer"].ToString()) ? Convert.ToInt32(fc["drp_item_printer"].ToString()) : (Int32?)null;
                else
                    objam_item_request.Nitemno2 = (Int32?)null;

                if (fc["drp_serialno_printer"] != null)
                    objam_item_request.Assetno2 = !string.IsNullOrEmpty(fc["drp_serialno_printer"].ToString()) ? Convert.ToInt32(fc["drp_serialno_printer"].ToString()) : (Int32?)null;
                else
                    objam_item_request.Assetno2 = (Int32?)null;
                
                dr.SubmitAllocationRequest(objam_item_request, out status, out returnmessage);

                status = 0;
                returnmessage = "Asset allocation has been successfully..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }
        public ActionResult AssetDeallocation()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetDeAllocationEmployee(string Search)
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetDeAllocationEmployee(Search);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["EMPNO"]   != null ? Convert.ToString(row["EMPNO"]) : "0",
                    Drptext = row["EMPNAME"] != null ? Convert.ToString(row["EMPNAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetAllocatedItems(string Empno)
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetAllocatedItems(Empno);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NREQUESTNO"] != null ? Convert.ToString(row["NREQUESTNO"]) : "0",
                    Drptext = row["Description"] != null ? Convert.ToString(row["Description"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
        public string GetAllocatedItemsDetails(string Itemno)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string jsonstring = string.Empty;
            dtTable = dr.GetAllocatedItemsDetails(Itemno);

            var a = (from d in dtTable.AsEnumerable()
                     select new am_item_request
                     {
                         SerialNo = d.Field<dynamic>("VSERIALNO"),
                         Hostname = d.Field<dynamic>("VHOSTNAME"),
                         Dallocationdate = d.Field<dynamic>("DALLOCATIONDATE"),
                         Vreason = d.Field<dynamic>("VREMARKS"),
                     }).FirstOrDefault();

            jsonstring = JsonConvert.SerializeObject(a, Formatting.Indented);

            return jsonstring;
        }
        public ActionResult SubmitDeAllocationRequest(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();

            try
            {
                objam_item_request.Nrequestno = !string.IsNullOrEmpty(fc["drp_item"].ToString()) ? Convert.ToInt32(fc["drp_item"].ToString()) : (Int32?)null;
                objam_item_request.Nstatus = Convert.ToInt32(fc["drp_action"].ToString());
                objam_item_request.Dallocationdate = !string.IsNullOrEmpty(fc["txt_deallocationdate"].ToString()) ? fc["txt_deallocationdate"].ToString() : string.Empty;
                objam_item_request.Vreason = (fc["txt_DeallocationRemarks"].ToString());

                dr.SubmitDeAllocationRequest(objam_item_request, out status, out returnmessage);

                status = 0;
                returnmessage = "Asset DeAllocation has been successfully..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }
        public ActionResult AssetAcceptance()
        {
            return View();
        }
        [HttpGet]
        public string GetSerialNoDetail(string Serialno)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string jsonstring = string.Empty;
            dtTable = dr.GetSerialNoDetail(Serialno);

            var a = (from d in dtTable.AsEnumerable()
                     select new am_item_request
                     {
                         CMPNAME = d.Field<dynamic>("CMPNAME"),
                         DIVNAME = d.Field<dynamic>("DIVNAME"),
                         UNITNAME = d.Field<dynamic>("UNTNAME"),
                         DEPTNAME = d.Field<dynamic>("DEPTNAME"),
                         Locationname = d.Field<dynamic>("LOCNAME"),
                         Vempno = d.Field<dynamic>("VALLOCATEDTO"),
                         Vempname = d.Field<dynamic>("EMPNAME"),
                         ItemName = d.Field<dynamic>("ITEM"),
                         Statusname = d.Field<dynamic>("ACTUAL_STATUS"),
                         Dallocationdate = d.Field<dynamic>("DALLOCATIONDATE"),
                         Hostname = d.Field<dynamic>("VHOSTNAME"),
                         Engname = d.Field<dynamic>("ENGNAME"),
                         Vreason = d.Field<dynamic>("REMARKS"),
                         Compliancename = d.Field<dynamic>("GXP"),
                         Domainname = d.Field<dynamic>("DOMAIN"),
                         //OSname = d.Field<dynamic>("OS"),
                         OS = d.Field<dynamic>("OS"),
                         Assetno1 = d.Field<dynamic>("NASSETNO"),
                         Vendorname = d.Field<dynamic>("VVENDORNAME"),
                         InvoiceNo = d.Field<dynamic>("VDOCUMENTNO"),
                         DocumentDate = d.Field<dynamic>("DDOCUMENTDATE"),
                     }).FirstOrDefault();

            jsonstring = JsonConvert.SerializeObject(a, Formatting.Indented);

            return jsonstring;
        }
        [HttpPost]
        public JsonResult GetPendingAcceptanceList(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string RequestNo = string.Empty;
            DataView dv = null;

            RequestNo = Request.QueryString["RequestNo"] != null ? Convert.ToString(Request.QueryString["RequestNo"]) : string.Empty;

            dtTable = dr.GetPendingAcceptanceList(RequestNo);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            List<am_item_request> objlist = new List<am_item_request>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                objlist = (from d in dtTable.AsEnumerable()
                                     select new am_item_request
                                     {
                                         Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                                         ItemName = d.Field<dynamic>("ITEM"),
                                         SerialNo= d.Field<dynamic>("VSERIALNO"),
                                         AllocationDate = d.Field<dynamic>("DALLOCATIONDATE"),
                                         Action = "",
                                     }).ToList();
            }
            var applicationUsersQuery = objlist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<am_item_request>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.Nrequestno,
                            cell = new object[] { s.Action, s.Nrequestno, s.ItemName, s.SerialNo, s.AllocationDate}
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }
        [HttpGet]
        public string fetchAssetDetails(string RequestNo)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string jsonstring = string.Empty;
            dtTable = dr.GetPendingAcceptanceList(RequestNo);

            var a = (from d in dtTable.AsEnumerable()
                     select new am_item_request
                     {
                         Vempname = d.Field<dynamic>("ALLOCATEDTO"),
                         DEPTNAME = d.Field<dynamic>("DEPTNAME"),
                         ItemName = d.Field<dynamic>("ITEM"),
                         ITEMTYPE= d.Field<dynamic>("ITEMTYPE"),
                         BRAND= d.Field<dynamic>("BRAND"),
                         VMODELNO= d.Field<dynamic>("VMODELNO"),
                         UNIT= d.Field<dynamic>("UNIT"),
                         ITEM_SIZE= d.Field<dynamic>("ITEM_SIZE"),
                         SPECS = d.Field<dynamic>("SPECS"),
                         SerialNo = d.Field<dynamic>("VSERIALNO"),
                         Hostname= d.Field<dynamic>("VHOSTNAME"),
                         AllocationDate = d.Field<dynamic>("DALLOCATIONDATE"),
                         Vreason= d.Field<dynamic>("VREMARKS"),
                     }).FirstOrDefault();

            jsonstring = JsonConvert.SerializeObject(a, Formatting.Indented);

            return jsonstring;
        }
        [HttpPost]
        public JsonResult AssetAcceptanceEmployee(String Itemno, string Remarks)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.AssetAcceptanceEmployee(Itemno, Remarks, out status, out returnmessage);
                status = 0;
                returnmessage = "Asset acceptance have been accepted Successfully..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public JsonResult GetFinanceController()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetFinanceController();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NFCEMPNO"] != null ? Convert.ToString(row["NFCEMPNO"]) : "0",
                    Drptext = row["EMPNAME"] != null ? Convert.ToString(row["EMPNAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult ApproveRequest()
        {
            int status = -1;
            string returnmessage = string.Empty;
            string Reqno = string.Empty;
            string Status = string.Empty;
            string Remarks = string.Empty;
            string fcempno = string.Empty;
            string statusdesc = "";

            Reqno = Request.QueryString["Reqno"] != null ? Convert.ToString(Request.QueryString["Reqno"]) : string.Empty;
            Status = Request.QueryString["Status"] != null ? Convert.ToString(Request.QueryString["Status"]) : string.Empty;
            Remarks = Request.QueryString["Remarks"] != null ? Convert.ToString(Request.QueryString["Remarks"]) : string.Empty;
            fcempno = Request.QueryString["fcempno"] != null ? Convert.ToString(Request.QueryString["fcempno"]) : string.Empty;

            dr = new DatabaseRepository();
            try
            {
                dr.ApproveRequest(Reqno, Status, Remarks, fcempno, out status, out returnmessage, out statusdesc);
                status = 0;
                returnmessage = "Asset Request is " + statusdesc + " Successfully..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public JsonResult GetGXPList()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetGXPList();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NO"] != null ? Convert.ToString(row["NO"]) : "0",
                    Drptext = row["DESCRIPTION"] != null ? Convert.ToString(row["DESCRIPTION"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetClassfication()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetClassfication();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NO"] != null ? Convert.ToString(row["NO"]) : "0",
                    Drptext = row["DESCRIPTION"] != null ? Convert.ToString(row["DESCRIPTION"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetDomain()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetDomain();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NO"] != null ? Convert.ToString(row["NO"]) : "0",
                    Drptext = row["DESCRIPTION"] != null ? Convert.ToString(row["DESCRIPTION"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetOS()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetOS();
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["NO"] != null ? Convert.ToString(row["NO"]) : "0",
                    Drptext = row["DESCRIPTION"] != null ? Convert.ToString(row["DESCRIPTION"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ServerAsset()
        {
            return View();
        }

        public ActionResult SoftwareAsset()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSoftwareAssetListJQGrid(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            software_asset objsoftware_asset = new software_asset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;
            string SRNO = string.Empty;

            SRNO = Request.QueryString["SRNO"] != null ? Convert.ToString(Request.QueryString["SRNO"]) : string.Empty;

            dtTable = dr.GetSoftwareAsset(SRNO);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<software_asset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new software_asset
                                     {
                                         NSRNO = d.Field<dynamic>("NSRNO") != null ? Convert.ToInt32(d.Field<dynamic>("NSRNO")) : 0,
                                         LICENSEPRODUCT = d.Field<dynamic>("LICENSEPRODUCTNAME"),
                                         LICENSEVERSION = d.Field<dynamic>("LICENSEVERSION"),
                                         QUANTITY = d.Field<dynamic>("QUANTITY"),
                                         EXPIRYDATE = d.Field<dynamic>("EXPIRYDATE"),
                                         REMARKS = d.Field<dynamic>("REMARKS"),
                                         LOCATION = d.Field<dynamic>("LOCATION"),
                                         Action = "",
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<software_asset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.NSRNO,
                            cell = new object[] { s.Action, s. NSRNO, s.LICENSEPRODUCT, s.LICENSEVERSION, s.QUANTITY, s.EXPIRYDATE, s.REMARKS, s.LOCATION}
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult DeleteSoftwareAsset(String NSRNO)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteSoftwareAsset(NSRNO, out status, out returnmessage);
                status = 0;
                returnmessage = "Software Asset Deleted sucessfully.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public JsonResult GetLicenseProduct()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetLicenceProduct(null);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["LICENSEPRODUCTNO"] != null ? Convert.ToString(row["LICENSEPRODUCTNO"]) : "0",
                    Drptext = row["LICENSEPRODUCTNAME"] != null ? Convert.ToString(row["LICENSEPRODUCTNAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult EditSoftwareAsset(String SRNO)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            software_asset a = new software_asset();

            SRNO = Request.QueryString["SRNO"] != null ? Convert.ToString(Request.QueryString["SRNO"]) : string.Empty;

            dtTable = dr.GetSoftwareAsset(SRNO);

            try
            {
                 a = (from d in dtTable.AsEnumerable()
                     select new software_asset
                     {
                         NSRNO = d.Field<dynamic>("NSRNO") != null ? Convert.ToInt32(d.Field<dynamic>("NSRNO")) : 0,
                         LICENSEPRODUCTCODE = d.Field<dynamic>("LICENSEPRODUCT"),
                         LICENSEVERSION = d.Field<dynamic>("LICENSEVERSION"),
                         QUANTITY = d.Field<dynamic>("QUANTITY"),
                         EXPIRYDATE = d.Field<dynamic>("EXPIRYDATE"),
                         REMARKS = d.Field<dynamic>("REMARKS"),
                         LOCATIONCODE = d.Field<dynamic>("LOCATIONCODE"),                         
                     }).FirstOrDefault();

                status = 0;

            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus  = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status, hdata = a } };
        }

        public ActionResult SubmitSoftwareAsset(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            software_asset objsoftware_asset = new software_asset();
            
            try
            {
                objsoftware_asset.NSRNO = Convert.ToInt32(fc["hdn_srno"].ToString());
                objsoftware_asset.LICENSEPRODUCTCODE = !string.IsNullOrEmpty(fc["drp_Product"].ToString()) ? Convert.ToInt32(fc["drp_Product"].ToString()) : (Int32?)null;
                objsoftware_asset.LICENSEVERSION = (fc["txt_Version"].ToString());
                objsoftware_asset.QUANTITY = Convert.ToInt32(fc["txt_Quantity"].ToString());
                objsoftware_asset.EXPIRYDATE = !string.IsNullOrEmpty(fc["txt_ExpiryDate"].ToString()) ? fc["txt_ExpiryDate"].ToString() : string.Empty;
                objsoftware_asset.REMARKS = (fc["txt_remarks"].ToString());
                objsoftware_asset.LOCATIONCODE = !string.IsNullOrEmpty(fc["drp_location"].ToString()) ? Convert.ToInt32(fc["drp_location"].ToString()) : (Int32?)null;
                
                dr.SubmitSoftwareAsset(objsoftware_asset, out status, out returnmessage);

                status = 0;
                returnmessage = "Software Asset has been successfully saved..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult ExporttoExcel()
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            try
            {
                dtTable = dr.ExporttoExcel(out status, out returnmessage);

                if (dtTable.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "inline; filename=SoftwareAsset.xls"); // Download File Name.
                    Response.Write("<table border='1 px' style='width: 941px; cellspacing=0 frame=void rules=none border-color: #C0C0C0;'><tr style='background-color: #FFFFCC'>");

                    for (int i = 0; i < dtTable.Columns.Count; i++)
                    {
                        Response.Write("<td style='height: 22px; font-weight: lighter; background-color: lightcyan; text-align: center;'>");
                        Response.Write(dtTable.Columns[i].ColumnName.ToString());
                        Response.Write("</span></td>");
                    }

                    Response.Write("</tr>");
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        Response.Write("<tr>");
                        for (int j = 0; j < dtTable.Columns.Count; j++)
                        {
                            Response.Write("<td style='height: 22px;'>" + dtTable.Rows[i][j].ToString() + " </td>");
                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</table>");
                    //Most Important -Without this, Data in Excel Sheet would not be populated
                    Response.End();
                }
            }
            catch
            {
            }
            return View();           
            //return RedirectToAction("PendingList");
        }

        public ActionResult LicenseProductMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetLicenceProduct(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            software_asset objsoftware_asset = new software_asset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;
            string Licenseproductcode = string.Empty;

            Licenseproductcode = Request.QueryString["Licenseproductcode"] != null ? Convert.ToString(Request.QueryString["Licenseproductcode"]) : string.Empty;

            dtTable = dr.GetLicenceProduct(Licenseproductcode);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<software_asset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new software_asset
                                     {
                                         LICENSEPRODUCTCODE = d.Field<dynamic>("LICENSEPRODUCTNO") != null ? Convert.ToInt32(d.Field<dynamic>("LICENSEPRODUCTNO")) : 0,
                                         LICENSEPRODUCT = d.Field<dynamic>("LICENSEPRODUCTNAME"),
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<software_asset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.NSRNO,
                            cell = new object[] { s.Action, s.LICENSEPRODUCTCODE, s.LICENSEPRODUCT }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult DeleteLicenceProduct(String Licenseproductcode)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteLicenceProduct(Licenseproductcode, out status, out returnmessage);
                status = 0;
                returnmessage = "Licence Product Deleted sucessfully.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public JsonResult EditLicenseproductcode(String Licenseproductcode)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            software_asset a = new software_asset();

            Licenseproductcode = Request.QueryString["Licenseproductcode"] != null ? Convert.ToString(Request.QueryString["Licenseproductcode"]) : string.Empty;

            dtTable = dr.GetLicenceProduct(Licenseproductcode);

            try
            {
                a = (from d in dtTable.AsEnumerable()
                     select new software_asset
                     {
                         LICENSEPRODUCTCODE = d.Field<dynamic>("LICENSEPRODUCTNO") != null ? Convert.ToInt32(d.Field<dynamic>("LICENSEPRODUCTNO")) : 0,
                         LICENSEPRODUCT = d.Field<dynamic>("LICENSEPRODUCTNAME"),
                     }).FirstOrDefault();

                status = 0;

            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status, hdata = a } };
        }

        public ActionResult SubmitLicenseProduct(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            software_asset objsoftware_asset = new software_asset();

            try
            {
                objsoftware_asset.LICENSEPRODUCTCODE = !string.IsNullOrEmpty(fc["hdn_Licenseproductcode"].ToString()) ? Convert.ToInt32(fc["hdn_Licenseproductcode"].ToString()) : (Int32?)null;
                objsoftware_asset.LICENSEPRODUCT = (fc["txt_LicenseproductName"].ToString());

                dr.SubmitLicenseProduct(objsoftware_asset, out status, out returnmessage);

                status = 0;
                returnmessage = "License Product has been successfully created..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult VendorMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetVendorDetails(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;
            string Vendorno = string.Empty;

            Vendorno = Request.QueryString["Vendorno"] != null ? Convert.ToString(Request.QueryString["Vendorno"]) : string.Empty;

            dtTable = dr.GetVendorDetails(Vendorno);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<Server_Asset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new Server_Asset
                                     {
                                         VENDORNO = d.Field<dynamic>("VENDORNO") != null ? Convert.ToInt32(d.Field<dynamic>("VENDORNO")) : 0,
                                         VENDORNAME = d.Field<dynamic>("VENDORNAME"),
                                         CONTACTPERSON = d.Field<dynamic>("CONTACTPERSON"),
                                         CONTACTNUMBER = d.Field<dynamic>("CONTACTNUMBER") != null ? Convert.ToInt64(d.Field<dynamic>("CONTACTNUMBER")) : 0,
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<Server_Asset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.VENDORNO,
                            cell = new object[] { s.Action, s.VENDORNO, s.VENDORNAME, s.CONTACTPERSON, s.CONTACTNUMBER }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult EditVendorDetails(String Vendorno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            Server_Asset a = new Server_Asset();

            Vendorno = Request.QueryString["Vendorno"] != null ? Convert.ToString(Request.QueryString["Vendorno"]) : string.Empty;

            dtTable = dr.GetVendorDetails(Vendorno);

            try
            {
                a = (from d in dtTable.AsEnumerable()
                     select new Server_Asset
                     {
                         VENDORNO = d.Field<dynamic>("VENDORNO") != null ? Convert.ToInt32(d.Field<dynamic>("VENDORNO")) : 0,
                         VENDORNAME = d.Field<dynamic>("VENDORNAME"),
                         CONTACTPERSON = d.Field<dynamic>("CONTACTPERSON"),
                         CONTACTNUMBER = d.Field<dynamic>("CONTACTNUMBER") != null ? Convert.ToInt64(d.Field<dynamic>("CONTACTNUMBER")) : 0,
                     }).FirstOrDefault();

                status = 0;

            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status, hdata = a } };
        }

        [HttpPost]
        public JsonResult DeleteVendorDetails(String Vendorno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteVendorDetails(Vendorno, out status, out returnmessage);
                status = 0;
                returnmessage = "Vendor Deleted sucessfully.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult SubmitVendorDetails(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();

            try
            {
                objServer_Asset.VENDORNO = !string.IsNullOrEmpty(fc["hdn_vendorcode"].ToString()) ? Convert.ToInt32(fc["hdn_vendorcode"].ToString()) : (Int32?)null;
                objServer_Asset.VENDORNAME = (fc["txt_vendorname"].ToString());
                objServer_Asset.CONTACTPERSON = (fc["txt_contactperson"].ToString());
                objServer_Asset.CONTACTNUMBER = !string.IsNullOrEmpty(fc["txt_contactnumber"].ToString()) ? Convert.ToInt64(fc["txt_contactnumber"].ToString()) : (Int64?)null;

                dr.SubmitVendorDetails(objServer_Asset, out status, out returnmessage);

                status = 0;
                returnmessage = "Vendor has been successfully created..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult AssetAMC()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetWarrantyDetails(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;
            string Warrantyno = string.Empty;

            Warrantyno = Request.QueryString["Warrantyno"] != null ? Convert.ToString(Request.QueryString["Warrantyno"]) : string.Empty;

            dtTable = dr.GetWarrantyDetails(Warrantyno);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<Server_Asset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new Server_Asset
                                     {

                                         WARRANTY_AMC_NO = d.Field<dynamic>("WARRANTY_AMC_NO") != null ? Convert.ToInt32(d.Field<dynamic>("WARRANTY_AMC_NO")) : 0,
                                         WARRANTY_AMC_TYPE = d.Field<dynamic>("WARRANTY_AMC_TYPE"),
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<Server_Asset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.WARRANTY_AMC_NO,
                            cell = new object[] { s.Action, s.WARRANTY_AMC_NO, s.WARRANTY_AMC_TYPE }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult EditWarrantyDetails(String Warrantyno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            Server_Asset a = new Server_Asset();

            Warrantyno = Request.QueryString["Warrantyno"] != null ? Convert.ToString(Request.QueryString["Warrantyno"]) : string.Empty;

            dtTable = dr.GetWarrantyDetails(Warrantyno);

            try
            {
                a = (from d in dtTable.AsEnumerable()
                     select new Server_Asset
                     {
                         WARRANTY_AMC_NO = d.Field<dynamic>("WARRANTY_AMC_NO") != null ? Convert.ToInt32(d.Field<dynamic>("WARRANTY_AMC_NO")) : 0,
                         WARRANTY_AMC_TYPE = d.Field<dynamic>("WARRANTY_AMC_TYPE"),
                     }).FirstOrDefault();

                status = 0;

            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status, hdata = a } };
        }

        [HttpPost]
        public JsonResult DeleteWarrantyDetails(String Warrantyno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteWarrantyDetails(Warrantyno, out status, out returnmessage);
                status = 0;
                returnmessage = "Warranty / AMC Deleted sucessfully.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult SubmitWarrantyDetails(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();

            try
            {
                objServer_Asset.WARRANTY_AMC_NO = !string.IsNullOrEmpty(fc["hdn_Warranty"].ToString()) ? Convert.ToInt32(fc["hdn_Warranty"].ToString()) : (Int32?)null;
                objServer_Asset.WARRANTY_AMC_TYPE = (fc["txt_typename"].ToString());

                dr.SubmitWarrantyDetails(objServer_Asset, out status, out returnmessage);

                status = 0;
                returnmessage = "Vendor has been successfully created..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult ServerOS()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetServerOSDetails(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;
            string OSNO = string.Empty;

            OSNO = Request.QueryString["OSNO"] != null ? Convert.ToString(Request.QueryString["OSNO"]) : string.Empty;

            dtTable = dr.GetServerOSDetails(OSNO);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<Server_Asset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new Server_Asset
                                     {
                                         OSNO = d.Field<dynamic>("OSNO") != null ? Convert.ToInt32(d.Field<dynamic>("OSNO")) : 0,
                                         OSNAME = d.Field<dynamic>("OSNAME"),
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<Server_Asset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.WARRANTY_AMC_NO,
                            cell = new object[] { s.Action, s.OSNO, s.OSNAME }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult EditOSDetails(String OSNO)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            Server_Asset a = new Server_Asset();

            OSNO = Request.QueryString["OSNO"] != null ? Convert.ToString(Request.QueryString["OSNO"]) : string.Empty;

            dtTable = dr.GetServerOSDetails(OSNO);

            try
            {
                a = (from d in dtTable.AsEnumerable()
                     select new Server_Asset
                     {
                         OSNO = d.Field<dynamic>("OSNO") != null ? Convert.ToInt32(d.Field<dynamic>("OSNO")) : 0,
                         OSNAME = d.Field<dynamic>("OSNAME"),
                     }).FirstOrDefault();

                status = 0;

            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status, hdata = a } };
        }

        [HttpPost]
        public JsonResult DeleteOSNODetails(String OSNO)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteOSNODetails(OSNO, out status, out returnmessage);
                status = 0;
                returnmessage = "Server OS Deleted sucessfully.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult SubmitOSDetails(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();

            try
            {
                objServer_Asset.OSNO = !string.IsNullOrEmpty(fc["hdn_osno"].ToString()) ? Convert.ToInt32(fc["hdn_osno"].ToString()) : (Int32?)null;
                objServer_Asset.OSNAME = (fc["txt_osname"].ToString());

                dr.SubmitOSDetails(objServer_Asset, out status, out returnmessage);

                status = 0;
                returnmessage = "Server OS has been successfully created..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult ServerAssetType()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAssetTypeDetails(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;
            string Assettypeno = string.Empty;

            Assettypeno = Request.QueryString["Assettypeno"] != null ? Convert.ToString(Request.QueryString["Assettypeno"]) : string.Empty;

            dtTable = dr.GetAssetTypeDetails(Assettypeno);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<Server_Asset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new Server_Asset
                                     {
                                         ASSETTYPENO = d.Field<dynamic>("ASSETTYPENO") != null ? Convert.ToInt32(d.Field<dynamic>("ASSETTYPENO")) : 0,
                                         ASSETTYPENAME = d.Field<dynamic>("ASSETTYPENAME"),
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<Server_Asset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.ASSETTYPENO,
                            cell = new object[] { s.Action, s.ASSETTYPENO, s.ASSETTYPENAME }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult EditServerAssetTypeDetails(String Assettypeno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            Server_Asset a = new Server_Asset();

            Assettypeno = Request.QueryString["Assettypeno"] != null ? Convert.ToString(Request.QueryString["Assettypeno"]) : string.Empty;

            dtTable = dr.GetAssetTypeDetails(Assettypeno);

            try
            {
                a = (from d in dtTable.AsEnumerable()
                     select new Server_Asset
                     {
                         ASSETTYPENO = d.Field<dynamic>("ASSETTYPENO") != null ? Convert.ToInt32(d.Field<dynamic>("ASSETTYPENO")) : 0,
                         ASSETTYPENAME = d.Field<dynamic>("ASSETTYPENAME"),
                     }).FirstOrDefault();

                status = 0;

            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status, hdata = a } };
        }

        [HttpPost]
        public JsonResult DeleteServerAssetTypeDetails(String Assettypeno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteServerAssetTypeDetails(Assettypeno, out status, out returnmessage);
                status = 0;
                returnmessage = "Server Asset Type Deleted sucessfully.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult SubmitServerAssetType(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();

            try
            {
                objServer_Asset.ASSETTYPENO = !string.IsNullOrEmpty(fc["hdn_Assettypeno"].ToString()) ? Convert.ToInt32(fc["hdn_Assettypeno"].ToString()) : (Int32?)null;
                objServer_Asset.ASSETTYPENAME = (fc["txt_typename"].ToString());

                dr.SubmitServerAssetType(objServer_Asset, out status, out returnmessage);

                status = 0;
                returnmessage = "Server Asset Type has been successfully created..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult ServerMake()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetServerMake(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;
            string Makeno = string.Empty;

            Makeno = Request.QueryString["Makeno"] != null ? Convert.ToString(Request.QueryString["Makeno"]) : string.Empty;

            dtTable = dr.GetServerMake(Makeno);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<Server_Asset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new Server_Asset
                                     {
                                         MAKENO = d.Field<dynamic>("MAKENO") != null ? Convert.ToInt32(d.Field<dynamic>("MAKENO")) : 0,
                                         MAKENAME = d.Field<dynamic>("MAKENAME"),
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<Server_Asset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.MAKENO,
                            cell = new object[] { s.Action, s.MAKENO, s.MAKENAME }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult EditServerMake(String Makeno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            Server_Asset a = new Server_Asset();

            Makeno = Request.QueryString["Makeno"] != null ? Convert.ToString(Request.QueryString["Makeno"]) : string.Empty;

            dtTable = dr.GetServerMake(Makeno);

            try
            {
                a = (from d in dtTable.AsEnumerable()
                     select new Server_Asset
                     {
                         MAKENO   = d.Field<dynamic>("MAKENO") != null ? Convert.ToInt32(d.Field<dynamic>("MAKENO")) : 0,
                         MAKENAME = d.Field<dynamic>("MAKENAME"),
                     }).FirstOrDefault();

                status = 0;

            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status, hdata = a } };
        }

        [HttpPost]
        public JsonResult DeleteServerMake(String Makeno)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteServerMake(Makeno, out status, out returnmessage);
                status = 0;
                returnmessage = "Server / Network Make Deleted sucessfully.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult SubmitServerMake(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();

            try
            {
                objServer_Asset.MAKENO = !string.IsNullOrEmpty(fc["hdn_makeno"].ToString()) ? Convert.ToInt32(fc["hdn_makeno"].ToString()) : (Int32?)null;
                objServer_Asset.MAKENAME = (fc["txt_makename"].ToString());

                dr.SubmitServerMake(objServer_Asset, out status, out returnmessage);

                status = 0;
                returnmessage = "Server / Network Make has been successfully created..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public JsonResult GetServerAssetListJQGrid(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;
            string SRNO = string.Empty;

            SRNO = Request.QueryString["SRNO"] != null ? Convert.ToString(Request.QueryString["SRNO"]) : string.Empty;

            dtTable = dr.GetServerAsset(SRNO);

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<Server_Asset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new Server_Asset
                                     {
                                         NSRNO = d.Field<dynamic>("NSRNO") != null ? Convert.ToInt32(d.Field<dynamic>("NSRNO")) : 0,
                                         MAKENAME = d.Field<dynamic>("MAKENAME"),
                                         VMODEL = d.Field<dynamic>("VMODEL"),
                                         VSERIALNO = d.Field<dynamic>("VSERIALNO"),
                                         ASSETTYPENAME = d.Field<dynamic>("ASSETTYPENAME"),
                                         HOSTNAME = d.Field<dynamic>("HOSTNAME"),
                                         TAG = d.Field<dynamic>("TAG"),
                                         IPADDRESS = d.Field<dynamic>("IPADDRESS"),
                                         CPU = d.Field<dynamic>("CPU"),
                                         CORE = d.Field<dynamic>("CORE"),
                                         RAM = d.Field<dynamic>("RAM"),
                                         OSNAME = d.Field<dynamic>("OSNAME"),
                                         WARRANTY_AMC_TYPE = d.Field<dynamic>("WARRANTY_AMC_TYPE"),
                                         WARRANTY_AMC_DATE = d.Field<dynamic>("WARRANTY_AMC_DATE"),
                                         PONO = d.Field<dynamic>("PONO"),
                                         VENDORNAME = d.Field<dynamic>("VENDORNAME"),
                                         CONTACTPERSON = d.Field<dynamic>("CONTACTPERSON"),
                                         CONTACTNUMBER = d.Field<dynamic>("CONTACTNUMBER") != null ? Convert.ToInt64(d.Field<dynamic>("CONTACTNUMBER")) : 0,
                                         LocationName = d.Field<dynamic>("LOCNAME"),
                                         SUBLocationName = d.Field<dynamic>("SUBLOCATION"),
                                         PORT = d.Field<dynamic>("PORT"),
                                         Action = "",
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<Server_Asset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.NSRNO,
                            cell = new object[] { s.Action, s.NSRNO, s.MAKENAME, s.VMODEL, s.VSERIALNO, s.ASSETTYPENAME, s.HOSTNAME, s.TAG, s.IPADDRESS, s.CPU, s.CORE, s.RAM, s.OSNAME, s.WARRANTY_AMC_TYPE, s.WARRANTY_AMC_DATE, s.PONO, s.VENDORNAME, s.CONTACTPERSON, s.CONTACTNUMBER, s.LocationName, s.SUBLocationName, s.PORT}
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult GetMakeDetails()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetServerMake(null);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["MAKENO"] != null ? Convert.ToString(row["MAKENO"]) : "0",
                    Drptext = row["MAKENAME"] != null ? Convert.ToString(row["MAKENAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAssetType()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetAssetTypeDetails(null);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["ASSETTYPENO"] != null ? Convert.ToString(row["ASSETTYPENO"]) : "0",
                    Drptext = row["ASSETTYPENAME"] != null ? Convert.ToString(row["ASSETTYPENAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetOSDetails()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetServerOSDetails(null);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["OSNO"] != null ? Convert.ToString(row["OSNO"]) : "0",
                    Drptext = row["OSNAME"] != null ? Convert.ToString(row["OSNAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetWarrantyAMC()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetWarrantyDetails(null);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["WARRANTY_AMC_NO"] != null ? Convert.ToString(row["WARRANTY_AMC_NO"]) : "0",
                    Drptext = row["WARRANTY_AMC_TYPE"] != null ? Convert.ToString(row["WARRANTY_AMC_TYPE"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult ExporttoExcelServerAsset()
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            try
            {
                dtTable = dr.ExporttoExcelServerAsset(out status, out returnmessage);

                if (dtTable.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "inline; filename=ServerAsset.xls"); // Download File Name.
                    Response.Write("<table border='1 px' style='width: 941px; cellspacing=0 frame=void rules=none border-color: #C0C0C0;'><tr style='background-color: #FFFFCC'>");

                    for (int i = 0; i < dtTable.Columns.Count; i++)
                    {
                        Response.Write("<td style='height: 22px; font-weight: lighter; background-color: lightcyan; text-align: center;'>");
                        Response.Write(dtTable.Columns[i].ColumnName.ToString());
                        Response.Write("</span></td>");
                    }

                    Response.Write("</tr>");
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        Response.Write("<tr>");
                        for (int j = 0; j < dtTable.Columns.Count; j++)
                        {
                            Response.Write("<td style='height: 22px;'>" + dtTable.Rows[i][j].ToString() + " </td>");
                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</table>");
                    //Most Important -Without this, Data in Excel Sheet would not be populated
                    Response.End();
                }
            }
            catch
            {
            }
            return View();
            //return RedirectToAction("PendingList");
        }

        [HttpPost]
        public JsonResult DeleteServerAsset(String NSRNO)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            try
            {
                dr.DeleteServerAsset(NSRNO, out status, out returnmessage);
                status = 0;
                returnmessage = "Server / Network Asset Deleted sucessfully.";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public JsonResult EditServerAsset(String SRNO)
        {
            int status = -1;
            string returnmessage = string.Empty;
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            //software_asset a = new software_asset();
            Server_Asset a = new Server_Asset();

            SRNO = Request.QueryString["SRNO"] != null ? Convert.ToString(Request.QueryString["SRNO"]) : string.Empty;

            dtTable = dr.GetServerAsset(SRNO);

            try
            {
                a = (from d in dtTable.AsEnumerable()
                     select new Server_Asset
                     {
                         NSRNO = d.Field<dynamic>("NSRNO") != null ? Convert.ToInt32(d.Field<dynamic>("NSRNO")) : 0,
                         MAKENO = d.Field<dynamic>("NMAKENO") != null ? Convert.ToInt32(d.Field<dynamic>("NMAKENO")) : 0,
                         VMODEL = d.Field<dynamic>("VMODEL"),
                         VSERIALNO = d.Field<dynamic>("VSERIALNO"),
                         ASSETTYPENO = d.Field<dynamic>("ASSETTYPENO") != null ? Convert.ToInt32(d.Field<dynamic>("ASSETTYPENO")) : 0,
                         HOSTNAME = d.Field<dynamic>("HOSTNAME"),
                         TAG = d.Field<dynamic>("TAG"),
                         IPADDRESS = d.Field<dynamic>("IPADDRESS"),
                         CPU = d.Field<dynamic>("CPU"),
                         CORE = d.Field<dynamic>("CORE"),
                         RAM = d.Field<dynamic>("RAM"),
                         OSNO = d.Field<dynamic>("OSNO") != null ? Convert.ToInt32(d.Field<dynamic>("OSNO")) : 0,
                         WARRANTY_AMC_NO = d.Field<dynamic>("WARRANTY_AMC_NO") != null ? Convert.ToInt32(d.Field<dynamic>("WARRANTY_AMC_NO")) : 0,
                         WARRANTY_AMC_DATE = d.Field<dynamic>("WARRANTY_AMC_DATE"),
                         PONO = d.Field<dynamic>("PONO"),
                         VENDORNO = d.Field<dynamic>("VENDORNO") != null ? Convert.ToInt32(d.Field<dynamic>("VENDORNO")) : 0,
                         CONTACTPERSON = d.Field<dynamic>("CONTACTPERSON"),
                         CONTACTNUMBER = d.Field<dynamic>("CONTACTNUMBER") != null ? Convert.ToInt64(d.Field<dynamic>("CONTACTNUMBER")) : 0,
                         Locationcode = d.Field<dynamic>("LOCATION") != null ? Convert.ToInt32(d.Field<dynamic>("LOCATION")) : 0,
                         SUBLocationName = d.Field<dynamic>("SUBLOCATION"),
                         PORT = d.Field<dynamic>("PORT"),
                     }).FirstOrDefault();

                status = 0;

            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }
            a.reqStatus = status;
            a.reqMessage = returnmessage;
            var jsonResult = Json(a, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

            // return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status, hdata = a } };
        }

        [HttpPost]
        public JsonResult GetVendor()
        {
            var objStatusList = new List<dropdown>();
            dr = new DatabaseRepository();
            DataTable dtTable = dr.GetVendorDetails(null);
            objStatusList = new List<dropdown>(dtTable.Rows.Count);

            foreach (DataRow row in dtTable.Rows)
            {
                var obj = new dropdown()
                {
                    Drpvalu = row["VENDORNO"] != null ? Convert.ToString(row["VENDORNO"]) : "0",
                    Drptext = row["VENDORNAME"] != null ? Convert.ToString(row["VENDORNAME"]) : string.Empty
                };
                objStatusList.Add(obj);
            }

            var jsonResult = Json(objStatusList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult SubmitServerAsset(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            Server_Asset objServer_Asset = new Server_Asset();

            try
            {
                objServer_Asset.NSRNO = Convert.ToInt32(fc["hdn_srno"].ToString());
                objServer_Asset.MAKENO = !string.IsNullOrEmpty(fc["drp_make"].ToString()) ? Convert.ToInt32(fc["drp_make"].ToString()) : (Int32?)null;
                objServer_Asset.VMODEL = (fc["txt_model"].ToString());
                objServer_Asset.VSERIALNO = (fc["txt_serialno"].ToString());
                objServer_Asset.ASSETTYPENO = !string.IsNullOrEmpty(fc["drp_assettype"].ToString()) ? Convert.ToInt32(fc["drp_assettype"].ToString()) : (Int32?)null;
                objServer_Asset.HOSTNAME = (fc["txt_hostname"].ToString());
                objServer_Asset.TAG = (fc["txt_assettag"].ToString());
                objServer_Asset.IPADDRESS = (fc["txt_ipaddress"].ToString());
                objServer_Asset.CPU = (fc["txt_cpu"].ToString());
                objServer_Asset.CORE = (fc["txt_core"].ToString());
                objServer_Asset.RAM = (fc["txt_ram"].ToString());
                objServer_Asset.OSNO = !string.IsNullOrEmpty(fc["drp_os"].ToString()) ? Convert.ToInt32(fc["drp_os"].ToString()) : (Int32?)null;
                objServer_Asset.WARRANTY_AMC_NO = !string.IsNullOrEmpty(fc["drp_Contracttypes"].ToString()) ? Convert.ToInt32(fc["drp_Contracttypes"].ToString()) : (Int32?)null;
                objServer_Asset.WARRANTY_AMC_DATE = !string.IsNullOrEmpty(fc["txt_expireydate"].ToString()) ? fc["txt_expireydate"].ToString() : string.Empty;
                objServer_Asset.PONO = (fc["txt_ponumber"].ToString());
                objServer_Asset.Locationcode = !string.IsNullOrEmpty(fc["drp_location"].ToString()) ? Convert.ToInt32(fc["drp_location"].ToString()) : (Int32?)null;
                objServer_Asset.SUBLocationName = (fc["txt_sublocation"].ToString());
                objServer_Asset.VENDORNO = !string.IsNullOrEmpty(fc["drp_vendor"].ToString()) ? Convert.ToInt32(fc["drp_vendor"].ToString()) : (Int32?)null;
                objServer_Asset.PORT = (fc["txt_port"].ToString());

                dr.SubmitServerAsset(objServer_Asset, out status, out returnmessage);

                status = 0;
                returnmessage = "Server / Network request has been successfully generated..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        [HttpPost]
        public JsonResult GetRequestStatusListJQGrid(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            DataView dv = null;

            dtTable = dr.GetRequestStatusListJQGrid();

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<am_item_request>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new am_item_request
                                     {
                                         Nrequestno = d.Field<dynamic>("NREQUESTNO") != null ? Convert.ToInt32(d.Field<dynamic>("NREQUESTNO")) : 0,
                                         Dinsertdate = d.Field<dynamic>("DINSERTDATE"),
                                         Vempno = d.Field<dynamic>("VEMPNO"),
                                         Vempname = d.Field<dynamic>("EMPNAME"),
                                         Locationname = d.Field<dynamic>("LOCATION"),
                                         DEPTNAME = d.Field<dynamic>("FUNCTIONNAME"),
                                         ItemName = d.Field<dynamic>("VTYPEDESCRIPTION"),
                                         NREQTYPENAME = d.Field<dynamic>("NREQTYPE"),
                                         RequestFor = d.Field<dynamic>("RequestFor"),
                                         Engname = d.Field<dynamic>("VEMPNAME"),
                                         Statusname = d.Field<dynamic>("PendingWith"),
                                         Vreason = d.Field<dynamic>("VREASON"),
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<am_item_request>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            Nrequestno = s.Nrequestno,
                            cell = new object[] {s.Nrequestno, s.Dinsertdate, s.Vempno, s.Vempname,s.Locationname, s.DEPTNAME, s.ItemName, s.NREQTYPENAME, s.RequestFor, s.Engname, s.Statusname, s.Vreason }
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }
        //Bind HostName with SerialNo
        [HttpGet]
        public string GetHostnameDetails(string Vserialno)
        {
            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();
            DataTable dtTable = new DataTable();
            string jsonstring = string.Empty;
            dtTable = dr.GetHostnameDetails(Vserialno);

            var a = (from d in dtTable.AsEnumerable()
                     select new am_item_request
                     {
                         Hostname = d.Field<dynamic>("VHOSTNAME"),
                     }).FirstOrDefault();

            jsonstring = JsonConvert.SerializeObject(a, Formatting.Indented);

            return jsonstring;
        }
        public ActionResult ResignedEmpAllocatedAsset()
        {
            return View();
        }

        [HttpPost]
        public JsonResult fetchResignedEmpAsset(string sidx, string sord, int page, int rows,
            bool _search, string searchField, string searchOper, string searchString, string filters, string RFCNo)
        {
            dr = new DatabaseRepository();
            //Server_Asset objServer_Asset = new Server_Asset();
            ResignedEmpAsset objResignedEmpAsset = new ResignedEmpAsset();
            DataTable dtTable = new DataTable();
            string Self = string.Empty;
            DataView dv = null;

            dtTable = dr.fetchResignedEmpAsset();

            if (dtTable != null)
            {
                if (dtTable.Rows.Count > 0)
                {
                    if (_search)
                    {
                        //multipleSearch: true in jqgrid
                        BusinessClass.Filters objFilters = new BusinessClass.Filters();

                        var f = (!_search || string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<BusinessClass.Filters>(filters);
                        if (f.rules.Count > 0)
                        {
                            dv = objFilters.SearchDataTable(dtTable, f, sidx, sord);
                            dtTable = dv.ToTable();
                        }
                    }
                }
            }

            var TimeEstimateslist = new List<ResignedEmpAsset>(dtTable.Rows.Count);

            if (dtTable != null)
            {
                TimeEstimateslist = (from d in dtTable.AsEnumerable()
                                     select new ResignedEmpAsset
                                     {

                                         //WARRANTY_AMC_NO = d.Field<dynamic>("WARRANTY_AMC_NO") != null ? Convert.ToInt32(d.Field<dynamic>("WARRANTY_AMC_NO")) : 0,
                                         Vempno = d.Field<dynamic>("VALLOCATEDTO"),
                                         Vempname = d.Field<dynamic>("EMPNAME"),
                                         DEPTNAME = d.Field<dynamic>("DEPTNAME"),
                                         Locationname = d.Field<dynamic>("LOCNAME"),
                                         VMODELNO = d.Field<dynamic>("VMODELNO"),
                                         VTYPEDESCRIPTION = d.Field<dynamic>("VTYPEDESCRIPTION"),
                                         RESIGNDT = d.Field<dynamic>("RESIGNDT"),
                                     }).ToList();
            }
            var applicationUsersQuery = TimeEstimateslist.AsQueryable();
            var sortedUsers = SortIQueryableFileData<ResignedEmpAsset>(applicationUsersQuery, sidx, sord);
            var totalRecords = applicationUsersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)rows);

            // Prepare the data to fit the requirement of jQGrid

            var data = (from s in sortedUsers
                        select new
                        {
                            SrNo = s.Vempno,
                            cell = new object[] { s.Vempno, s.Vempname, s.DEPTNAME, s.Locationname, s.VMODELNO, s.VTYPEDESCRIPTION, s.RESIGNDT}
                        }).ToArray();

            // Send the data to the jQGrid

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip((page - 1) * rows).Take(rows)
            };

            return Json(jsonData);
        }
        public ActionResult SubmitSearchAsset(FormCollection fc)
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            am_item_request objam_item_request = new am_item_request();


            try
            {
                //objam_item_request.Nrequestno = !string.IsNullOrEmpty(fc["drp_item"].ToString()) ? Convert.ToInt32(fc["drp_item"].ToString()) : (Int32?)null;
                //objam_item_request.Nstatus = Convert.ToInt32(fc["drp_action"].ToString());
                //objam_item_request.Dallocationdate = !string.IsNullOrEmpty(fc["txt_deallocationdate"].ToString()) ? fc["txt_deallocationdate"].ToString() : string.Empty;
                //objam_item_request.Vreason = (fc["txt_DeallocationRemarks"].ToString());
                objam_item_request.Hostname = (fc["txt_hostname"].ToString());
                objam_item_request.OS = !string.IsNullOrEmpty(fc["drp_OS"].ToString()) ? Convert.ToInt32(fc["drp_OS"].ToString()) : (Int32?)null;
                objam_item_request.Remarks = (fc["txt_remarks"].ToString());
                objam_item_request.Vempno = (fc["txt_empno"].ToString());
                objam_item_request.Assetno1 = !string.IsNullOrEmpty(fc["hdn_Assetno"].ToString()) ? Convert.ToInt32(fc["hdn_Assetno"].ToString()) : (Int32?)null;
                objam_item_request.Nstatus = !string.IsNullOrEmpty(fc["drp_action"].ToString()) ? Convert.ToInt32(fc["drp_action"].ToString()) : (Int32?)null;

                dr.SubmitSearchAsset(objam_item_request, out status, out returnmessage);

                status = 0;
                returnmessage = "OS detail has been successfully updated..!!";
            }
            catch (Exception ex)
            {
                status = -1;
                returnmessage = "Error:" + ex.Message.ToString();
            }

            return new CustomJsonResult { Data = new { message = returnmessage, MessageCode = status } };
        }

        public ActionResult DownloadAllocationRequest()
        {
            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            try
            {
                dtTable = dr.GetpendingAllocation();

                if (dtTable.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "inline; filename=PendingAllocationRequest.xls"); // Download File Name.
                    Response.Write("<table border='1 px' style='width: 941px; cellspacing=0 frame=void rules=none border-color: #C0C0C0;'><tr style='background-color: #FFFFCC'>");

                    for (int i = 0; i < dtTable.Columns.Count; i++)
                    {
                        Response.Write("<td style='height: 22px; font-weight: lighter; background-color: lightcyan; text-align: center;'>");
                        Response.Write(dtTable.Columns[i].ColumnName.ToString());
                        Response.Write("</span></td>");
                    }

                    Response.Write("</tr>");
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        Response.Write("<tr>");
                        for (int j = 0; j < dtTable.Columns.Count; j++)
                        {
                            Response.Write("<td style='height: 22px;'>" + dtTable.Rows[i][j].ToString() + " </td>");
                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</table>");
                    Response.End();
                }
            }
            catch
            {
            }
            return View();
        }

        public ActionResult FSAssetReport()
        {
            int status = -1;
            string returnmessage = "";

            dr = new DatabaseRepository();
            DataTable dtTable = new DataTable();
            asset_report objasset_report = new asset_report();

            try
            {
                objasset_report.FromDate = !string.IsNullOrEmpty(Request.QueryString["fdate"]) ? Convert.ToString(Request.QueryString["fdate"]) : string.Empty;
                objasset_report.ToDate = !string.IsNullOrEmpty(Request.QueryString["tdate"]) ? Convert.ToString(Request.QueryString["tdate"]) : string.Empty;

                dtTable = dr.FSAssetReport(objasset_report, out status, out returnmessage);

                if (dtTable.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "inline; filename=FSAssetReport.xls"); // Download File Name.
                    Response.Write("<table border='1 px' style='width: 941px; cellspacing=0 frame=void rules=none border-color: #C0C0C0;'><tr style='background-color: #FFFFCC'>");

                    for (int i = 0; i < dtTable.Columns.Count; i++)
                    {
                        Response.Write("<td style='height: 22px; font-weight: lighter; background-color: lightcyan; text-align: center;'>");
                        Response.Write(dtTable.Columns[i].ColumnName.ToString());
                        Response.Write("</span></td>");
                    }
                    Response.Write("</tr>");
                    for (int i = 0; i < dtTable.Rows.Count; i++)
                    {
                        Response.Write("<tr>");
                        for (int j = 0; j < dtTable.Columns.Count; j++)
                        {
                            Response.Write("<td style='height: 22px;'>" + dtTable.Rows[i][j].ToString() + " </td>");
                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</table>");
                    Response.End();
                    status = 0;
                }
            }
            catch
            {
            }
            return View();
        }
    }
}