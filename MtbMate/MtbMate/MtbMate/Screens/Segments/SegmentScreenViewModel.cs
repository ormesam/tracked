using MtbMate.Contexts;
using MtbMate.Models;

namespace MtbMate.Screens.Segments
{
    public class SegmentScreenViewModel : ViewModelBase
    {
        public SegmentModel Segment { get; }

        public SegmentScreenViewModel(MainContext context, SegmentModel segment) : base(context)
        {
            Segment = segment;
        }

        public override string Title => Segment.Name;
    }
}
