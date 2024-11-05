using VJN.Models;

namespace VJN.Repositories
{
    public interface IRegisterEmployerRepository
    {
        public Task<int> RegisterEmployer(RegisterEmployer employer);
    }
}
