using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PeopleApi.Models;
using System.Net.Cache;

namespace PeopleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly string connectionString = "Server=tcp:testserver0799.database.windows.net,1433;Initial Catalog=testdb;Persist Security Info=False;User ID=sqladmin;Password=Unreal@016;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        [HttpGet]
        public IActionResult GetPeople()
        {
            try
            {
                var people = new List<Person>();
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("SELECT Id, FirstName, Age, Affiliation, Bio, ImageUrl FROM People", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    people.Add(new Person
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        Affiliation = reader.GetString(3),
                        Bio = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        ImageUrl = reader.IsDBNull(5) ? "" : reader.GetString(5)
                    });
                }

                return Ok(people);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddPerson([FromBody] Person person)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("INSERT INTO People (FirstName, Age, Affiliation,Bio,ImageUrl) VALUES (@fn, @age,@aff,@bio,@img)", conn);
                cmd.Parameters.AddWithValue("@fn", person.FirstName);
                cmd.Parameters.AddWithValue("@age", person.Age);
                cmd.Parameters.AddWithValue("@aff", person.Affiliation);
                cmd.Parameters.AddWithValue("@bio", person.Bio);
                cmd.Parameters.AddWithValue("@img", person.ImageUrl);

                cmd.ExecuteNonQuery();

                return Ok("Person added.");
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("DELETE FROM People WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound($"No person with ID {id} was found.");

                return Ok("Person deleted.");
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpDelete("name/{firstName}")]
        public IActionResult DeletePersonByName(string firstName)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("DELETE FROM People WHERE FirstName = @fn", conn);
                cmd.Parameters.AddWithValue("@fn", firstName);
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound($"No person with the name '{firstName}' was found.");
                }

                return Ok($"Person with the name '{firstName}' was deleted.");
            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpGet("name/{firstName}")]

        public IActionResult GetPersonByName(string firstName)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("SELECT Id, FirstName, Age, Affiliation,Bio,ImageUrl FROM People WHERE FirstName = @fn", conn);
                cmd.Parameters.AddWithValue("@fn", firstName);

                using var reader = cmd.ExecuteReader();

                var people = new List<Person>();
                while (reader.Read())
                {
                    people.Add(new Person
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        Affiliation = reader.GetString(3),
                        Bio = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        ImageUrl = reader.IsDBNull(5) ? "" : reader.GetString(5)

                    });
                }
                if(people.Count == 0)
                    return NotFound($"No person with the name '{firstName}' was found.");

                return Ok(people);


            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePerson(int id, [FromBody] Person updatedPerson)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand(@"
                               UPDATE People
                               SET FirstName = @fn, Age  = @age, Affiliation = @aff, Bio =@bio, ImageUrl = @img
                               WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@fn", updatedPerson.FirstName);
                cmd.Parameters.AddWithValue("@age", updatedPerson.Age);
                cmd.Parameters.AddWithValue("@aff", updatedPerson.Affiliation);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@bio", updatedPerson.Bio);
                cmd.Parameters.AddWithValue("@img", updatedPerson.ImageUrl);

                int rowsAffected = cmd.ExecuteNonQuery();

                if(rowsAffected == 0)
                {
                    return NotFound($"No person with ID {id} found.");
                }
                else
                {
                    return Ok("Person with ID {id} was updated.");
                }


            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetPersonById(int id)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using var cmd = new SqlCommand("SELECT Id, FirstName, Age, Affiliation, Bio, ImageUrl FROM People WHERE Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var person = new Person
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        Affiliation = reader.GetString(3),
                        Bio = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        ImageUrl = reader.IsDBNull(5) ? "" : reader.GetString(5)
                    };
                    return Ok(person);
                }
                else
                {
                    return NotFound($"No person with ID {id} was found.");
                }

            }
            catch (SqlException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
       
    }
}


