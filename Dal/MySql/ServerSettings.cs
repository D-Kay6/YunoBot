using Dal.Json;
using IDal.Interfaces.Database;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using IDal.Structs.Localization;

namespace Dal.MySql
{
    public class ServerSettings : IServerSettings
    {
        private Connection _connection;

        public ServerSettings()
        {
            var database = new Database();
            _connection = database.Read();
        }

        public bool RegisterServer(ulong serverId, string name)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("RegisterServer", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Problem registering the server. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool UpdateServer(ulong serverId, string name)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("UpdateServer", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Problem updating the server. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool DeleteServer(ulong serverId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("DeleteServer", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Problem deleting the server. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool SetLanguage(ulong serverId, Language language)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("SetLanguage", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Language", language.ToString());
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Problem setting the language. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public Language GetLanguage(ulong serverId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("GetCommandPrefix", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    con.Open();
                    Language language;
                    Enum.TryParse((string)command.ExecuteScalar(), out language);
                    return language;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Problem getting the command prefix. {e}");
                    return default;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool SetCommandPrefix(ulong serverId, string prefix)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("SetCommandPrefix", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Prefix", prefix);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Problem setting the command prefix. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public string GetCommandPrefix(ulong serverId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("GetCommandPrefix", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    con.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return reader.GetString("Prefix");
                        }
                    }

                    return null;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Problem getting the command prefix. {e}");
                    return null;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
