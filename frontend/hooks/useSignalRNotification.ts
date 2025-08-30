import { useEffect, useRef } from "react";
import { getConnection, stopConnection } from "@/lib/connectionHub";
import toast from "react-hot-toast";

// Lấy token từ localStorage (hoặc truyền vào hook nếu muốn)
function getToken() {
  if (typeof window !== "undefined") {
    return localStorage.getItem("token") || "";
  }
  return "";
}

// Định nghĩa kiểu cho notification nhận từ SignalR
type SimpleUser = {
  id: string;
  username?: string;
  fullName?: string;
  email?: string;
};

type ChatNotification = {
  id: string;
  userId: string;
  fromUserId?: string;
  type: number;
  content?: string;
  isRead: boolean;
  createdAt: string;
  user?: SimpleUser;
  fromUser?: SimpleUser;
};

export function useSignalRNotification() {
  useEffect(() => {
    const token = getToken();
    if (!token) return;
    const connection = getConnection(token);

    // Nhận lời mời kết bạn
connection.on("FriendRequestReceived", (data: any) => {
  console.log("[SignalR FE] FriendRequestReceived:", data);
  const noti = data.notification;
  // Ưu tiên tên đầy đủ, nếu không có thì dùng username, nếu không có thì dùng fromUserId
  let sender =
    noti?.fromUser?.fullName ||
    noti?.fromUser?.username ||
    noti?.fromUserId?.toString() ||
    "ai đó";
  let message =
    noti?.content ||
    `Bạn nhận được lời mời kết bạn từ ${sender}`;
  toast.success(message);
});

    // Nhận thông báo khi lời mời được chấp nhận
    connection.on("FriendRequestAccepted", (data: any) => {
      console.log("[SignalR FE] FriendRequestAccepted:", data);
      const noti = data.notification;
      let sender = noti?.fromUser?.fullName || noti?.fromUser?.username || "ai đó";
      let message = noti?.content || "Lời mời kết bạn của bạn đã được chấp nhận.";
      toast.success(`Lời mời kết bạn đã được ${sender} chấp nhận!`);
    });

    connection.start().catch(console.error);
    return () => {
      stopConnection();
    };
  }, []);
}
