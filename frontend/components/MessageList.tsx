'use client';
import React from "react";

const messages = [
  { id: 1, user: { username: "alice", avatar: "/avatar-default.svg" }, content: "Hi!", timestamp: "09:00" },
  { id: 2, user: { username: "me", avatar: "/avatar-default.svg" }, content: "Hello Alice!", timestamp: "09:01" },
  { id: 3, user: { username: "alice", avatar: "/avatar-default.svg" }, content: "How are you?", timestamp: "09:02" },
];

export default function MessageList() {
  // Giả lập username của mình
  const myUsername = "me";
  return (
    <div className="flex-1 overflow-y-auto px-4 py-2 flex flex-col justify-end">
      <div className="space-y-4">
        {messages.map((msg) => {
          const isMe = msg.user.username === myUsername;
          return (
            <div
              key={msg.id}
              className={`flex items-end gap-3 mb-2 ${isMe ? "justify-end" : ""}`}
            >
              {/* Tin nhắn của mình: text bên phải, avatar bên phải */}
              {!isMe && (
                <img
                  src={msg.user.avatar || "/avatar-default.svg"}
                  className="w-8 h-8 rounded-full mt-1"
                  alt="avatar"
                />
              )}
              <div className={`max-w-xs flex flex-col ${isMe ? "items-end" : "items-start"}`}>
                <div className={`flex items-center gap-2 ${isMe ? "justify-end" : ""}`}>
                  {isMe ? (
                    <>
                      <span className="text-xs text-gray-400">{msg.timestamp}</span>
                      <span className="text-white font-semibold">{msg.user.username}</span>
                    </>
                  ) : (
                    <>
                      <span className="text-white font-semibold">{msg.user.username}</span>
                      <span className="text-xs text-gray-400">{msg.timestamp}</span>
                    </>
                  )}
                </div>
                <div className={`inline-block px-4 py-2 rounded-lg mt-1 ${isMe ? "bg-[#5865f2] text-white ml-auto" : "bg-[#40444b] text-gray-200 mr-auto"}`}>
                  {msg.content}
                </div>
              </div>
              {isMe && (
                <img
                  src={msg.user.avatar || "/avatar-default.svg"}
                  className="w-8 h-8 rounded-full mt-1"
                  alt="avatar"
                />
              )}
            </div>
          );
        })}
      </div>
    </div>
  );
}
