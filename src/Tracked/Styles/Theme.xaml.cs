using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Styles {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Theme : ResourceDictionary {
        public Theme() {
            InitializeComponent();
        }
    }
}