using System;
using DataAccess.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAccess {
    public class Transaction : IDisposable {
        private readonly SqlConnection connection;
        private readonly SqlTransaction transaction;
        private readonly bool isReadOnly;

        public Transaction(string connectionString, bool isReadOnly) {
            this.isReadOnly = isReadOnly;

            connection = new SqlConnection(connectionString);
            connection.Open();

            transaction = connection.BeginTransaction();
        }

        public ModelDataContext CreateDataContext() {
            var options = new DbContextOptionsBuilder<ModelDataContext>()
                .UseSqlServer(connection)
                .Options;

            var context = new ModelDataContext(options);
            context.Database.UseTransaction(transaction);

            if (isReadOnly) {
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            }

            return context;
        }

        public void Commit() {
            if (!isReadOnly) {
                transaction.Commit();
            }
        }

        public void Dispose() {
            connection.Close();
        }
    }
}
