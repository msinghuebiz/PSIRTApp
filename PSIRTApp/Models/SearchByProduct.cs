using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSIRTApp.Models
{
	public class EOXResultByProduct
	{
		public PaginationResult PaginationResponseRecord { get; set; }
		public List<SearchByProductModel> EOXRecord { get; set; }
	}

	public class PaginationResult
	{
		public int PageIndex { get; set; }
		public int LastIndex { get; set; }
		public int TotalRecords { get; set; }
		public int PageRecords { get; set; }
		//"PageIndex": 1,
		//"LastIndex": 1,
		//"TotalRecords": 1,
		//"PageRecords": 1
	}
	public class DateValue
	{
		public DateTime? value { get; set; }
		public string dateFormat { get; set; }
	}

	public class MigrationDetails
	{
		public string PIDActiveFlag { get; set; }
		public string MigrationInformation { get; set; }
		public string MigrationOption { get; set; }
		public string MigrationProductId { get; set; }
		public string MigrationProductName { get; set; }
		public string MigrationStrategy { get; set; }
		public string MigrationProductInfoURL { get; set; }
		//	"PIDActiveFlag": "Y ",
		//	"MigrationInformation": "WAN Interface Card",
		//	"MigrationOption": "Enter PID(s)",
		//	"MigrationProductId": "HWIC-1T=",
		//	"MigrationProductName": " ",
		//	"MigrationStrategy": " ",
		//	"MigrationProductInfoURL": "https://www.cisco.com"  
	}
	public class SearchByProductModel
	{
		public string EOLProductID { get; set; }
		public string ProductIDDescription { get; set; }
		public string ProductBulletinNumber { get; set; }
		public string LinkToProductBulletinURL { get; set; }
		public DateValue EOXExternalAnnouncementDate { get; set; }
		public DateValue EndOfSaleDate { get; set; }
		public DateValue EndOfSWMaintenanceReleases { get; set; }
		public DateValue EndOfSecurityVulSupportDate { get; set; }
		public DateValue EndOfRoutineFailureAnalysisDate { get; set; }
		public DateValue EndOfServiceContractRenewal { get; set; }
		public DateValue LastDateOfSupport { get; set; }
		public DateValue EndOfSvcAttachDate { get; set; }
		public DateValue UpdatedTimeStamp { get; set; }
		public MigrationDetails EOXMigrationDetails { get; set; }
		public string EOXInputType { get; set; }
		public string EOXInputValue { get; set; }
		//      "EOLProductID": "WIC-1T=",
		//"ProductIDDescription": "WAN Interface Card",
		//"ProductBulletinNumber": "EOL6640",
		//"LinkToProductBulletinURL": "https://www.cisco.com",  
		//"EOXExternalAnnouncementDate": {
		//	"value": "2008-12-28",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"EndOfSaleDate": {
		//	"value": "2009-12-28",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"EndOfSWMaintenanceReleases": {
		//	"value": "2010-12-28",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"EndOfSecurityVulSupportDate": {
		//	"value": "2012-12-15",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"EndOfRoutineFailureAnalysisDate": {
		//	"value": "2010-12-28",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"EndOfServiceContractRenewal": {
		//	"value": "2014-03-28",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"LastDateOfSupport": {
		//	"value": "2014-12-27",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"EndOfSvcAttachDate": {
		//	"value": "2010-12-28",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"UpdatedTimeStamp": {
		//	"value": "2009-08-25",
		//	"dateFormat": "YYYY-MM-DD"
		//},
		//"EOXMigrationDetails": {
		//	"PIDActiveFlag": "Y ",
		//	"MigrationInformation": "WAN Interface Card",
		//	"MigrationOption": "Enter PID(s)",
		//	"MigrationProductId": "HWIC-1T=",
		//	"MigrationProductName": " ",
		//	"MigrationStrategy": " ",
		//	"MigrationProductInfoURL": "https://www.cisco.com"  
		//},
		//"EOXInputType": "ShowEOXByPids",
		//"EOXInputValue": "WIC-1T= "
	}

	public class SearchByProduct
	{





		public const string URL = "https://api.cisco.com/supporttools/eox/4/EOXLookupService";

		public string GetEnvelope()
		{
			var result = "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\"" +
							   "xmlns:ns = \"http://www.cisco.com/services/ssapi/eox/1\">" +
							   "<soap:Header/>" +
							   "<soap:Body>" +
							   "<ns:ShowEOXByProductIDRequest>" +
							   "<ns:ProductIDs>{0}</ns:ProductIDs>" +
							   "<!--Optional:-->" +
							   "<ns:PaginationRequestRecord>" +
							   "<!--Optional:-->" +
							   "<ns:PageIndex>10</ns:PageIndex>" +
							   "</ns:PaginationRequestRecord>" +
							   "</ns:ShowEOXByProductIDRequest>" +
							   "</soap:Body>" +
							   "</soap:Envelope>";

			return result;
		}

	}
}
