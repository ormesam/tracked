using System.Collections.Generic;
using Tracked.Contexts;
using Tracked.Models;

namespace Tracked.Screens {
    public abstract class TabbedViewModelBase : ViewModelBase {
        public TabbedViewModelBase(MainContext context) : base(context) {
            TabItemType selected = SelectedTab;

            TabItems = new List<TabItem>(){
                new TabItem {
                    Order = 0,
                    Text = "Rides",
                    ImageName = "feed",
                    IsSelected = selected == TabItemType.Rides,
                    OnClick = Context.UI.GoToRideOverviewScreenAsync,
                },
                new TabItem {
                    Order = 1,
                    Text = "Trails",
                    ImageName = "trails",
                    IsSelected = selected == TabItemType.Trails,
                    OnClick = Context.UI.GoToExploreTrailsScreenAsync,
                },
                new TabItem {
                    Order = 2,
                    Text = "Profile",
                    ImageName = "profile",
                    IsSelected = selected == TabItemType.Profile,
                    OnClick = Context.UI.GoToProfileScreenAsync,
                },
            };
        }

        protected abstract TabItemType SelectedTab { get; }

        public IList<TabItem> TabItems { get; set; }
    }
}
