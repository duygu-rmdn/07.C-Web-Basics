namespace SharedTrip.Models
{
    using System;

    public class UserTrip
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();
        public User User { get; set; }
        public string TripId { get; set; } = Guid.NewGuid().ToString();
        public Trip Trip { get; set; }
    }
}
