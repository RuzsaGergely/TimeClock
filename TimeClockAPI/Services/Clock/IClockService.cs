using Domain;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Clock
{
    public interface IClockService
    {
        public void StartStopClock(string username, ClockStartStopDto data);
        public ClockStatusDto GetClockStatus(string username);
    }
}
