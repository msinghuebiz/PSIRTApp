using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSIRTApp.Models
{
    public class CombinationModel
    {

        public string SearchType { get; set; }
        public string SearchText { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Vuln VulnList { get; set; }

        public List<Dictionary<string, object>> OrionList { get; set; }
        public List<string> OrionDistinctList { get; set; }
    }

    public class EOLCombinationModel
    {

        public string SearchType { get; set; }
        public string SearchText { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public EOXResultByProduct EOLList { get; set; }

        public List<Dictionary<string, object>> OrionList { get; set; }
        public Dictionary<string, int> VulCountList { get; set; }
        public Dictionary<string,List<Bug>> BugList { get; set; }
        public Dictionary<string, List<VulnStructure>> VuldistList { get; set; }
        public List<string> ImpactDistinctList { get; set; }
        public List<string> OrionDistinctList { get; set; }
    }
}
