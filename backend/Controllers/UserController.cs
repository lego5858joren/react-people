using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PeopleApi.Models;
using Microsoft.AspNetCore.Identity;




namespace PeopleApi.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly string connectionString = "Server=tcp:testserver0799.database.windows.net,1433;Initial Catalog=testdb;Persist Security Info=False;User ID=sqladmin;Password=Unreal@016;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            try
            {
                var hasher = new PasswordHasher<User>();
                string hashedPassword = hasher.HashPassword(newUser, newUser.Password);

                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("INSERT INTO Users (Username, Password) VALUES(@username, @password)", conn);
                cmd.Parameters.AddWithValue("@username", newUser.Username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);

                cmd.ExecuteNonQuery();

                return Ok("User created successfully");
            }
            catch
            {
                return StatusCode(500, $"Internal server error");
            }


        }
        [HttpPut("{id}")]
        public IActionResult UpdatePassword(int id, [FromBody] string newPassword)
        {
            try
            {
                var user = new User();
                var hasher = new PasswordHasher<User>();
                string hashedPassword = hasher.HashPassword(user, newPassword);

                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("UPDATE Users SET Password = @password WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound("User not found");
                }
                return Ok("Password updated successfully.");
            }
            catch
            {
                return StatusCode(500, $"Internal server error");
            }

        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("DELETE FROM Users WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    return NotFound("User not found");

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var users = new List<User>();
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("SELECT Id, Username FROM Users", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1)
                        // Omit Password
                    });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


    }



}
