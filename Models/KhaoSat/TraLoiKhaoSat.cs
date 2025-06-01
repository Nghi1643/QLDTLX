using System.ComponentModel.DataAnnotations;

namespace QuanLyDaoTaoLaiXe.Models.KhaoSat;

public class TraLoiKhaoSat
{
    public int MaHocVien { get; set; }
    public int MaGiaoVien { get; set; }
    [Range(1, 5, ErrorMessage = "Điểm đánh giá phải từ 1 đến 5")]
    public byte Diem { get; set; }
    [MaxLength(500, ErrorMessage = "Ý kiến khác không được vượt quá 500 ký tự")]
    public string? YKienKhac { get; set; }
}