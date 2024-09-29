using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ClockEntry : BaseEntity
    {
        public string EntryGUID { get; set; }
        public DateTime ClockIn {  get; set; }
        public DateTime? ClockOut { get; set; }
        public string? EntryName { get; set; }
        public string? Description { get; set; }
        // Shadow property
        public int UserId { get; set; }
    }
}
