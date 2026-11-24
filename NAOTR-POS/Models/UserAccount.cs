
using System;

namespace NAOTR_POS.Models
{
    public class UserAccount
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }
    }
}
