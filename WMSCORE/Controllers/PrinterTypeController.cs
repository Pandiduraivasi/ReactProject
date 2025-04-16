using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using WMSCORE.Common;
using WMSCORE.Services;
using static WMSCORE.Services.PrinterTypeService;

namespace WMSCORE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PrinterTypeController : ControllerBase
	{
		private readonly IPrinterTypeService _PrinterTypeService;
		Utility _utiObj = new Utility();
		public PrinterTypeController(IPrinterTypeService PrinterTypeService)
		{
			_PrinterTypeService = PrinterTypeService;
		}
		[HttpGet("GetPrinterType")]
		public async Task<IActionResult> GetPrinterType([FromQuery] string PRT_CODE, [FromQuery] string MODE)
		{
			var parameters = new object[2] { PRT_CODE, MODE };
			//var printerTypes = new List<Printer>
			//{
			//	new Printer { PrtCode = "F", PrtDesc = "asasa", PrtLabelCount = 1, PrtStatus = "Enable", CreatedDtm = "13/10/2021 20:33", UpdatedDtm = "13/10/2021 20:33" },
			//	new Printer { PrtCode = "L", PrtDesc = "Lazers", PrtLabelCount = 1, PrtStatus = "Enable", CreatedDtm = "21/07/2021 16:04", UpdatedDtm = "21/07/2021 16:04" },
			//	new Printer { PrtCode = "Z", PrtDesc = "Zebra", PrtLabelCount = 1, PrtStatus = "Enable", CreatedDtm = "14/09/2021 22:52", UpdatedDtm = "04/10/2021 21:00" },
			//	new Printer { PrtCode = "D", PrtDesc = "DOMINO – G Series", PrtLabelCount = 40, PrtStatus = "Enable", CreatedDtm = "03/05/2021 11:56", UpdatedDtm = "03/05/2021 12:16" },
			//	new Printer { PrtCode = "A", PrtDesc = "DOMINO – Print and Apply", PrtLabelCount = 1, PrtStatus = "Enable", CreatedDtm = "22/09/2023 15:52", UpdatedDtm = "22/09/2023 15:52" },
			//	new Printer { PrtCode = "z", PrtDesc = "ZEBRA", PrtLabelCount = 1, PrtStatus = "Enable", CreatedDtm = "20/07/2023 22:44", UpdatedDtm = "20/07/2023 22:44" }
			//};
			//return Ok(printerTypes);
			var result = await _PrinterTypeService.GetPrinterType(parameters);

			if (result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
			{
				// Convert DataTable to a list of strongly-typed objects
				var printerTypes = _utiObj.ConvertDataTableToDictionary(result.Tables[0]);
				return Ok(printerTypes);
			}

			return NotFound("No printer types found");
		}
		
	}
	public class Printer
	{
		public string PrtCode { get; set; }
		public string PrtDesc { get; set; }
		public int PrtLabelCount { get; set; }
		public string PrtStatus { get; set; }
		public string CreatedDtm { get; set; }
		public string UpdatedDtm { get; set; }
	}
}
