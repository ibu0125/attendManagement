using Microsoft.AspNetCore.Mvc;
using DotNetEnv;
using System.ComponentModel;
using MainForm.Models;
using MySql.Data.MySqlClient;

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
                string employeeRegistQuer = "INSERT INTO TimeAttendanceList (Name,CompanyId) VALUES (@Name,@CompanyId)";
                using(MySqlCommand cmd = new MySqlCommand(employeeRegistQuer, connection)) {
                    cmd.Parameters.AddWithValue("@Name", models.Name);
                    cmd.Parameters.AddWithValue("@CompanyId", id);
                    cmd.ExecuteNonQuery();
                }
                return Ok(new
                {
                    message = "登録しました"
                });
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
                    cmd.Parameters.AddWithValue("@Hourly", models.Hourly); // int型のHourlyを適切に渡す
                    cmd.Parameters.AddWithValue("@CompanyName", models.CompanyName);
                    cmd.Parameters.AddWithValue("@WorkplaceAddress", models.WorkplaceAddress);
                    cmd.Parameters.AddWithValue("@Email", models.Email);
                    cmd.Parameters.AddWithValue("@Phone", models.Phone);
                    cmd.Parameters.AddWithValue("@Password", models.Password);

                    // LONGとして取得、ここで適切な型に合わせる
                    long newCompanyId = Convert.ToInt64(cmd.ExecuteScalar() ?? 0);
                    return Ok(new
                    {
                        Message = "Registration successful",
                        CompanyId = newCompanyId
                    });
                }
            }
            catch(Exception ex) {
                return StatusCode(500, new
                {
                    message = "サーバーエラーが発生しました",
                    detail = ex.Message // デバッグ用に詳細メッセージを返す
                });
            }
        }


    }
}
