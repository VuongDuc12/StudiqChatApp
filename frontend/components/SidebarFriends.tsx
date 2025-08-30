
'use client';
import React from "react";
import { FiUser, FiGift, FiShoppingCart, FiPlus, FiX } from "react-icons/fi";



const defaultAvatar = "https://ui-avatars.com/api/?name=User&background=5865f2&color=fff&rounded=true&size=64";

interface SidebarFriendsProps {
  onSelectDM: (id: string) => void;
  onMenuClick?: (menu: string) => void;
  conversations?: any[]; // optional: if provided render conversations instead of sample dms
}

export default function SidebarFriends({ onSelectDM, onMenuClick, conversations }: SidebarFriendsProps) {
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
          {(() => {
            const list = (conversations && conversations.length) ? conversations : [];
            if (list.length === 0) {
              return (
                <div className="text-gray-400 text-center py-8">Bạn chưa có cuộc trò chuyện — bắt đầu bằng cách tìm hoặc thêm bạn.</div>
              );
            }

            return list.map((item: any) => {
              const isConv = !!(item.members);
              const meId = typeof window !== 'undefined' ? localStorage.getItem('userId') || '' : '';
              const avatar = isConv ? (item.avatarUrl ?? (item.members && item.members[0] ? item.members[0].avatarUrl : null)) : (item.avatar ?? null);
              const title = isConv ? (item.name ?? (item.members && item.members.find((m: any) => m.id !== meId)?.username) ?? (item.members && item.members[0]?.username) ?? 'Cuộc trò chuyện') : (item.username ?? 'Người dùng');
              const online = item.online ?? false;
              const active = item.active ?? false;
              const unread = item.unreadCount ?? 0;

              return (
                <div
                  key={item.id}
                  className={`flex items-center gap-3 px-2 py-2 rounded-2xl cursor-pointer transition relative group ${active ? "bg-[#393c41] shadow-lg" : "hover:bg-[#313338]"}`}
                  onClick={() => onSelectDM(item.id)}
                >
                  {item.discord ? (
                    <span className="w-10 h-10 rounded-full bg-[#5865F2] flex items-center justify-center shadow-lg">
                      <svg width="28" height="28" viewBox="0 0 40 40" fill="none"><circle cx="20" cy="20" r="20" fill="#5865F2"/><circle cx="20" cy="16" r="7" fill="#fff"/><ellipse cx="20" cy="31" rx="11" ry="6" fill="#fff"/></svg>
                    </span>
                  ) : (
                    <img src={avatar ?? defaultAvatar} alt={title} className="w-10 h-10 rounded-full border-2 border-[#23272a] object-cover shadow group-hover:scale-105 transition" />
                  )}

                  <span className={`flex-1 text-base truncate ${active ? "text-white font-bold" : "text-gray-200"}`}>{title}</span>

                  {typeof online === "boolean" && (
                    <span className={`w-3 h-3 rounded-full border-2 border-[#23272a] ${online ? "bg-green-500" : "bg-gray-500"} ml-1`} />
                  )}

                  {unread > 0 && (
                    <div className="ml-2 bg-red-500 text-white text-xs font-bold rounded-full px-2 py-0.5">{unread}</div>
                  )}

                  {active && (
                    <button className="ml-2 text-gray-400 hover:text-white opacity-80 group-hover:opacity-100"><FiX size={18} /></button>
                  )}
                </div>
              );
            });
          })()}
        </div>
      </div>
    </aside>
  );
}
