using Dapper;
using Microsoft.Data.SqlClient;
using QuanLyDaoTaoLaiXe.Models;

namespace QuanLyDaoTaoLaiXe.Components.XeTapLai
{
    public class XeTapLaiRepository : IXeTapLaiRepository
    {
        private readonly IConfiguration _configuration;
        public XeTapLaiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Result<IEnumerable<XeTapLaiInfo>>> Gets()
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                string spName = "SP_XeTapLai_DanhSach";
                using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionDB")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<XeTapLaiInfo>(new CommandDefinition(spName, parameters, commandType: System.Data.CommandType.StoredProcedure));
                    return Result<IEnumerable<XeTapLaiInfo>>.Success(result);
                }
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<XeTapLaiInfo>>.Failure(ex.Message);
            }
        }
    }
}
