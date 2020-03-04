using Android.Content;
using Tracked.Controls;
using Tracked.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]
namespace Tracked.Droid.Renderers {
    public class ExtendedListViewRenderer : ListViewRenderer {
        public ExtendedListViewRenderer(Context context) : base(context) {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e) {
            base.OnElementChanged(e);

            if (e.NewElement == null) {
                return;
            }

            if (Control != null) {
                var listView = (ExtendedListView)e.NewElement;

                Control.NestedScrollingEnabled = listView.IsScrollEnabled;
            }
        }
    }
}