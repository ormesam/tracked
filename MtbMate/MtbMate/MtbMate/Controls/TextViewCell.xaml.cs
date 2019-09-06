using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MtbMate.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextViewCell : ContentView {
        public TextViewCell() {
            InitializeComponent();
        }

        private void View_Tapped(object sender, System.EventArgs e) {
            OnTapped();
        }
    }
}