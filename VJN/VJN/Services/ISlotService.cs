using VJN.ModelsDTO.SlotDTOs;

namespace VJN.Services
{
    public interface ISlotService
    {
        public Task<IEnumerable<SlotDTO>> GetSlotByPostjobId(int id);
    }
}
