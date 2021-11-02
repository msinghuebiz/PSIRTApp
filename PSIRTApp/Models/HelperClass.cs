using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSIRTApp.Models
{
    public class HelperClass
    {

        public bool  isAllowed( Dictionary<string,object> currentRow , string columnName, string ColumnValue , string column2Name, string Column2Value)
        {
            var isAllowed = false;
            var isAllowedColumn1 = false;
            var isAllowedColumn2 = false;

            foreach (var item in currentRow.Keys)
            {
                if (item.ToLower() == columnName.ToLower() )
                {
                    var value1 = currentRow[item].ToString();
                    if (ColumnValue == value1)
                    {
                        isAllowedColumn1 = true;
                    }
                }

                if (item.ToLower() == column2Name.ToLower())
                {
                    var value1 = currentRow[item].ToString();
                    if (Column2Value == value1)
                    {
                        isAllowedColumn2 = true;
                    }
                }

            }


            return (isAllowedColumn1 & isAllowedColumn2);

        }
    }
}
