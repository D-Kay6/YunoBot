using IDal.Interfaces.Database;
using IDal.Structs.Database;
using System;
using System.Data;
using Dal.Json;
using MySql.Data.MySqlClient;

namespace Dal.MySql
{
    public class AutoChannel : IAutoChannel
    {
        private Connection _connection;

        public AutoChannel()
        {
            var database = new Database();
            _connection = database.Read();
        }

        public bool IsAutoChannel(ulong serverId, string name)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("IsAutoChannel", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    return (long)command.ExecuteScalar() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not check auto channel. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool IsPermaChannel(ulong serverId, string name)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("IsPermaChannel", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    return (long)command.ExecuteScalar() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not check perma channel. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }
        
        public bool IsGeneratedChannel(ulong serverId, ulong channelId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("IsGeneratedChannel", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@ChannelId", channelId);
                    con.Open();
                    return (long)command.ExecuteScalar() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not check generated channel. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool AddGeneratedChannel(ulong serverId, ulong channelId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("AddGeneratedChannel", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@ChannelId", channelId);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not add generated channel. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool RemoveGeneratedChannel(ulong serverId, ulong channelId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("RemoveGeneratedChannel", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@ChannelId", channelId);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not remove generated channel. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool ClearGeneratedChannels(ulong serverId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("ClearGeneratedChannels", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not clear generated channels. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool SetAutoPrefix(ulong serverId, string prefix)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("SetAutoChannelPrefix", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Prefix", prefix);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not set auto channel prefix. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool SetAutoName(ulong serverId, string name)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("SetAutoChannelName", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not set auto channel name. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool SetPermaPrefix(ulong serverId, string prefix)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("SetPermaChannelPrefix", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Prefix", prefix);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not set perma channel prefix. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool SetPermaName(ulong serverId, string name)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("SetPermaChannelName", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not set perma channel name. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public ChannelData GetData(ulong serverId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("GetChannelInfo", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    con.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var autoPrefix = reader.GetString("AcPrefix");
                            var autoName = reader.GetString("AcName");
                            var permaPrefix = reader.GetString("PcPrefix");
                            var permaName = reader.GetString("PcName");
                            return new ChannelData(autoPrefix, autoName, permaPrefix, permaName);
                        }
                    }
                    return null;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not get channel data. {e}");
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
