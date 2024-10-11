using System.ComponentModel.DataAnnotations;

namespace MainForm.Models {
    public class TimeAttendanceModels {

        public int EmployeeId {
            get; set;
        }
        public string? Name {
            get; set;
        }

        public TimeSpan? CheckInTime {
            get; set;
        } // 出勤時間

        public TimeSpan? CheckOutTime {
            get; set;
        } // 退勤時間

        public TimeSpan? BreakTime {
            get; set;
        } // 休憩時間

        public TimeSpan? BreakEndTime {
            get; set;
        } // 休憩終了時間

        public DateTime? Date {
            get; set;
        } // 日付

        public int Hourly {
            get; set;
        }

    }
}
