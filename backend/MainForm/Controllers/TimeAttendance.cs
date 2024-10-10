using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MainForm.Models;
using DotNetEnv;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace MainForm.Controllers {
    [ApiController]
    [Route("api/[Controller]")]
    public class TimeAttendance : ControllerBase {
        private readonly string connectionString;
        public TimeAttendance() {
            connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
             ?? throw new InvalidOperationException("環境変数が設定されていません");
            Console.WriteLine(connectionString);
        }
        [HttpPost("post")]
        public IActionResult TimeAttendancePost([FromBody] TimeAttendanceModels info) {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            try {
                connection.Open();
                string postQuery = @"
                    INSERT INTO timeattendancelist (EmployeeId, Name, CheckInTime, CheckOutTime, BreakTime, BreadEndTime, Date)
                    VALUES (@EmployeeId, @Name, @CheckInTime, @CheckOutTime, @BreakTime, @BreadEndTime, @Date);
                ";
                using(MySqlCommand cmd = new MySqlCommand(postQuery, connection)) {
                    cmd.Parameters.AddWithValue("@EmployeeId", info.EmployeeId);
                    cmd.Parameters.AddWithValue("@Name", info.Name);
                    cmd.Parameters.AddWithValue("@CheckInTime", info.CheckInTime);
                    cmd.Parameters.AddWithValue("@CheckOutTime", info.CheckOutTime);
                    cmd.Parameters.AddWithValue("@BreakTime", info.BreakTime);
                    cmd.Parameters.AddWithValue("@BreadEndTime", info.BreadEndTime);
                    cmd.Parameters.AddWithValue("@Date", info.Date);

                    cmd.ExecuteNonQuery();
                };
                return Ok(new
                {
                    message = "記録しました",
                    data = info
                });
            }
            catch(Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
