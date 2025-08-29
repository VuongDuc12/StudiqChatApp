
'use client';
import React from "react";
import { FiUser, FiGift, FiShoppingCart, FiPlus, FiX } from "react-icons/fi";

const dms = [
  { id: "1", username: ">>VTH<<", avatar: "https://randomuser.me/api/portraits/men/11.jpg", discord: true },
  { id: "2", username: "ManhHungdz", avatar: "https://randomuser.me/api/portraits/men/12.jpg", online: true },
  { id: "3", username: "dangkhuong0402", avatar: "https://randomuser.me/api/portraits/men/13.jpg", online: false },
  { id: "4", username: "khuongm152", avatar: "https://randomuser.me/api/portraits/men/14.jpg", online: false },
  { id: "5", username: "khánh dep trai vcll", avatar: "https://randomuser.me/api/portraits/men/15.jpg", online: false },
  { id: "6", username: "Khánh đẹp trai vcl", avatar: "https://randomuser.me/api/portraits/men/16.jpg", online: true},
  { id: "7", username: "Noobie Cheesed Cat", avatar: "https://randomuser.me/api/portraits/men/17.jpg", online: false },
];

const defaultAvatar = "https://ui-avatars.com/api/?name=User&background=5865f2&color=fff&rounded=true&size=64";

interface SidebarFriendsProps {
  onSelectDM: (id: string) => void;
  onMenuClick?: (menu: string) => void;
}

export default function SidebarFriends({ onSelectDM, onMenuClick }: SidebarFriendsProps) {
  const [search, setSearch] = React.useState("");
  const handleMenu = (menu: string) => {
    if (onMenuClick) onMenuClick(menu);
  };
  return (
    <aside className="w-60 bg-[#23272a] h-full flex flex-col border-r border-[#23272a]">
      {/* Search */}
      <div className="p-3">
        <input
          type="text"
          value={search}
          onChange={e => setSearch(e.target.value)}
          placeholder="Tìm hoặc bắt đầu cuộc trò chuyện"
          className="w-full px-3 py-2 rounded bg-[#1e1f22] text-sm text-white placeholder-gray-400 focus:outline-none"
        />
      </div>
      {/* Menu */}
      <div className="flex flex-col gap-1 px-2 pb-2 font-inter">
        <button className="flex items-center gap-3 px-4 py-2 rounded-full text-gray-200 hover:bg-[#393c41] transition font-semibold text-base group" onClick={() => handleMenu("friends")}> 
          <FiUser className="w-5 h-5" />
          <span>Bạn bè</span>
        </button>
        <button className="flex items-center gap-3 px-4 py-2 rounded-full text-gray-200 hover:bg-[#393c41] transition font-semibold text-base group" onClick={() => handleMenu("nitro")}> 
          <FiGift className="w-5 h-5" />
          <span>Nitro</span>
        </button>
        <button className="flex items-center gap-3 px-4 py-2 rounded-full text-gray-200 hover:bg-[#393c41] transition font-semibold text-base group" onClick={() => handleMenu("shop")}> 
          <FiShoppingCart className="w-5 h-5" />
          <span>Cửa hàng</span>
        </button>
      </div>
      {/* DM List */}
      <div className="flex-1 overflow-y-auto px-2">
        <div className="flex items-center justify-between text-xs text-gray-400 px-2 mb-1 mt-2">
          <span>Tin nhắn trực tiếp</span>
          <button className="hover:text-white"><FiPlus /></button>
        </div>
        <div className="flex flex-col gap-2">
          {dms.map(dm => (
            <div
              key={dm.id}
              className={`flex items-center gap-3 px-2 py-2 rounded-2xl cursor-pointer transition relative group ${dm.active ? "bg-[#393c41] shadow-lg" : "hover:bg-[#313338]"}`}
              onClick={() => onSelectDM(dm.id)}
            >
              {/* Discord icon nếu là bot/discord */}
              {dm.discord ? (
                <span className="w-10 h-10 rounded-full bg-[#5865F2] flex items-center justify-center shadow-lg">
                  <svg width="28" height="28" viewBox="0 0 40 40" fill="none"><circle cx="20" cy="20" r="20" fill="#5865F2"/><circle cx="20" cy="16" r="7" fill="#fff"/><ellipse cx="20" cy="31" rx="11" ry="6" fill="#fff"/></svg>
                </span>
              ) : (
                <img src={dm.avatar || defaultAvatar} alt={dm.username} className="w-10 h-10 rounded-full border-2 border-[#23272a] object-cover shadow group-hover:scale-105 transition" />
              )}
              <span className={`flex-1 text-base truncate ${dm.active ? "text-white font-bold" : "text-gray-200"}`}>{dm.username}</span>
              {/* Trạng thái online/offline */}
              {typeof dm.online === "boolean" && (
                <span className={`w-3 h-3 rounded-full border-2 border-[#23272a] ${dm.online ? "bg-green-500" : "bg-gray-500"} ml-1`} />
              )}
              {/* Nút đóng */}
              {dm.active && (
                <button className="ml-2 text-gray-400 hover:text-white opacity-80 group-hover:opacity-100"><FiX size={18} /></button>
              )}
            </div>
          ))}
        </div>
      </div>
    </aside>
  );
}
