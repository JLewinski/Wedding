using Microsoft.EntityFrameworkCore;

namespace Wedding.Data
{
    public class Note
    {
        public int Id { get; set; }
        public string NoteText { get; set; }
        public Guid GuestId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}