import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

function ManagementSetting({
  hourlyRate,
  setHourlyRate,
  userName,
  setUserName,
  Id,
}) {
  const navigate = useNavigate();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isRateMenuOpen, setIsRateMenuOpen] = useState(false);
  const [isUserRegisterOpen, setIsUserRegisterOpen] = useState(false);
  const [isQrMenuOpen, setIsQrMenuOpen] = useState(false); // QRコードメニューの状態
  const [qrNumber, setQrNumber] = useState("");
  const [employeeId, setEmployeeId] = useState([]);

  useEffect(() => {
    const storedEmployeeIds = localStorage.getItem("employeeIds");
    if (storedEmployeeIds) {
      setEmployeeId(JSON.parse(storedEmployeeIds));
    }
  }, []);

  const handleHourlyRateChange = async (e) => {
    e.preventDefault();
    const value = Number(e.target.value);
    if (!isNaN(value) && value >= 0) {
      setHourlyRate(value);
    } else {
      alert("正しい数字を入力してください。");
    }
    try {
      const response = await axios.put(
        "https://mainformwebapp.azurewebsites.net/api/Registration/Hourly",
        { Hourly: hourlyRate }
      );
      alert(response.data.Hourly);
    } catch (error) {
      console.error(error);
      alert("時給を更新できませんでした。");
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        `https://mainformwebapp.azurewebsites.net/api/Registration/employeeRegister/${Id}`,
        { Name: userName }
      );
      alert(`${userName}さんを追加しました`);
      const updatedEmployeeId = [...employeeId, response.data.detail];
      localStorage.setItem("employeeIds", JSON.stringify(updatedEmployeeId));
      setEmployeeId(updatedEmployeeId);
      setUserName("");
      window.location.reload();
      console.log(response.data.detail);
    } catch (error) {
      console.error(error);
    }
  };

  const handleQrgenerater = () => {
    const token = localStorage.getItem("token");
    if (!token) return null;

    const decoded = jwtDecode(token);
    const userIdFromToken = parseInt(decoded.sub, 10);

    console.log("User ID from token:", userIdFromToken, "URL ID:", Id);

    if (
      parseInt(userIdFromToken) === parseInt(Id) &&
      employeeId.includes(parseInt(qrNumber))
    ) {
      navigate(`/QRcode/${qrNumber}`);
    } else {
      console.log(employeeId, qrNumber);
      alert("登録されているIDではありません");
    }
  };

  return (
    <div>
      <div className="button" onClick={() => setIsMenuOpen(!isMenuOpen)}>
        <i></i>
        <i></i>
        <i></i>
      </div>
      <div className={`menu ${isMenuOpen ? "open" : ""}`}>
        <h2>管理設定</h2>
        <ul className="menu-list">
          <li
            key="hourly-rate"
            onClick={() => setIsRateMenuOpen(!isRateMenuOpen)}
            className={`${isRateMenuOpen ? "open-list" : "close-list"}`}
          >
            時給管理
          </li>
          {isRateMenuOpen && (
            <div className="rate-menu" key="rate-menu">
              <h4>時給設定</h4>
              <input
                type="number"
                placeholder="時給を入力"
                value={hourlyRate}
                onChange={(e) => setHourlyRate(e.target.value)}
              />
              <button onClick={handleHourlyRateChange}>設定</button>
            </div>
          )}
          <li
            key="user-registration"
            onClick={() => setIsUserRegisterOpen(!isUserRegisterOpen)}
            className={`${isUserRegisterOpen ? "open-list" : "close-list"}`}
          >
            ユーザー登録
          </li>
          {isUserRegisterOpen && (
            <div className="user-register-menu" key="user-register-menu">
              <h4>ユーザー名を入力してください</h4>
              <input
                type="text"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                placeholder="名前を入力"
              />
              <button onClick={handleRegister}>登録</button>
            </div>
          )}
          <li
            key="qr-code"
            onClick={() => setIsQrMenuOpen(!isQrMenuOpen)} // QRコードメニューのトグル
            className={`${isQrMenuOpen ? "open-list" : "close-list"}`}
          >
            QRコード生成
          </li>
          {isQrMenuOpen && (
            <div className="qr-input-section">
              <input
                type="number"
                value={qrNumber}
                onChange={(e) => setQrNumber(e.target.value)}
                placeholder="IDを入力"
              />
              <button onClick={handleQrgenerater}>生成</button>
            </div>
          )}
          {/* {isUserQrOpen && qrUrl && (
            <div style={{ marginTop: "20px" }}>
              <h4>QRコード:</h4>
              <QRCodeCanvas value={qrUrl} size={256} />
              <p>{qrUrl}</p>
            </div>
          )} */}
        </ul>
      </div>
    </div>
  );
}

export default ManagementSetting;
