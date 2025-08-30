import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5210/api",
  headers: {
    "Content-Type": "application/json"
  }
});

// Tự động thêm Authorization nếu có token
api.interceptors.request.use((config) => {
  if (typeof window !== "undefined") {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers = config.headers || {};
      config.headers["Authorization"] = `Bearer ${token}`;
    }
  }
  return config;
});

export default api;
