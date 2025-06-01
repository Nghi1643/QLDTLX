using System.ComponentModel.DataAnnotations;

namespace QuanLyDaoTaoLaiXe.Models.KhaoSat
{
    public class Tao_CapNhat_CauHoiKhaoSat : IValidatableObject
    {
        //[Required(ErrorMessage = "Nhóm câu hỏi là bắt buộc")] //co the null theo database 
        //[Range(1,byte.MaxValue,ErrorMessage = "Nhóm câu hỏi không hợp lệ")] // có thể mở rộng thêm nhóm
        [Range(1, 2, ErrorMessage = "Nhóm câu hỏi không hợp lệ")] // cố định 2 nhóm phải sửa thủ công
        public byte? Nhom { get; set; }

        //[Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime? NgayBatDau { get; set; }

        //[Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateTime? NgayKetThuc { get; set; }

        [Required(ErrorMessage = "Nội dung câu hỏi không được để trống")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Nội dung phải từ 10 đến 500 ký tự")]
        public string NoiDung { get; set; } = null!;

        public bool SuDung { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Chỉ validate khi cả 2 ngày đều có giá trị
            if (NgayBatDau.HasValue && NgayKetThuc.HasValue)
            {
                if (NgayKetThuc <= NgayBatDau)
                    yield return new ValidationResult("Ngày kết thúc phải lớn hơn ngày bắt đầu", new[] { nameof(NgayKetThuc) });
            }

            // Validate ngày kết thúc với ngày hiện tại (chỉ khi có giá trị)
            if (NgayKetThuc.HasValue && NgayKetThuc <= DateTime.Now)
                yield return new ValidationResult("Ngày kết thúc phải lớn hơn ngày hiện tại", new[] { nameof(NgayKetThuc) });
        }
    }
}