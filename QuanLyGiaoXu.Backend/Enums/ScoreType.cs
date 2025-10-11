namespace QuanLyGiaoXu.Backend.Enums;

/// <summary>
/// Enum định nghĩa các loại điểm số cố định trong hệ thống.
/// Buộc GLV phải chọn từ danh sách này, tránh nhập liệu tùy ý.
/// </summary>
public enum ScoreType
{
    Mieng,          // Điểm kiểm tra miệng
    KiemTra15Phut,  // Điểm kiểm tra 15 phút
    KiemTra1Tiet,   // Điểm kiểm tra 1 tiết
    GiuaKy,         // Điểm kiểm tra giữa kỳ
    CuoiKy          // Điểm thi cuối kỳ
}