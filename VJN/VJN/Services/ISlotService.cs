using VJN.ModelsDTO.SlotDTOs;

namespace VJN.Services
{
    public interface ISlotService
    {
        public Task<IEnumerable<SlotDTO>> GetSlotByPostjobId(int id);

        Task<IEnumerable<int>> CreateSlotsWithSchedules(IEnumerable<SlotCreateDTO> slotDTOs);

        Task<IEnumerable<int>> UpadateSlot(IEnumerable<SlotCreateDTO> slotDTOs, int postid);
        public Task<bool> DeleteAllSlot(int postid);
    }
}
