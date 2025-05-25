import React, { useState, useEffect } from "react";
import "../assets/Notification.css";

const Notification = ({ message }) => {
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    if (message) {
      setVisible(true);

      const timer = setTimeout(() => {
        setVisible(false);
      }, 4000);

      return () => clearTimeout(timer);
    }
  }, [message]);

  return (
    <div className={`notification ${visible ? "show" : ""}`}>{message}</div>
  );
};

export default Notification;
