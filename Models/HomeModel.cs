using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileReimbursement.Models
{
    public class HomeModel
    {
    }
    [Serializable]
    public class UserLoginData
    {
        public long Rolecode;
        public string Rolename;
        public String EMPNO { get; set; }
        public String EMPNAME { get; set; }
        public Int32 STATUS { get; set; }
        public String MailID { get; set; }
        public String MobileNo { get; set; }
        public String usertype { get; set; }
        public string emptype { get; set; }
        public string division { get; set; }
        public string divisionname { get; set; }
        public Int64 unit { get; set; }
        public string unitname { get; set; }
        public Int64 loccd { get; set; }
        public string locname { get; set; }
        public Int64 company { get; set; }
        public string cmpname { get; set; }
        public string Token { get => _Token; set => _Token = value; }
        public int ApplicationCode { get => _ApplicationCode; set => _ApplicationCode = value; }
        public DateTime? CurrentLoginTime { get => _CurrentLoginTime; set => _CurrentLoginTime = value; }
        public DateTime? NextLoginTime { get => _NextLoginTime; set => _NextLoginTime = value; }
        public string Nexttime { get => _Nexttime; set => _Nexttime = value; }

        private string _Token;
        private Int32 _ApplicationCode;

        private DateTime? _CurrentLoginTime;
        private DateTime? _NextLoginTime;
        private string _Nexttime;

        public UserLoginData()
        {

        }
    }
    public class LoginModel
    {
        public LoginModel()
        {

        }

        public String uid { get; set; }

        public String pwd { get; set; }

        public String LoginError { get; set; }

        public String MessageCode { get; set; }

        public String RoleName { get; set; }
    }
    public class MenuDetails
    {
        public MenuDetails()
        {

        }
        public String MenuCode { get; set; }
        public String ParentMenuCode { get; set; }
        public String MenuName { get; set; }
        public String URL { get; set; }
        public String AccessKey { get; set; }
    }

    public class Applicationmenu
    {
        private Int64 applicationcode;

        public Int64 Applicationcode
        {
            get { return applicationcode; }
            set { applicationcode = value; }
        }

        private string applicationname;

        public string Applicationname
        {
            get { return applicationname; }
            set { applicationname = value; }
        }

        private Int64 menucode;

        public Int64 Menucode
        {
            get { return menucode; }
            set { menucode = value; }
        }

        private string menuname;

        public string Menuname
        {
            get { return menuname; }
            set { menuname = value; }
        }

        private Int64 parentmenucode;

        public Int64 Parentmenucode
        {
            get { return parentmenucode; }
            set { parentmenucode = value; }
        }

        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private Int64 rolecode;

        public Int64 Rolecode
        {
            get { return rolecode; }
            set { rolecode = value; }
        }

        private string rolename;

        public string Rolename
        {
            get { return rolename; }
            set { rolename = value; }
        }

        private string empcode;

        public string Empcode
        {
            get { return empcode; }
            set { empcode = value; }
        }

        private string accesskey;

        public string Accesskey
        {
            get { return accesskey; }
            set { accesskey = value; }
        }
    }
}