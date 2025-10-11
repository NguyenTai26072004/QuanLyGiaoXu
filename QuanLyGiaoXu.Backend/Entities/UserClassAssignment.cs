namespace QuanLyGiaoXu.Backend.Entities
{
    public class UserClassAssignment
    {
        public string UserId { get; set; }
        public int ClassId { get; set; }
        public User User { get; set; } = null!;
        public Class Class { get; set; } = null!;
    }
}
