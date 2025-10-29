using System;
using System.Collections.Generic;

namespace QuanLyGiaoXu.Backend.DTOs.AccountDtos
{
    // DTO này dùng để hiển thị thông tin chi tiết của một tài khoản
    public class AccountDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string? SaintName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Position { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsActive { get; set; }

        // Danh sách các vai trò của người dùng này (VD: ["Admin", "GLV"])
        public IList<string> Roles { get; set; }
    }
}