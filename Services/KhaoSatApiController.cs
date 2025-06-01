using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyDaoTaoLaiXe.Components.KhaoSat;
using QuanLyDaoTaoLaiXe.Models.KhaoSat;

namespace QuanLyDaoTaoLaiXe.Services;
/// <summary>
///  API khảo sát
/// </summary>
public class KhaoSatApiController : BaseApiController
{
    private readonly IKhaoSatRepository _khaoSatRepository;
    public KhaoSatApiController(IWebHostEnvironment hostingEnvironment,IKhaoSatRepository khaoSatRepository) : base(hostingEnvironment)
    {
        _khaoSatRepository = khaoSatRepository;
    }

    /// <summary>
    /// Tạo câu hỏi khảo sát mới
    /// </summary>
    /// <param name="capNhatCauHoiKhaoSat">Thông tin câu hỏi khảo sát</param>
    /// <returns>Kết quả tạo câu hỏi</returns>
    /// <response code="200">Tạo thành công</response>
    /// <response code="400">Dữ liệu không hợp lệ</response>
    [HttpPost("TaoCauHoiKhaoSat")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TaoCauHoiKhaoSat([FromBody] Tao_CapNhat_CauHoiKhaoSat capNhatCauHoiKhaoSat)
    {
        return HandlerResult(await _khaoSatRepository.TaoCauHoiKhaoSat(capNhatCauHoiKhaoSat));
    }

    /// <summary>
    /// Cập nhật toàn bộ thông tin câu hỏi khảo sát
    /// </summary>
    /// <param name="maCauHoi">Mã câu hỏi khảo sát cần cập nhật</param>
    /// <param name="cauHoiKhaoSat">Thông tin cập nhật câu hỏi</param>
    /// <returns>Kết quả cập nhật</returns>
    /// <response code="200">Cập nhật thành công</response>
    /// <response code="400">Dữ liệu không hợp lệ</response>
    /// <response code="404">Không tìm thấy câu hỏi</response>
    [HttpPut("CapNhatCauHoiKhaoSat/{maCauHoi}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CapNhatCauHoiKhaoSat([FromRoute] short maCauHoi, [FromBody] Tao_CapNhat_CauHoiKhaoSat cauHoiKhaoSat)
    {
        return HandlerResult(await _khaoSatRepository.CapNhatCauHoiKhaoSat(maCauHoi, cauHoiKhaoSat));
    }

    /// <summary>
    /// Chuyển đổi trạng thái sử dụng câu hỏi khảo sát (bật/tắt)
    /// </summary>
    /// <param name="maCauHoi">Mã câu hỏi khảo sát</param>
    /// <returns>Kết quả chuyển đổi trạng thái</returns>
    /// <response code="200">Chuyển đổi thành công</response>
    /// <response code="404">Không tìm thấy câu hỏi</response>
    [HttpPatch("ChuyenDoiTrangThai/{maCauHoi}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChuyenDoiTrangThaiSuDung([FromRoute] int maCauHoi)
    {
        return HandlerResult(await _khaoSatRepository.CapNhatTrangThaiSuDung(maCauHoi));
    }

    /// <summary>
    /// Lấy danh sách câu hỏi khảo sát đánh giá tổng kết khóa học
    /// </summary>
    /// <param name="maHocVien">Mã học viên cần xem danh sách câu hỏi</param>
    /// <param name="daTraLoi">Lọc câu hỏi đã trả lời (true) hoặc chưa trả lời (false)</param>
    /// <returns>Danh sách câu hỏi khảo sát tổng kết</returns>
    /// <response code="200">Trả về danh sách câu hỏi khảo sát tổng kết</response>
    [HttpGet("DanhSachCauHoiKhaoSat_TongKet/{maHocVien}")]
    [ProducesResponseType(typeof(IEnumerable<DanhSachCauHoiKhaoSatVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DanhSachCauHoiKhaoSat_TongKet([FromRoute] int maHocVien, [FromQuery] bool daTraLoi = false)
    {
        return HandlerResult(await _khaoSatRepository.DanhSachCauHoiKhaoSat_TongKet(maHocVien, daTraLoi));
    }

    /// <summary>
    /// Lấy danh sách câu hỏi khảo sát đánh giá từng buổi học
    /// </summary>
    /// <param name="maHocVien">Mã học viên cần xem danh sách câu hỏi</param>
    /// <param name="daTraLoi">Lọc câu hỏi đã trả lời (true) hoặc chưa trả lời (false)</param>
    /// <returns>Danh sách câu hỏi khảo sát cho từng buổi học</returns>
    /// <response code="200">Trả về danh sách câu hỏi khảo sát theo buổi học</response>
    [HttpGet("DanhSachCauHoiKhaoSat_TungBuoi/{maHocVien}")]
    [ProducesResponseType(typeof(IEnumerable<DanhSachCauHoiKhaoSatVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DanhSachCauHoiKhaoSat_TungBuoi([FromRoute] int maHocVien, [FromQuery] bool daTraLoi = false)
    {
        return HandlerResult(await _khaoSatRepository.DanhSachCauHoiKhaoSat_TungBuoi(maHocVien, daTraLoi));
    }

    /// <summary>
    /// Trả lời câu hỏi khảo sát
    /// </summary>
    /// <param name="maCauHoi">Mã câu hỏi cần trả lời</param>
    /// <param name="traLoiKhaoSat">Thông tin trả lời khảo sát</param>
    /// <returns>Kết quả lưu trả lời khảo sát</returns>
    /// <response code="200">Lưu trả lời thành công</response>
    /// <response code="400">Dữ liệu không hợp lệ hoặc câu hỏi đã được trả lời</response>
    /// <response code="404">Không tìm thấy câu hỏi hoặc giáo viên</response>
    [HttpPost("TraLoiKhaoSat/{maCauHoi}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TraLoiKhaoSat([FromRoute] short maCauHoi, [FromBody] TraLoiKhaoSat traLoiKhaoSat)
    {
        return HandlerResult(await _khaoSatRepository.TraLoiKhaoSat(maCauHoi,traLoiKhaoSat));
    }

    /// <summary>
    /// Lấy kết quả đánh giá khảo sát của giáo viên
    /// </summary>
    /// <param name="maCauHoi">Mã câu hỏi khảo sát (không bắt buộc, nếu không chỉ định sẽ lấy kết quả tất cả khảo sát)</param>
    /// <returns>Danh sách giáo viên với điểm đánh giá trung bình</returns>
    /// <response code="200">Trả về danh sách kết quả đánh giá</response>
    [HttpGet("KetQuaKhaoSat")]
    [ProducesResponseType(typeof(IEnumerable<KetQuaKhaoSatGiaoVienVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> KetQuaKhaoSat([FromQuery] short? maCauHoi = null)
    {
        return HandlerResult(await _khaoSatRepository.KetQuaKhaoSatDanhGia(maCauHoi));
    }
}
