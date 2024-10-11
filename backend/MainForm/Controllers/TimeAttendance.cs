using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MainForm.Models;
using DotNetEnv;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;


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

        [HttpGet("get")]
        public IActionResult TimeAttendanceGet() {
            List<TimeAttendanceModels> lists = new List<TimeAttendanceModels>();
            using MySqlConnection connection = new MySqlConnection(connectionString);
            try {
                connection.Open();
                string getQuery = "SELECT * FROM TimeAttendanceList";
                using(MySqlCommand cmd = new MySqlCommand(getQuery, connection)) {
                    using(MySqlDataReader reader = cmd.ExecuteReader()) {
                        while(reader.Read()) {
                            var list = new TimeAttendanceModels
                            {
                                Name = reader.IsDBNull("Name") ? default(string) : reader.GetString("Name"),
                                EmployeeId = reader.GetInt32("EmployeeId"),
                                CheckInTime = reader.IsDBNull("CheckInTime") ? default(TimeSpan) : reader.GetTimeSpan("CheckInTime"),
                                CheckOutTime = reader.IsDBNull("CheckOutTime") ? default(TimeSpan) : reader.GetTimeSpan("CheckOutTime"),
                                BreakTime = reader.IsDBNull("BreakTime") ? default(TimeSpan) : reader.GetTimeSpan("BreakTime"),
                                BreakEndTime = reader.IsDBNull("BreakEndTime") ? default(TimeSpan) : reader.GetTimeSpan("BreakEndTime"),
                                Date = reader.IsDBNull("Date") ? (DateTime?)null : reader.GetDateTime("Date"),
                            };
                            lists.Add(list);
                        }
                    }
                }
                return Ok(lists);
            }
            catch(Exception ex) {
                return StatusCode(500, $"エラー: {ex.Message}"); // エラーメッセージを返す
            }
        }

        [HttpPut("{id}")]
        public IActionResult TimeUpdate(int id, [FromBody] TimeAttendanceModels info) {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            try {
                connection.Open();
                string putQuery = @"UPDATE TimeAttendanceList SET CheckInTime = @CheckInTime, CheckOutTime = @CheckOutTime, BreakTime = @BreakTime, BreakEndTime = @BreakEndTime WHERE EmployeeId = @EmployeeId";
                using(MySqlCommand command = new MySqlCommand(putQuery, connection)) {
                    command.Parameters.AddWithValue("@CheckInTime", info.CheckInTime);
                    command.Parameters.AddWithValue("@CheckOutTime", info.CheckOutTime);
                    command.Parameters.AddWithValue("@BreakTime", info.BreakTime);
                    command.Parameters.AddWithValue("@BreakEndTime", info.BreakEndTime);
                    command.Parameters.AddWithValue("@EmployeeId", id);

                    int affectedRows = command.ExecuteNonQuery();

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
                return StatusCode(500, ex.Message);
            }
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
                    cmd.Parameters.AddWithValue("@Date", info.Date.HasValue ? (object)info.Date.Value : DBNull.Value);
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
