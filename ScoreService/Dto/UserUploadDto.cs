namespace ScoreService.Dto
{
    public class UserUploadDto
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Zone { get; set; }

        public bool IsDeleted { get; set; }
       
    }
}