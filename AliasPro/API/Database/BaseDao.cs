using AliasPro.API.Configuration;
using AliasPro.API.Configuration.Models;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace AliasPro.API.Database
{
    public abstract class BaseDao
    {
        private readonly string _connectionString;
        private readonly Action<object> _errorPrinter;
        private readonly IConfigurationController _configurationController;

        protected BaseDao(IConfigurationController configurationController)
        {
            _configurationController = configurationController;
            _errorPrinter = (text) =>
            {
                Console.WriteLine("Caught error: " + text);
            };

            IConfigurationData configData = _configurationController.ConfigurationData;
            _connectionString = configData.ConnectionString;
        }

        private async Task CreateConnection(Action<MySqlConnection> connection)
        {
            using (MySqlConnection con = new MySqlConnection(_connectionString))
            {
                await con.OpenAsync();
                connection(con);
                await con.CloseAsync();
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
                _errorPrinter(ex);
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
                _errorPrinter(ex);
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
