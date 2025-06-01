using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyDaoTaoLaiXe.Components.XeTapLai;

namespace QuanLyDaoTaoLaiXe.Services
{
    public class XeTapLaiApiController : BaseApiController
    {
       private readonly IXeTapLaiRepository xeTapLaiRepository;
        public XeTapLaiApiController(IWebHostEnvironment hostingEnvironment, IXeTapLaiRepository xeTapLaiRepository) : base(hostingEnvironment)
        {
            this.xeTapLaiRepository = xeTapLaiRepository;
        }
        [AllowAnonymous]
        [HttpGet("DanhSach")]
        public async Task<IActionResult> DanhSach()
        {
            var result = await xeTapLaiRepository.Gets();
            return HandlerResult(result);
        }
        [AllowAnonymous]
        [HttpGet("Hello")]
        public IActionResult Hello()
        {
            return Ok("Hello list data");
        }
    }
}
