using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System;
using Services.Data;
using OfficeOpenXml;
using System.Security.Claims;

namespace TimeClockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("DownloadClockDataExcel")]
        public IActionResult DownloadClockDataExcel()
        {
            ExcelPackage exp;
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                exp = new ExcelPackage();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userName = identity?.FindFirst("user")?.Value;
            if (userName == null)
                return Unauthorized();

            Response.Headers.Accept = "application/vnd.ms-excel";
            return File(_dataService.GetClockDataAsExcel(exp, userName), "application/vnd.ms-excel", "ApplicationExport.xlsx");
        }
    }
}
