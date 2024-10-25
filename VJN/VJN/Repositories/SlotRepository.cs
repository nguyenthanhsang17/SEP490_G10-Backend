using Microsoft.EntityFrameworkCore;
using VJN.Models;
using VJN.ModelsDTO.SlotDTOs;

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

        public async Task<IEnumerable<int>> CreateSlotsWithSchedules(IEnumerable<SlotCreateDTO> slotDTOs)
        {
            var createdSlotIds = new List<int>();

            foreach (var slotDTO in slotDTOs)
            {
                var slot = new Slot
                {
                    PostId = slotDTO.PostId.Value,
                    UserId = null,
                };

                _context.Slots.Add(slot);
                await _context.SaveChangesAsync();

                if (slotDTO.jobScheduleCreateDTO != null)
                {
                    foreach (var scheduleDTO in slotDTO.jobScheduleCreateDTO)
                    {
                        var jobSchedule = new JobSchedule
                        {
                            SlotId = slot.SlotId,
                            DayOfWeek = scheduleDTO.DayOfWeek.Value
                        };

                        _context.JobSchedules.Add(jobSchedule);
                        await _context.SaveChangesAsync();

                        if (scheduleDTO.workingHourCreateDTOs != null)
                        {
                            foreach (var workingHourDTO in scheduleDTO.workingHourCreateDTOs)
                            {
                                var workingHour = new WorkingHour
                                {
                                    ScheduleId = jobSchedule.ScheduleId,
                                    StartTime = workingHourDTO.StartTime.Value,
                                    EndTime = workingHourDTO.EndTime.Value
                                };

                                _context.WorkingHours.Add(workingHour);
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                createdSlotIds.Add(slot.SlotId);
            }

            return createdSlotIds;
        }
    }
}
