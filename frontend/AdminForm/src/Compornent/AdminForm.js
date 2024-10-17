import React, { useEffect, useState } from "react";
import axios from "axios";
import "./css/AdminForm.css";
import ManagementSetting from "./ManagementSettings";
import { useParams } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

function AdminForm() {
  const [data, setData] = useState([]);
  const [userName, setUserName] = useState("");
  const [hourlyRate, setHourlyRate] = useState();
  const { id } = useParams();

  // console.log("Fetched ID:", id);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(
          `https://mainformwebapp.azurewebsites.net/api/TimeAttendance/get/${id}`
        );
        setData(response.data);
      } catch (error) {
        console.error("データ取得エラー:", error);
      }
    };

    if (id) {
      fetchData();
    }
  }, [id]);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) return null;

    const decoded = jwtDecode(token);
    const userIdFromToken = parseInt(decoded.sub, 10);

    console.log("User ID from token:", userIdFromToken, "URL ID:", id);
    if (parseInt(userIdFromToken) !== parseInt(id)) {
      window.location.href = `/admin/${userIdFromToken}`;
      alert("不正なアクセスです。");
    }
  }, [id]);

  const formatTime = (date) => {
    const hours = date.getHours().toString().padStart(2, "0");
    const minutes = date.getMinutes().toString().padStart(2, "0");
    const seconds = date.getSeconds().toString().padStart(2, "0");
    return `${hours}:${minutes}:${seconds}`;
  };

  const calculateTotalHours = (
    date,
    checkIn,
    checkOut,
    breakStart,
    breakEnd
  ) => {
    if (!checkIn || !checkOut || !date) return "0.00";

    const checkInTime = new Date(`${date.split("T")[0]}T${checkIn}`);
    const checkOutTime = new Date(`${date.split("T")[0]}T${checkOut}`);

    if (!date || typeof date !== "string") return "0.00";

    const breakStartTime = breakStart
      ? new Date(`${date.split("T")[0]}T${breakStart}`)
      : null;
    const breakEndTime = breakEnd
      ? new Date(`${date.split("T")[0]}T${breakEnd}`)
      : null;

    let totalHours = (checkOutTime - checkInTime) / (1000 * 60 * 60);

    if (breakStartTime && breakEndTime) {
      totalHours -= (breakEndTime - breakStartTime) / (1000 * 60 * 60);
    }

    return totalHours.toFixed(2);
  };

  const calculateSalary = (totalHours) => {
    return (totalHours * hourlyRate).toFixed(2);
  };

  const saveTime = async (e, employeeId, timeType) => {
    e.preventDefault();
    const newTime = prompt(`${timeType} を入力してください (HH:MM:SS):`);

    if (newTime) {
      const date = new Date();
      const parsedTime = formatTime(new Date(`1970-01-01T${newTime}`));
      const currentData = data.find((item) => item.EmployeeId === employeeId);

      // dateがnullの可能性を考慮する
      const requestData = {
        EmployeeId: employeeId,
        CheckInTime:
          timeType === "attend" ? parsedTime : currentData.CheckInTime,
        CheckOutTime:
          timeType === "leaving" ? parsedTime : currentData.CheckOutTime,
        BreakTime: timeType === "break" ? parsedTime : currentData.BreakTime,
        BreakEndTime:
          timeType === "break-end" ? parsedTime : currentData.BreakEndTime,
        Date: date.toISOString().split("T")[0] || null, // nullを許容
      };

      console.log("Sending request data:", requestData);

      try {
        const response = await axios.put(
          `https://mainformwebapp.azurewebsites.net/api/TimeAttendance/${employeeId}`,
          requestData
        );

        if (response.status === 200) {
          alert("時間が更新されました");
          window.location.reload();
        }
      } catch (error) {
        console.error(error);
        console.error("エラー詳細:", error.response.data);
        alert("時間の更新に失敗しました");
      }
    }
  };

  return (
    <div className="container">
      <div className="title-container">
        <div className="title">
          <h1>管理画面</h1>
        </div>
        <div>
          <ManagementSetting
            Id={id}
            hourlyRate={hourlyRate}
            setHourlyRate={setHourlyRate}
            userName={userName}
            setUserName={setUserName}
          />
        </div>
        <div className="total">
          <h3>Total Posts: {data.length}</h3>
        </div>
      </div>

      {data.map((item) => {
        const totalHours = calculateTotalHours(
          item.Name,
          item.Date,
          item.CheckInTime,
          item.CheckOutTime,
          item.BreakTime,
          item.BreakEndTime
        );

        return (
          <div key={item.EmployeeId} className="list">
            <h4 className="id-and-date">ID: {item.EmployeeId}</h4>
            <h4 className="name">{item.Name}</h4>
            <div className="time-list">
              <div style={{ display: "flex" }}>
                <p>出勤時間: {item.CheckInTime}</p>
                <button
                  style={{
                    marginLeft: "10px",
                    marginTop: "15px",
                    padding: "0px 10px",
                    marginBottom: "10px",
                  }}
                  onClick={(e) => saveTime(e, item.EmployeeId, "attend")}
                >
                  編集
                </button>
              </div>
              <div style={{ display: "flex" }}>
                <p>休憩開始: {item.BreakTime}</p>
                <button
                  style={{
                    marginLeft: "10px",
                    marginTop: "15px",
                    padding: "0px 10px",
                    marginBottom: "10px",
                  }}
                  onClick={(e) => saveTime(e, item.EmployeeId, "break")}
                >
                  編集
                </button>
              </div>
              <div style={{ display: "flex" }}>
                <p>休憩終了: {item.BreakEndTime}</p>
                <button
                  style={{
                    marginLeft: "10px",
                    marginTop: "15px",
                    padding: "0px 10px",
                    marginBottom: "10px",
                  }}
                  onClick={(e) => saveTime(e, item.EmployeeId, "break-end")}
                >
                  編集
                </button>
              </div>
              <div style={{ display: "flex" }}>
                <p>退勤時間: {item.CheckOutTime}</p>
                <button
                  style={{
                    marginLeft: "10px",
                    marginTop: "15px",
                    padding: "0px 10px",
                    marginBottom: "10px",
                  }}
                  onClick={(e) => saveTime(e, item.EmployeeId, "leaving")}
                >
                  編集
                </button>
              </div>
            </div>
            <div className="one-day-time">
              <p>総労働時間: {totalHours} 時間</p>
              <p>総支給額: {calculateSalary(totalHours)} 円</p>
            </div>
          </div>
        );
      })}
    </div>
  );
}

export default AdminForm;
