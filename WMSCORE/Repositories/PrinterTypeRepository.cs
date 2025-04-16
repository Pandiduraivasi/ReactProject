using System.Data;
using WMSCORE.Common;
using static WMSCORE.Repositories.PrinterTypeRepository;

namespace WMSCORE.Repositories
{
	public class PrinterTypeRepository : IPrinterTypeRepository
	{
		public interface IPrinterTypeRepository
		{
			Task<DataSet> GetPrinterType(object[] _objParameters);
			Task<string> SavePrinterType(object[] _objParameters);
		}
		DataAccess _daObj = new DataAccess();
		private DataSet _ds = new DataSet();
		public async Task<DataSet> GetPrinterType(object[] _objParameters)
		{
			try
			{
				_ds = _daObj.GetPrinterType(_objParameters);
				return await Task.Run(() => _ds);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<string> SavePrinterType(object[] _objParameters)
		{
			try
			{
				string _Result = _daObj.SavePrinterType(_objParameters);
				return await Task.Run(() => _Result);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
