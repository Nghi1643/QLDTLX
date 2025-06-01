
namespace QuanLyDaoTaoLaiXe.Models.KhaoSat
{
    public class DanhSachCauHoiKhaoSatVM
    {
        public int MaCauHoi { get; set; }

        public byte Nhom { get; set; }

        public string NhomText => Nhom switch
        {
            1 => "Đánh giá tác phong nền nếp giáo viên",
            2 => "Đánh giá chất lượng dạy của giáo viên Từng buổi học",
            _ => "Không xác định"
        };
        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public string NoiDung { get; set; } = null!;

       public int MaGiaoVien { get; set; }
       public string HoTen { get; set; } = null!;
       public short MaMon { get; set; }
       public string TenMon { get; set; } = null!;
       // đánh giá từng buổi thì có thêm mã lịch và mã lớp 
       public long? MaLich { get; set; }
       public long? MaLop { get; set; }
    }
}
