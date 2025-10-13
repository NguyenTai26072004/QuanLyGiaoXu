using System.ComponentModel.DataAnnotations; // Namespace cần thiết cho các Attribute validation

namespace QuanLyGiaoXu.Backend.DTOs.GradeDtos
{
    /// <summary>
    /// DTO chứa dữ liệu mà người dùng gửi lên để Tạo hoặc Cập nhật một Khối/Ngành.
    /// Chúng ta sử dụng Data Annotations ([Required], [MaxLength]) để validate dữ liệu đầu vào.
    /// </summary>
    public class CreateUpdateGradeDto
    {
        // Bắt buộc người dùng phải nhập tên Khối
        [Required(ErrorMessage = "Tên Khối/Ngành không được để trống.")]
        // Giới hạn độ dài tối đa là 50 ký tự, khớp với CSDL
        [MaxLength(50)]
        public string Name { get; set; }
    }
}