using System;
using Tracked.Dependancies;

namespace Tracked.Auth {
    public static class CrossGoogleClient {
        private static IGoogleClientManager instance;

        public static void SetInstance(IGoogleClientManager googleClientManager) {
            if (instance != null) {
                throw new InvalidOperationException("CrossGoogleClient: Instance already initialized.");
            }

            instance = googleClientManager;
        }

        public static IGoogleClientManager Current {
            get {
                if (instance == null) {
                    throw new InvalidOperationException("CrossGoogleClient: Instance not initialized.");
                }

                return instance;
            }
        }
    }
}
