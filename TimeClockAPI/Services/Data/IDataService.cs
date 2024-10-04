using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Data
{
    public interface IDataService
    {
        public byte[] GetClockDataAsExcel(ExcelPackage exp, string username);
    }
}
