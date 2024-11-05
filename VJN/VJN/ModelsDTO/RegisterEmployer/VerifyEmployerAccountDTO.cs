namespace VJN.ModelsDTO.RegisterEmployer
{
    public class VerifyEmployerAccountDTO
    {
        public string? BussinessName { get; set; }
        public string? BussinessAddress { get; set; }
        public List<IFormFile>? files { get; set; }
    }
}
