

"use client";
import React, { useEffect, useState } from "react";
import { FiUserPlus, FiUsers, FiList } from "react-icons/fi";
import toast from "react-hot-toast";



const defaultAvatar = "https://ui-avatars.com/api/?name=User&background=5865f2&color=fff&rounded=true&size=64";

interface FriendTabsProps {
  tab: string;
  setTab: (tab: string) => void;
}

export default function FriendTabs({ tab, setTab }: FriendTabsProps) {
  // State
  const [friends, setFriends] = useState<any[]>([]);
  const [invites, setInvites] = useState<any[]>([]);
  const [search, setSearch] = useState("");
  const [searchResults, setSearchResults] = useState<any[]>([]);
  const [searchLoading, setSearchLoading] = useState(false);
  const [addLoading, setAddLoading] = useState(false);
  // Removed error/success state, use toast only
  const token = typeof window !== "undefined" ? localStorage.getItem("token") : "";

  // Fetch friends
  useEffect(() => {
    if (!token) return;
    fetch("http://localhost:5210/api/Friends", {
      headers: { Authorization: `Bearer ${token}` }
    })
      .then(r => r.json())
      .then(d => setFriends(d.data || []));
  }, [token]);

  // Fetch invites
  useEffect(() => {
    if (!token) return;
    fetch("http://localhost:5210/api/Friends/received-requests", {
      headers: { Authorization: `Bearer ${token}` }
    })
      .then(r => r.json())
      .then(d => setInvites(d.data || []));
  }, [token]);

  // Search user
  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    setSearchLoading(true);
  // clear notifications
    try {
      const res = await fetch(`http://localhost:5210/api/Friends/search?keyword=${encodeURIComponent(search)}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      const data = await res.json();
      setSearchResults(data.data || []);
      if (!data.data || data.data.length === 0) toast.error("Không tìm thấy người dùng.");
    } catch {
      toast.error("Không tìm thấy người dùng.");
    } finally {
      setSearchLoading(false);
    }
  };

  // Send friend request
  const handleAddFriend = async (userId: string) => {
    setAddLoading(true);
  // clear notifications
    try {
      const res = await fetch("http://localhost:5210/api/Friends/request", {
        method: "POST",
        headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
        body: JSON.stringify(userId)
      });
      const data = await res.json();
      if (!data.success) throw new Error(data.message || "Gửi lời mời thất bại");
      toast.success("Đã gửi lời mời kết bạn!");
    } catch (err: any) {
      toast.error(err.message || "Gửi lời mời thất bại");
    } finally {
      setAddLoading(false);
    }
  };

  // Accept invite
  const handleAccept = async (inviteId: string) => {
  // clear notifications
    try {
      const res = await fetch(`http://localhost:5210/api/Friends/accept/${inviteId}`, {
        method: "POST",
        headers: { Authorization: `Bearer ${token}` }
      });
      const data = await res.json();
  if (!data.success) throw new Error(data.message || "Chấp nhận thất bại");
  toast.success("Đã chấp nhận lời mời!");
      // Refresh invites & friends
      fetch("http://localhost:5210/api/Friends/received-requests", {
        headers: { Authorization: `Bearer ${token}` }
      })
        .then(r => r.json())
        .then(d => setInvites(d.data || []));
      fetch("http://localhost:5210/api/Friends", {
        headers: { Authorization: `Bearer ${token}` }
      })
        .then(r => r.json())
        .then(d => setFriends(d.data || []));
    } catch (err: any) {
      toast.error(err.message || "Chấp nhận thất bại");
    }
  };

  return (
    <div className="flex flex-col h-full">
      <div className="flex gap-2 border-b border-[#23272a] px-4 pt-4 font-inter">
        <button
          className={`flex items-center gap-2 px-5 py-2 text-base font-semibold rounded-full transition-all duration-200 shadow-sm
            ${tab === "online" ? "bg-gradient-to-r from-indigo-500 to-purple-500 text-white shadow-lg scale-105" : "text-gray-400 hover:text-white hover:bg-[#23272a]"}`}
          onClick={() => setTab("online")}
        >
          <FiUsers className="w-5 h-5" />
          Bạn bè
          <span className="ml-2 bg-green-500 text-white text-xs rounded-full px-2 py-0.5 font-bold shadow border-2 border-[#36393f]">{friends.length}</span>
        </button>
        <button
          className={`flex items-center gap-2 px-5 py-2 text-base font-semibold rounded-full transition-all duration-200 shadow-sm
            ${tab === "all" ? "bg-gradient-to-r from-indigo-500 to-purple-500 text-white shadow-lg scale-105" : "text-gray-400 hover:text-white hover:bg-[#23272a]"}`}
          onClick={() => setTab("all")}
        >
          <FiList className="w-5 h-5" />
          Tất cả
          <span className="ml-2 bg-gray-500 text-white text-xs rounded-full px-2 py-0.5 font-bold shadow border-2 border-[#36393f]">{friends.length}</span>
        </button>
        <button
          className={`flex items-center gap-2 px-5 py-2 text-base font-semibold rounded-full transition-all duration-200 shadow-sm
            ${tab === "invites" ? "bg-gradient-to-r from-indigo-500 to-purple-500 text-white shadow-lg scale-105" : "text-gray-400 hover:text-white hover:bg-[#23272a]"}`}
          onClick={() => setTab("invites")}
        >
          <span className="inline-block w-5 h-5 bg-yellow-400 rounded-full flex items-center justify-center text-white font-bold text-xs">!</span>
          Lời mời
          <span className="ml-2 bg-yellow-500 text-white text-xs rounded-full px-2 py-0.5 font-bold shadow border-2 border-[#36393f]">{invites.length}</span>
        </button>
        <button
          className={`flex items-center gap-2 px-5 py-2 text-base font-semibold rounded-full transition-all duration-200 shadow-sm
            ${tab === "add" ? "bg-gradient-to-r from-indigo-500 to-purple-500 text-white shadow-lg scale-105" : "text-gray-400 hover:text-white hover:bg-[#23272a]"}`}
          onClick={() => setTab("add")}
        >
          <FiUserPlus className="w-5 h-5" />
          Thêm bạn bè
        </button>
      </div>
  <div className="flex-1 overflow-y-auto bg-[#36393f] p-8 font-inter">
        {tab === "online" && (
          <ul className="space-y-4">
            {friends.map(f => (
              <li key={f.id} className="flex items-center gap-4 p-4 rounded-2xl bg-[#23272a] hover:bg-[#393c41] transition shadow group cursor-pointer">
                <div className="relative">
                  <img
                    src={defaultAvatar}
                    alt={f.friend?.username || f.user?.username || "User"}
                    className="w-14 h-14 rounded-full border-4 border-white shadow-lg object-cover group-hover:scale-105 transition"
                  />
                  <span className="absolute bottom-1 right-1 w-4 h-4 bg-green-400 border-2 border-white rounded-full"></span>
                </div>
                <div className="flex flex-col justify-center">
                  <span className="text-white font-bold text-lg tracking-wide">{f.friend?.username || f.user?.username}</span>
                  <span className="text-green-400 text-xs font-medium">Bạn bè</span>
                </div>
              </li>
            ))}
          </ul>
        )}
        {tab === "all" && (
          <ul className="space-y-4">
            {friends.map(f => (
              <li key={f.id} className="flex items-center gap-4 p-4 rounded-2xl bg-[#23272a] hover:bg-[#393c41] transition shadow group cursor-pointer">
                <div className="relative">
                  <img
                    src={defaultAvatar}
                    alt={f.friend?.username || f.user?.username || "User"}
                    className="w-14 h-14 rounded-full border-4 border-green-400 object-cover shadow-lg group-hover:scale-105 transition"
                  />
                  <span className="absolute bottom-1 right-1 w-4 h-4 bg-green-400 border-2 border-white rounded-full"></span>
                </div>
                <div className="flex flex-col justify-center">
                  <span className="text-white font-bold text-lg tracking-wide">{f.friend?.username || f.user?.username}</span>
                  <span className="text-green-400 text-xs font-medium">Bạn bè</span>
                </div>
              </li>
            ))}
          </ul>
        )}
        {tab === "invites" && (
          <ul className="space-y-4">
            {invites.length === 0 && (
              <li className="text-gray-400 text-center py-8">Không có lời mời kết bạn nào.</li>
            )}
            {invites.map(invite => (
              <li key={invite.id} className="flex items-center gap-4 p-4 rounded-2xl bg-[#23272a] hover:bg-[#393c41] transition shadow group cursor-pointer">
                <img
                  src={defaultAvatar}
                  alt={invite.fromUser?.username || "User"}
                  className="w-14 h-14 rounded-full border-4 border-yellow-400 object-cover shadow-lg group-hover:scale-105 transition"
                />
                <div className="flex flex-col justify-center flex-1">
                  <span className="text-white font-bold text-lg tracking-wide">{invite.fromUser?.username}</span>
                  <span className="text-yellow-400 text-xs font-medium">Đã gửi lời mời</span>
                </div>
                <div className="flex gap-2">
                  <button className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-lg font-semibold transition" onClick={() => handleAccept(invite.id)}>Chấp nhận</button>
                </div>
              </li>
            ))}
          </ul>
        )}
        {tab === "add" && (
          <form className="flex flex-col gap-4 max-w-md mx-auto bg-[#23272a] p-8 rounded-2xl shadow-lg border border-[#36393f]" onSubmit={handleSearch}>
            <label className="text-white text-xl font-bold mb-2">Tìm kiếm bạn mới</label>
            <div className="flex items-center gap-3">
              <img src={defaultAvatar} className="w-12 h-12 rounded-full border-2 border-[#5865f2] shadow" alt="avatar minh họa" />
              <input
                type="text"
                placeholder="Nhập username..."
                className="flex-1 px-4 py-3 rounded-lg bg-[#36393f] text-white outline-none border border-[#23272a] focus:border-[#5865f2] transition text-base"
                value={search}
                onChange={e => setSearch(e.target.value)}
              />
            </div>
            <button type="submit" className="bg-gradient-to-r from-indigo-500 to-purple-500 text-white px-8 py-3 rounded-lg font-bold shadow hover:scale-105 transition text-base" disabled={searchLoading}>{searchLoading ? "Đang tìm..." : "Tìm kiếm"}</button>
            {/* toast only, no in-page error/success */}
            {searchResults.length > 0 && (
              <div className="mt-4">
                <div className="text-white font-semibold mb-2">Kết quả:</div>
                <ul className="space-y-2">
                  {searchResults.map(u => (
                    <li key={u.id} className="flex items-center gap-3 bg-[#36393f] p-3 rounded-lg">
                      <img src={defaultAvatar} className="w-10 h-10 rounded-full border-2 border-[#5865f2]" alt={u.username} />
                      <div className="flex-1">
                        <div className="text-white font-bold">{u.username}</div>
                        <div className="text-gray-400 text-xs">{u.fullName}</div>
                      </div>
                      <button className="bg-indigo-500 hover:bg-indigo-600 text-white px-4 py-2 rounded font-semibold" onClick={() => handleAddFriend(u.id)} disabled={addLoading}>{addLoading ? "Đang gửi..." : "Kết bạn"}</button>
                    </li>
                  ))}
                </ul>
              </div>
            )}
          </form>
        )}
      </div>
    </div>
  );
}
