﻿using VJN.Models;

namespace VJN.Repositories
{
    public interface ICvRepository
    {
        public Task<IEnumerable<Cv>> GetCvByUserID(int user);

        public Task<bool> UpdateCV(List<Cv> cvs, int userid);
    }
}
