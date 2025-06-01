using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuanLyDaoTaoLaiXe.Models.MonHoc
{
    public class CapNhatMotPhan_MonHoc
    {
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Tên môn học phải từ 1 đến 150 ký tự")]
        public string? TenMon { get; set; }

        [StringLength(10, ErrorMessage = "Ký hiệu tối đa 10 ký tự")]
        public string? KyHieu { get; set; }

        [StringLength(4000, MinimumLength = 10, ErrorMessage = "Mô tả phải từ 10 đến 3000 ký tự")]
        public string? MoTa { get; set; }

        public bool? HinhThucHoc { get; set; }
        public bool? ThucHanh { get; set; }
        public bool? ChamDiem { get; set; }
        public bool? SuDung { get; set; }

        public string? TepKemTheo { get; set; }
    }
}