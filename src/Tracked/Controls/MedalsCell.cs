using System.Collections;
using System.Collections.Generic;
using Shared;
using Xamarin.Forms;

namespace Tracked.Controls {
    public partial class MedalsCell : ContentView {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(MedalsCell),
            default(string),
            propertyChanged: UpdateText);

        public static readonly BindableProperty DetailProperty = BindableProperty.Create(
            nameof(Detail),
            typeof(string),
            typeof(MedalsCell),
            default(string),
            propertyChanged: UpdateDetail);

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(MedalsCell),
            Color.Black,
            propertyChanged: UpdateTextColor);

        public static readonly BindableProperty DetailColorProperty = BindableProperty.Create(
            nameof(DetailColor),
            typeof(Color),
            typeof(MedalsCell),
            Color.Default,
            propertyChanged: UpdateDetailColor);

        public static readonly BindableProperty MedalsProperty =
            BindableProperty.Create(
                nameof(Medals),
                typeof(IEnumerable),
                typeof(MedalsCell),
                null);

        public static readonly BindableProperty MedalProperty =
            BindableProperty.Create(
                nameof(Medal),
                typeof(Medal),
                typeof(MedalsCell),
                null);

        public string Detail {
            get { return (string)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        public Color DetailColor {
            get { return (Color)GetValue(DetailColorProperty); }
            set { SetValue(DetailColorProperty, value); }
        }

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Color TextColor {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public IEnumerable Medals {
            get => (IEnumerable)GetValue(MedalsProperty);
            set => SetValue(MedalsProperty, value);
        }

        public Medal? Medal {
            get => (Medal?)GetValue(MedalProperty);
            set => SetValue(MedalsProperty, value);
        }

        private static void UpdateText(BindableObject bindable, object oldValue, object newValue) {
            (bindable as MedalsCell).TitleLabel.Text = newValue as string;
        }

        private static void UpdateTextColor(BindableObject bindable, object oldValue, object newValue) {
            (bindable as MedalsCell).TitleLabel.TextColor = (Color)newValue;
        }

        private static void UpdateDetail(BindableObject bindable, object oldValue, object newValue) {
            (bindable as MedalsCell).DetailLabel.Text = newValue as string;
        }

        private static void UpdateDetailColor(BindableObject bindable, object oldValue, object newValue) {
            (bindable as MedalsCell).DetailLabel.TextColor = (Color)newValue;
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();

            Populate();
        }

        public void Populate() {
            if (Medals != null || Medal != null) {
                var source = new List<Medal>();

                if (Medals != null) {
                    foreach (var item in Medals) {
                        source.Add((Medal)item);
                    }
                } else {
                    source.Add(Medal.Value);
                }

                medals.Children.Clear();

                foreach (Medal medal in source) {
                    if (medal == Shared.Medal.None) {
                        continue;
                    }

                    medals.Children.Add(new Image {
                        Source = ImageSource.FromFile(GetMedalImage(medal)),
                    });
                }
            }
        }

        private string GetMedalImage(Medal medal) {
            switch (medal) {
                case Shared.Medal.Bronze:
                    return "bronze.png";
                case Shared.Medal.Silver:
                    return "silver.png";
                case Shared.Medal.Gold:
                    return "gold.png";
                default:
                    return null;
            }
        }
    }
}