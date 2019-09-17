using System;

namespace MtbMate.Models {
    public class Settings {
        public Guid? Id { get; set; }
        public bool DetectJumps { get; set; }

        public void ResetDefaults() {
            DetectJumps = true;
        }
    }
}
