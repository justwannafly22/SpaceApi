using System.Data;

namespace PlanetApi.Repository.Interfaces;

public interface IDapperContext
{
    IDbConnection CreateConnection();
}
