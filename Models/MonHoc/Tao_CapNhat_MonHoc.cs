using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuanLyDaoTaoLaiXe.Models.MonHoc
{
    public class Tao_CapNhat_MonHoc
    {
        [Required(ErrorMessage = "Tên môn học không được để trống")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên môn học phải từ 1 đến 150 ký tự")]
        public string TenMon { get; set; } = null!;

        [StringLength(10, ErrorMessage = "Ký hiệu tối đa 10 ký tự")]
        public string? KyHieu { get; set; }

        [StringLength(4000, MinimumLength = 10, ErrorMessage = "Mô tả phải từ 10 đến 3000 ký tự")]
        public string? MoTa { get; set; }

        public bool HinhThucHoc { get; set; } = true;
        public bool ThucHanh { get; set; } = false;
        public bool ChamDiem { get; set; } = true;
        public bool SuDung { get; set; } = true;

        public string? TepKemTheo { get; set; }
    }
}
