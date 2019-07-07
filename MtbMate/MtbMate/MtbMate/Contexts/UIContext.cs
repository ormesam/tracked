using System;
using System.Threading.Tasks;
using MtbMate.Utilities;
using Xamarin.Forms;

namespace MtbMate.Contexts
{
    public class UIContext
    {
        private MainContext context;

        public UIContext(MainContext context)
        {
            this.context = context;
        }

        public void ShowInputDialog(string title, string defaultText, Action<string> onOk)
        {
            DependencyService.Get<IPromptUtility>().ShowInputDialog(title, defaultText, onOk);
        }

        public void ShowInputDialog(string title, string defaultText, Func<string, Task<string>> onOk)
        {
            DependencyService.Get<IPromptUtility>().ShowInputDialog(title, defaultText, onOk);
        }
    }
}
