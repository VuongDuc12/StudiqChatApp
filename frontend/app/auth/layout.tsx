"use client";
import React from "react";

export default function authLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen w-screen h-screen flex flex-col bg-[#23272a]">
      {/* Có thể thêm header riêng cho chat nếu muốn */}
      <main className="flex-1 flex flex-col h-full w-full">{children}</main>
    </div>
  );
}