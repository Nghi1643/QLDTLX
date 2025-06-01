using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using QuanLyDaoTaoLaiXe.Models.KhaoSat;

namespace QuanLyDaoTaoLaiXe.Components.KhaoSat;

public class KhaoSatRepository : IKhaoSatRepository
{
    private readonly string _connectionString;

    public KhaoSatRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionDB");
    }
    public async Task<Result<bool>> TaoCauHoiKhaoSat(Tao_CapNhat_CauHoiKhaoSat cauHoiKhaoSat)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_KhaoSat_TaoCauHoi";
            var param = new DynamicParameters();
            param.Add("@nhom", cauHoiKhaoSat.Nhom);
            param.Add("@ngayBatDau", cauHoiKhaoSat.NgayBatDau);
            param.Add("@ngayKetThuc", cauHoiKhaoSat.NgayKetThuc);
            param.Add("@noiDung", cauHoiKhaoSat.NoiDung);
            param.Add("@suDung", cauHoiKhaoSat.SuDung);
            await connection.ExecuteAsync(
                spName,
                param,
                commandType: CommandType.StoredProcedure);
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e.Message);
        }
    }

    public async Task<Result<bool?>> CapNhatCauHoiKhaoSat(short maCauhoi, Tao_CapNhat_CauHoiKhaoSat cauHoiKhaoSat)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_KhaoSat_CapNhatCauHoi";
            var param = new DynamicParameters();
            param.Add("@maCauHoi", maCauhoi);
            param.Add("@nhom", cauHoiKhaoSat.Nhom);
            param.Add("@ngayBatDau", cauHoiKhaoSat.NgayBatDau);
            param.Add("@ngayKetThuc", cauHoiKhaoSat.NgayKetThuc);
            param.Add("@noiDung", cauHoiKhaoSat.NoiDung);
            param.Add("@suDung", cauHoiKhaoSat.SuDung);
            var success = await connection.QuerySingleAsync<bool>(
                spName,
                param,
                commandType: CommandType.StoredProcedure);
            // success & null thi trả về not found
            return Result<bool?>.Success(success ? true : null);

        }
        catch (Exception e)
        {
            return Result<bool?>.Failure(e.Message);
        }
    }

    public async Task<Result<bool?>> CapNhatTrangThaiSuDung(int maCauHoi)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_KhaoSat_CapNhatTrangThaiSuDung";
            //var param = new DynamicParameters();
            //param.Add("@maCauHoi", maCauHoi);
            var success = await connection.QuerySingleAsync<bool>(
                spName,
                new { maCauHoi },
                commandType: CommandType.StoredProcedure);
            return Result<bool?>.Success(success ? true : null);
        }
        catch (Exception e)
        {
            return Result<bool?>.Failure(e.Message);
        }
    }

    public async Task<Result<IEnumerable<DanhSachCauHoiKhaoSatVM>>> DanhSachCauHoiKhaoSat_TongKet(int maHocVien, bool daTraLoi)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_KhaoSat_DanhSachCauHoi_TongKet";
            var param = new DynamicParameters();
            param.Add("@MaHocVien", maHocVien);
            param.Add("@DaTraLoi", daTraLoi);
            var result = await connection.QueryAsync<DanhSachCauHoiKhaoSatVM>(
                spName,
                param,
                commandType: CommandType.StoredProcedure);
            return Result<IEnumerable<DanhSachCauHoiKhaoSatVM>>.Success(result);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<DanhSachCauHoiKhaoSatVM>>.Failure(e.Message);
        }
    }
    public async Task<Result<IEnumerable<DanhSachCauHoiKhaoSatVM>>> DanhSachCauHoiKhaoSat_TungBuoi(int maHocVien, bool daTraLoi)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_KhaoSat_DanhSachCauHoi_TungBuoi";
            var param = new DynamicParameters();
            param.Add("@MaHocVien", maHocVien);
            param.Add("@DaTraLoi", daTraLoi);
            var result = await connection.QueryAsync<DanhSachCauHoiKhaoSatVM>(
                spName,
                param,
                commandType: CommandType.StoredProcedure);
            return Result<IEnumerable<DanhSachCauHoiKhaoSatVM>>.Success(result);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<DanhSachCauHoiKhaoSatVM>>.Failure(e.Message);
        }
    }
    public async Task<Result<bool>> TraLoiKhaoSat(short maCauHoi, TraLoiKhaoSat traLoiKhaoSat)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "sp_khaosat_traloi";
            var param = new DynamicParameters();
            param.Add("@maCauHoi", maCauHoi);
            param.Add("@maHocVien", traLoiKhaoSat.MaHocVien);
            param.Add("@maGiaoVien", traLoiKhaoSat.MaGiaoVien);
            param.Add("@diem", traLoiKhaoSat.Diem);
            param.Add("@yKienKhac", traLoiKhaoSat.YKienKhac);
            param.Add("@returnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync(
                spName,
                param,
                commandType: CommandType.StoredProcedure);

            var returnValue = param.Get<int>("@returnValue");

            if (returnValue == 0)
                return Result<bool>.Failure("Câu hỏi này đã được trả lời, không thể trả lời lại.");

            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            return Result<bool>.Failure(e.Message);
        }
    }

    public async Task<Result<IEnumerable<KetQuaKhaoSatGiaoVienVM>>> KetQuaKhaoSatDanhGia(short? maCauHoi = null)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_KhaoSat_KetQuaDanhGia";
            var param = new DynamicParameters();
            param.Add("@maCauHoi", maCauHoi);

            var result = await connection.QueryAsync<KetQuaKhaoSatGiaoVienVM>(
                spName,
                param,
                commandType: CommandType.StoredProcedure);

            return Result<IEnumerable<KetQuaKhaoSatGiaoVienVM>>.Success(result);
        }
        catch (Exception e)
        {
            return Result<IEnumerable<KetQuaKhaoSatGiaoVienVM>>.Failure(e.Message);
        }
    }
}
