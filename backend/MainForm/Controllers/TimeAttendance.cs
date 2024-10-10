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
            if(info == null) {
                return BadRequest("リクエストボディが空です。");
            }

            using MySqlConnection connection = new MySqlConnection(connectionString);
            try {
                connection.Open();
                string postQuery = @"
            INSERT INTO timeattendancelist (EmployeeId, CheckInTime, CheckOutTime, BreakTime, BreakEndTime, Date)
            VALUES (@EmployeeId, @CheckInTime, @CheckOutTime, @BreakTime, @BreakEndTime, @Date);
        ";

                using(MySqlCommand cmd = new MySqlCommand(postQuery, connection)) {
                    cmd.Parameters.AddWithValue("@EmployeeId", info.EmployeeId);
                    cmd.Parameters.AddWithValue("@CheckInTime", info.CheckInTime.HasValue ? (object)info.CheckInTime.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@CheckOutTime", info.CheckOutTime.HasValue ? (object)info.CheckOutTime.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@BreakTime", info.BreakTime.HasValue ? (object)info.BreakTime.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@BreakEndTime", info.BreakEndTime.HasValue ? (object)info.BreakEndTime.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Date", info.Date);

                    cmd.ExecuteNonQuery();
                }

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
