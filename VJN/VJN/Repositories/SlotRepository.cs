using Microsoft.EntityFrameworkCore;
using VJN.Models;

namespace VJN.Repositories
{
    public class SlotRepository : ISlotRepository
    {

        public readonly VJNDBContext _context;

        public SlotRepository(VJNDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobSchedule>> GetJobScheduleBySlotID(int slotId)
        {
            var jobSchedule = await _context.JobSchedules.Where(js=>js.SlotId==slotId).ToListAsync();
            return jobSchedule;
        }

        public async Task<IEnumerable<WorkingHour>> GetWorkingHoursByJobSchedule(int JobScheduleId)
        {
            var wh = await _context.WorkingHours.Where(wh=>wh.ScheduleId==JobScheduleId).ToListAsync();
            return wh;
        }

        public async Task<IEnumerable< Slot>> GetSlotByPostjobId(int postjobId)
        {
            var slot = await _context.Slots.Where(slot=>slot.PostId == postjobId).ToListAsync();
            return slot;
        } 
    }
}
