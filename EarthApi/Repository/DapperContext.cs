using PlanetApi.Repository.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace PlanetApi.Repository;

public class DapperContext : IDapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connString = _configuration.GetConnectionString("sqlConnection")!;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connString);
}
