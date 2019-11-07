using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Name { get; set; }
    }
}
