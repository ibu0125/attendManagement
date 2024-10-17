using Microsoft.AspNetCore.Mvc;
using DotNetEnv;
using System.ComponentModel;
using MainForm.Models;
using MySql.Data.MySqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MainForm.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase {
        private readonly string connectionString;

        public RegistrationController() {
            connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
              ?? throw new InvalidOperationException("環境変数が設定されていません");
            Console.WriteLine(connectionString);
        }

        [HttpPut("Hourly/{id}")]
        public IActionResult HourlyRegiter([FromBody] TimeAttendanceModels models, int id) {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            try {
                connection.Open();
                string hourlyQuery = "UPDATE TimeAttendanceList SET Hourly=@Hourly WHERE CompanyId = @CompanyId";
                using(MySqlCommand cmd = new MySqlCommand(hourlyQuery, connection)) {
                    cmd.Parameters.AddWithValue("@Hourly", models.Hourly);
                    cmd.Parameters.AddWithValue("@CompanyId", id);

                    int affectedRows = cmd.ExecuteNonQuery();
                    if(affectedRows > 0) {
                        return Ok(new
                        {
                            message = "Update successful"
                        });
                    }
                    else {
                        return NotFound(new
                        {
                            message = "Record not found"
                        });
                    }
                }
            }
            catch(Exception ex) {
                return StatusCode(500, new
                {
                    message = "サーバーエラーが発生しました",
                    detail = ex.Message
                });
            }
        }

        [HttpPost("employeeRegister/{id}")]
        public IActionResult Employ([FromBody] TimeAttendanceModels models, int id) {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            try {
                connection.Open();
                string employeeRegistQuer = "INSERT INTO TimeAttendanceList (Name,CompanyId) VALUES (@Name,@CompanyId); SELECT LAST_INSERT_ID();";
                using(MySqlCommand cmd = new MySqlCommand(employeeRegistQuer, connection)) {
                    cmd.Parameters.AddWithValue("@Name", models.Name);
                    cmd.Parameters.AddWithValue("@CompanyId", id);

                    long newCompanyId = Convert.ToInt64(cmd.ExecuteScalar() ?? 0);
                    return Ok(new
                    {
                        message = "登録しました",
                        detail = newCompanyId
                    });
                }

            }
            catch(Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] CompanyElementsModels models) {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            try {
                connection.Open();
                string registerQuery = "INSERT INTO CompanyElements (Hourly, WorkplaceAddress, Email, Phone, Password, CompanyName) VALUES (@Hourly, @WorkplaceAddress, @Email, @Phone, @Password, @CompanyName); SELECT LAST_INSERT_ID();";
                using(MySqlCommand cmd = new MySqlCommand(registerQuery, connection)) {
                    cmd.Parameters.AddWithValue("@Hourly", models.Hourly);
                    cmd.Parameters.AddWithValue("@CompanyName", models.CompanyName);
                    cmd.Parameters.AddWithValue("@WorkplaceAddress", models.WorkplaceAddress);
                    cmd.Parameters.AddWithValue("@Email", models.Email);
                    cmd.Parameters.AddWithValue("@Phone", models.Phone);
                    cmd.Parameters.AddWithValue("@Password", models.Password);

                    long newCompanyId = Convert.ToInt64(cmd.ExecuteScalar() ?? 0);
                    if(newCompanyId <= 0) {
                        Console.WriteLine("Error: New company ID is 0 or negative. Insert may have failed.");
                    }
                    else {
                        Console.WriteLine($"New company ID: {newCompanyId}");
                    }
                    var token = GenerateJwtToken(newCompanyId);

                    return Ok(new
                    {
                        token,
                        Message = "Registration successful",
                        CompanyId = newCompanyId
                    });
                }
            }
            catch(Exception ex) {
                return StatusCode(500, new
                {
                    message = "サーバーエラーが発生しました",
                    detail = ex.Message
                });
            }
        }

        private string GenerateJwtToken(long id) {
            var secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");

            if(string.IsNullOrEmpty(secretKey)) {
                throw new InvalidOperationException("SECRET_KEY環境変数が設定されていません。");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(31),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
