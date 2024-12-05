using System;

namespace MobileReimbursement.BusinessClass
{
    public class am_item_request
    {
        private Int32? _Nrequestno;
        public Int32? Nrequestno { get; set; }

        private Int32 _Nreqtype;
        public Int32 Nreqtype { get; set; }

        private String _Vempno;
        public String Vempno { get; set; }

        private Int32 _Nitemno;
        public Int32 Nitemno { get; set; }

        private Int32 _Nqty;
        public Int32 Nqty { get; set; }

        private Int32 _Ncancleqty;
        public Int32 Ncancleqty { get; set; }

        private String _Vhodempno;
        public String Vhodempno { get; set; }

        private String _Vfcempno;
        public String Vfcempno { get; set; }

        private String _Vreason;
        public String Vreason { get; set; }

        private String _Vcanclereason;
        public String Vcanclereason { get; set; }

        private Int32? _Nstatus;
        public Int32? Nstatus { get; set; }

        private Int32 _Nallotreqno;
        public Int32 Nallotreqno { get; set; }

        private Int32 _Nallocationtype;
        public Int32 Nallocationtype { get; set; }

        private string _Dallocationdate;
        public string Dallocationdate { get; set; }

        private String _Vempname;
        public String Vempname { get; set; }

        private String _Ncompany;
        public String Ncompany { get; set; }

        private String _Ndivision;
        public String Ndivision { get; set; }

        private String _Ndepartment;
        public String Ndepartment { get; set; }

        private String _Ndesignation;
        public String Ndesignation { get; set; }

        private Int32? _Nlocation;
        public Int32? Nlocation { get; set; }

        private string _Djoiningdate;
        public string Djoiningdate { get; set; }

        private string _Dinsertdate;
        public string Dinsertdate { get; set; }

        private String _Nunit;
        public String Nunit { get; set; }

        private DateTime _Dcanceldate;
        public DateTime Dcanceldate { get; set; }

        private string _RequestFor;
        public string RequestFor { get; set; }

        private string _NREQTYPENAME;
        public string NREQTYPENAME { get; set; }
        
        private string _ItemName;
        public string ItemName { get; set; }

        private string _HODName;
        public string HODName { get; set; }

        private string _FCName;
        public string FCName { get; set; }

        private string _Action;
        public string Action { get; set; }

        public Int32 reqStatus { get; set; }
        public string reqMessage { get; set; }

        public string Level { get; set; }
        public string Statusname { get; set; }

        private string _ApprovalDate;
        public string ApprovalDate { get; set; }

        private string _Locationname;
        public string Locationname { get; set; }

        private string _Allocation;
        public string Allocation { get; set; }

        private string _CMPNAME;
        public string CMPNAME { get; set; }

        private string _DIVNAME;
        public string DIVNAME { get; set; }

        private string _UNITNAME;
        public string UNITNAME { get; set; }

        private string _DEPTNAME;
        public string DEPTNAME { get; set; }

        private string _DESIGNATION;
        public string DESIGNATION { get; set; }

        private string _SerialNo;
        public string SerialNo { get; set; }

        private string _AllocationDate;
        public string AllocationDate { get; set; }

        private string _Engname;
        public string Engname { get; set; }

        private string _Assetno;
        public string Assetno { get; set; }

        private string _Hostname;
        public string Hostname { get; set; }

        private char _chkbox;
        public char chkbox { get; set; }

        private Int32? _Nitemno1;
        public Int32? Nitemno1 { get; set; }

        private Int32? _Assetno1;
        public Int32? Assetno1 { get; set; }

        private string _ITEMTYPE;
        public string ITEMTYPE { get; set; }

        private string _BRAND;
        public string BRAND { get; set; }

        private string _VMODELNO;
        public string VMODELNO { get; set; }

        private string _UNIT;
        public string UNIT { get; set; }

        private string _ITEM_SIZE;
        public string ITEM_SIZE { get; set; }

        private string _SPECS;
        public string SPECS { get; set; }

        private string _HODCOUNT;
        public string HODCOUNT { get; set; }

        private Int32? _Compliance;
        public Int32? Compliance { get; set; }

        private string _Compliancename;
        public string Compliancename { get; set; }

        private Int32? _Domain;
        public Int32? Domain { get; set; }

        private string _Domainname;
        public string Domainname { get; set; }

        private Int32? _OS;
        public Int32? OS { get; set; }

        private string _OSname;
        public string OSname { get; set; }

        private char _chkbox1;
        public char chkbox1 { get; set; }

        private Int32? _Nitemno2;
        public Int32? Nitemno2 { get; set; }

        private Int32? _Assetno2;
        public Int32? Assetno2 { get; set; }

        private string _Remarks;
        public string Remarks { get; set; }

        private Int32? _Classification;
        public Int32? Classification { get; set; }

        private string _Classificationname;
        public string Classificationname { get; set; }

        private string _Vendorname;
        public string Vendorname { get; set; }

        private string _InvoiceNo;
        public string InvoiceNo { get; set; }

        private string _DocumentDate;
        public string DocumentDate { get; set; }
    }
}