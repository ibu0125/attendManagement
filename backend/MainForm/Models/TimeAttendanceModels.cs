namespace MainForm.Models {
    public class TimeAttendanceModels {
        public int EmployeeId {
            get; set;
        }
        public string? Name {
            get; set;
        }
        public DateTime CheckInTime {
            get; set;
        }
        public DateTime CheckOutTime {
            get; set;
        }
        public DateTime BreakTime {
            get; set;
        }
        public DateTime BreadEndTime {
            get; set;
        }
        public DateTime Date {
            get; set;
        }
    }
}
