namespace VJN.ModelsDTO.EmailDTOs
{
    public class EmailForgotPassword
    {
        public string ToEmail { get; set; }
        public string Opt { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword {  get; set; }
    }
}
