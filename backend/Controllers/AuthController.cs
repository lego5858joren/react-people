using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PeopleApi.Models;


namespace PeopleApi.Controllers
{

    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly string connectionString = "Server=tcp:testserver0799.database.windows.net,1433;Initial Catalog=testdb;Persist Security Info=False;User ID=sqladmin;Password=Unreal@016;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("SELECT Id, Username, Password FROM Users WHERE Username = @username", conn);
                cmd.Parameters.AddWithValue("@username", loginUser.Username);

                using var reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return Unauthorized("Invalid username or password");

                string hashedPasswordFromDb = reader.GetString(2);

                var hasher = new PasswordHasher<User>();
                var verificationResult = hasher.VerifyHashedPassword(loginUser, hashedPasswordFromDb, loginUser.Password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    return Ok("Login successful");
                }

                return Unauthorized("Invalid username or password");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
