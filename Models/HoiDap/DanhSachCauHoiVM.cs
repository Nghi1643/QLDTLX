namespace QuanLyDaoTaoLaiXe.Models.HoiDap
{
    public class DanhSachCauHoiVM
    {
        public Guid MaTroChuyen { get; set; }
        public int MaNguoiGui { get; set; }
        public string NoiDung { get; set; } = null!;
        public DateTime NgayGui { get; set; }
    }
}
