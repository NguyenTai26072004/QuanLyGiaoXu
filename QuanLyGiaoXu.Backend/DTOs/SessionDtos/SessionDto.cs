using System;
namespace QuanLyGiaoXu.Backend.DTOs.SessionDtos;

public class SessionDto
{
    public int Id { get; set; }
    public DateTime SessionDate { get; set; }
    public string? Title { get; set; }
    public bool IsCancelled { get; set; }
}