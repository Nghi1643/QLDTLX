using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyDaoTaoLaiXe.Components.MonHoc;
using QuanLyDaoTaoLaiXe.Models.MonHoc;

namespace QuanLyDaoTaoLaiXe.Services
{
    /// <summary>
    /// API quản lý môn học
    /// </summary>
    public class MonHocApiController : BaseApiController
    {
        private readonly IMonHocRepository _monHocRepository;
        public MonHocApiController(IWebHostEnvironment hostingEnvironment,IMonHocRepository monHocRepository) : base(hostingEnvironment)
        {
            _monHocRepository = monHocRepository;
        }

        /// <summary>
        /// Lấy thông tin môn học theo mã môn
        /// </summary>
        /// <param name="MaMon">Mã môn học cần truy vấn</param>
        /// <returns>Thông tin chi tiết của môn học</returns>
        /// <response code="200">Lấy thông tin thành công</response>
        /// <response code="404">Không tìm thấy môn học</response>
        [HttpPost("{MaMon}")]
        [ProducesResponseType(typeof(MonHocVM),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMonHocByMaMon([FromRoute] short MaMon)
        {
            return HandlerResult(await _monHocRepository.GetMonHocByMaMon(MaMon));
        }

        /// <summary>
        /// Lấy danh sách tất cả môn học có phân trang
        /// </summary>
        /// <param name="pageNumber">Số trang, mặc định là 1</param>
        /// <param name="pageSize">Số lượng mỗi trang, mặc định là 10</param>
        /// <param name="searchKey">Từ khóa tìm kiếm (tùy chọn)</param>
        /// <returns>Danh sách môn học được phân trang</returns>
        /// <response code="200">Lấy danh sách thành công</response>
        [HttpGet("DanhSach")]
        [ProducesResponseType(typeof(MonHocVM), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllMonHoc(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchKey = null)
        {
            return HandlerResult(await _monHocRepository.GetAllMonHoc(pageNumber, pageSize, searchKey));
        }

        /// <summary>
        /// Tạo môn học mới
        /// </summary>
        /// <param name="monHoc">Thông tin môn học cần tạo</param>
        /// <returns>Thông tin môn học sau khi tạo</returns>
        /// <response code="200">Tạo môn học thành công</response>
        /// <response code="400">Dữ liệu không hợp lệ</response>
        [HttpPost("TaoMonHoc")]
        [ProducesResponseType(typeof(MonHocVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TaoMonHoc([FromBody] Tao_CapNhat_MonHoc monHoc)
        {
            return HandlerResult(await _monHocRepository.TaoMonHoc(monHoc));
        }

        /// <summary>
        /// Cập nhật toàn bộ thông tin môn học
        /// </summary>
        /// <param name="MaMon">Mã môn học cần cập nhật</param>
        /// <param name="monHoc">Thông tin môn học mới</param>
        /// <returns>Thông tin môn học sau khi cập nhật</returns>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="404">Không tìm thấy môn học</response>
        /// <response code="400">Dữ liệu không hợp lệ</response>
        [HttpPut("CapNhatMonHoc/{MaMon}")]
        [ProducesResponseType(typeof(MonHocVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CapNhatMonHoc([FromRoute] short MaMon, [FromBody] Tao_CapNhat_MonHoc monHoc)
        {
            return HandlerResult(await _monHocRepository.CapNhatMonHoc(MaMon, monHoc));
        }

        /// <summary>
        /// Cập nhật một phần thông tin môn học
        /// </summary>
        /// <param name="MaMon">Mã môn học cần cập nhật</param>
        /// <param name="monHoc">Thông tin cần cập nhật</param>
        /// <returns>Thông tin môn học sau khi cập nhật</returns>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="404">Không tìm thấy môn học</response>
        /// <response code="400">Dữ liệu không hợp lệ</response>
        [HttpPatch("CapNhatMotPhanMonHoc/{MaMon}")]
        [ProducesResponseType(typeof(MonHocVM), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CapNhatMotPhanMonHoc([FromRoute] short MaMon, [FromBody] CapNhatMotPhan_MonHoc monHoc)
        {
            return HandlerResult(await _monHocRepository.CapNhatMotPhanMonHoc(MaMon, monHoc));
        }
    }
}
