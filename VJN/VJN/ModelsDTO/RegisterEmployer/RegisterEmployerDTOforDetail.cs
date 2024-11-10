using VJN.Models;

namespace VJN.ModelsDTO.RegisterEmployer
{
    public class RegisterEmployerDTOforDetail
    {
        public int RegisterEmployerId { get; set; }
        public int? UserId { get; set; }
        public string? BussinessName { get; set; }
        public string? BussinessAddress { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public  UserDTOs.UserDTOinRegisterEmployer? User { get; set; }
        public  List<String> ListIMG { get; set; }
    }
}
