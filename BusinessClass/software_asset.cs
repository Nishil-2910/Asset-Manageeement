using System;

namespace MobileReimbursement.BusinessClass
{
    public class software_asset
    {
        private Int32? _NSRNO;
        public Int32? NSRNO { get; set; }

        private Int32? _LICENSEPRODUCTCODE;
        public Int32? LICENSEPRODUCTCODE { get; set; }
                
        private String _LICENSEPRODUCT;
        public String LICENSEPRODUCT { get; set; }

        private String _LICENSEVERSION;
        public String LICENSEVERSION { get; set; }

        private Int32? _QUANTITY;
        public Int32? QUANTITY { get; set; }

        private String _EXPIRYDATE;
        public String EXPIRYDATE { get; set; }

        private String _REMARKS;
        public String REMARKS { get; set; }

        private Int32? _LOCATIONCODE;
        public Int32? LOCATIONCODE { get; set; }
        
        private String _LOCATION;
        public String LOCATION { get; set; }

        private string _Action;
        public string Action { get; set; }

        public Int32 reqStatus { get; set; }
        public string reqMessage { get; set; }
    }
}