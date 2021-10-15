using Shared.Dtos;
using Xamarin.Forms;

namespace Tracked.Selectors {
    public class FeedDataTemplateSelector : DataTemplateSelector {
        public DataTemplate RideTemplate { get; set; }
        public DataTemplate FollowTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container) {
            if (item is RideFeedDto) {
                return RideTemplate;
            }

            if (item is FollowFeedDto) {
                return FollowTemplate;
            }

            return null;
        }
    }
}
