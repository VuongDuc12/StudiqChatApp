
'use client';
import React from "react";
import { useRouter } from "next/navigation";

const groups = [
  { id: "1", name: "Study Group", icon: "/group1.png" },
  { id: "2", name: "Gaming", icon: "/group2.png" },
  { id: "3", name: "Work", icon: "/group3.png" },
];


function getUserFromLocal() {
  if (typeof window !== "undefined") {
    try {
      const userStr = localStorage.getItem("user");
      if (userStr) {
        const user = JSON.parse(userStr);
        return {
          username: user.username || "User",
          avatar: user.avatar || "https://randomuser.me/api/portraits/men/21.jpg",
          status: "Trực tuyến",
          info: user.info || "#1234"
        };
      }
    } catch {}
  }
  return {
    username: "User",
    avatar: "https://randomuser.me/api/portraits/men/21.jpg",
    status: "Trực tuyến",
    info: "#1234"
  };
}

export default function SidebarGroups() {
  const router = useRouter();
  const [user, setUser] = React.useState({
    username: "User",
    avatar: "https://randomuser.me/api/portraits/men/21.jpg",
    status: "Trực tuyến",
    info: "#1234"
  });
  React.useEffect(() => {
    try {
      const userStr = localStorage.getItem("user");
      if (userStr) {
        const u = JSON.parse(userStr);
        setUser({
          username: u.username || "User",
          avatar: u.avatar || "https://randomuser.me/api/portraits/men/21.jpg",
          status: "Trực tuyến",
          info: u.info || "#1234"
        });
      }
    } catch {}
  }, []);
  const handleLogout = () => {
    if (typeof window !== "undefined") {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      document.cookie = "auth=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    }
    router.push("/auth");
  };
  return (
    <aside className="flex flex-col items-center bg-[#23272a] py-4 w-16 h-full relative">
      <div className="flex-1 flex flex-col gap-3">
        {groups.map((g) => (
          <div key={g.id} className="group relative">
            <button className="w-12 h-12 rounded-full bg-[#36393f] flex items-center justify-center hover:bg-[#5865f2] transition">
              <img src={g.icon} alt={g.name} className="w-8 h-8" />
            </button>
            <span className="absolute left-16 top-1/2 -translate-y-1/2 bg-black text-white text-xs rounded px-2 py-1 opacity-0 group-hover:opacity-100 pointer-events-none whitespace-nowrap z-10">
              {g.name}
            </span>
          </div>
        ))}
        <button className="w-12 h-12 rounded-full bg-[#36393f] flex items-center justify-center hover:bg-[#3ba55d] mt-4 transition">
        <span className="text-2xl text-white">+</span>
      </button>
      </div>
      
      {/* User status bar - fixed horizontal */}
  <div className="fixed left-0 bottom-0 w-64 flex items-center gap-3 bg-[#23272a] border-t border-[#36393f] px-3 py-2 z-50 shadow-xl">
        <div className="w-10 h-10 rounded-full bg-[#36393f] flex items-center justify-center border-2 border-[#5865f2] shadow relative">
          <img src={user.avatar} alt={user.username} className="w-9 h-9 rounded-full object-cover" />
          <span className="absolute bottom-0 right-0 w-3 h-3 bg-green-500 border-2 border-white rounded-full"></span>
        </div>
        <div className="flex flex-col flex-1 min-w-0">
          <span className="text-white text-sm font-bold truncate leading-tight">{user.username}</span>
          <span className="text-gray-400 text-xs truncate">{user.info}</span>
          <span className="text-green-400 text-xs">{user.status}</span>
        </div>
        <button className="w-8 h-8 rounded-full bg-[#36393f] flex items-center justify-center hover:bg-[#5865f2] text-gray-300 hover:text-white transition text-lg" title="Mic"><svg width="16" height="16" fill="none" viewBox="0 0 24 24"><path fill="currentColor" d="M12 15a3 3 0 0 0 3-3V7a3 3 0 1 0-6 0v5a3 3 0 0 0 3 3Zm5-3a1 1 0 1 1 2 0 7 7 0 0 1-6 6.93V21a1 1 0 1 1-2 0v-2.07A7 7 0 0 1 5 12a1 1 0 1 1 2 0 5 5 0 0 0 10 0Z"/></svg></button>
        <button className="w-8 h-8 rounded-full bg-[#36393f] flex items-center justify-center hover:bg-[#5865f2] text-gray-300 hover:text-white transition text-lg" title="Cài đặt"><svg width="16" height="16" fill="none" viewBox="0 0 24 24"><path fill="currentColor" d="M12 15a3 3 0 1 0 0-6 3 3 0 0 0 0 6Zm7.94-2.34-1.06-.18a6.97 6.97 0 0 0-.5-1.21l.6-.9a1 1 0 0 0-1.32-1.32l-.9.6a6.97 6.97 0 0 0-1.21-.5l-.18-1.06A1 1 0 0 0 15 4h-2a1 1 0 0 0-1 .88l-.18 1.06a6.97 6.97 0 0 0-1.21.5l-.9-.6A1 1 0 0 0 6.4 7.4l.6.9a6.97 6.97 0 0 0-.5 1.21l-1.06.18A1 1 0 0 0 4 9v2a1 1 0 0 0 .88 1l1.06.18a6.97 6.97 0 0 0 .5 1.21l-.6.9A1 1 0 0 0 7.4 17.6l.9-.6a6.97 6.97 0 0 0 1.21.5l.18 1.06A1 1 0 0 0 9 20h2a1 1 0 0 0 1-.88l.18-1.06a6.97 6.97 0 0 0 1.21-.5l.9.6a1 1 0 0 0 1.32-1.32l-.6-.9a6.97 6.97 0 0 0 .5-1.21l1.06-.18A1 1 0 0 0 20 15v-2a1 1 0 0 0-.88-1Z"/></svg></button>
        <button
          className="w-8 h-8 rounded-full bg-[#36393f] flex items-center justify-center hover:bg-red-500 text-gray-300 hover:text-white transition text-lg"
          title="Đăng xuất"
          onClick={handleLogout}
        >
          <svg width="16" height="16" fill="none" viewBox="0 0 24 24"><path fill="currentColor" d="M16 13v-2H7V8l-5 4 5 4v-3h9Zm3-10H5a2 2 0 0 0-2 2v6h2V5h14v14H5v-6H3v6a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V5a2 2 0 0 0-2-2Z"/></svg>
        </button>
      </div>
    </aside>
  );
}
