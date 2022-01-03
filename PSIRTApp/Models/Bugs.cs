using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSIRTApp.Models
{
    public class Bugs
    {
        public List<Bug> bugs { get; set; }
    }

    public class BugModelList
    {
        public string model { get; set; }
        public List<Bug> bugs { get; set; }

    }

    public class Bug
    {
        public string id { get; set; }
        public string bug_id { get; set; }
        public string headline { get; set; }
        public string description { get; set; }
        public int severity { get; set; }
        public string status { get; set; }
        public string behavior_changed { get; set; }
        public string duplicate_of { get; set; }
        public string created_date { get; set; }
        public string last_modified_date { get; set; }
        public string product { get; set; }
        public string known_affected_releases { get; set; }
        public string known_fixed_releases { get; set; }
        public string support_case_count { get; set; }
        //   "id": "1",
        //"bug_id": "CSCdr21997",
        //"headline": "BPS stops sending SNMP traps after a while",
        //"description": "When configuring an access list... before.",
        //"severity": "2",
        //"status": "F",
        //"behavior_changed": "",
        //"duplicate_of": "",
        //"created_date": "",
        //"last_modified_date": "2002-02-17",
        //"product": "Cisco IOS",
        //"known_affected_releases": "12.1(1)",
        //"known_fixed_releases": "12.1(4)DC 12.1(4)DB 12.1(3.3)T 12.1(3.1) 12.1(3.1)AA",
        //"support_case_count": "0"
    }
}
