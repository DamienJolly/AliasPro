using AliasPro.API.Configuration;
using AliasPro.API.Configuration.Models;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AliasPro.API.Database
{
    public abstract class BaseDao
    {
        private readonly ILogger<BaseDao> _logger;
        private readonly string _connectionString;
        private readonly IConfigurationController _configurationController;

        protected BaseDao(ILogger<BaseDao> logger, IConfigurationController configurationController)
        {
            _logger = logger;
            _configurationController = configurationController;
            IConfigurationData configData = _configurationController.ConfigurationData;
            _connectionString = configData.ConnectionString;
        }

        private async Task CreateConnection(Action<MySqlConnection> connection)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    connection(con);
                    await con.CloseAsync();
                }
            }
            catch
            {
                _logger.LogCritical("Unable to connect to database, check settings and restart Alias.");
            }
        }

        protected async Task CreateTransaction(Action<MySqlTransaction> transaction)
        {

            await CreateConnection(async connection =>
            {
                try
                {
                    using (MySqlTransaction trans = await connection.BeginTransactionAsync())
                    {
                        transaction(trans);
                        trans.Commit();
                    }
                }
                catch { }
            });
        }

        protected async Task<int> Insert(
            MySqlTransaction transaction,
            string query,
            params object[] parameters)
        {
            int id = -1;
            try
            {
                using (MySqlCommand command = transaction.Connection.CreateCommand())
                {
                    command.CommandText = query;
                    AddParameters(command.Parameters, parameters);
                    await command.ExecuteNonQueryAsync();
                    id = (int)command.LastInsertedId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"MySql Error");
                transaction.Rollback();
            }

            return id;
        }

        protected async Task Select(
            MySqlTransaction transaction,
            Action<DbDataReader> reader,
            string query,
            params object[] parameters)
        {
            try
            {
                using (MySqlCommand command = transaction.Connection.CreateCommand())
                {
                    command.CommandText = query;
                    AddParameters(command.Parameters, parameters);


                    using (DbDataReader dbReader = await command.ExecuteReaderAsync())
                    {
                        reader(dbReader);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"MySql Error");
                transaction.Rollback();
            }
        }

        private static void AddParameters(MySqlParameterCollection sqlParams, params object[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                sqlParams.AddWithValue($"@{i}", parameters[i]);
            }
        }
    }
}
