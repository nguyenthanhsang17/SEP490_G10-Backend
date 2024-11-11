using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VJN.ModelsDTO.SlotDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlotsWithSchedules([FromBody] IEnumerable<SlotCreateDTO> slotDTOs)
        {
            var createdSlotIds = await _slotService.CreateSlotsWithSchedules(slotDTOs);
            return Ok(createdSlotIds);
        }

        [HttpDelete("DeleteAllSlot/{postid}")]
        public async Task<ActionResult<bool>> DeleteAllSlot(int postid)
        {
            var c  = await _slotService.DeleteAllSlot(postid);
            return Ok(c);
        }

        [HttpPut("UpdateSlot/{postid}")]
        public async Task<ActionResult<IEnumerable<int>>> UpdateSlot(int postid, [FromBody] IEnumerable<SlotCreateDTO> slotDTOs)
        {
            var c = await _slotService.UpadateSlot(slotDTOs, postid);
            return Ok(c);
        }

    }
}
