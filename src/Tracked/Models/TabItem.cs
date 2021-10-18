using System;
using System.Threading.Tasks;

namespace Tracked.Models {
    public class TabItem {
        public int Order { get; set; }
        public string Icon { get; set; }
        public bool IsSelected { get; set; }
        public Func<Task> OnClick { get; set; }
    }

    public enum TabItemType {
        Feed,
        Trails,
        Profile,
        Settings,
        Search,
    }
}
