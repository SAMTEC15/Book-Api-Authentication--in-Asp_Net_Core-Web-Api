namespace MyBook.Domain.Dto
{
    public class AuthResultDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireAt{ get; set; }
    }
}
