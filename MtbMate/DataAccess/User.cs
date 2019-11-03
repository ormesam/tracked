using System;
using System.Linq;
using System.Security.Cryptography;

namespace DataAccess.Models {
    public partial class User {
        public bool IsPasswordCorrect(string password) {
            if (PasswordSalt == null || PasswordHash == null) {
                return false;
            }

            return IsPasswordCorrect(PasswordSalt, PasswordHash, password);
        }

        public static bool IsPasswordCorrect(byte[] salt, byte[] hash, string password) {
            return ByteArrayCompare(hash, HashPassword(salt, password));
        }

        private static bool ByteArrayCompare(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2) {
            return a1.SequenceEqual(a2);
        }

        public void SetPassword(string password) {
            if (string.IsNullOrEmpty(password)) {
                PasswordHash = null;
                PasswordSalt = null;

                return;
            }

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
                byte[] buffer = new byte[16];

                rng.GetBytes(buffer);

                PasswordSalt = buffer;
            }

            PasswordHash = HashPassword(PasswordSalt, password);
        }

        private static byte[] HashPassword(byte[] salt, string password) {
            using (Rfc2898DeriveBytes enc = new Rfc2898DeriveBytes(password, salt.ToArray())) {
                return enc.GetBytes(20);
            }
        }
    }
}
