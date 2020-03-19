using System.Collections.Generic;
using Shared.Dtos;
using Xamarin.Forms;

namespace Tracked.Controls {
    public partial class JumpsControl : ContentView {
        public static readonly BindableProperty JumpsProperty =
            BindableProperty.Create(
                nameof(Jumps),
                typeof(IEnumerable<JumpDto>),
                typeof(JumpsControl),
                null);

        public IEnumerable<JumpDto> Jumps {
            get => (IEnumerable<JumpDto>)GetValue(JumpsProperty);
            set => SetValue(JumpsProperty, value);
        }
    }
}
