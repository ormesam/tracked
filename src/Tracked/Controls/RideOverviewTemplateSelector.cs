using Shared.Dtos;
using Xamarin.Forms;

namespace Tracked.Controls {
    public class RideOverviewTemplateSelector : DataTemplateSelector {
        public DataTemplate UploadTemplate { get; set; }
        public DataTemplate RideTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container) {
            return ((RideOverviewDto)item).IsAwaitingUpload ? UploadTemplate : RideTemplate;
        }
    }
}
