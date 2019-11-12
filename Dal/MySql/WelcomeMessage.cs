using Dal.Json;
using IDal.Interfaces.Database;
using IDal.Structs.Database;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Dal.MySql
{
    public class WelcomeMessage : IWelcomeMessage
    {
        private Connection _connection;

        public WelcomeMessage()
        {
            var database = new Database();
            _connection = database.Read();
        }

        public bool Enable(ulong serverId, ulong channelId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("EnableWelcomeMessage", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@ChannelId", channelId);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not enable welcome message. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool Disable(ulong serverId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("DisableWelcomeMessage", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not disable welcome message. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool UseImage(ulong serverId, bool value)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("UseWelcomeImage", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Value", value);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not set usage welcome image. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool SetWelcomeMessage(ulong serverId, string message)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("SetWelcomeMessage", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Message", message);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not set welcome message. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public WelcomeData GetWelcomeMessage(ulong serverId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("GetWelcomeSettings", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    con.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var channelId = reader.IsDBNull(reader.GetOrdinal("ChannelId")) ? (ulong?)null : reader.GetUInt64("ChannelId");
                            var useImage = reader.GetBoolean("UseImage");
                            var message = reader.GetString("Message");
                            return new WelcomeData(channelId, useImage, message);
                        }
                    }
                    return null;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not get welcome message. {e}");
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
