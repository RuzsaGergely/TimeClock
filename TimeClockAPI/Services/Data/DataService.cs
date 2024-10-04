using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Data
{
    public class DataService : IDataService
    {
        private readonly TCDbContext _context;
        public DataService(TCDbContext context)
        {
            _context = context;
        }

        public byte[] GetClockDataAsExcel(ExcelPackage exp, string username)
        {
            var user = _context.Users.Include(x => x.ClockEntries).FirstOrDefault(x => x.Username.ToLower() == username.ToLower());

            exp.Workbook.Worksheets.Add("ClockData");
            exp.Workbook.Worksheets[0].Cells["A1"].Value = "#";
            exp.Workbook.Worksheets[0].Cells["A1:A2"].Merge = true;

            exp.Workbook.Worksheets[0].Cells["B1"].Value = "Óra GUID";
            exp.Workbook.Worksheets[0].Cells["B1:B2"].Merge = true;

            exp.Workbook.Worksheets[0].Cells["C1"].Value = "Belépés dátuma";
            exp.Workbook.Worksheets[0].Cells["C1:D1"].Merge = true;
            exp.Workbook.Worksheets[0].Cells["C2"].Value = "Nap";
            exp.Workbook.Worksheets[0].Cells["D2"].Value = "Óra";

            exp.Workbook.Worksheets[0].Cells["E1"].Value = "Kilépés dátuma";
            exp.Workbook.Worksheets[0].Cells["E1:F1"].Merge = true;
            exp.Workbook.Worksheets[0].Cells["E2"].Value = "Nap";
            exp.Workbook.Worksheets[0].Cells["F2"].Value = "Óra";

            exp.Workbook.Worksheets[0].Cells["G1"].Value = "Időtartam";
            exp.Workbook.Worksheets[0].Cells["G1:G2"].Merge = true;

            exp.Workbook.Worksheets[0].Cells["H1"].Value = "Megnevezés";
            exp.Workbook.Worksheets[0].Cells["H1:H2"].Merge = true;

            exp.Workbook.Worksheets[0].Cells["I1"].Value = "Egyéb leírás";
            exp.Workbook.Worksheets[0].Cells["I1:I2"].Merge = true;

            exp.Workbook.Worksheets[0].Cells["A1:I2"].Style.Font.Bold = true;
            exp.Workbook.Worksheets[0].Cells["A1:I2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            exp.Workbook.Worksheets[0].Cells["A1:I2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

            int counter = 3;
            foreach (var item in user.ClockEntries.Where(x=>!x.Deleted))
            {
                exp.Workbook.Worksheets[0].Cells[$"A{counter}"].Value = item.Id;
                exp.Workbook.Worksheets[0].Cells[$"B{counter}"].Value = item.EntryGUID;

                exp.Workbook.Worksheets[0].Cells[$"C{counter}"].Value = item.ClockIn.ToShortDateString();
                exp.Workbook.Worksheets[0].Cells[$"D{counter}"].Value = item.ClockIn.ToLongTimeString();
                
                if(item.ClockOut != null)
                {
                    exp.Workbook.Worksheets[0].Cells[$"E{counter}"].Value = item.ClockOut.Value.ToShortDateString();
                    exp.Workbook.Worksheets[0].Cells[$"F{counter}"].Value = item.ClockOut.Value.ToLongTimeString();
                    TimeSpan diff = (DateTime)item.ClockOut - item.ClockIn;
                    exp.Workbook.Worksheets[0].Cells[$"G{counter}"].Value = diff.Duration().ToString();
                } 
                else
                {
                    exp.Workbook.Worksheets[0].Cells[$"E{counter}"].Value = "";
                    exp.Workbook.Worksheets[0].Cells[$"F{counter}"].Value = "";
                    exp.Workbook.Worksheets[0].Cells[$"G{counter}"].Value = "-";
                }

                exp.Workbook.Worksheets[0].Cells[$"H{counter}"].Value = item.EntryName;
                exp.Workbook.Worksheets[0].Cells[$"I{counter}"].Value = item.Description;
                counter++;
            }
            exp.Workbook.Worksheets[0].Cells["A:I"].AutoFitColumns();
            return exp.GetAsByteArray();
        }
    }
}
