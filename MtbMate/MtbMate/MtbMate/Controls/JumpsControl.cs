using System.Collections.Generic;
using MtbMate.Models;
using Xamarin.Forms;

namespace MtbMate.Controls {
    public partial class JumpsControl : ContentView {
        public static readonly BindableProperty JumpsProperty =
            BindableProperty.Create(
                nameof(Jumps),
                typeof(IEnumerable<Jump>),
                typeof(JumpsControl),
                null);

        public IEnumerable<Jump> Jumps {
            get => (IEnumerable<Jump>)GetValue(JumpsProperty);
            set => SetValue(JumpsProperty, value);
        }
    }
}
