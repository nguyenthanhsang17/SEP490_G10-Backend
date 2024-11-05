namespace VJN.Repositories
{
    public interface IRegisterEmployerMediaRepository
    {
        public Task<bool> CreateRegisterEmployerMedia(int registerID, List<int> imageid);
    }
}
