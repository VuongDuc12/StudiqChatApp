"use client";
import React from "react";
import { useSignalRNotification } from "@/hooks/useSignalRNotification";

export default function ChatLayout({ children }: { children: React.ReactNode }) {
  useSignalRNotification();
  return (
    <div className="min-h-screen w-screen h-screen flex flex-col bg-[#23272a]">
      {/* Có thể thêm header riêng cho chat nếu muốn */}
      <main className="flex-1 flex flex-col h-full w-full">{children}</main>
    </div>
  );
}