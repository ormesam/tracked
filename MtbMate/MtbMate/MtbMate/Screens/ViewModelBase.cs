using MtbMate.Contexts;
using MtbMate.Utilities;

namespace MtbMate.Screens {
    public class ViewModelBase : NotifyPropertyChangedBase {
        public MainContext Context { get; }

#if DEBUG
        public bool IsDebugMode => true;
#else
        public bool IsDebugMode => false;
#endif

        public ViewModelBase(MainContext context) {
            Context = context;
        }

        public void Refresh() {
            OnPropertyChanged();
        }

        public virtual string Title => "Mtb Mate";
    }
}
