namespace QuanLyGiaoXu.Backend.Enums;

/// <summary>
/// Enum định nghĩa các trạng thái điểm danh.
/// Việc dùng enum giúp code an toàn kiểu dữ liệu và dễ đọc hơn dùng số (0, 1, 2).
/// </summary>
public enum AttendanceStatus
{
    VangKhongPhep = 0, // Trạng thái vắng không phép
    CoMat = 1,         // Trạng thái có mặt
    VangCoPhep = 2     // Trạng thái vắng có phép
}
