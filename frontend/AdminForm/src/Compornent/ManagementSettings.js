import React, { useState } from "react";
import axios from "axios";

function ManagementSetting({
  hourlyRate,
  setHourlyRate,
  userName,
  setUserName,
}) {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isRateMenuOpen, setIsRateMenuOpen] = useState(false);
  const [isUserRegisterOpen, setIsUserRegisterOpen] = useState(false);

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
        "http://localhost:5220/api/Registration/Hourly",
        { Hourly: hourlyRate }
      );
      alert(response.data.Hourly);
      console.log(response.data);
    } catch (error) {
      console.error(error);
      alert("時給を更新できませんでした。");
      return;
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(
        "http://localhost:5220/api/Registration/register",
        {
          Name: userName,
        }
      );
      console.log(response.data);
      alert(`${userName}さんを追加しました`);
      toggleUserRegisterMenu();
      setUserName("");
    } catch (error) {
      console.error(error);
    }
  };

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const toggleRateMenu = () => {
    setIsRateMenuOpen(!isRateMenuOpen);
  };

  const toggleUserRegisterMenu = () => {
    setIsUserRegisterOpen(!isUserRegisterOpen);
  };

  return (
    <div>
      <div className="button" onClick={toggleMenu}>
        <i></i>
        <i></i>
        <i></i>
      </div>
      <div className={`menu ${isMenuOpen ? "open" : ""}`}>
        <h2>管理設定</h2>
        <ul className="menu-list">
          <li
            key="hourly-rate"
            onClick={toggleRateMenu}
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
              <button
                onClick={(e) => {
                  alert(`時給が設定されました: ${hourlyRate} 円`);
                  handleHourlyRateChange(e);
                }}
              >
                設定
              </button>
            </div>
          )}
          <li
            key="user-registration"
            onClick={toggleUserRegisterMenu}
            className={`${isUserRegisterOpen ? "open-list" : "close-list"}`}
          >
            ユーザー登録
          </li>
          {isUserRegisterOpen && (
            <div className="user-register-menu" key="user-register-menu">
              <h4>ユーザー名を入力してください</h4>
              <input
                key={"register"}
                type="text"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                placeholder="名前を入力"
              />
              <button key={"register-button"} onClick={handleRegister}>
                登録
              </button>
            </div>
          )}
        </ul>
      </div>
    </div>
  );
}

export default ManagementSetting;
