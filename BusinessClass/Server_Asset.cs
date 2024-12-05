using System;

namespace MobileReimbursement.BusinessClass
{
    public class Server_Asset 
    {
        private Int32? _VENDORNO;
        public Int32? VENDORNO { get; set; }

        private String _VENDORNAME;
        public String VENDORNAME { get; set; }

        private String _CONTACTPERSON;
        public String CONTACTPERSON { get; set; }

        private Int64? _CONTACTNUMBER;
        public Int64? CONTACTNUMBER { get; set; }

        private string _Action;
        public string Action { get; set; }

        public Int32 reqStatus { get; set; }
        public string reqMessage { get; set; }

        private Int32? _WARRANTY_AMC_NO;
        public Int32? WARRANTY_AMC_NO { get; set; }

        private String _WARRANTY_AMC_TYPE;
        public String WARRANTY_AMC_TYPE { get; set; }

        private Int32? _OSNO;
        public Int32? OSNO { get; set; }

        private String _OSNAME;
        public String OSNAME { get; set; }

        private Int32? _ASSETTYPENO;
        public Int32? ASSETTYPENO { get; set; }

        private String _ASSETTYPENAME;
        public String ASSETTYPENAME { get; set; }

        private Int32? _MAKENO;
        public Int32? MAKENO { get; set; }

        private String _VMODEL;
        public String VMODEL { get; set; }

        private String _MAKENAME;
        public String MAKENAME { get; set; }

        private Int32? _NSRNO;
        public Int32? NSRNO { get; set; }

        private String _VSERIALNO;
        public String VSERIALNO { get; set; }

        private String _HOSTNAME;
        public String HOSTNAME { get; set; }

        private String _TAG;
        public String TAG { get; set; }

        private String _IPADDRESS;
        public String IPADDRESS { get; set; }

        private String _CPU;
        public String CPU { get; set; }

        private String _CORE;
        public String CORE { get; set; }

        private String _RAM;
        public String RAM { get; set; }

        private String _WARRANTY_AMC_DATE;
        public String WARRANTY_AMC_DATE { get; set; }

        private String _PONO;
        public String PONO { get; set; }

        private Int32? _Locationcode;
        public Int32? Locationcode { get; set; }

        private String _LocationName;
        public String LocationName { get; set; }

        private String _SUBLocationName;
        public String SUBLocationName { get; set; }

        private String _PORT;
        public String PORT { get; set; }
    }
}
