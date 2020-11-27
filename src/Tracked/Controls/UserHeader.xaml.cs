using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tracked.Controls {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserHeader : ContentView {
        public UserHeader() {
            InitializeComponent();
        }

        public static readonly BindableProperty ProfileImageUrlProperty = BindableProperty.Create(
            nameof(ProfileImageUrl),
            typeof(string),
            typeof(UserHeader),
            default(string),
            propertyChanged: UpdateProfileImageUrl);

        public static readonly BindableProperty DetailProperty = BindableProperty.Create(
            nameof(Detail),
            typeof(string),
            typeof(UserHeader),
            default(string),
            propertyChanged: UpdateDetail);

        public static readonly BindableProperty NameProperty = BindableProperty.Create(
            nameof(Name),
            typeof(string),
            typeof(UserHeader),
            default(string),
            propertyChanged: UpdateName);

        public string ProfileImageUrl {
            get { return (string)GetValue(ProfileImageUrlProperty); }
            set { SetValue(ProfileImageUrlProperty, value); }
        }

        public string Detail {
            get { return (string)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }

        public string Name {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        private static void UpdateProfileImageUrl(BindableObject bindable, object oldValue, object newValue) {
            (bindable as UserHeader).ProfileImage.Source = newValue as string;
        }

        private static void UpdateDetail(BindableObject bindable, object oldValue, object newValue) {
            (bindable as UserHeader).DetailLabel.Text = newValue as string;
        }

        private static void UpdateName(BindableObject bindable, object oldValue, object newValue) {
            (bindable as UserHeader).NameLabel.Text = newValue as string;
        }
    }
}