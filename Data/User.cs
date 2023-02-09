using Microsoft.EntityFrameworkCore;

namespace Wedding.Data
{
        [PrimaryKey("UserId")]
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public bool IsAdmin { get; set; }
    }
}