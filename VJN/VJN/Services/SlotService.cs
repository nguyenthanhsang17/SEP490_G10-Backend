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

        public async Task<IEnumerable<int>> CreateSlotsWithSchedules(IEnumerable<SlotCreateDTO> slotDTOs)
        {
            return await _slotRepository.CreateSlotsWithSchedules(slotDTOs);
        }

        public async Task<IEnumerable<SlotDTO>> GetSlotByPostjobId(int id)
        {
            var slot = await _slotRepository.GetSlotByPostjobId(id);
            if(slot == null||!slot.Any()||slot.Count()<=0) {
                return null;
            }
            var slotDTO = _mapper.Map<IEnumerable<SlotDTO>>(slot);

            foreach (SlotDTO dTO in slotDTO)
            {
                var Js = await _slotRepository.GetJobScheduleBySlotID(dTO.SlotId);
                var jsdto = _mapper.Map<IEnumerable<JobScheduleDTO>>(Js);
                foreach (JobScheduleDTO dTO1 in jsdto)
                {
                    var wh = await _slotRepository.GetWorkingHoursByJobSchedule(dTO1.ScheduleId);
                    var whDTO = _mapper.Map<IEnumerable<WorkingHourDTO>>(wh);
                    var whDTO2 = whDTO.OrderBy(wh => wh.StartTime);
                    dTO1.workingHourDTOs = whDTO2;
                }
                dTO.jobScheduleDTOs = jsdto;
            }

            return slotDTO;
        }

        public async Task<bool> DeleteAllSlot(int postid)
        {
            var c = await _slotRepository.DeleteAllSlotByPostId(postid);
            return c;
        }

        public async Task<IEnumerable<int>> UpadateSlot(IEnumerable<SlotCreateDTO> slotDTOs, int postid)
        {
            var c = await _slotRepository.DeleteAllSlotByPostId(postid);
            Console.WriteLine("c: " + c);

            var c1 = await _slotRepository.CreateSlotsWithSchedules(slotDTOs, postid);
            Console.WriteLine("c1: " + c1);
            return c1;

        }
    }
}
