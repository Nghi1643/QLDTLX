using System.ComponentModel.DataAnnotations;

namespace QuanLyDaoTaoLaiXe.Models.HoiDap
{
    public class TaoCauHoi
    {
        [Required]
        public int MaNguoiGui { get; set; }

        [Required]
        [StringLength(4000)]
        public string NoiDung { get; set; }=null!;

        [Required]
        [MinLength(1)]
        public List<int> MaNguoiNhan { get; set; } = null!;
    }
}
