using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CompanyMS.Shared.Common
{
    public class DBconnections
    {
        public DBconnections() 
        {
            CMDB = ConfigurationManager.ConnectionStrings["CMDB"].ConnectionString;
        }
        public string CMDB { get; set; }
    }
}