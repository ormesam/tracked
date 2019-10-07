using System.Collections;
using Xamarin.Forms;

namespace MtbMate.Controls {
    public partial class JumpsControl : ContentView {
        public static readonly BindableProperty JumpsProperty =
            BindableProperty.Create(
                nameof(Jumps),
                typeof(IEnumerable),
                typeof(JumpsControl),
                null);

        public IEnumerable Jumps {
            get => (IEnumerable)GetValue(JumpsProperty);
            set => SetValue(JumpsProperty, value);
        }
    }
}
