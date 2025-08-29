"use client";
import ChatWindow from "@/components/ChatWindow";
import FriendTabs from "@/components/FriendTabs";
import SidebarFriends from "@/components/SidebarFriends";
import SidebarGroups from "@/components/SidebarGroups";
import React, { useState } from "react";


export default function ChatPage() {
  const [mainView, setMainView] = useState<'friends' | 'nitro' | 'shop' | 'chat'>("friends");
  const [friendTab, setFriendTab] = useState("online");
  return (
    <>
      <div className="flex h-screen">
        <SidebarGroups />
        <div className="hidden md:flex flex-col w-60 h-full">
          <SidebarFriends
            onSelectDM={() => setMainView("chat")}
            onMenuClick={(menu) => setMainView(menu as any)}
          />
        </div>
        <main className="flex-1 flex flex-col h-full">
          {mainView === "friends" && <FriendTabs tab={friendTab} setTab={setFriendTab} />}
          {mainView === "chat" && <ChatWindow />}
          {mainView === "nitro" && (
            <div className="flex-1 flex items-center justify-center text-white text-2xl">Nitro Coming Soon!</div>
          )}
          {mainView === "shop" && (
            <div className="flex-1 flex items-center justify-center text-white text-2xl">Shop Coming Soon!</div>
          )}
        </main>
      </div>
    </>
  );
}
