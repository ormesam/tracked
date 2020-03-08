using System.Collections.Generic;
using Shared.Dtos;
using Xamarin.Forms;

namespace Tracked.Controls {
    public partial class JumpsControl : ContentView {
        public static readonly BindableProperty JumpsProperty =
            BindableProperty.Create(
                nameof(Jumps),
                typeof(IEnumerable<RideJumpDto>),
                typeof(JumpsControl),
                null);

        public IEnumerable<RideJumpDto> Jumps {
            get => (IEnumerable<RideJumpDto>)GetValue(JumpsProperty);
            set => SetValue(JumpsProperty, value);
        }
    }
}
