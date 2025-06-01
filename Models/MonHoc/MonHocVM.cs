using System.ComponentModel.DataAnnotations;

namespace QuanLyDaoTaoLaiXe.Models.MonHoc
{
    public class MonHocVM
    {
        public short MaMon { get; set; }
        
        public string TenMon { get; set; } = null!;
        
        public string? KyHieu { get; set; }
        
        public string? MoTa { get; set; }
        
        public bool HinhThucHoc { get; set; }
        
        public bool ThucHanh { get; set; }
        
        public bool ChamDiem { get; set; }
        
        public bool SuDung { get; set; }
        
        public string? TepKemTheo { get; set; }

        // Thuộc tính mô tả trạng thái dưới dạng text
        public string HinhThucHocText => HinhThucHoc ? "Tập trung" : "Trực tuyến";
        public string ThucHanhText => ThucHanh ? "Môn thực hành" : "Môn lý thuyết";
        public string ChamDiemText => ChamDiem ? "Môn chấm điểm" : "Môn điều kiện";
        public string TrangThaiSuDungText => SuDung ? "Đang sử dụng" : "Không sử dụng";
    }
}
