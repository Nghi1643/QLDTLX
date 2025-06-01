using Dapper;
using Microsoft.Data.SqlClient;
using QuanLyDaoTaoLaiXe.Models;
using QuanLyDaoTaoLaiXe.Models.HoiDap;
using System.Text.Json;

namespace QuanLyDaoTaoLaiXe.Components.HoiDap;

public class HoiDapRepository : IHoiDapRepository
{
    private readonly string _connectionString;
    public HoiDapRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionDB");
    }


    public async Task<Result<List<ChiTietHoiDapVM>>> TaoCauHoi(TaoCauHoi taoCauHoi)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_HoiDap_TaoCauHoi";
            var parameters = new DynamicParameters();
            parameters.Add("@MaNguoiGui", taoCauHoi.MaNguoiGui);
            parameters.Add("@NoiDung", taoCauHoi.NoiDung);
            // lặp lại ở 3 method những tăng hiệu xuất và tính tập trung logic ở database
            parameters.Add("@MaNguoiNhan", JsonSerializer.Serialize(taoCauHoi.MaNguoiNhan));
            var result = await connection.QueryMultipleAsync(
                spName,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            var ketQua = new List<ChiTietHoiDapVM>();
            var cauHoi = await result.ReadFirstOrDefaultAsync<ChiTietHoiDapVM>();
            if (cauHoi == null) return Result<List<ChiTietHoiDapVM>>.Success(ketQua);
            //var danhSachNguoiNhan= await result.ReadAsync<Guid>();
            //cauHoi.DanhSachNguoiNhan = danhSachNguoiNhan.ToList();
            cauHoi.DanhSachNguoiNhan = (await result.ReadAsync<int>()).ToList();
            ketQua.Add(cauHoi);
            var cauTraLoi = await result.ReadAsync<ChiTietHoiDapVM>();
            ketQua.AddRange(cauTraLoi);
            return Result<List<ChiTietHoiDapVM>>.Success(ketQua);
        }
        catch (Exception ex)
        {
            return Result<List<ChiTietHoiDapVM>>.Failure(ex.Message);
        }
    }
    public async Task<Result<List<ChiTietHoiDapVM>>> ChiTietHoiDap(int MaNguoiDung, Guid MaTroChuyen)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_HoiDap_ChiTietHoiDap";
            var parameters = new DynamicParameters();
            parameters.Add("@MaNguoiDung", MaNguoiDung);
            parameters.Add("@MaTroChuyen", MaTroChuyen);
            var result = await connection.QueryMultipleAsync(
                spName,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            var ketQua = new List<ChiTietHoiDapVM>();
            var cauHoi = await result.ReadFirstOrDefaultAsync<ChiTietHoiDapVM>();
            if (cauHoi == null) return Result<List<ChiTietHoiDapVM>>.Success(ketQua);
            //var danhSachNguoiNhan= await result.ReadAsync<Guid>();
            //cauHoi.DanhSachNguoiNhan = danhSachNguoiNhan.ToList();
            cauHoi.DanhSachNguoiNhan = (await result.ReadAsync<int>()).ToList();
            ketQua.Add(cauHoi);
            var cauTraLoi = await result.ReadAsync<ChiTietHoiDapVM>();
            ketQua.AddRange(cauTraLoi);
            return Result<List<ChiTietHoiDapVM>>.Success(ketQua);

        }
        catch (Exception ex)
        {
            return Result<List<ChiTietHoiDapVM>>.Failure(ex.Message);
        }
    }
    public async Task<Result<List<ChiTietHoiDapVM>>> TaoCauTraLoi(Guid MaTroChuyen, TaoCauTraLoi taoCauTraLoi)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_HoiDap_TaoTraLoi";
            var parameters = new DynamicParameters();
            parameters.Add("@MaTroChuyen", MaTroChuyen);
            parameters.Add("@MaNguoiGui", taoCauTraLoi.MaNguoiGui);
            parameters.Add("@NoiDung", taoCauTraLoi.NoiDung);
            var result = await connection.QueryMultipleAsync(
                spName,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            var ketQua = new List<ChiTietHoiDapVM>();
            var cauHoi = await result.ReadFirstOrDefaultAsync<ChiTietHoiDapVM>();
            if (cauHoi == null) return Result<List<ChiTietHoiDapVM>>.Success(ketQua);
            //var danhSachNguoiNhan= await result.ReadAsync<Guid>();
            //cauHoi.DanhSachNguoiNhan = danhSachNguoiNhan.ToList();
            cauHoi.DanhSachNguoiNhan = (await result.ReadAsync<int>()).ToList();
            ketQua.Add(cauHoi);
            var cauTraLoi = await result.ReadAsync<ChiTietHoiDapVM>();
            ketQua.AddRange(cauTraLoi);
            return Result<List<ChiTietHoiDapVM>>.Success(ketQua);
        }
        catch(Exception ex)
        {
            return Result<List<ChiTietHoiDapVM>>.Failure(ex.Message);
        }
    }

    public async Task<Result<Pagination<DanhSachCauHoiVM>>> DanhSachCauHoi(
        int MaNguoiDung, 
        string role, 
        int pageNumber = 1, 
        int pageSize = 10, 
        string? searchKey = null)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_HoiDap_DanhSachCauHoi";
            var parameters = new DynamicParameters();
            parameters.Add("MaNguoiDung", MaNguoiDung);
            parameters.Add("role", role);
            parameters.Add("pageNumber", pageNumber);
            parameters.Add("pageSize", pageSize);
            parameters.Add("searchKey",searchKey);
            var result = await connection.QueryMultipleAsync(
                spName,
                parameters,
                commandType: System.Data.CommandType.StoredProcedure);
            var totalItems = await result.ReadFirstOrDefaultAsync<int>();//chi lay 1 phan tu dau tien
            var items = (await result.ReadAsync<DanhSachCauHoiVM>()).ToList();// lay ra danh sach IEnumerable can .ToList()

            var resultPagination = new Pagination<DanhSachCauHoiVM>(
                items, 
                totalItems, 
                pageNumber, 
                pageSize);
            return Result<Pagination<DanhSachCauHoiVM>>.Success(resultPagination);
        }
        catch(Exception ex)
        {
            return Result<Pagination<DanhSachCauHoiVM>>.Failure(ex.Message);
        }
    }
}
