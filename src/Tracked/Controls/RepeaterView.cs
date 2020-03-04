using System.Collections;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace Tracked.Controls {
    public class RepeaterView : StackLayout {
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(RepeaterView),
            null);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(RepeaterView),
            propertyChanging: ItemsSourceChanging);

        public DataTemplate ItemTemplate {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public IEnumerable ItemsSource {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanging(BindableObject bindable, object oldValue, object newValue) {
            if (oldValue != null && oldValue is INotifyCollectionChanged) {
                ((INotifyCollectionChanged)oldValue).CollectionChanged -= ((RepeaterView)bindable).OnCollectionChanged;
            }

            if (newValue != null && newValue is INotifyCollectionChanged) {
                ((INotifyCollectionChanged)newValue).CollectionChanged += ((RepeaterView)bindable).OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            Populate();
        }

        protected override void OnPropertyChanged(string propertyName = null) {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ItemTemplateProperty.PropertyName ||
                propertyName == ItemsSourceProperty.PropertyName) {

                Populate();
            }
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();

            Populate();
        }

        public void Populate() {
            if (ItemsSource != null) {
                Children.Clear();

                foreach (var item in ItemsSource) {
                    object content = ItemTemplate.CreateContent();

                    if (content is View view) {
                        view.BindingContext = item;
                        Children.Add(view);
                    }
                }
            }
        }
    }
}