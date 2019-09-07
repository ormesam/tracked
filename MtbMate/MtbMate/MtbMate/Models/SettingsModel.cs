using System;

namespace MtbMate.Models {
    public class SettingsModel
    {
        public Guid? Id { get; set; }
        public bool DetectJumps { get; set; }

        public void ResetDefaults()
        {
            DetectJumps = true;
        }
    }
}
