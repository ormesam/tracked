using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string ApiKey { get; set; }
    }
}
