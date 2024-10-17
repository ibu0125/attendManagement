import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function CompanyRegister() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    CompanyName: "",
    Hourly: "",
    WorkplaceAddress: "",
    Email: "",
    Phone: "",
    Password: "",
  });

  useEffect(() => {
    localStorage.removeItem("token");
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post(
        "https://mainformwebapp.azurewebsites.net/api/Registration/register",
        formData
      );
      console.log("登録成功:", response.data.token);
      const companyId = response.data.CompanyId;
      console.log(companyId);
      if (response.data.token) {
        localStorage.setItem("token", response.data.token);
        navigate(`/admin/${companyId}`);
      } else {
        alert("登録に失敗しました。");
        navigate("/register");
        localStorage.removeItem("token");
      }
    } catch (error) {
      if (error.response) {
        // サーバーからのレスポンスがあった場合、エラーメッセージを出力
        console.error("登録エラー:", error.response.data);
      } else {
        console.error("登録エラー:", error);
      }
    }
  };

  return (
    <div>
      <h2>会社登録</h2>
      <form onSubmit={handleSubmit}>
        <label>会社名:</label>
        <input
          type="text"
          name="CompanyName"
          value={formData.CompanyName}
          onChange={handleChange}
          required
        />
        <label>時給:</label>
        <input
          type="number"
          name="Hourly"
          value={formData.Hourly}
          onChange={handleChange}
          required
        />
        <label>勤務先住所:</label>
        <input
          type="text"
          name="WorkplaceAddress"
          value={formData.WorkplaceAddress}
          onChange={handleChange}
          required
        />
        <label>メールアドレス:</label>
        <input
          type="email"
          name="Email"
          value={formData.Email}
          onChange={handleChange}
          required
        />
        <label>電話番号:</label>
        <input
          type="tel"
          name="Phone"
          value={formData.Phone}
          onChange={handleChange}
        />
        <label>パスワード:</label>
        <input
          type="password"
          name="Password"
          value={formData.Password}
          onChange={handleChange}
          required
        />
        <button type="submit">登録</button>
      </form>
    </div>
  );
}

export default CompanyRegister;
