using VJN.ModelsDTO.SlotDTOs;

namespace VJN.Services
{
    public interface ISlotService
    {
        public Task<IEnumerable<SlotDTO>> GetSlotByPostjobId(int id);

        Task<IEnumerable<int>> CreateSlotsWithSchedules(IEnumerable<SlotCreateDTO> slotDTOs);
    }
}
