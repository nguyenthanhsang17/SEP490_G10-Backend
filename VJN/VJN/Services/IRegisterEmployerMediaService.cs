namespace VJN.Services
{
    public interface IRegisterEmployerMediaService
    {
        public Task<bool> CreateRegisterEmployerMedia(int registerID, List<int> imageid);
    }
}
