using System;
using System.IO;

namespace MobileReimbursement.BusinessClass
{
    public class Logging
    {
        public void ErrorLog(String Path1, String sErrMsg)
        {
            String sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
            String sYear = DateTime.Now.Year.ToString();
            String sMonth = DateTime.Now.Month.ToString();
            String sDay = DateTime.Now.Day.ToString();
            String sErrorTime = sYear + sMonth + sDay;
            using (StreamWriter sw = new StreamWriter(Path1 + "\\" + sErrorTime, true))
            {
                sw.WriteLine(sLogFormat + sErrMsg);
                sw.Flush();
                sw.Close();
            }
        }
    }
}