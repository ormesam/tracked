using MtbMate.Contexts;
using MtbMate.Utilities;

namespace MtbMate.Screens
{
    public class ViewModelBase : NotifyPropertyChangedBase
    {
        public MainContext Context { get; }

        public ViewModelBase(MainContext context)
        {
            Context = context;
        }

        public void Refresh()
        {
            OnPropertyChanged();
        }
    }
}
