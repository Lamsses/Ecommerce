using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Configuration;

namespace EcommerceLibrary.DataAccess;

public class SqlDataAccess : ISqlDataAccess, IDisposable
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public SqlDataAccess()
    {
        
    }

    public string GetConnectionString(string connectionString)
    {
        return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
    }
    public async Task<List<T>> Loaddata<T, U>(string storedProcedure, U parameters, string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using IDbConnection connection = new SqlConnection(connectionString);

        var rows = await connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure);

        return rows.ToList();
    }

    public async Task<List<T>> Loaddata<T>(string storedProcedure, string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using IDbConnection connection = new SqlConnection(connectionString);

        var rows = await connection.QueryAsync<T>(storedProcedure,
            commandType: CommandType.StoredProcedure);

        return rows.ToList();
    }

    public async Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName)

    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using IDbConnection connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    private IDbConnection _connection;
    private IDbTransaction _transaction;

    public void StartTransaction(string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }

    public async Task<List<T>> LoaddataInTransaction<T, U>(string storedProcedure, U parameters)
    {
        var rows = await _connection.QueryAsync<T>(storedProcedure, parameters,
            commandType: CommandType.StoredProcedure,
            transaction: _transaction
        );

        return rows.ToList();
    }

    public async Task<List<T>> LoaddataInTransaction<T>(string storedProcedure)
    {
        var rows = await _connection.QueryAsync<T>(storedProcedure,
            commandType: CommandType.StoredProcedure, transaction: _transaction);

        return rows.ToList();
    }

    public async Task SaveDataInTransaction<T>(string storedProcedure, T parameters)

    {
        await _connection.ExecuteAsync(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: _transaction
        );
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
        _connection?.Close();
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
        _connection?.Close();
    }

    public void Dispose()
    {
        CommitTransaction();
    }
}