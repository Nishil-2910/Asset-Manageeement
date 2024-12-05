using MobileReimbursement.BusinessClass;
using MobileReimbursement.CPLHelperService;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileReimbursement.Models
{
    public class DatabaseRepository
    {
        private enum Schema
        {
            UPIS,
            MDR,
            ASSET
        }

        private enum Packagename
        {
            AM_ITEM_PACKAGE,
            ASSET_COMMON_DDL,
            ASSET_SOFTWARE_MASTER,
            ASSET_SERVER_MASTER,
            AM_ITEM_PACKAGE_SF,
            ASSET_SERVER_MASTER_SF,
            ASSET_SOFTWARE_MASTER_SF,
            ASSET_COMMON_DDL_SF,
            SSO_DML,
            FS_ASSET
        }

        private enum Procedurename
        {
            ASSET_REPORT,
            GETITEM,
            APPROVAL_HOD,
            GETCOMPANY,
            GETDIVISION,
            GETUNIT,
            GETDEPARTMENT,
            GETDESIGNATION,
            GETLOCATION,
            GETHOD,
            SAVE_ITEM_EMPNEWREQUEST,
            GETASSETREQUEST,
            EDITASSETREQUEST,
            DELETE_EMP_NEWREQUEST,
            ASSETHISTORY,
            GETAPPROVALREQUEST,
            GETASSETREQDETAILS,
            PENDING_USER_ACCEPTANCE,
            UNEXPECTED_DEALLOCATION,
            PENDING_ALLOCATION_LIST,
            REJECT_APPROVED_REQUEST,
            GETEMPLOYEE,
            GETEMPDETAILS,
            GETSERIALNO,
            SAVE_ITEM_ALLOTMENT,
            GETDEALLOCATIONEMPLOYEE,
            GETALLOCATEDITEM,
            GETALLOCATEDITEMDETAILS,
            SAVE_ITEM_DEALLOTMENT,
            SEARCH_SERIALNO,
            ASSET_REQUEST_STATUS,
            GETPENDINGACCEPTANCE,
            SAVE_ITEM_RECEIVE,
            FETCH_FC_DETAILS,
            NEW_REQUEST_APPROVAL,
            APPROVAL_REMINDER,
            GETGXP_NONGXP,
            GETASSET_DOMAIN,
            GETASSET_OS,
            GET_ASSET_SOFTWARE_DETAILS,
            DELETE_ASSET_SOFTWARE_DETAILS,
            GET_ASSET_LICENSE_PRODUCT,
            UPSERT_ASSET_SOFTWARE_DETAILS,
            EXPORTTOEXCEL,
            DELETE_ASSET_LICENSE_PRODUCT,
            UPSERT_ASSET_LICENSE_PRODUCT,
            GET_ASSET_SERVER_VENDOR,
            DELETE_ASSET_SERVER_VENDOR,
            UPSERT_ASSET_SERVER_VENDOR,
            GET_ASSET_SERVER_WARRANTY,
            DELETE_ASSET_SERVER_WARRANTY,
            UPSERT_ASSET_SERVER_WARRANTY,
            GET_ASSET_SERVER_OS,
            DELETE_ASSET_SERVER_OS,
            ADD_ASSET_SERVER_OS,
            GET_ASSET_SERVER_ASSETTYPE,
            DELETE_ASSET_SERVER_ASSETTYPE,
            ADD_ASSET_SERVER_ASSETTYPE,
            GET_ASSET_MAKE,
            DELETE_ASSET_SERVER_MAKE,
            ADD_ASSET_SERVER_MAKE,
            GET_ASSET_SERVER_DETAILS,
            EXPORTTOEXCEL_SERVERASSET,
            DELETE_ASSET_SERVER_DETAILS,
            UPSERT_ASSET_SERVER_DETAILS,
            GRID_VIEW_REQUEST_STATUS,
            GETVSERIALNODETAILS,
            VIEWRESIGNEDEMPASSET,
            GETSSO,
            INSERT_SSO,
            DELETE_SSO,
            UPDATE_ALLOCATION_DETAILS,
            FS_ASSET_REPORT,
            ASSET_CLASSIFICATION
        }

        private enum ParameterName
        {
            V_I_NSTATUS,
            V_I_DFROMDATE,
            V_I_DTODATE,
            FETCH_DETAILS,
            V_I_EMPNO,
            V_I_COMPANY,
            V_I_DIVISION,
            V_I_UNIT,
            V_I_NREQUESTNO,
            V_I_NREQTYPE,
            V_I_VEMPNO,
            V_I_NITEMNO,
            V_I_VREASON,
            V_I_VHODEMPNO,
            V_I_NALLOCATIONTYPE,
            V_I_DALLOCATIONDATE,
            V_I_VEMPNAME,
            V_I_NCOMPANY,
            V_I_NDIVISION,
            V_I_NDEPARTMENT,
            V_I_NDESIGNATION,
            V_I_NLOCATION,
            DJOININGDATE,
            V_I_DJOININGDATE,
            V_I_NUNIT,
            V_I_SELF,
            NREQUESTNO,
            T_NREQUESTNO,
            V_I_VDEALLOCATEREMARKS,
            V_I_VDEALLOCATEBY,
            V_I_VAPPROVENO,
            VAPPROVEREMARKS,
            V_I_SEARCH,
            V_I_RECEIPT,
            V_I_ITEMNO,
            V_I_NASSETNO,
            V_I_VHOSTNAME,
            V_I_VREMARKS,
            V_I_VALLOCATEDBY,
            V_I_NREQNO,
            V_I_CHKMONITOR,
            V_I_NITEMNO1,
            V_I_NASSETNO1,
            V_I_VALLOCATEDTO,
            V_I_DDEALLOCATEDATE,
            V_I_SERIALNO,
            V_I_VAPPROVEREMARKS,
            V_I_VFCEMPNO,
            V_I_NSTATUSDESC,
            V_O_NSTATUSDESC,
            V_I_SRNO,
            V_I_LICENSEPRODUCT,
            V_I_LICENSEVERSION,
            V_I_QUANTITY,
            V_I_EXPIRYDATE,
            V_I_REMARKS,
            V_I_LOCATION,
            V_I_LEMPNO,
            V_I_LICENSEPRODUCTNO,
            V_I_LICENSEPRODUCTNAME,
            V_I_VENDORNO,
            V_I_CONTACTNUMBER,
            V_I_VENDORNAME,
            V_I_CONTACTPERSON,
            V_I_WARRANTY_AMC_NO,
            V_I_WARRANTY_AMC_TYPE,
            V_I_OSNO,
            V_I_OSNAME,
            V_I_ASSETTYPENO,
            V_I_ASSETTYPENAME,
            V_I_MAKENO,
            V_I_MAKENAME,
            V_I_NSRNO,
            V_I_NMAKENO,
            V_I_VMODEL,
            V_I_VSERIALNO,
            V_I_HOSTNAME,
            V_I_TAG,
            V_I_IPADDRESS,
            V_I_CPU,
            V_I_CORE,
            V_I_RAM,
            V_I_WARRANTY_AMC_DATE,
            V_I_PONO,
            V_I_SUBLOCATION,
            V_I_PORT,
            V_I_COMPLIANCE,
            V_I_DOMAIN,
            V_I_OS,
            V_I_NASSETNO2,
            V_I_NITEMNO2,
            V_I_CHKMONITOR1,
            V_I_PRINTER,
            V_I_ACTION,
            V_I_CLASSIFICATION
        }

        private void SetOracleParameter(List<OracleParameter> parameterList, string ParameterName, OracleDbType pOracleDbType, OracleCollectionType pOracleCollectionType, ParameterDirection pDirection, object Value)
        {
            OracleParameter dsrlistparam = new OracleParameter();
            dsrlistparam.ParameterName = ParameterName;
            dsrlistparam.OracleDbType = pOracleDbType;
            dsrlistparam.CollectionType = pOracleCollectionType;
            dsrlistparam.Direction = pDirection;

            if (ParameterDirection.Output != pDirection)
                dsrlistparam.Value = Value;

            parameterList.Add(dsrlistparam);
        }

        public OracleConnection GetConnection(string SchemaName)
        {
            OracleConnection cn = new OracleConnection();
            cn.ConnectionString = ConfigurationManager.ConnectionStrings[SchemaName].ConnectionString; //"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.16.83)(PORT=1531)))(CONNECT_DATA=(SERVICE_NAME=FFRPRD)));Persist Security Info=True;User ID=FFRCASIL;Password=FFRCASIL";
            return cn;
        }

        public string GetConnectionstring(string SchemaName)
        {
            return ConfigurationManager.ConnectionStrings[SchemaName].ConnectionString.ToString(); //"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.16.83)(PORT=1531)))(CONNECT_DATA=(SERVICE_NAME=FFRPRD)));Persist Security Info=True;User ID=FFRCASIL;Password=FFRCASIL";
        }

        public OracleConnection OpenConnection(OracleConnection conn)
        {
            conn.Open();
            return conn;
        }

        public OracleConnection CloseConnection(OracleConnection conn)
        {
            conn.Close();
            return conn;
        }

        public OracleCommand GetProcedureParameter(String ProcedureName, string SchemaName)
        {
            OracleConnection conn = new OracleConnection();
            conn = GetConnection(SchemaName);
            conn = OpenConnection(conn);

            OracleCommand cmd = new OracleCommand(ProcedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            OracleCommandBuilder.DeriveParameters(cmd);

            return cmd;
        }

        public Tuple<OracleConnection, OracleCommand> GetProcedureParameterTupal(String ProcedureName, String SchemaName)
        {
            OracleConnection conn = new OracleConnection();
            conn = GetConnection(SchemaName);
            conn = OpenConnection(conn);

            OracleCommand cmd = new OracleCommand(ProcedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            OracleCommandBuilder.DeriveParameters(cmd);

            return new Tuple<OracleConnection, OracleCommand>(conn, cmd);
        }

        public void SetParameterValue(OracleParameter Param, dynamic Value, Int32 Size = 0)
        {
            if (Param.Direction == ParameterDirection.Input)
            {
                if (Param.DbType.ToString().Equals(OracleDbType.Decimal.ToString()))
                {
                    Param.Value = !string.IsNullOrEmpty(Value) ? Convert.ToString(Convert.ToDouble(Value)) : null;
                }
                else if (Param.DbType.ToString().Equals(OracleDbType.Double.ToString()))
                {
                    Param.Value = Convert.ToDouble(Value);
                }
                else if (Param.DbType.ToString().Equals(OracleDbType.Date.ToString()))
                {
                    Param.Value = !string.IsNullOrEmpty(Value) ? DateTime.ParseExact(Value, "dd/MM/yyyy", null) : null;
                }
                else
                {
                    Param.Value = Value;
                }
            }
            if (Size > 0)
                Param.Size = Size;
        }

        internal int ChangePassword(string Employeeid, string OldPwd, string NewPwd)
        {
            int nStatus = -1;
            SuccessfactorOdataServiceReference.SuccessfactorOdataService service = new SuccessfactorOdataServiceReference.SuccessfactorOdataService();
            service.Timeout = -1;
            if (service.UpdatePassword(Employeeid, OldPwd, NewPwd))
                nStatus = 0;

            return nStatus;
        }

        public Int32 CheckLogin(string EmployeeID, string Password)
        {
            Int32 nStatus = -1;
            SuccessfactorOdataServiceReference.SuccessfactorOdataService sfsc = new SuccessfactorOdataServiceReference.SuccessfactorOdataService();
            try
            {

                sfsc.Timeout = -1;
                bool ReturnStatus = sfsc.ValidUser(EmployeeID, Password);
                //WorkLineServiceReference.WorklineServiceSoapClient serviceSoapClient = new WorkLineServiceReference.WorklineServiceSoapClient();
                //bool ReturnStatus = serviceSoapClient.WorklineValidUser();
                if (ReturnStatus)
                {
                    nStatus = 0;
                    var udl = sfsc.UserDetails(EmployeeID);
                    UserLoginData uld = new UserLoginData();
                    foreach (var item in udl)
                    {
                        uld = new UserLoginData
                        {
                            EMPNO = item.Empno,
                            EMPNAME = item.Empname,
                            MailID = item.Emailid,
                            MobileNo = item.Mobile,
                            STATUS = nStatus,
                            Token = Guid.NewGuid().ToString("N")
                        };
                    }

                    //CPLHelperService.CPLHelperServiceSoapClient sc = new CPLHelperServiceSoapClient();

                    //CPLHelperService.CPLHelperService objCPLHelperService = new CPLHelperService.CPLHelperService();

                    //var objUserLoginDataList = objCPLHelperService.UserDetails(uld.EMPNO);

                    // sc = new CPLHelperServiceSoapClient();
                    BindRoleCode(uld);

                    //Role Based Menu
                    List<Applicationmenu> objApplicationmenuList = new List<Applicationmenu>();
                    Applicationmenu objApplicationmenu = new Applicationmenu();
                    // sc = new CPLHelperServiceSoapClient();
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
                    Int32 applicationcode = Convert.ToInt32(ConfigurationManager.AppSettings["applicationcode"].ToString());
                    uld.ApplicationCode = applicationcode;

                    objApplicationmenuList = ApplicationMenuList(applicationcode, empno);
                    var roleid = (from n in objApplicationmenuList
                                  where n.Rolecode == objApplicationmenuList.Min(n2 => n2.Rolecode)
                                  select n.Rolecode).FirstOrDefault();
                    List<Applicationmenu> list = new List<Applicationmenu>();
                    list = (from n in objApplicationmenuList
                            where n.Rolecode.Equals(roleid)
                            select n).ToList();

                    //objApplicationmenuList = ApplicationMenuList(applicationcode, empno);

                    //var menu = sc.GetApplicationMenu(Convert.ToInt64(ConfigurationManager.AppSettings["applicationcode"].ToString()), empno);

                    //var roleid = (from n in menu
                    //              where n.Rolecode == menu.Min(n2 => n2.Rolecode)
                    //              select n.Rolecode).FirstOrDefault();

                    //var menu1 = from n in menu
                    //            where n.Rolecode.Equals(roleid)
                    //            select n;

                    //foreach (var item in menu1)
                    //{
                    //    objApplicationmenu = new Applicationmenu();
                    //    objApplicationmenu.Applicationcode = item.Applicationcode;
                    //    objApplicationmenu.Applicationname = item.Applicationname;
                    //    objApplicationmenu.Menucode = item.Menucode;
                    //    objApplicationmenu.Menuname = item.Menuname;
                    //    objApplicationmenu.Parentmenucode = item.Parentmenucode;
                    //    objApplicationmenu.Url = item.Url;
                    //    objApplicationmenu.Rolecode = item.Rolecode;
                    //    objApplicationmenu.Rolename = item.Rolename;
                    //    objApplicationmenu.Empcode = item.Empcode;
                    //    objApplicationmenu.Accesskey = item.Accesskey;
                    //    objApplicationmenuList.Add(objApplicationmenu);
                    //}

                    if (objApplicationmenuList.Count > 0)
                    {
                        HttpContext.Current.Session["UserObject"] = uld;
                        HttpContext.Current.Session["UserMenu"] = list;
                        HttpContext.Current.Session["LoginMsg"] = "Login Successful.....";
                    }
                    else
                    {
                        nStatus = 1;
                        HttpContext.Current.Session["LoginMsg"] = "You are not eligible user for this application.";
                    }
                }
                else
                {
                    HttpContext.Current.Session["LoginMsg"] = "Invalid login credential.....";
                }
            }
            catch (Exception e1)
            {
                HttpContext.Current.Session["LoginMsg"] = e1.Message.ToString();
                return nStatus;
            }

            return nStatus;
        }

        internal void BindRoleCode(UserLoginData uld)
        {
            CPLHelperService.CPLHelperServiceSoapClient sc = new CPLHelperServiceSoapClient();
            var rolelist = sc.GetEmployeeApplicationUserRole(Convert.ToInt64(ConfigurationManager.AppSettings["applicationcode"].ToString()), uld.EMPNO);

            var r = (from n in rolelist
                     where n.Rolecode == rolelist.Min(n2 => n2.Rolecode)
                     select n.Rolecode).FirstOrDefault();

            var rolelist1 = from n in rolelist
                            where n.Rolecode.Equals(r)
                            select n;

            if (rolelist1.Count() > 0)
            {
                foreach (var item in rolelist1) { uld.Rolecode = item.Rolecode; uld.Rolename = item.Rolename; }
            }
            else
            {
                var role = sc.GetEmployeeApplicationUserRole(Convert.ToInt64(ConfigurationManager.AppSettings["applicationcode"].ToString()), "0");
                foreach (var item in role) { uld.Rolecode = item.Rolecode; uld.Rolename = item.Rolename; }
            }
        }

        internal List<Applicationmenu> ApplicationMenuList(Int32 applicationcode, string empno)
        {
            List<Applicationmenu> objApplicationmenuList = new List<Applicationmenu>();
            Applicationmenu objApplicationmenu = new Applicationmenu();
            CPLHelperService.CPLHelperServiceSoapClient sc = new CPLHelperServiceSoapClient();
            var menu = sc.GetApplicationMenu(applicationcode, empno);
            foreach (var item in menu)
            {
                objApplicationmenu = new Applicationmenu();
                objApplicationmenu.Applicationcode = item.Applicationcode;
                objApplicationmenu.Applicationname = item.Applicationname;
                objApplicationmenu.Menucode = item.Menucode;
                objApplicationmenu.Menuname = item.Menuname;
                objApplicationmenu.Parentmenucode = item.Parentmenucode;
                objApplicationmenu.Url = item.Url;
                objApplicationmenu.Rolecode = item.Rolecode;
                objApplicationmenu.Rolename = item.Rolename;
                objApplicationmenu.Empcode = item.Empcode;
                objApplicationmenu.Accesskey = item.Accesskey;
                objApplicationmenuList.Add(objApplicationmenu);
            }
            return objApplicationmenuList;
        }

        public DataTable GetMenuDetails(String UserType)
        {
            OracleConnection cn = new OracleConnection();
            DataTable dt = new DataTable();

            using (cn = GetConnection(Schema.MDR.ToString()))
            {
                UserLoginData uld = new UserLoginData();
                uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

                OracleDataAdapter oda = new OracleDataAdapter(@"SELECT MENUCODE, PARENTMENUCODE, MENUNAME, URL, ACCESSKEY, URLPARAMETER FROM (SELECT A.APPLICATIONCODE, A.APPLICATIONNAME , B.MENUCODE, B.MENUNAME,
                    B.PARENTMENUCODE, B.URL, C.ROLECODE, D.ROLENAME, E.EMPCODE,B.ACCESSKEY, B.URLPARAMETER
                    FROM APPLICATIONMST A LEFT JOIN MENUMST B ON B.APPLICATIONCODE = A.APPLICATIONCODE AND B.NACTIVE = 1
                    LEFT JOIN APPROLEMENUMAP C ON C.APPLICATIONCODE = A.APPLICATIONCODE
                    AND C.MENUCODE = B.MENUCODE AND C.APPLICATIONCODE = A.APPLICATIONCODE
                    LEFT JOIN ROLEMST D ON D.ROLECODE = C.ROLECODE
                    LEFT JOIN APPROLEEMPMAP E ON E.ROLECODE = C.ROLECODE AND E.APPLICATIONCODE = A.APPLICATIONCODE
                    WHERE A.APPLICATIONCODE = 25
                    AND E.EMPCODE = '" + uld.EMPNO + "' "
                        + " AND E.ISACTIVE =1 ) "
                        + " START WITH PARENTMENUCODE = 0  "
                        + " CONNECT BY PRIOR MENUCODE = PARENTMENUCODE", cn);
                oda.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    oda = new OracleDataAdapter(@"SELECT MENUCODE, PARENTMENUCODE, MENUNAME, URL, ACCESSKEY, URLPARAMETER FROM
                    (SELECT A.APPLICATIONCODE, A.APPLICATIONNAME, B.MENUCODE, B.MENUNAME,
                    B.PARENTMENUCODE, B.URL, C.ROLECODE, D.ROLENAME, B.ACCESSKEY, B.URLPARAMETER
                    FROM APPLICATIONMST A LEFT JOIN MENUMST B ON B.APPLICATIONCODE = A.APPLICATIONCODE AND B.NACTIVE = 1
                    LEFT JOIN APPROLEMENUMAP C ON C.APPLICATIONCODE = A.APPLICATIONCODE
                    AND C.MENUCODE = B.MENUCODE AND C.APPLICATIONCODE = A.APPLICATIONCODE
                    LEFT JOIN ROLEMST D ON D.ROLECODE = C.ROLECODE
                    WHERE A.APPLICATIONCODE = 25
                    AND D.ROLECODE  = decode('" + UserType + "','ENG', 1, 'CL', '4', '1')) "
                    + " START WITH PARENTMENUCODE = 0  "
                    + " CONNECT BY PRIOR MENUCODE = PARENTMENUCODE", cn);
                    oda.Fill(dt);
                }
            }

            return dt;
        }

        internal DataTable DownloadReport(MobileReimbursement.BusinessClass.asset_report objasset_report, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.V_I_NSTATUS.ToString(), OracleDbType.Varchar2, OracleCollectionType.None, ParameterDirection.Input, objasset_report.Status);
                SetOracleParameter(parameterList, ParameterName.V_I_DFROMDATE.ToString(), OracleDbType.Varchar2, OracleCollectionType.None, ParameterDirection.Input, objasset_report.FromDate);
                SetOracleParameter(parameterList, ParameterName.V_I_DTODATE.ToString(), OracleDbType.Date, OracleCollectionType.None, ParameterDirection.Input, objasset_report.ToDate);
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.ASSET_REPORT, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.ASSET_REPORT, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetItem()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETITEM, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETITEM, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetHOD()
        {
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_EMPNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = uld.EMPNO;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.APPROVAL_HOD, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.APPROVAL_HOD, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetCompany()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETCOMPANY, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETCOMPANY, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        //internal DataTable GetDivision(int? CompanyID)
        internal DataTable GetDivision(string CompanyID)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();

                dsrlistparam.ParameterName = ParameterName.V_I_COMPANY.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = CompanyID;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETDIVISION, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETDIVISION, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetUnit(string CompanyID, string DivisionID)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_COMPANY.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = CompanyID;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_DIVISION.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = DivisionID;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETUNIT, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETUNIT, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetDepartment(string CompanyID, string DivisionID, string UnitID)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_COMPANY.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = CompanyID;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_DIVISION.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = DivisionID;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_UNIT.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = UnitID;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETDEPARTMENT, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETDEPARTMENT, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetDesignation(string CompanyID)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam.ParameterName = ParameterName.V_I_COMPANY.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = CompanyID;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETDESIGNATION, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETDESIGNATION, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetLocation()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETLOCATION, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETLOCATION, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetAssetHOD()
        {
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_EMPNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = uld.EMPNO;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETHOD, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETHOD, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void AssetRequest(am_item_request objam_item_request, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.SAVE_ITEM_EMPNEWREQUEST.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.SAVE_ITEM_EMPNEWREQUEST.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQUESTNO.ToString()], objam_item_request.Nrequestno.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQTYPE.ToString()], objam_item_request.Nreqtype.ToString(), 1);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VEMPNO.ToString()], uld.EMPNO.ToString(), 8);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NITEMNO.ToString()], objam_item_request.Nitemno.ToString(), 6);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VHODEMPNO.ToString()], objam_item_request.Vhodempno.ToString(), 8);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VREASON.ToString()], objam_item_request.Vreason.ToString(), 200);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NALLOCATIONTYPE.ToString()], objam_item_request.Nallocationtype.ToString(), 1);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_DALLOCATIONDATE.ToString()], objam_item_request.Dallocationdate.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VEMPNAME.ToString()], objam_item_request.Vempname.ToString(), 40);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NCOMPANY.ToString()], objam_item_request.Ncompany.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NDIVISION.ToString()], objam_item_request.Ndivision.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NDEPARTMENT.ToString()], objam_item_request.Ndepartment.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NDESIGNATION.ToString()], objam_item_request.Ndesignation.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NLOCATION.ToString()], objam_item_request.Nlocation.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_DJOININGDATE.ToString()], objam_item_request.Djoiningdate.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NUNIT.ToString()], objam_item_request.Nunit.ToString(), 10);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetAssetRequest(string Self)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_VEMPNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = uld.EMPNO;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_SELF.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Self;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETASSETREQUEST, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETASSETREQUEST, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable fetchAssetReqDetails(string Reqno, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.V_I_NREQUESTNO.ToString(), OracleDbType.Varchar2, OracleCollectionType.None, ParameterDirection.Input, Reqno);
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.EDITASSETREQUEST, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.EDITASSETREQUEST, parameterList.ToArray());
                status = 0;
                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message;
            }

            return dt;
        }

        internal void DeleteAssetRequest(string Reqno, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            OracleCommand cmd = null;

            //cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.DELETE_EMP_NEWREQUEST.ToString(), Schema.ASSET.ToString());
            cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.DELETE_EMP_NEWREQUEST.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.T_NREQUESTNO.ToString()], Reqno.ToString(), 20);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal void RequestReminder(string Reqno, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            OracleCommand cmd = null;

            //cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.APPROVAL_REMINDER.ToString(), Schema.ASSET.ToString());
            cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.APPROVAL_REMINDER.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQUESTNO.ToString()], Reqno.ToString(), 20);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable AssetHistory(string Reqno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.V_I_NREQUESTNO.ToString(), OracleDbType.Varchar2, OracleCollectionType.None, ParameterDirection.Input, Reqno);
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.ASSETHISTORY, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.ASSETHISTORY, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetAssetApprovalRequest()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_VEMPNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = uld.EMPNO;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETAPPROVALREQUEST, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETAPPROVALREQUEST, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable ViewRequest(string Reqno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                SetOracleParameter(parameterList, ParameterName.V_I_NREQUESTNO.ToString(), OracleDbType.Varchar2, OracleCollectionType.None, ParameterDirection.Input, Reqno);
                SetOracleParameter(parameterList, ParameterName.V_I_VAPPROVENO.ToString(), OracleDbType.Varchar2, OracleCollectionType.None, ParameterDirection.Input, uld.EMPNO);
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETASSETREQDETAILS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETASSETREQDETAILS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetPendingAcceptance()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.PENDING_USER_ACCEPTANCE, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.PENDING_USER_ACCEPTANCE, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void UnexpectedDeallocation(string Reqno, string Remarks, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            OracleCommand cmd = null;
            //cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.UNEXPECTED_DEALLOCATION.ToString(), Schema.ASSET.ToString());
            cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.UNEXPECTED_DEALLOCATION.ToString(), Schema.ASSET.ToString());
            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQUESTNO.ToString()], Reqno.ToString(), 20);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VDEALLOCATEREMARKS.ToString()], Remarks.ToString(), 100);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VDEALLOCATEBY.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetpendingAllocation()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.PENDING_ALLOCATION_LIST, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.PENDING_ALLOCATION_LIST, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void RejectApprovedRequest(string Reqno, string Remarks, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            OracleCommand cmd = null;
            //cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.REJECT_APPROVED_REQUEST.ToString(), Schema.ASSET.ToString());
            cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.REJECT_APPROVED_REQUEST.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQUESTNO.ToString()], Reqno.ToString(), 20);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VAPPROVENO.ToString()], uld.EMPNO.ToString(), 8);
                SetParameterValue(cmd.Parameters[ParameterName.VAPPROVEREMARKS.ToString()], Remarks.ToString(), 100);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetAllocationEmployee(string Search)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_SEARCH.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Search;
                dsrlistparam.Size = 200;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETEMPLOYEE, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETEMPLOYEE, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetAllocationEmployeeDetails(string Empno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_EMPNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Int32;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Empno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETEMPDETAILS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETEMPDETAILS, parameterList.ToArray());
                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetItemType()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETITEM, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETITEM, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetSerialNo(string ItemNo, string Receipt)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_RECEIPT.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Int32;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Receipt;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_ITEMNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Int32;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = ItemNo;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETSERIALNO, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETSERIALNO, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void SubmitAllocationRequest(am_item_request objam_item_request, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.SAVE_ITEM_ALLOTMENT.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.SAVE_ITEM_ALLOTMENT.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VALLOCATEDTO.ToString()], objam_item_request.Vempno.ToString(), 8);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NITEMNO.ToString()], objam_item_request.Nitemno.ToString(), 6);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NASSETNO.ToString()], !string.IsNullOrEmpty(objam_item_request.Assetno) ? objam_item_request.Assetno.ToString() : string.Empty, 6);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VHOSTNAME.ToString()], !string.IsNullOrEmpty(objam_item_request.Hostname) ? objam_item_request.Hostname.ToString() : string.Empty, 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_DALLOCATIONDATE.ToString()], objam_item_request.Dallocationdate.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VREMARKS.ToString()], objam_item_request.Vreason.ToString(), 200);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VALLOCATEDBY.ToString()], uld.EMPNO, 8);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQNO.ToString()], objam_item_request.Nrequestno.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_CHKMONITOR.ToString()], objam_item_request.chkbox.ToString(), 1);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NITEMNO1.ToString()], objam_item_request.Nitemno1.ToString(), 6);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NASSETNO1.ToString()], objam_item_request.Assetno1.ToString(), 6);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_COMPLIANCE.ToString()], objam_item_request.Compliance.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_DOMAIN.ToString()], objam_item_request.Domain.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_OS.ToString()], objam_item_request.OS.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_PRINTER.ToString()], objam_item_request.chkbox1.ToString(), 1);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NITEMNO2.ToString()], objam_item_request.Nitemno2.ToString(), 6);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NASSETNO2.ToString()], objam_item_request.Assetno2.ToString(), 6);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_CLASSIFICATION.ToString()], objam_item_request.Classification.ToString(), 3);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }
        internal DataTable GetDeAllocationEmployee(string Search)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_SEARCH.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Search;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETDEALLOCATIONEMPLOYEE, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETDEALLOCATIONEMPLOYEE, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        internal DataTable GetAllocatedItems(string Empno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_ITEMNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Empno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETALLOCATEDITEM, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETALLOCATEDITEM, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        internal DataTable GetAllocatedItemsDetails(string Itemno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_ITEMNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Int32;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Itemno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETALLOCATEDITEMDETAILS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETALLOCATEDITEMDETAILS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        internal void SubmitDeAllocationRequest(am_item_request objam_item_request, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.SAVE_ITEM_DEALLOTMENT.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.SAVE_ITEM_DEALLOTMENT.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQUESTNO.ToString()], objam_item_request.Nrequestno.ToString(), 7);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NSTATUS.ToString()], objam_item_request.Nstatus.ToString(), 1);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_DDEALLOCATEDATE.ToString()], objam_item_request.Dallocationdate.ToString(), 15);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VDEALLOCATEREMARKS.ToString()], objam_item_request.Vreason.ToString(), 200);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VDEALLOCATEBY.ToString()], uld.EMPNO, 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }
        internal DataTable GetSerialNoDetail(string Serialno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_SERIALNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Serialno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.SEARCH_SERIALNO, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.SEARCH_SERIALNO, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        internal DataTable AssetRequestStatus(asset_report objasset_report, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.V_I_DFROMDATE.ToString(), OracleDbType.Date, OracleCollectionType.None, ParameterDirection.Input, objasset_report.FromDate);
                SetOracleParameter(parameterList, ParameterName.V_I_DTODATE.ToString(), OracleDbType.Date, OracleCollectionType.None, ParameterDirection.Input, objasset_report.ToDate);
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.ASSET_REQUEST_STATUS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.ASSET_REQUEST_STATUS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetPendingAcceptanceList(string RequestNo)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_NREQUESTNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = RequestNo;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_VALLOCATEDTO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = uld.EMPNO;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETPENDINGACCEPTANCE, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETPENDINGACCEPTANCE, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        internal void AssetAcceptanceEmployee(string Itemno, string Remarks, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            OracleCommand cmd = null;
            //cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.SAVE_ITEM_RECEIVE.ToString(), Schema.ASSET.ToString());
            cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.SAVE_ITEM_RECEIVE.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQUESTNO.ToString()], Itemno.ToString(), 20);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VREMARKS.ToString()], Remarks.ToString(), 100);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }
        internal DataTable GetFinanceController()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.FETCH_FC_DETAILS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.FETCH_FC_DETAILS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void ApproveRequest(string Reqno, string Status, string Remarks, string fcempno, out int status, out string message, out string statusdesc)
        {
            status = -1;
            message = string.Empty;
            statusdesc = string.Empty;
            int count = 0;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE.ToString() + "." + Procedurename.NEW_REQUEST_APPROVAL.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.NEW_REQUEST_APPROVAL.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NREQUESTNO.ToString()], Reqno, 20);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VAPPROVENO.ToString()], uld.EMPNO, 8);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VAPPROVEREMARKS.ToString()], Remarks, 100);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NSTATUS.ToString()], Status, 1);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VFCEMPNO.ToString()], fcempno, 8);
                SetParameterValue(cmd.Parameters[ParameterName.V_O_NSTATUSDESC.ToString()], null, 20);

                count = cmd.ExecuteNonQuery();

                statusdesc = Convert.ToString(cmd.Parameters[ParameterName.V_O_NSTATUSDESC.ToString()].Value);

                cmd.Connection.Close();
                if (count == -1)
                    status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetGXPList()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETGXP_NONGXP, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETGXP_NONGXP, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetClassfication()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETGXP_NONGXP, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.ASSET_CLASSIFICATION, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetDomain()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETASSET_DOMAIN, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETASSET_DOMAIN, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetOS()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL + "." + Procedurename.GETASSET_OS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_COMMON_DDL_SF + "." + Procedurename.GETASSET_OS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetSoftwareAsset(string SRNO)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_SRNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = SRNO;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SOFTWARE_MASTER + "." + Procedurename.GET_ASSET_SOFTWARE_DETAILS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SOFTWARE_MASTER_SF + "." + Procedurename.GET_ASSET_SOFTWARE_DETAILS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void DeleteSoftwareAsset(string NSRNO, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SOFTWARE_MASTER.ToString() + "." + Procedurename.DELETE_ASSET_SOFTWARE_DETAILS.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SOFTWARE_MASTER_SF.ToString() + "." + Procedurename.DELETE_ASSET_SOFTWARE_DETAILS.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_SRNO.ToString()], NSRNO.ToString(), 20);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetLicenseProduct()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                UserLoginData uld = new UserLoginData();
                uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SOFTWARE_MASTER + "." + Procedurename.GET_ASSET_LICENSE_PRODUCT, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SOFTWARE_MASTER_SF + "." + Procedurename.GET_ASSET_LICENSE_PRODUCT, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void SubmitSoftwareAsset(software_asset objsoftware_asset, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SOFTWARE_MASTER.ToString() + "." + Procedurename.UPSERT_ASSET_SOFTWARE_DETAILS.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SOFTWARE_MASTER_SF.ToString() + "." + Procedurename.UPSERT_ASSET_SOFTWARE_DETAILS.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_SRNO.ToString()], objsoftware_asset.NSRNO.ToString(), 5);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LICENSEPRODUCT.ToString()], objsoftware_asset.LICENSEPRODUCTCODE.ToString(), 5);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LICENSEVERSION.ToString()], objsoftware_asset.LICENSEVERSION.ToString(), 40);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_QUANTITY.ToString()], objsoftware_asset.QUANTITY.ToString(), 5);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_EXPIRYDATE.ToString()], objsoftware_asset.EXPIRYDATE.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_REMARKS.ToString()], objsoftware_asset.REMARKS.ToString(), 100);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LOCATION.ToString()], objsoftware_asset.LOCATIONCODE.ToString(), 4);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable ExporttoExcel(out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SOFTWARE_MASTER + "." + Procedurename.EXPORTTOEXCEL, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SOFTWARE_MASTER_SF + "." + Procedurename.EXPORTTOEXCEL, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable GetLicenceProduct(string Licenseproductcode)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_LICENSEPRODUCTNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Licenseproductcode;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SOFTWARE_MASTER + "." + Procedurename.GET_ASSET_LICENSE_PRODUCT, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SOFTWARE_MASTER_SF + "." + Procedurename.GET_ASSET_LICENSE_PRODUCT, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void DeleteLicenceProduct(string Licenseproductcode, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SOFTWARE_MASTER.ToString() + "." + Procedurename.DELETE_ASSET_LICENSE_PRODUCT.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SOFTWARE_MASTER_SF.ToString() + "." + Procedurename.DELETE_ASSET_LICENSE_PRODUCT.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LICENSEPRODUCTNO.ToString()], Licenseproductcode.ToString(), 4);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal void SubmitLicenseProduct(software_asset objsoftware_asset, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SOFTWARE_MASTER.ToString() + "." + Procedurename.UPSERT_ASSET_LICENSE_PRODUCT.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SOFTWARE_MASTER_SF.ToString() + "." + Procedurename.UPSERT_ASSET_LICENSE_PRODUCT.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LICENSEPRODUCTNO.ToString()], objsoftware_asset.LICENSEPRODUCTCODE.ToString(), 4);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LICENSEPRODUCTNAME.ToString()], objsoftware_asset.LICENSEPRODUCT.ToString(), 100);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetVendorDetails(string Vendorno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_VENDORNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Vendorno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER + "." + Procedurename.GET_ASSET_SERVER_VENDOR, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER_SF + "." + Procedurename.GET_ASSET_SERVER_VENDOR, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void DeleteVendorDetails(string Vendorno, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_VENDOR.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_VENDOR.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VENDORNO.ToString()], Vendorno.ToString(), 4);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal void SubmitVendorDetails(Server_Asset objServer_Asset, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.UPSERT_ASSET_SERVER_VENDOR.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.UPSERT_ASSET_SERVER_VENDOR.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VENDORNO.ToString()], objServer_Asset.VENDORNO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VENDORNAME.ToString()], objServer_Asset.VENDORNAME.ToString(), 40);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_CONTACTPERSON.ToString()], objServer_Asset.CONTACTPERSON.ToString(), 30);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_CONTACTNUMBER.ToString()], objServer_Asset.CONTACTNUMBER.ToString(), 38);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetWarrantyDetails(string Warrantyno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_WARRANTY_AMC_NO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Warrantyno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER + "." + Procedurename.GET_ASSET_SERVER_WARRANTY, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER_SF + "." + Procedurename.GET_ASSET_SERVER_WARRANTY, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void DeleteWarrantyDetails(string Warrantyno, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_WARRANTY.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_WARRANTY.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_WARRANTY_AMC_NO.ToString()], Warrantyno.ToString(), 4);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal void SubmitWarrantyDetails(Server_Asset objServer_Asset, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.UPSERT_ASSET_SERVER_WARRANTY.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.UPSERT_ASSET_SERVER_WARRANTY.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_WARRANTY_AMC_NO.ToString()], objServer_Asset.WARRANTY_AMC_NO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_WARRANTY_AMC_TYPE.ToString()], objServer_Asset.WARRANTY_AMC_TYPE.ToString(), 30);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetServerOSDetails(string OSNO)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_OSNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = OSNO;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER + "." + Procedurename.GET_ASSET_SERVER_OS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER_SF + "." + Procedurename.GET_ASSET_SERVER_OS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void DeleteOSNODetails(string OSNO, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_OS.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_OS.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_OSNO.ToString()], OSNO.ToString(), 4);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal void SubmitOSDetails(Server_Asset objServer_Asset, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.ADD_ASSET_SERVER_OS.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.ADD_ASSET_SERVER_OS.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_OSNO.ToString()], objServer_Asset.OSNO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_OSNAME.ToString()], objServer_Asset.OSNAME.ToString(), 30);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetAssetTypeDetails(string Assettypeno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_ASSETTYPENO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Assettypeno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER + "." + Procedurename.GET_ASSET_SERVER_ASSETTYPE, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER_SF + "." + Procedurename.GET_ASSET_SERVER_ASSETTYPE, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void DeleteServerAssetTypeDetails(string Assettypeno, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_ASSETTYPE.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_ASSETTYPE.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_ASSETTYPENO.ToString()], Assettypeno.ToString(), 4);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal void SubmitServerAssetType(Server_Asset objServer_Asset, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.ADD_ASSET_SERVER_ASSETTYPE.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.ADD_ASSET_SERVER_ASSETTYPE.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_ASSETTYPENO.ToString()], objServer_Asset.ASSETTYPENO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_ASSETTYPENAME.ToString()], objServer_Asset.ASSETTYPENAME.ToString(), 30);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetServerMake(string Makeno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_MAKENO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Makeno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER + "." + Procedurename.GET_ASSET_MAKE, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER_SF + "." + Procedurename.GET_ASSET_MAKE, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void DeleteServerMake(string Makeno, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_MAKE.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_MAKE.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_MAKENO.ToString()], Makeno.ToString(), 4);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal void SubmitServerMake(Server_Asset objServer_Asset, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.ADD_ASSET_SERVER_MAKE.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.ADD_ASSET_SERVER_MAKE.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_MAKENO.ToString()], objServer_Asset.MAKENO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_MAKENAME.ToString()], objServer_Asset.MAKENAME.ToString(), 30);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetServerAsset(string SRNO)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_NSRNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = SRNO;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER + "." + Procedurename.GET_ASSET_SERVER_DETAILS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER_SF + "." + Procedurename.GET_ASSET_SERVER_DETAILS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable ExporttoExcelServerAsset(out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER + "." + Procedurename.EXPORTTOEXCEL_SERVERASSET, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.ASSET_SERVER_MASTER_SF + "." + Procedurename.EXPORTTOEXCEL_SERVERASSET, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal void DeleteServerAsset(string NSRNO, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.DELETE_ASSET_SERVER_DETAILS.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NSRNO.ToString()], NSRNO.ToString(), 5);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal void SubmitServerAsset(Server_Asset objServer_Asset, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            //OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER.ToString() + "." + Procedurename.UPSERT_ASSET_SERVER_DETAILS.ToString(), Schema.ASSET.ToString());
            OracleCommand cmd = GetProcedureParameter(Packagename.ASSET_SERVER_MASTER_SF.ToString() + "." + Procedurename.UPSERT_ASSET_SERVER_DETAILS.ToString(), Schema.ASSET.ToString());

            try
            {
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NSRNO.ToString()], objServer_Asset.NSRNO.ToString(), 5);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NMAKENO.ToString()], objServer_Asset.MAKENO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VMODEL.ToString()], objServer_Asset.VMODEL.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VSERIALNO.ToString()], objServer_Asset.VSERIALNO.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_ASSETTYPENO.ToString()], objServer_Asset.ASSETTYPENO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_HOSTNAME.ToString()], objServer_Asset.HOSTNAME.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_TAG.ToString()], objServer_Asset.TAG.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_IPADDRESS.ToString()], objServer_Asset.IPADDRESS.ToString(), 30);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_CPU.ToString()], objServer_Asset.IPADDRESS.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_CORE.ToString()], objServer_Asset.CORE.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_RAM.ToString()], objServer_Asset.RAM.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_OSNO.ToString()], objServer_Asset.OSNO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_WARRANTY_AMC_NO.ToString()], objServer_Asset.WARRANTY_AMC_NO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_WARRANTY_AMC_DATE.ToString()], objServer_Asset.WARRANTY_AMC_DATE.ToString(), 10);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_PONO.ToString()], objServer_Asset.PONO.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VENDORNO.ToString()], objServer_Asset.VENDORNO.ToString(), 3);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LOCATION.ToString()], objServer_Asset.Locationcode.ToString(), 4);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_SUBLOCATION.ToString()], objServer_Asset.SUBLocationName.ToString(), 20);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_PORT.ToString()], objServer_Asset.PORT.ToString(), 60);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_LEMPNO.ToString()], uld.EMPNO.ToString(), 8);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable GetRequestStatusListJQGrid()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GRID_VIEW_REQUEST_STATUS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GRID_VIEW_REQUEST_STATUS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        //Bind HostName with SerialNo
        internal DataTable GetHostnameDetails(string Vserialno)
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.V_I_VSERIALNO.ToString();
                dsrlistparam.OracleDbType = OracleDbType.Varchar2;
                dsrlistparam.Direction = ParameterDirection.Input;
                dsrlistparam.Value = Vserialno;
                parameterList.Add(dsrlistparam);

                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();
                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.GETVSERIALNODETAILS, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.GETVSERIALNODETAILS, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        internal DataTable fetchResignedEmpAsset()
        {
            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];
            try
            {
                OracleParameter dsrlistparam = new OracleParameter();
                dsrlistparam = new OracleParameter();
                dsrlistparam.ParameterName = ParameterName.FETCH_DETAILS.ToString();
                dsrlistparam.OracleDbType = OracleDbType.RefCursor;
                dsrlistparam.Direction = ParameterDirection.Output;
                parameterList.Add(dsrlistparam);

                objOracleDataAccess = new OracleDataAccess();

                //ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE + "." + Procedurename.VIEWRESIGNEDEMPASSET, parameterList.ToArray());
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.AM_ITEM_PACKAGE_SF + "." + Procedurename.VIEWRESIGNEDEMPASSET, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        #region :: SSO ::
        internal void InsertIntoSSO(UserLoginData uld)
        {

            //OracleConnection cn = null;
            //OracleCommand cmd = null;
            try
            {
                var info = GetProcedureParameterTupal(Packagename.SSO_DML + "." + Procedurename.INSERT_SSO, "MDR");
                //cn = info.Item1;
                using (OracleConnection cn = info.Item1)
                {

                    using (OracleCommand cmd = info.Item2)
                    {
                        cmd.Connection = cn;
                        //cmd = info.Item2;
                        SetParameterValue(cmd.Parameters["V_I_VEMPLOYEEID"], uld.EMPNO, 8);
                        SetParameterValue(cmd.Parameters["V_I_NAPPLICATIONCODE"], Convert.ToString(uld.ApplicationCode), 3);
                        SetParameterValue(cmd.Parameters["V_I_VTOKEN"], uld.Token, 40);
                        SetParameterValue(cmd.Parameters["N_I_EXTTIME"], uld.Nexttime, 40);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //cmd.Dispose();
                //cn.Close();
            }

        }
        internal DataTable GetSSODetails(string token, Int32 applicationcode, string nexttime)
        {
            OracleConnection cn = null;
            OracleCommand cmd = null;
            DataTable dt = new DataTable();
            try
            {
                var info = GetProcedureParameterTupal(Packagename.SSO_DML + "." + Procedurename.GETSSO, "MDR");
                cn = info.Item1;
                cmd = info.Item2;
                SetParameterValue(cmd.Parameters["V_I_VTOKEN"], token, 40);
                SetParameterValue(cmd.Parameters["N_I_NLLOGAPPCODE"], applicationcode.ToString(), 3);
                SetParameterValue(cmd.Parameters["N_I_EXTTIME"], nexttime, 40);
                cmd.ExecuteNonQuery();
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
            }
            return dt;
        }
        internal void DeleteSSODetails(string token)
        {

            OracleConnection cn = null;
            OracleCommand cmd = null;
            try
            {
                var info = GetProcedureParameterTupal(Packagename.SSO_DML + "." + Procedurename.DELETE_SSO, "MDR");
                cn = info.Item1;
                cmd = info.Item2;

                SetParameterValue(cmd.Parameters["V_I_VTOKEN"], token, 40);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                cn.Close();
            }

        }
        #endregion
        internal void SubmitSearchAsset(am_item_request objam_item_request, out int status, out string message)
        {
            status = -1;
            message = string.Empty;
            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            OracleCommand cmd = GetProcedureParameter(Packagename.AM_ITEM_PACKAGE_SF.ToString() + "." + Procedurename.UPDATE_ALLOCATION_DETAILS.ToString(), Schema.ASSET.ToString());

            try
            {
                //SetParameterValue(cmd.Parameters[ParameterName.V_I_VALLOCATEDTO.ToString()], uld.EMPNO, 7);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VHOSTNAME.ToString()], objam_item_request.Hostname.ToString(), 15);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_OS.ToString()], objam_item_request.OS.ToString(), 1);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_REMARKS.ToString()], objam_item_request.Remarks.ToString(), 200);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_VALLOCATEDTO.ToString()], objam_item_request.Vempno.ToString(), 7);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_NASSETNO.ToString()], objam_item_request.Assetno1.ToString(), 7);
                SetParameterValue(cmd.Parameters[ParameterName.V_I_ACTION.ToString()], objam_item_request.Nstatus.ToString(), 1);

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                status = 0;
            }
            catch (Exception ex)
            {
                status = -1;
                message = ex.Message.ToString();
                cmd.Connection.Close();
            }
        }

        internal DataTable FSAssetReport(asset_report objasset_report, out int status, out string message)
        {
            status = -1;
            message = string.Empty;

            UserLoginData uld = new UserLoginData();
            uld = (UserLoginData)HttpContext.Current.Session["UserObject"];

            List<OracleParameter> parameterList = new List<OracleParameter>();
            OracleDataAccess objOracleDataAccess = new OracleDataAccess();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                SetOracleParameter(parameterList, ParameterName.V_I_DFROMDATE.ToString(), OracleDbType.Varchar2, OracleCollectionType.None, ParameterDirection.Input, objasset_report.FromDate);
                SetOracleParameter(parameterList, ParameterName.V_I_DTODATE.ToString(), OracleDbType.Date, OracleCollectionType.None, ParameterDirection.Input, objasset_report.ToDate);
                SetOracleParameter(parameterList, ParameterName.FETCH_DETAILS.ToString(), OracleDbType.RefCursor, OracleCollectionType.None, ParameterDirection.Output, null);

                objOracleDataAccess = new OracleDataAccess();
                ds = objOracleDataAccess.ExecuteDataset(GetConnectionstring(Schema.ASSET.ToString()), CommandType.StoredProcedure, Packagename.FS_ASSET + "." + Procedurename.FS_ASSET_REPORT, parameterList.ToArray());

                if (ds.Tables.Count > 0) { if (ds.Tables[0].Rows.Count > 0) { dt = ds.Tables[0]; } }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
    }
}