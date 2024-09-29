using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Clock
{
    public interface IClockService
    {
        public Task StartStopClock(string username);
    }
}
