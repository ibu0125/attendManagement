using System.ComponentModel.DataAnnotations;

namespace MainForm.Models {
    public class TimeAttendanceModels {

        public int EmployeeId {
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

        [Required]
        public DateTime? Date {
            get; set;
        } // 日付
    }
}
