namespace MtbMate.Utilities
{
    public delegate void SpeedChangedEventHandler(SpeedChangedEventArgs e);

    public class SpeedChangedEventArgs
    {
        public double MetresPerSecond { get; set; }
    }
}
