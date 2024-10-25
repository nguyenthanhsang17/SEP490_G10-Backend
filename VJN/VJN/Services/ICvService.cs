﻿using VJN.Models;
using VJN.ModelsDTO.CvDTOs;

namespace VJN.Services
{
    public interface ICvService
    {
        public Task<IEnumerable<CvDTODetail>> GetCvByUserID(int user);
    }
}
