﻿using Microsoft.EntityFrameworkCore;

namespace Wedding.Data
{
    [PrimaryKey(nameof(UserId))]
    public class Guest
    {
        public Guid UserId { get; set; }
        public byte NumberAdults { get; set; }
        public byte NumberChildren { get; set; }
        public bool? IsGoing { get; set; }
        public string GuestName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}