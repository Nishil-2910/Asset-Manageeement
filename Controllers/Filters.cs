using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Text;

namespace MobileReimbursement.BusinessClass
{
    public class Filters
    {
        // ReSharper disable InconsistentNaming
        public enum GroupOp
        {
            AND,
            OR
        }

        public enum Operations
        {
            eq, // "equal"
            ne, // "not equal"
            lt, // "less"
            le, // "less or equal"
            gt, // "greater"
            ge, // "greater or equal"
            bw, // "begins with"
            bn, // "does not begin with"

            //in, // "in"
            //ni, // "not in"
            ew, // "ends with"

            en, // "does not end with"
            cn, // "contains"
            nc  // "does not contain"
        }

        public class Rule
        {
            public string field { get; set; }
            public Operations op { get; set; }
            public string data { get; set; }
        }

        public GroupOp groupOp { get; set; }
        public List<Rule> rules { get; set; }

        // ReSharper restore InconsistentNaming
        private static readonly string[] FormatMapping = {
                " {0} = {1} ",                 // "eq" - equal
                " {0} <> {1} ",                // "ne" - not equal
                " {0} < {1} ",                 // "lt" - less than
                " {0} <= {1} ",                // "le" - less than or equal to
                " {0} > {1} ",                 // "gt" - greater than
                " {0} >= {1} ",                // "ge" - greater than or equal to
                " {0} LIKE '{1}*' ",        // "bw" - begins with
                " {0} NOT LIKE '{1}*' ",    // "bn" - does not begin with
                " {0} LIKE '*{1}' ",        // "ew" - ends with
                " {0} NOT LIKE '*{1}' ",    // "en" - does not end with
                " {0} LIKE '*{1}*' ",    // "cn" - contains
                " {0} NOT LIKE '*{1}*' " //" nc" - does not contain
            };

        internal DataView SearchDataTable(System.Data.DataTable dtTable, Filters f, string sidx, string sord)
        {
            DataView dv = new DataView(dtTable);
            string expression = string.Empty;
            string sortOrder = string.Empty;
            var sb = new StringBuilder();
            var objParams = new List<ObjectParameter>(f.rules.Count);
            int counter = f.rules.Count;
            for (int i = 0; i < f.rules.Count; i++)
            {
                if (!string.IsNullOrEmpty(f.rules[i].data))
                {
                    var iParam = objParams.Count;
                    sb.AppendFormat(FormatMapping[(int)f.rules[i].op], f.rules[i].field.ToUpper(), f.rules[i].data.ToUpper());
                    if (counter > 1)
                    {
                        sb.Append(f.groupOp.ToString());
                    }
                }
                counter--;
            }

            expression = sb.ToString();
            dv.RowFilter = expression;
            if (!string.IsNullOrEmpty(sidx) && !string.IsNullOrEmpty(sord))
            {
                sortOrder = sidx.ToUpper().ToString() + " " + sord.ToString();
                dv.Sort = sortOrder;
            }

            return dv;
        }
    }
}