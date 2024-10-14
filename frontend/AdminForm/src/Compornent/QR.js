import React, { useEffect } from "react";
import { QRCodeCanvas } from "qrcode.react";
import { useParams } from "react-router-dom";

function QR({ QrNumber }) {
  const { id } = useParams();
  const [qrUrl, setQrUrl] = React.useState(null);

  useEffect(() => {
    if (id) {
      const url = `http://localhost:3000/${id}`; // IDを含むURLを生成
      setQrUrl(url);
    } else {
      alert("IDを入力してください。");
    }
  }, [id]);

  return (
    <div>
      <h1>QR Code Generator</h1>
      <div style={{ marginTop: "20px" }}>
        <QRCodeCanvas value={qrUrl} size={256} />
        <p>{qrUrl}</p>
      </div>
    </div>
  );
}

export default QR;
