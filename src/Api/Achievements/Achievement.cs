using Shared.Dtos;

namespace Api.Achievements {
    public abstract class Achievement {
        public string Name { get; set; }

        public abstract bool Check(RideDto ride);
    }
}
