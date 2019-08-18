using System;
using System.Threading.Tasks;

namespace MtbMate.Controls
{
    public class ExtendedMenuItem
    {
        public string Title { get; set; }
        public Func<Task> OnClick { get; set; }
    }
}
