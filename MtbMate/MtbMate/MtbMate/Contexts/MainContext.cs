using System.Collections.Generic;
using MtbMate.Models;

namespace MtbMate.Contexts
{
    public class MainContext
    {
        public StorageContext StorageContext { get; }
        public IList<RideModel> Rides { get; set; }

        public MainContext()
        {
            StorageContext = new StorageContext();
            Rides = StorageContext.LoadRides();
        }
    }
}
