using System;
using System.Data;
using Dal.Json;
using IDal.Interfaces.Database;
using IDal.Structs.Database;
using MySql.Data.MySqlClient;

namespace Dal.MySql
{
    public class AutoRole : IAutoRole
    {
        private Connection _connection;

        public AutoRole()
        {
            var database = new Database();
            _connection = database.Read();
        }

        public bool IsAutoRole(ulong serverId, string name)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("IsAutoRole", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    return (long)command.ExecuteScalar() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not check auto role. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool IsPermaRole(ulong serverId, string name)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("IsPermaRole", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Name", name);
                    con.Open();
                    return (long)command.ExecuteScalar() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not check perma role. {e}");
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
                    var command = new MySqlCommand("SetAutoRolePrefix", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Prefix", prefix);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not set auto role prefix. {e}");
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
                    var command = new MySqlCommand("SetPermaRolePrefix", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@Prefix", prefix);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not set perma role prefix. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public RoleData GetData(ulong serverId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("GetRoleInfo", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    con.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var autoPrefix = reader.GetString("ArPrefix");
                            var permaPrefix = reader.GetString("PrPrefix");
                            return new RoleData(autoPrefix, permaPrefix);
                        }
                    }
                    return null;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not get role data. {e}");
                    return null;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool IsRoleIgnore(ulong serverId, ulong playerId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("IsRoleIgnore", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    con.Open();
                    return (long)command.ExecuteScalar() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not check role ignore. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool AddRoleIgnore(ulong serverId, ulong playerId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("AddRoleIgnore", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not add role ignore. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public bool RemoveRoleIgnore(ulong serverId, ulong playerId)
        {
            using (var con = _connection.CreateConnection())
            {
                try
                {
                    var command = new MySqlCommand("RemoveRoleIgnore", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ServerId", serverId);
                    command.Parameters.AddWithValue("@PlayerId", playerId);
                    con.Open();
                    return command.ExecuteNonQuery() > 0;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"Could not remove role ignore. {e}");
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}
