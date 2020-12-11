using Tracked.Contexts;
using Tracked.Utilities;

namespace Tracked.Screens {
    public class ViewModelBase : NotifyPropertyChangedBase {
        public MainContext Context { get; }

        public ViewModelBase(MainContext context) {
            Context = context;
        }

        public virtual string Title => "Tracked";
    }
}
