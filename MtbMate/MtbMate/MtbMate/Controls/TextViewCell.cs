
using System;
using Xamarin.Forms;

namespace MtbMate.Controls {
    public partial class TextViewCell : ContentView {
        public event EventHandler Tapped;

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(TextViewCell),
            default(string),
            propertyChanged: UpdateText);

        public static readonly BindableProperty DetailProperty = BindableProperty.Create(
            nameof(Detail),
            typeof(string),
            typeof(TextViewCell),
            default(string),
            propertyChanged: UpdateDetail);

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(TextViewCell),
            Color.Black,
            propertyChanged: UpdateTextColor);

        public static readonly BindableProperty DetailColorProperty = BindableProperty.Create(
            nameof(DetailColor),
            typeof(Color),
            typeof(TextViewCell),
            Color.Default,
            propertyChanged: UpdateDetailColor);

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

        protected internal virtual void OnTapped() {
            Tapped?.Invoke(this, EventArgs.Empty);
        }

        private static void UpdateText(BindableObject bindable, object oldValue, object newValue) {
            (bindable as TextViewCell).TitleLabel.Text = newValue as string;
        }

        private static void UpdateTextColor(BindableObject bindable, object oldValue, object newValue) {
            (bindable as TextViewCell).TitleLabel.TextColor = (Color)newValue;
        }

        private static void UpdateDetail(BindableObject bindable, object oldValue, object newValue) {
            (bindable as TextViewCell).DetailLabel.Text = newValue as string;
        }

        private static void UpdateDetailColor(BindableObject bindable, object oldValue, object newValue) {
            (bindable as TextViewCell).DetailLabel.TextColor = (Color)newValue;
        }
    }
}