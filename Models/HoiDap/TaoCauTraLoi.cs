using System.ComponentModel.DataAnnotations;

namespace QuanLyDaoTaoLaiXe.Models.HoiDap
{
    public class TaoCauTraLoi
    {
        [Required]
        public int MaNguoiGui { get; set; }

        [Required]
        [StringLength(4000)]
        public string NoiDung { get; set; } = null!;
    }
}
