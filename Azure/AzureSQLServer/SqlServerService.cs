using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AzureSQLServer;

/// <summary>
/// Service for connecting to and interacting with Azure SQL Server.
/// </summary>
public class SqlServerService
{
    private readonly string _connectionString;
    private readonly ILogger<SqlServerService> _logger;
    private readonly bool _useAzureAdAuthentication;

    /// <summary>
    /// Initializes a new instance of the SqlServerService class.
    /// </summary>
    /// <param name="configuration">Configuration instance to retrieve connection settings.</param>
    /// <param name="logger">Logger instance for logging operations.</param>
    /// <exception cref="InvalidOperationException">Thrown when connection string or server settings are not configured.</exception>
    public SqlServerService(IConfiguration configuration, ILogger<SqlServerService> logger)
    {
        _logger = logger;

        // Check if using Azure AD authentication or connection string
        _useAzureAdAuthentication = configuration.GetValue<bool>("AzureSQL:UseAzureAdAuthentication", false);

        if (_useAzureAdAuthentication)
        {
            string? serverName = configuration["AzureSQL:ServerName"];
            string? databaseName = configuration["AzureSQL:DatabaseName"];

            if (string.IsNullOrWhiteSpace(serverName) || string.IsNullOrWhiteSpace(databaseName))
            {
                throw new InvalidOperationException("Azure SQL Server name and database name must be configured in appsettings.json when using Azure AD authentication");
            }

            // Build connection string for Azure AD authentication
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = serverName,
                InitialCatalog = databaseName,
                Authentication = SqlAuthenticationMethod.ActiveDirectoryDefault,
                Encrypt = true,
                TrustServerCertificate = false
            };

            _connectionString = builder.ConnectionString;
        }
        else
        {
            // Use connection string from configuration
            string? connectionString = configuration["AzureSQL:ConnectionString"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Azure SQL connection string is not configured in appsettings.json");
            }

            _connectionString = connectionString;
        }
    }

    /// <summary>
    /// Creates and opens a SQL connection.
    /// </summary>
    /// <returns>An open SqlConnection instance.</returns>
    public async Task<SqlConnection> GetConnectionAsync()
    {
        SqlConnection connection = new SqlConnection(_connectionString);

        if (_useAzureAdAuthentication)
        {
            // Use DefaultAzureCredential for Azure AD authentication
            DefaultAzureCredential credential = new DefaultAzureCredential();
            Azure.Core.AccessToken token = await credential.GetTokenAsync(
                new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" }));

            connection.AccessToken = token.Token;
        }

        await connection.OpenAsync();
        _logger.LogInformation("Successfully opened connection to Azure SQL Server");
        
        return connection;
    }

    /// <summary>
    /// Executes a SQL query and returns the results as a DataTable.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional SQL parameters for the query.</param>
    /// <returns>A DataTable containing the query results.</returns>
    public async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[]? parameters = null)
    {
        DataTable dataTable = new DataTable();

        try
        {
            using (SqlConnection connection = await GetConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                }
            }

            _logger.LogInformation("Successfully executed query. Rows returned: {RowCount}", dataTable.Rows.Count);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error executing query: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing query: {ErrorMessage}", ex.Message);
            throw;
        }

        return dataTable;
    }

    /// <summary>
    /// Executes a SQL command (INSERT, UPDATE, DELETE) and returns the number of rows affected.
    /// </summary>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="parameters">Optional SQL parameters for the command.</param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[]? parameters = null)
    {
        int rowsAffected = 0;

        try
        {
            using (SqlConnection connection = await GetConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }

            _logger.LogInformation("Successfully executed command. Rows affected: {RowsAffected}", rowsAffected);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error executing command: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing command: {ErrorMessage}", ex.Message);
            throw;
        }

        return rowsAffected;
    }

    /// <summary>
    /// Executes a SQL query and returns a scalar value.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional SQL parameters for the query.</param>
    /// <returns>The scalar value result.</returns>
    public async Task<object?> ExecuteScalarAsync(string query, SqlParameter[]? parameters = null)
    {
        object? result = null;

        try
        {
            using (SqlConnection connection = await GetConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    result = await command.ExecuteScalarAsync();
                }
            }

            _logger.LogInformation("Successfully executed scalar query");
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error executing scalar query: {ErrorMessage}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing scalar query: {ErrorMessage}", ex.Message);
            throw;
        }

        return result;
    }

    /// <summary>
    /// Tests the connection to Azure SQL Server.
    /// </summary>
    /// <returns>True if the connection is successful, otherwise false.</returns>
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using (SqlConnection connection = await GetConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("SELECT 1", connection))
                {
                    object? result = await command.ExecuteScalarAsync();
                    return result != null;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Connection test failed: {ErrorMessage}", ex.Message);
            return false;
        }
    }
}

