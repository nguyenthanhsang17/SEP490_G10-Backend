namespace VJN.ModelsDTO.UserDTOs
{
    public class UserChangePasswordDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set;}
    }
}
