using MtbMate.Models;

namespace MtbMate.Utilities
{
    public delegate void JumpEventHandler(JumpEventArgs e);

    public class JumpEventArgs
    {
        public JumpModel Jump { get; set; }
    }
}
