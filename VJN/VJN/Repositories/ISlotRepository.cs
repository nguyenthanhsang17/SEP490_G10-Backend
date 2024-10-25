using VJN.Models;
using VJN.ModelsDTO.SlotDTOs;

namespace VJN.Repositories
{
    public interface ISlotRepository
    {
        public Task<IEnumerable<Slot>> GetSlotByPostjobId(int  postjobId);
        public Task<IEnumerable< JobSchedule>> GetJobScheduleBySlotID(int slotId);
        public Task<IEnumerable<WorkingHour>> GetWorkingHoursByJobSchedule(int JobScheduleId);
        public Task<IEnumerable<int>> CreateSlotsWithSchedules(IEnumerable<SlotCreateDTO> slotDTOs);

    }
}
