using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Tracked.Models;

namespace Tracked.Contexts {
    public class StorageContext {
        public readonly IBlobCache Storage = BlobCache.LocalMachine;

        public IList<Ride> GetRides() {
            var rides = Storage.GetAllObjects<Ride>().Wait();

            return rides.ToList();
        }

        public IList<Segment> GetSegments() {
            var segments = Storage.GetAllObjects<Segment>().Wait();

            return segments.ToList();
        }

        public IList<SegmentAttempt> GetSegmentAttempts() {
            var attempts = Storage.GetAllObjects<SegmentAttempt>().Wait();

            return attempts.ToList();
        }

        public async Task SaveObject<T>(Guid id, T obj) {
            await Storage.InsertObject(id.ToString(), obj);
        }

        public async Task RemoveObject<T>(Guid id) {
            await Storage.InvalidateObject<T>(id.ToString());
        }

        public Settings GetSettings() {
            var settings = Storage.GetAllObjects<Settings>().Wait().SingleOrDefault();

            if (settings == null) {
                settings = new Settings();
                SaveSettings(settings).Wait();
            }

            Debug.WriteLine("Settings: Detect Jumps = " + settings.DetectJumps);

            return settings;
        }

        public async Task SaveSettings(Settings settings) {
            if (settings.Id == null) {
                settings.Id = Guid.NewGuid();
                settings.ResetDefaults();
            }

            await SaveObject(settings.Id.Value, settings);
        }

        public async Task SetAccessToken(string token) {
            await BlobCache.Secure.InsertObject("AccessToken", token);
        }

        public string GetAccessToken() {
            return BlobCache.Secure.GetOrCreateObject("AccessToken", () => string.Empty).Wait();
        }

        public async Task SetName(string name) {
            await BlobCache.Secure.InsertObject("Name", name);
        }

        public string GetName() {
            return BlobCache.Secure.GetOrCreateObject("Name", () => string.Empty).Wait();
        }
    }
}
