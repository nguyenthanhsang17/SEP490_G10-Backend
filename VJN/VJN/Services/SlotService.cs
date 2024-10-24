﻿using AutoMapper;
using VJN.Models;
using VJN.ModelsDTO.SlotDTOs;
using VJN.Repositories;

namespace VJN.Services
{
    public class SlotService : ISlotService
    {
        public readonly ISlotRepository _slotRepository;
        private IMapper _mapper;

        public SlotService(ISlotRepository slotRepository, IMapper mapper)
        {
            _slotRepository = slotRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SlotDTO>> GetSlotByPostjobId(int id)
        {
            var slot = await _slotRepository.GetSlotByPostjobId(id);
            var slotDTO = _mapper.Map<IEnumerable<SlotDTO>>(slot);

            foreach(SlotDTO dTO in slotDTO)
            {
                var Js = await _slotRepository.GetJobScheduleBySlotID(dTO.SlotId);
                var jsdto = _mapper.Map<IEnumerable<JobScheduleDTO>>(Js);
                foreach(JobScheduleDTO dTO1 in jsdto)
                {
                    var wh = await _slotRepository.GetWorkingHoursByJobSchedule(dTO1.ScheduleId);
                    var whDTO = _mapper.Map<IEnumerable<WorkingHourDTO>>(wh);
                    dTO1.workingHourDTOs = whDTO;
                }
                dTO.jobScheduleDTOs = jsdto;
            }

            return slotDTO;
        }
    }
}
