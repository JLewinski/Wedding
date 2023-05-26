using Wedding.Data;

namespace Wedding.Models
{
    public class ThankYouViewModel
    {
        public ThankYouViewModel() { }
        public ThankYouViewModel(ChangeViewModel vm)
        {
            Name = vm.Name;
            GuestId = vm.UserId;
            Email = vm.Email;
            IsComing = vm.NumberChildren + vm.NumberAdults > 0;
        }
        public ThankYouViewModel(Guest guest)
        {
            Name = guest.GuestName;
            GuestId = guest.UserId;
            Email = guest.Email;
            IsComing = guest.NumberChildren + guest.NumberAdults > 0;
        }
        public string Name { get; set; } = null!;
        public Guid GuestId { get; set; }
        public string Email { get; set; } = null!;
        public string? Url { get; set; } = null!;
        public bool IsComing { get; set; } = false;
    }
}