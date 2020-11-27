using System.Linq;
using FFImageLoading.Forms;
using FFImageLoading.Svg.Forms;
using Shared.Dtos;
using Xamarin.Forms;

namespace Tracked.Controls {
    public class RideOverviewCell : ViewCell {
        private SvgCachedImage cachedRouteSvg = null;
        private CachedImage cachedProfileImage = null;

        protected override void OnBindingContextChanged() {
            var frame = (Frame)View;
            var stackLayout = (StackLayout)frame.Content;
            var profileGrid = stackLayout.Children.First() as UserHeader;

            if (cachedRouteSvg == null) {
                cachedRouteSvg = stackLayout.Children.FirstOrDefault(i => i is SvgCachedImage) as SvgCachedImage;
            }

            if (cachedProfileImage == null) {
                cachedProfileImage = profileGrid.ProfileImage;
            }

            // prevent showing old images occasionally
            cachedRouteSvg.Source = null;
            cachedProfileImage.Source = null;

            var item = BindingContext as RideOverviewDto;

            if (item == null) {
                return;
            }

            cachedRouteSvg.Source = item.RouteSvg;
            cachedProfileImage.Source = item.UserProfileImageUrl;

            base.OnBindingContextChanged();
        }
    }
}
