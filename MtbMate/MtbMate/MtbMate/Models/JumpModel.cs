using System;

namespace MtbMate.Models
{
    public class JumpModel
    {
        public DateTime LandingTimeStamp { get; set; }
        public double LandingGForce { get; set; }
        public DateTime TakeOffTimeStamp { get; set; }
        public double TakeOffGForce { get; set; }
        public double Airtime => (LandingTimeStamp - TakeOffTimeStamp).TotalSeconds;
    }
}
