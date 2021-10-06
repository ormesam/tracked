using DataAccess;

namespace Api.Utility {
    public class DbFactory {
        private readonly string connectionString;

        public DbFactory(string connectionString) {
            this.connectionString = connectionString;
        }

        public Transaction CreateTransaction() {
            return new Transaction(connectionString, false);
        }

        public Transaction CreateReadOnlyTransaction() {
            return new Transaction(connectionString, true);
        }
    }
}
