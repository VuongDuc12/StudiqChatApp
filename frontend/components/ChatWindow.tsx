'use client';
import React from "react";
import MessageList from "./MessageList";
import MessageInput from "./MessageInput";

const user = { username: "alice", avatar: "/avatar-default.svg" };

export default function ChatWindow() {
  return (
    <div className="flex flex-col h-full bg-[#36393f]">
      <header className="flex items-center gap-3 px-4 py-3 border-b border-[#23272a]">
  <img src={user.avatar || "/avatar-default.svg"} className="w-8 h-8 rounded-full" alt="avatar" />
        <span className="text-white font-semibold text-lg">{user.username}</span>
        <div className="flex-1" />
        <button className="text-gray-400 hover:text-white mx-1">📞</button>
        <button className="text-gray-400 hover:text-white mx-1">🚫</button>
      </header>
      <MessageList />
      <MessageInput />
    </div>
  );
}
