'use client';
import React from "react";

export default function MessageInput() {
  return (
    <form className="flex items-center gap-2 px-4 py-3 bg-[#40444b]">
      <button type="button" className="text-gray-400 hover:text-white text-xl">😊</button>
      <button type="button" className="text-gray-400 hover:text-white text-xl">GIF</button>
      <button type="button" className="text-gray-400 hover:text-white text-xl">📎</button>
      <input
        type="text"
        placeholder="Nhập tin nhắn..."
        className="flex-1 px-3 py-2 rounded bg-[#36393f] text-white outline-none"
      />
      <button type="submit" className="bg-[#5865f2] text-white px-4 py-2 rounded">Gửi</button>
    </form>
  );
}
