using System.Collections.Generic;
using MtbMate.Models;
using Xamarin.Forms;

namespace MtbMate.Controls {
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
                typeof(IEnumerable<Medal>),
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

        public IEnumerable<Medal> Medals {
            get => (IEnumerable<Medal>)GetValue(MedalsProperty);
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
                var source = Medal != null ? new List<Medal>() { Medal.Value } : Medals;

                medals.Children.Clear();

                foreach (Medal medal in source) {
                    if (medal == Models.Medal.None) {
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
                case Models.Medal.Bronze:
                    return "bronze.png";
                case Models.Medal.Silver:
                    return "silver.png";
                case Models.Medal.Gold:
                    return "gold.png";
                default:
                    return null;
            }
        }
    }
}