using Wedding.Data;

namespace Wedding.Models
{
    public class ThankYouViewModel
    {
        public ThankYouViewModel() { }
        public ThankYouViewModel(ChangeViewModel vm)
        {
            Name = vm.GuestName;
            GuestId = vm.UserId;
            Email = vm.Email;
        }
        public ThankYouViewModel(Guest guest)
        {
            Name = guest.GuestName;
            GuestId = guest.UserId;
            Email = guest.Email;
        }
        public string Name { get; set; }
        public Guid GuestId { get; set; }
        public string Email { get; set; }
        public string? Url { get; set; }
    }
}