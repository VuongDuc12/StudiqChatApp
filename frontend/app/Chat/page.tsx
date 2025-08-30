"use client";
import React, { useEffect, useState } from "react";
import ConversationsList, { ConversationItem } from "@/components/ConversationsList";
import ChatWindow from "@/components/ChatWindow";
import api from "@/lib/api";
import { getConnection } from "@/lib/connectionHub";
import toast from "react-hot-toast";

export default function ChatPage() {
  const [conversations, setConversations] = useState<ConversationItem[]>([]);
  const [selected, setSelected] = useState<ConversationItem | null>(null);
  const token = typeof window !== "undefined" ? localStorage.getItem("token") || "" : "";

  const fetchConversations = async () => {
    try {
      const res = await api.get("/Conversations");
      const data = res.data.data || [];
      setConversations(data);
      if (selected) {
        const s = data.find((d: any) => d.id === selected.id);
        if (s) setSelected(s);
      }
    } catch (e) { console.error(e); }
  };

  useEffect(() => { fetchConversations(); }, []);

  useEffect(() => {
    if (!token) return;
    const conn = getConnection(token);
    (async () => {
      try { if (conn.state === 'Disconnected') await conn.start(); } catch (err) { console.error('SignalR start failed', err); }
    })();

    const onNewMessage = (payload: any) => {
      const msg = payload.message ?? payload;
      if (!msg || !msg.conversationId) return;
      setConversations(prev => {
        const idx = prev.findIndex(p => p.id === msg.conversationId);
        if (idx === -1) return prev;
        const next = [...prev];
        const updated = { ...next[idx], lastMessage: { id: msg.id, content: msg.content, createdAt: msg.createdAt, senderId: msg.senderId }, lastMessageAt: msg.createdAt, unreadCount: (next[idx].id === selected?.id) ? 0 : ((next[idx].unreadCount || 0) + 1) };
        next.splice(idx, 1);
        return [updated, ...next];
      });

      if (selected && selected.id === msg.conversationId) {
        // ChatWindow listens and appends
      } else {
        toast(`${payload.sender?.username ?? 'Someone'}: ${msg.content}`);
      }
    };

    const onMessagesRead = (payload: any) => {
      const convId = payload.conversationId;
      setConversations(prev => prev.map(c => c.id === convId ? { ...c, unreadCount: 0 } : c));
    };

    conn.on("NewMessage", onNewMessage);
    conn.on("MessagesRead", onMessagesRead);

    return () => { try { conn.off("NewMessage", onNewMessage); conn.off("MessagesRead", onMessagesRead); } catch {} };
  }, [selected?.id, token]);

  return (
    <div className="h-full flex">
      <ConversationsList conversations={conversations} selectedId={selected?.id} onSelect={(c)=>setSelected(c)} onRefresh={fetchConversations} />
      <div className="flex-1 bg-[#071327]">
        <ChatWindow conversation={selected} onConversationUpdated={fetchConversations} />
      </div>
    </div>
  );
}
