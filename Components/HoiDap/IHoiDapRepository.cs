using QuanLyDaoTaoLaiXe.Models.HoiDap;
using QuanLyDaoTaoLaiXe.Models;
namespace QuanLyDaoTaoLaiXe.Components.HoiDap
{
    public interface IHoiDapRepository
    {
        Task<Result<List<ChiTietHoiDapVM>>> TaoCauHoi(TaoCauHoi taoCauHoi);
        Task<Result<List<ChiTietHoiDapVM>>> ChiTietHoiDap(int MaNguoiDung, Guid MaTroChuyen);
        Task<Result<List<ChiTietHoiDapVM>>> TaoCauTraLoi(Guid MaTroChuyen, TaoCauTraLoi taoCauTraLoi);
        Task<Result<Pagination<DanhSachCauHoiVM>>> DanhSachCauHoi(int MaNguoiDung, string role, int pageNumber = 1, int pageSize = 10, string? searchKey = null);
    }
}
