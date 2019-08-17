using MtbMate.Contexts;

namespace MtbMate.Screens.Settings
{
    public class SettingsScreenViewModel : ViewModelBase
    {
        public SettingsScreenViewModel(MainContext context) : base(context)
        {
        }

        public override string Title => "Settings";
    }
}
