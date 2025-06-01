namespace QuanLyDaoTaoLaiXe.Models.HoiDap
{
    public class ChiTietHoiDapVM
    {
        public Guid MaTroChuyen { get; set; }
        public int MaNguoiGui { get; set; }
        public string NoiDung { get; set; } = null!;
        public DateTime NgayGui { get; set; }
        public List<int>? DanhSachNguoiNhan { get; set; }
    }
}
