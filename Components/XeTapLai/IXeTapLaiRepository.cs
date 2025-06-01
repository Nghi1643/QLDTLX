using QuanLyDaoTaoLaiXe.Models;
using QuanLyDaoTaoLaiXe.Components;
namespace QuanLyDaoTaoLaiXe.Components.XeTapLai
{
    public interface IXeTapLaiRepository
    {
        public Task<Result<IEnumerable<XeTapLaiInfo>>> Gets();
    }
}
