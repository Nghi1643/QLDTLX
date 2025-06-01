namespace QuanLyDaoTaoLaiXe.Models.KhaoSat
{
    public class CauHoiKhaoSatVM
    {
        public int MaCauHoi { get; set; }

        public byte Nhom { get; set; }

        public string NhomText => Nhom switch
        {
            1 => "Đánh giá tác phong nền nếp giáo viên",
            2 => "Đánh giá chất lượng dạy của giáo viên Từng buổi học",
            _=>"Không xác định"
        };
        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public string NoiDung { get; set; } = null!;

        public bool SuDung { get; set; }
        // trả về đồng thời cả giá trị bool và string đe client dể hiển thị
        public string SuDungText => SuDung ? "Sử dụng" : "Không sử dụng";
    }
}