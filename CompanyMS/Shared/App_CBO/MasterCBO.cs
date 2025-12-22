using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace CompanyMS.Shared.App_CBO
{
    public class MasterCBO
    {
        public MasterCBO() { }
        public Guid branch_kid { get; set; }
        public  string branch_id { get; set; }
        public string branch_name { get; set; }

        public string branch_department { get; set; }

        public string branch_head { get; set; }

        public string branchdep_id { get; set; }
        public int mode { get; set; }
    }

    [Serializable]
    public class Employeeinfo
    {
        public Employeeinfo() { }
        public Guid emp_id { get; set; }

        public string emp_name { get; set; }

        public string emp_salary { get; set; }

        public string emp_emailid { get; set; }

        public string emp_gender { get; set; }

        public string emp_branch { get; set; }

        public string branch_id { get; set; }

        public string branch_kid { get; set; }

        public int mode { get; set; }

    }
}