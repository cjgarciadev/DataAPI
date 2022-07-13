using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace DataAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class DataController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DataController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{databaseId}/data/{tableName}")]
        public async Task<JsonResult> GetTableData(string databaseId, string tableName)
        {
            string connectionString = _configuration.GetConnectionString("DataAPIServer") + "Database=" + databaseId;
            string query = $"SELECT * FROM {databaseId}.{tableName}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        using (DataTable tableData = new DataTable())
                        {
                            tableData.Load(reader);
                            return new JsonResult(tableData);
                        }
                    }
                }
            }
        }
    }
}