using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using QuanLyDaoTaoLaiXe.Models;
using QuanLyDaoTaoLaiXe.Models.MonHoc;

namespace QuanLyDaoTaoLaiXe.Components.MonHoc;

public class MonHocRepository : IMonHocRepository
{
    private readonly string _connectionString;

    public MonHocRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("ConnectionDB");
    }
    public async Task<Result<MonHocVM>> GetMonHocByMaMon(short MaMon)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_MonHoc_GetMonHocByMaMon";
            var monHoc = await connection.QueryFirstOrDefaultAsync<MonHocVM>(
                spName,
                new { MaMon },
                commandType: CommandType.StoredProcedure);
            // khi monhoc==null thi handelrResult tra ve not found
            return Result<MonHocVM>.Success(monHoc);
        }
        catch (Exception e)
        {
            return Result<MonHocVM>.Failure(e.Message);
        }
    }

    public async Task<Result<Pagination<MonHocVM>>> GetAllMonHoc(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchKey = null)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_MonHoc_GetAllMonHoc";
            var parameters = new DynamicParameters();
            parameters.Add("@pageNumber", pageNumber);
            parameters.Add("@pageSize", pageSize);
            parameters.Add("@searchKey", searchKey);
            var multi = await connection.QueryMultipleAsync(
                spName,
                parameters,
                commandType: CommandType.StoredProcedure);
            var totalItems = await multi.ReadFirstOrDefaultAsync<int>();
            var items = (await multi.ReadAsync<MonHocVM>()).ToList();
            return Result<Pagination<MonHocVM>>.Success(
                new Pagination<MonHocVM>(
                items,
                totalItems,
                pageNumber, 
                pageSize));
        }
        catch (Exception e)
        {
            return Result<Pagination<MonHocVM>>.Failure(e.Message);
        }
    }

    public async Task<Result<MonHocVM>> TaoMonHoc(Tao_CapNhat_MonHoc monHoc)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_MonHoc_TaoMonHoc";
            var param =new DynamicParameters();
            param.Add("@TenMon",monHoc.TenMon);
            param.Add("@KyHieu",monHoc.KyHieu);
            param.Add("@MoTa",monHoc.MoTa);
            param.Add("@HinhThucHoc",monHoc.HinhThucHoc);
            param.Add("@ThucHanh",monHoc.ThucHanh);
            param.Add("@ChamDiem",monHoc.ChamDiem);
            param.Add("@SuDung",monHoc.SuDung);
            param.Add("@TepKemTheo",monHoc.TepKemTheo);
            var result = await connection.QueryFirstOrDefaultAsync<MonHocVM>(
                spName,
                param,
                commandType: CommandType.StoredProcedure);
            return Result<MonHocVM>.Success(result);
        }
        catch (Exception e)
        {
            return Result<MonHocVM>.Failure(e.Message);
        }
    }

    public async Task<Result<MonHocVM>> CapNhatMonHoc(short maMon, Tao_CapNhat_MonHoc monHoc)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_MonHoc_CapNhatMonHoc";
            var param = new DynamicParameters();
            param.Add("@MaMon", maMon);
            param.Add("@TenMon", monHoc.TenMon);
            param.Add("@KyHieu", monHoc.KyHieu);
            param.Add("@MoTa", monHoc.MoTa);
            param.Add("@HinhThucHoc", monHoc.HinhThucHoc);
            param.Add("@ThucHanh", monHoc.ThucHanh);
            param.Add("@ChamDiem", monHoc.ChamDiem);
            param.Add("@SuDung", monHoc.SuDung);
            param.Add("@TepKemTheo", monHoc.TepKemTheo);
            var result = await connection.QueryFirstOrDefaultAsync<MonHocVM>(
                spName,
                param,
                commandType: CommandType.StoredProcedure);
            return Result<MonHocVM>.Success(result);
        }
        catch (Exception e)
        {
            return Result<MonHocVM>.Failure(e.Message);
        }
    }

    public async Task<Result<MonHocVM>> CapNhatMotPhanMonHoc(short maMon, CapNhatMotPhan_MonHoc monHoc)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            const string spName = "SP_MonHoc_CapNhatMotPhanMonHoc";
            var param = new DynamicParameters();
            param.Add("@MaMon", maMon);
            param.Add("@TenMon", monHoc.TenMon);
            param.Add("@KyHieu", monHoc.KyHieu);
            param.Add("@MoTa", monHoc.MoTa);
            param.Add("@HinhThucHoc", monHoc.HinhThucHoc);
            param.Add("@ThucHanh", monHoc.ThucHanh);
            param.Add("@ChamDiem", monHoc.ChamDiem);
            param.Add("@SuDung", monHoc.SuDung);
            param.Add("@TepKemTheo", monHoc.TepKemTheo);
            var result = await connection.QueryFirstOrDefaultAsync<MonHocVM>(
                spName,
                param,
                commandType: CommandType.StoredProcedure);
            return Result<MonHocVM>.Success(result);
        }
        catch (Exception e)
        {
            return Result<MonHocVM>.Failure(e.Message);
        }
    }
}
