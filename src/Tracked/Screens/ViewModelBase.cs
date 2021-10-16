using System;
using System.Threading.Tasks;
using Tracked.Contexts;
using Tracked.Utilities;

namespace Tracked.Screens {
    public class ViewModelBase : NotifyPropertyChangedBase {
        private bool isRefreshing;

        public MainContext Context { get; }

        public ViewModelBase(MainContext context) {
            Context = context;
        }

        public virtual string Title => "Tracked";

        public async void IgnoreResult(Func<Task> action) {
            await action();
        }

        public bool IsRefreshing {
            get { return isRefreshing; }
            set {
                if (value != isRefreshing) {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }
    }
}
