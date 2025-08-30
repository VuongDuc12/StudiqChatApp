"use client";
import ChatWindow from "@/components/ChatWindow";
import FriendTabs from "@/components/FriendTabs";
import SidebarFriends from "@/components/SidebarFriends";
import api from "@/lib/api";
import { ConversationItem } from "@/components/ConversationsList";
import SidebarGroups from "@/components/SidebarGroups";
import React, { useState } from "react";


export default function ChatPage() {
  const [mainView, setMainView] = useState<'friends' | 'nitro' | 'shop' | 'chat'>("friends");
  const [friendTab, setFriendTab] = useState("online");
  const [conversations, setConversations] = useState<ConversationItem[]>([]);
  const [selectedConv, setSelectedConv] = useState<ConversationItem | null>(null);

  // decode JWT payload to extract user id (sub/nameid)
  const decodeUserIdFromToken = (token: string | null) => {
    if (!token) return null;
    try {
      const parts = token.split('.');
      if (parts.length < 2) return null;
      const payload = parts[1].replace(/-/g, '+').replace(/_/g, '/');
      const pad = payload.length % 4;
      const padded = pad ? payload + '='.repeat(4 - pad) : payload;
      const json = JSON.parse(atob(padded));
      return json.nameid || json.sub || json['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || null;
    } catch { return null; }
  };

  const fetchConversations = async () => {
    try {
      const res = await api.get('/Conversations');
      const data = res.data.data || [];
      setConversations(data);
      // ensure localStorage userId exists
      const token = typeof window !== 'undefined' ? localStorage.getItem('token') : null;
      if (token && !localStorage.getItem('userId')) {
        const uid = decodeUserIdFromToken(token);
        if (uid) localStorage.setItem('userId', uid);
      }
    } catch (err) {
      console.error('Failed to load conversations', err);
    }
  };

  React.useEffect(() => { fetchConversations(); }, []);

  const startDirectConversation = async (payload: { id: string; username?: string; avatarUrl?: string | null }) => {
    // create provisional conversation locally so UI opens immediately.
    const provisionalConv: ConversationItem = {
      id: payload.id, // use friend id as temporary conversation id; backend logic will accept this as recipient hint
      type: 0,
      name: payload.username || null,
      avatarUrl: payload.avatarUrl || null,
      members: [{ id: payload.id, username: payload.username }],
      provisional: true
    };
    setSelectedConv(provisionalConv);
    setMainView('chat');
  };

  const handleSelect = (id: string) => {
    const conv = conversations.find(c => c.id === id) || null;
    setSelectedConv(conv);
    setMainView('chat');
  };

  return (
    <>
      <div className="flex h-screen">
        <SidebarGroups />
        <div className="hidden md:flex flex-col w-60 h-full">
          <SidebarFriends
            conversations={conversations}
            onSelectDM={(id) => handleSelect(id)}
            onMenuClick={(menu) => setMainView(menu as any)}
          />
        </div>
        <main className="flex-1 flex flex-col h-full">
          {mainView === "friends" && <FriendTabs tab={friendTab} setTab={setFriendTab} onStartChat={startDirectConversation} />}
          {mainView === "chat" && <ChatWindow conversation={selectedConv} onConversationUpdated={fetchConversations} />}
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
