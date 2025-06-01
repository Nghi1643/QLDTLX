using QuanLyDaoTaoLaiXe.Models;
using QuanLyDaoTaoLaiXe.Models.MonHoc;

namespace QuanLyDaoTaoLaiXe.Components.MonHoc
{
    public interface IMonHocRepository
    {
        Task<Result<MonHocVM>> GetMonHocByMaMon(short MaMon);
        Task<Result<Pagination<MonHocVM>>> GetAllMonHoc(int pageNumber = 1, int pageSize = 10, string? searchKey = null);
        Task<Result<MonHocVM>> TaoMonHoc(Tao_CapNhat_MonHoc monHoc);
        Task<Result<MonHocVM>> CapNhatMonHoc(short maMon, Tao_CapNhat_MonHoc monHoc);
        Task<Result<MonHocVM>> CapNhatMotPhanMonHoc(short maMon, CapNhatMotPhan_MonHoc monHoc);
    }
}
