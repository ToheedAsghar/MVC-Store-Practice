using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Common;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Models
{
    public class UserRepository
    {
        public string? connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=UserDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public bool Login(User user)
        {
            bool retVal = false;

            // code to verify the user name and password
            string? Query = @"select * from Users where Username = @un and Password = @pswd; ";
            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new(Query, connection))
                {
                    cmd.Parameters.Add("@un", SqlDbType.VarChar, 32).Value = user.Username;
                    cmd.Parameters.Add("@pswd", SqlDbType.VarChar, 32).Value = user.Password;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        retVal = reader.HasRows;
                    }
                }
            }

            return retVal;
        }

        public User GetUser(int id)
        {
            using(SqlConnection conn = new(connectionString))
            {
                conn.Open();
                string? Query = @"select * from Users where Id = @id; ";
                using(SqlCommand cmd = new(Query, conn))
                {
                    SqlParameter Parameter = cmd.Parameters.Add("@id", SqlDbType.Int);
                    Parameter.Value = id;

                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        User user;
                        if (reader.Read())
                        {
                            string? username = reader.IsDBNull(1) ? "UnKnown" : reader[1].ToString();
                            string? password = reader.IsDBNull(2) ? "Unknown" : reader[2].ToString();
                            user = new(id, username, password);
                            return user;
                        }
                        // user not registered
                        return new User(id, "No User Registered!", "none");
                    }
                    
                }
            }
        }

        public bool Register(User user)
        {

			using (SqlConnection connection = new(connectionString))
            {
                connection.Open();
				String? Query = @"Insert into Users ( Username , Password ) values ( @username, @password )";
                using(SqlCommand cmd = new (Query, connection))
                {
                    cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = user.Username?.Trim() ??  "None";
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = user.Password?.Trim() ?? "Not Set";

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected == 1;
                }
            }
        }
    }
}
