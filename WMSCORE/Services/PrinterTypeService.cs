using System.Data;
using WMSCORE.Repositories;
using static WMSCORE.Repositories.LoginRepository;
using static WMSCORE.Repositories.PrinterTypeRepository;
using static WMSCORE.Services.PrinterTypeService;

namespace WMSCORE.Services
{
	public class PrinterTypeService : IPrinterTypeService
	{
		public interface IPrinterTypeService
		{
			Task<DataSet> GetPrinterType(object[] _objParameters);
			Task<string> SavePrinterType(object[] _objParameters);
		}
		private readonly IPrinterTypeRepository _PrinterTyperepository;

		public PrinterTypeService(IPrinterTypeRepository PrinterTyperepository)
		{
			_PrinterTyperepository = PrinterTyperepository;
		}
		public async Task<DataSet> GetPrinterType(object[] _objParameters)
		{
			return await _PrinterTyperepository.GetPrinterType(_objParameters);
		}
		public async Task<string> SavePrinterType(object[] _objParameters)
		{
			return await _PrinterTyperepository.SavePrinterType(_objParameters);
		}
	}
}
