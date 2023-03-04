using Microsoft.EntityFrameworkCore;

namespace Wedding.Data
{
        [PrimaryKey("UserId")]
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public bool IsAdmin { get; set; }
    }
}