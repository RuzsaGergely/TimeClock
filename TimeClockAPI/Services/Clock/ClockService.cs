using Domain;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Clock
{
    public class ClockService : IClockService
    {
        private readonly TCDbContext _context;
        public ClockService(TCDbContext context)
        {
            _context = context;
        }
        public async Task StartStopClock(string username)
        {
            List<ClockEntry> entries = await _context.ClockEntries.ToListAsync();
            var user = await _context.Users.Include(x=>x.ClockEntries).FirstOrDefaultAsync(x=> x.Username.ToLower() == username.ToLower());
            
            if(!entries.Any(x=>x.ClockOut != null && x.UserId == user.Id))
            {
                await _context.ClockEntries.AddAsync(new ClockEntry()
                {
                    ClockIn = DateTime.Now,
                    Deleted = false,
                    Description = "",
                    EntryName = "",
                    EntryGUID = Guid.NewGuid().ToString(),
                    UserId = user.Id
                });
            } else
            {
                var entry = entries.Where(x => x.ClockOut != null && x.UserId == user.Id).FirstOrDefault();
                entry.ClockOut = DateTime.Now;
                 _context.ClockEntries.Update(entry);
            }
            await _context.SaveChangesAsync();

        }
    }
}
