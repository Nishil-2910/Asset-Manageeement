using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileReimbursement.BusinessClass
{
    public class ResignedEmpAsset 
    {
        private String _Vempno;
        public String Vempno { get; set; }

        private String _Vempname;
        public String Vempname { get; set; }

        public string DEPTNAME { get; set; }

        private string _DESIGNATION;

        private string _Locationname;
        public string Locationname { get; set; }

        private string _VMODELNO;
        public string VMODELNO { get; set; }

        private string _VTYPEDESCRIPTION;
        public string VTYPEDESCRIPTION { get; set; }

        private string _RESIGNDT;
        public string RESIGNDT { get; set; }
    }
}
