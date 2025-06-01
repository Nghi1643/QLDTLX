using QuanLyDaoTaoLaiXe.Models.KhaoSat;

namespace QuanLyDaoTaoLaiXe.Components.KhaoSat
{
    public interface IKhaoSatRepository
    {
        Task<Result<bool>> TaoCauHoiKhaoSat(Tao_CapNhat_CauHoiKhaoSat capNhatCauHoiKhaoSat);
        Task<Result<bool?>> CapNhatCauHoiKhaoSat(short maCauhoi, Tao_CapNhat_CauHoiKhaoSat cauHoiKhaoSat);
        Task<Result<bool?>> CapNhatTrangThaiSuDung(int maCauHoi);
        Task<Result<IEnumerable<DanhSachCauHoiKhaoSatVM>>> DanhSachCauHoiKhaoSat_TongKet(int maHocVien, bool daTraLoi);
        Task<Result<IEnumerable<DanhSachCauHoiKhaoSatVM>>> DanhSachCauHoiKhaoSat_TungBuoi(int maHocVien, bool daTraLoi);
        Task<Result<bool>> TraLoiKhaoSat(short maCauHoi,TraLoiKhaoSat traLoiKhaoSat);
        Task<Result<IEnumerable<KetQuaKhaoSatGiaoVienVM>>> KetQuaKhaoSatDanhGia(short? maCauHoi = null);
    }
}
