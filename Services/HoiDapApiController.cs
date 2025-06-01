
using Microsoft.AspNetCore.Mvc;
using QuanLyDaoTaoLaiXe.Components.HoiDap;
using QuanLyDaoTaoLaiXe.Models;
using QuanLyDaoTaoLaiXe.Models.HoiDap;

namespace QuanLyDaoTaoLaiXe.Services;

/// <summary>
/// API hỏi đáp
/// </summary>
public class HoiDapApiController : BaseApiController
{
    private readonly IHoiDapRepository _hoiDapRepository;
    public HoiDapApiController(IWebHostEnvironment hostingEnvironment, IHoiDapRepository hoiDapRepository) : base(hostingEnvironment)
    {
        _hoiDapRepository = hoiDapRepository;
    }

    /// <summary>
    /// Tạo câu hỏi
    /// </summary>
    /// <param name="cauHoi">thông tin câu hỏi</param>
    /// <returns>câu hỏi sau khi tạo</returns>
    /// <response code="200">tạo thành công</response>
    [HttpPost("TaoCauHoi")]
    [ProducesResponseType(typeof(List<ChiTietHoiDapVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> TaoCauHoi([FromBody] TaoCauHoi cauHoi)
    {
        return HandlerResult(await _hoiDapRepository.TaoCauHoi(cauHoi));
    }

    /// <summary>
    /// Lấy chi tiết cuộc trò chuyện
    /// </summary>
    /// <param name="MaNguoiDung">Mã người dùng xem chi tiết</param>
    /// <param name="MaTroChuyen">Mã cuộc trò chuyện cần xem</param>
    /// <returns>Danh sách các câu hỏi và trả lời trong cuộc trò chuyện</returns>
    /// <response code="200">Lấy thành công</response>
    /// <response code="404">Không tìm thấy cuộc trò chuyện</response>
    [HttpGet("ChiTietHoiDap/{MaNguoiDung}/{MaTroChuyen}")]
    [ProducesResponseType(typeof(List<ChiTietHoiDapVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChiTietHoiDap([FromRoute] int MaNguoiDung, [FromRoute] Guid MaTroChuyen)
    {
        return HandlerResult(await _hoiDapRepository.ChiTietHoiDap(MaNguoiDung, MaTroChuyen));
    }

    /// <summary>
    /// Tạo câu trả lời cho một câu hỏi
    /// </summary>
    /// <param name="MaTroChuyen">Mã cuộc trò chuyện cần trả lời</param>
    /// <param name="taoCauTraLoi">Thông tin câu trả lời</param>
    /// <returns>Danh sách cập nhật của cuộc trò chuyện</returns>
    /// <response code="200">Tạo câu trả lời thành công</response>
    /// <response code="404">Không tìm thấy cuộc trò chuyện</response>
    [HttpPost("TaoCauTraLoi/{MaTroChuyen}")]
    [ProducesResponseType(typeof(List<ChiTietHoiDapVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TaoCauTraLoi([FromRoute] Guid MaTroChuyen, [FromBody] TaoCauTraLoi taoCauTraLoi)
    {
        return HandlerResult(await _hoiDapRepository.TaoCauTraLoi(MaTroChuyen, taoCauTraLoi));
    }

    /// <summary>
    /// Lấy danh sách câu hỏi theo điều kiện
    /// </summary>
    /// <param name="MaNguoiDung">Mã người dùng xem danh sách</param>
    /// <param name="role">Vai trò người dùng (HocVien/GiaoVien/BanGiamHieu)</param>
    /// <param name="pageNumber">Số trang, mặc định là 1</param>
    /// <param name="pageSize">Số lượng mỗi trang, mặc định là 10</param>
    /// <param name="searchKey">Từ khóa tìm kiếm trong nội dung câu hỏi</param>
    /// <returns>Danh sách câu hỏi phân trang</returns>
    /// <response code="200">Lấy danh sách thành công</response>
    [HttpGet("DanhSach")]
    [ProducesResponseType(typeof(Pagination<DanhSachCauHoiVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> DanhSachCauHoi(
        [FromQuery] int MaNguoiDung,
        [FromQuery] string role,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchKey = null)
    {
        return HandlerResult(await _hoiDapRepository.DanhSachCauHoi(MaNguoiDung,role,pageNumber,pageSize,searchKey));
    }
}
