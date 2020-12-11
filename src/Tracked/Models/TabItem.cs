using System;
using System.Threading.Tasks;

namespace Tracked.Models {
    public class TabItem {
        public int Order { get; set; } // bind grid column to this
        public string Text { get; set; }
        public string ImageName { get; set; }
        public bool IsSelected { get; set; }
        public Func<Task> OnClick { get; set; }
    }

    public enum TabItemType {
        Rides,
        Trails,
    }
}
