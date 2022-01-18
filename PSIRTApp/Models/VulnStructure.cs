using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSIRTApp.Models
{

    public class IndexModel
    {
        public string advisoryID { get; set; }

        public Vuln List { get; set; }
        public List<string> DistinctList { get; set; }
    }

    public class Vuln
    {
        public List<VulnStructure> advisories { get; set; }
    }
    public class VulnStructure
    {
        public string advisoryId { get; set; }
        public string advisoryTitle { get; set; }
        public List<string> bugIDs { get; set; }
        public List<object> ipsSignatures { get; set; }
        public string cvrfUrl { get; set; }
        public string cvssBaseScore { get; set; }
        public List<string> cwe { get; set; }
        public string firstPublished { get; set; }
        public string lastUpdated { get; set; }
        public string status { get; set; }
        public string version { get; set; }
        public List<string> productNames { get; set; }
        public string publicationUrl { get; set; }
        public string sir { get; set; }
        public string summary { get; set; }

        
        ////{\"advisories\":[{\"advisoryId\":\"cisco-sa-ade-xcvAQEOZ\",
        //\"advisoryTitle\":\"Cisco ADE-OS Local File Inclusion Vulnerability\",
        //\"bugIDs\":[\"CSCvv57166\",\"CSCvw47125\",\"CSCvw48396\"],
        //\"ipsSignatures\":[\"NA\"],
        // \"cves\":[\"CVE-2021-1306\"],
        //\"cvrfUrl\":\"https://tools.cisco.com/security/center/contentxml/CiscoSecurityAdvisory/cisco-sa-ade-xcvAQEOZ/cvrf/cisco-sa-ade-xcvAQEOZ_cvrf.xml\",
        //\"cvssBaseScore\":\"4.4\",
        // \"cwe\":[\"CWE-73\"]
        //,\"firstPublished\":\"2021-05-19T16:00:00\"
        //,\"lastUpdated\":\"2021-05-19T16:00:00\"
        //,\"status\":\"Final\"
        //,\"version\":\"1.0\"
        //,\"productNames\":[\"Cisco Identity Services Engine Software \",\"Cisco Prime Infrastructure \",\"Cisco Evolved Programmable Network Manager (EPNM) \"]
        //,\"publicationUrl\":\"https://tools.cisco.com/security/center/content/CiscoSecurityAdvisory/cisco-sa-ade-xcvAQEOZ\"
        //,\"sir\":\"Medium\"
        //,\"summary\":\"\\r\\n<p>A vulnerability in the restricted shell of Cisco&nbsp;Evolved Programmable Network (EPN) Manager, Cisco&nbsp;Identity Services Engine (ISE), and Cisco&nbsp;Prime Infrastructure could allow an authenticated, local attacker to identify directories and write arbitrary files to the file system.</p>\\r\\n<p>This vulnerability is due to improper validation of parameters that are sent to a CLI command within the restricted shell. An attacker could exploit this vulnerability by logging in to the device and issuing certain CLI commands. A successful exploit could allow the attacker to identify file directories on the affected device and write arbitrary files to the file system on the affected device. To exploit this vulnerability, the attacker must be an authenticated shell user.</p>\\r\\n<p>Cisco&nbsp;has released software updates that address this vulnerability. There are no workarounds that address this vulnerability.</p>\\r\\n<p>This advisory is available at the following link:<br><a href=\\\"https://tools.cisco.com/security/center/content/CiscoSecurityAdvisory/cisco-sa-ade-xcvAQEOZ\\\" target=\\\"_blank\\\">https://tools.cisco.com/security/center/content/CiscoSecurityAdvisory/cisco-sa-ade-xcvAQEOZ</a></p>\\r\\n\"}]}


        //        {
        //    "advisory_id": "cisco-sa-20180221-ucdm",
        //    "advisory_title": "Cisco Unified Communications Domain Manager Remote Code Execution Vulnerability",
        //    "bug_ids": [
        //        "CSCuv67964",
        //        "CSCvi10692"
        //    ],
        //    "cves": [
        //        "CVE-2018-0124"
        //    ],
        //    "cvrf_url": "https://tools.cisco.com/security/center/contentxml/CiscoSecurityAdvisory/cisco-sa-20180221-ucdm/cvrf/cisco-sa-20180221-ucdm_cvrf.xml",
        //    "cvss_base_score": "9.8",
        //    "cwe": [
        //        "CWE-320"
        //    ],
        //    "first_published": "2018-02-21T16:00:00-0600",
        //    "ips_signatures": [
        //        "NA"
        //    ],
        //    "last_updated": "2018-03-09T14:47:48-0600",
        //    "product_names": [
        //        "Cisco Unified Communications Domain Manager "
        //    ],
        //    "publication_url": "https://tools.cisco.com/security/center/content/CiscoSecurityAdvisory/cisco-sa-20180221-ucdm",
        //    "sir": "Critical",
        //    "summary": "A vulnerability in Cisco Unified Communications Domain Manager could allow an unauthenticated, remote attacker to bypass security protections, gain elevated privileges, and execute arbitrary code.<br />\n<br />\nThe vulnerability is due to insecure key generation during application configuration. An attacker could exploit this vulnerability by using a known insecure key value to bypass security protections by sending arbitrary requests using the insecure key to a targeted application. An exploit could allow the attacker to execute arbitrary code.<br />\n<br />\nCisco has released software updates that address this vulnerability. There are no workarounds that address this vulnerability. <br />\n<br />\nThis advisory is available at the following link:<br />\n<a href=\"https://tools.cisco.com/security/center/content/CiscoSecurityAdvisory/cisco-sa-20180221-ucdm\">https://tools.cisco.com/security/center/content/CiscoSecurityAdvisory/cisco-sa-20180221-ucdm</a>"
        //}

    }
}
