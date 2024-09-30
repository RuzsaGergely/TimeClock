using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ClockStatusDto
    {
        public string? EntryGUID { get; set; }
        public DateTime? StartDate {  get; set; }
        public double? Duration { get; set; }
        public string? EntryName { get; set; }
        public string? Description { get; set; }
        public bool Ticking { get; set; }

    }
}
