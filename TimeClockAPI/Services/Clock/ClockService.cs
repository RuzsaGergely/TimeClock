using Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.Clock
{
    public class ClockService : IClockService
    {
        private readonly TCDbContext _context;
        public ClockService(TCDbContext context)
        {
            _context = context;
        }
        public void StartStopClock(string username, ClockStartStopDto data)
        {
            var entries = _context.ClockEntries.ToList();
            var user =  _context.Users.Include(x=>x.ClockEntries).FirstOrDefault(x=> x.Username.ToLower() == username.ToLower());
            
            if(!entries.Any(x=>x.ClockOut == null && x.UserId == user.Id))
            {
                _context.ClockEntries.Add(new ClockEntry()
                {
                    ClockIn = DateTime.Now,
                    Deleted = false,
                    Description = data.Description != null ? data.Description : "",
                    EntryName = data.EntryName != null ? data.EntryName : "",
                    EntryGUID = Guid.NewGuid().ToString(),
                    UserId = user.Id
                });
            } else
            {
                var entry = entries.Where(x => x.ClockOut == null && x.UserId == user.Id).FirstOrDefault();
                entry.ClockOut = DateTime.Now;
                 _context.ClockEntries.Update(entry);
            }
             _context.SaveChanges();

        }

        public ClockStatusDto GetClockStatus(string username)
        {
            var entries = _context.ClockEntries.ToList();
            var user = _context.Users.Include(x => x.ClockEntries).FirstOrDefault(x => x.Username.ToLower() == username.ToLower());

            if (!entries.Any(x => x.ClockOut == null && x.UserId == user.Id))
            {
                return new ClockStatusDto()
                {
                    Ticking = false
                };
            }
            else
            {
                var entry = entries.Where(x => x.ClockOut == null && x.UserId == user.Id).FirstOrDefault();
                return new ClockStatusDto()
                {
                    Ticking = true,
                    Description = entry.Description,
                    EntryGUID = entry.EntryGUID,
                    EntryName = entry.EntryName,
                    StartDate = entry.ClockIn,
                    Duration = (DateTime.Now - entry.ClockIn).TotalSeconds
                };
            }
        }
    }
}
