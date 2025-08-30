"use client";
import React, { useEffect, useRef, useState } from "react";
import Picker from 'emoji-picker-react';
import { FiPhone, FiVideo, FiInfo, FiSmile, FiPaperclip } from "react-icons/fi";
import api from "@/lib/api";
import toast from "react-hot-toast";
import { ConversationItem } from "./ConversationsList";
import { getConnection, ensureConnected } from "@/lib/connectionHub";

interface Message {
  id: string;
  conversationId: string;
  senderId: string;
  content?: string | null;
  messageType?: number;
  createdAt?: string | null;
  sender?: { id: string; username?: string | null; fullName?: string | null; avatarUrl?: string | null } | null;
}

interface Props {
  conversation: ConversationItem | null;
  onConversationUpdated?: () => void;
}

function formatTime(ts?: string | null) {
  if (!ts) return "";
  const d = new Date(ts);
  return d.toLocaleString([], { hour: '2-digit', minute: '2-digit' });
}

export default function ChatWindow({ conversation, onConversationUpdated }: Props) {
  const [messages, setMessages] = useState<Message[]>([]);
  const [text, setText] = useState("");
  const [showEmoji, setShowEmoji] = useState(false);
  const fileInputRef = React.useRef<HTMLInputElement | null>(null);
  const emojiRef = React.useRef<HTMLDivElement | null>(null);
  const emojiButtonRef = React.useRef<HTMLButtonElement | null>(null);
  const [loading, setLoading] = useState(false);
  const [loadError, setLoadError] = useState<string | null>(null);
  const [resolvedConversationId, setResolvedConversationId] = useState<string | null>(null);
  const scrollRef = useRef<HTMLDivElement | null>(null);
  const token = typeof window !== 'undefined' ? localStorage.getItem('token') || '' : '';

  useEffect(() => {
    if (!conversation) return;
    (async () => {
      setLoading(true);
      try {
        // determine if this is a provisional friend conversation: no real conversation id yet
        const meId = typeof window !== 'undefined' ? localStorage.getItem('userId') || '' : '';
        // try to find other user's id from conversation prop
        let otherId: string | null = null;
        if (conversation?.members && conversation.members.length > 0) {
          const other = conversation.members.find(m => m.id !== meId);
          if (other && other.id) otherId = other.id;
        }
        // fallback: when provisional the conversation.id may be the friend's id
        if (!otherId && (conversation as any)?.provisional) {
          otherId = conversation.id;
        }

        let messagesData: any[] = [];
        // if we appear to be in a provisional friend chat, try to resolve an existing direct conversation
        if ((conversation as any)?.provisional && otherId) {
          try {
            const listRes = await api.get(`/Conversations`);
            const convs = listRes.data.data || [];
            const found = convs.find((c: any) => (c.type === 0) && c.members.some((m: any) => m.id === otherId));
            if (found) {
              setResolvedConversationId(found.id);
              // load messages for found conversation
              const msgRes = await api.get(`/Conversations/${found.id}/messages?page=1&pageSize=50`);
              messagesData = msgRes.data.data || [];
              // join group for real-time
              try {
                const conn = await ensureConnected(token);
                await conn.invoke('JoinConversation', found.id);
              } catch {}
              // mark read
              await api.post(`/Conversations/${found.id}/read`, null);
            } else {
              // no existing conversation
              messagesData = [];
              setResolvedConversationId(null);
            }
          } catch (err) {
            // network or other error — fall back to empty
            messagesData = [];
            setResolvedConversationId(null);
          }
        } else {
          // normal flow: conversation has real id
          const res = await api.get(`/Conversations/${conversation.id}/messages?page=1&pageSize=50`);
          messagesData = res.data.data || [];
          setResolvedConversationId(conversation.id);
          // join group for real-time
          try {
            const conn = await ensureConnected(token);
            await conn.invoke('JoinConversation', conversation.id);
          } catch {}
          // mark read
          await api.post(`/Conversations/${conversation.id}/read`, null);
        }

        setMessages(messagesData);
        if ((!messagesData || messagesData.length === 0) && (conversation as any)?.provisional) {
          const displayName = conversation?.name ?? conversation?.members?.map(m => m.username).filter(Boolean).join(', ') ?? 'người này';
          setLoadError(`Hãy gửi tin nhắn đầu tiên cho "${displayName}".`);
        } else {
          setLoadError(null);
        }
        onConversationUpdated?.();
        setTimeout(() => scrollToBottom(), 80);
      } catch (err) {
        const displayName = conversation?.name ?? conversation?.members?.map(m => m.username).filter(Boolean).join(', ') ?? 'người này';
        setLoadError(`Hãy gửi tin nhắn đầu tiên cho "${displayName}" .`);
      } finally { setLoading(false); }
    })();

    return () => {
      // leave group
      try {
        const conn = getConnection(token);
        if (conn) conn.invoke('LeaveConversation', conversation.id).catch(()=>{});
      } catch {}
    };
  }, [conversation?.id]);

  useEffect(() => {
    if (!conversation) return;
    let mounted = true;
    let registeredConn: any = null;

    (async () => {
      try {
        const conn = await ensureConnected(token);
        if (!mounted) return;
        registeredConn = conn;
        const handler = (payload: any) => {
          const incoming: Message = payload.message ?? payload;
          if (!incoming) return;
          const currentConvId = resolvedConversationId ?? conversation.id;
          if (incoming.conversationId !== currentConvId) return;
          setMessages(prev => {
            if (prev.some(m => m.id === incoming.id)) return prev;
            return [...prev, incoming];
          });
          setTimeout(() => scrollToBottom(), 50);
        };
        conn.on('NewMessage', handler);
      } catch (err) {
        // ignore; connection failed
      }
    })();

    return () => {
      mounted = false;
      try { if (registeredConn) registeredConn.off('NewMessage'); } catch {}
    };
  }, [conversation?.id, resolvedConversationId]);

  const scrollToBottom = (smooth = true) => {
    if (!scrollRef.current) return;
    try {
      scrollRef.current.scrollTo({ top: scrollRef.current.scrollHeight, behavior: smooth ? 'smooth' : 'auto' });
    } catch {
      // fallback
      scrollRef.current.scrollTop = scrollRef.current.scrollHeight;
    }
  };

  // always scroll to bottom when messages change
  React.useEffect(() => {
    if (messages.length === 0) return;
    scrollToBottom(true);
  }, [messages.length]);

  const handleSend = async () => {
    if (!conversation || !text.trim()) return;
    // For direct (1:1) chats we use MessageType==11 so server treats it as direct
    const isDirect = conversation?.type === 0;
    const payload: any = { content: text.trim(), messageType: isDirect ? 11 : 0 };

    // determine post target id: if we resolved a conversation, use it; otherwise use conversation.id (may be friend's id)
    const postId = resolvedConversationId ?? conversation.id;

    // Always include conversationId explicitly to avoid server treating missing field as "create new" on subsequent sends.
    if (resolvedConversationId) {
      payload.conversationId = resolvedConversationId;
    } else if (isDirect && (conversation as any).provisional) {
      // no resolved conversation yet and this is a direct provisional chat: ask server to create
      payload.conversationId = null;
      const other = conversation.members?.find(m => m.id !== (typeof window !== 'undefined' ? localStorage.getItem('userId') : null));
      if (other && other.id) payload.recipientId = other.id;
    } else {
      // normal case with a real conversation id in route
      payload.conversationId = conversation.id;
    }

    try {
      const res = await api.post(`/Conversations/${postId}/messages`, payload);
      const msg: Message = res.data.data;
      setMessages(prev => [...prev, msg]);
      setText('');
      setTimeout(() => scrollToBottom(), 80);
      // if server created a conversation, update resolvedConversationId from returned message.conversationId
      if (!resolvedConversationId && msg?.conversationId) {
        setResolvedConversationId(msg.conversationId);
      }
      onConversationUpdated?.();
    } catch (err: any) {
      toast.error(err?.response?.data?.message || 'Gửi thất bại');
    }
  };

  const onEmojiClick = (emojiData: any) => {
    // emojiData.emoji contains the emoji character
    try {
      const ch = emojiData.emoji || (emojiData as any).unified || '';
      setText(t => t + ch);
      // keep picker open so user can add more emojis; do not auto-close here
    } catch {
      // fallback: do nothing
    }
  };

  // close emoji picker when clicking outside of the picker or the emoji button
  useEffect(() => {
    if (!showEmoji) return;
    const handleOutside = (e: MouseEvent | TouchEvent) => {
      const target = e.target as Node | null;
      if (!target) return;
      if (emojiRef.current && emojiRef.current.contains(target)) return;
      if (emojiButtonRef.current && emojiButtonRef.current.contains(target)) return;
      setShowEmoji(false);
    };
    document.addEventListener('mousedown', handleOutside);
    document.addEventListener('touchstart', handleOutside);
    return () => {
      document.removeEventListener('mousedown', handleOutside);
      document.removeEventListener('touchstart', handleOutside);
    };
  }, [showEmoji]);

  if (!conversation) return <div className="flex-1 flex items-center justify-center text-gray-400">Chọn cuộc trò chuyện</div>;

  const meId = typeof window !== 'undefined' ? localStorage.getItem('userId') || '' : '';

  return (
    <div className="flex-1 flex flex-col bg-[#071327]">
      <div className="px-5 py-4 flex items-center gap-3 border-b border-[#0b2230]">
        <div className="w-12 h-12 rounded-full overflow-hidden bg-gray-700">
          {conversation.avatarUrl ? (
            <img src={conversation.avatarUrl ?? ''} alt={conversation.name ?? ''} className="w-full h-full object-cover" />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-white font-bold">{(conversation.name || (conversation.members || []).map((m:any)=>m.username).join(', '))[0]}</div>
          )}
        </div>

        <div className="flex-1">
          {/* show other user's name in 1:1 chat */}
          {(() => {
            const meIdLocal = typeof window !== 'undefined' ? localStorage.getItem('userId') || '' : '';
            const other = (conversation?.members || []).find((m: any) => m.id !== meIdLocal) || null;
            const displayName = conversation.name ?? other?.username ?? other?.fullName ?? 'Người dùng';
            const isOnline = (other as any)?.online ?? (other as any)?.isOnline ?? false;
            return (
              <>
                <div className="text-white font-semibold">{displayName}</div>
                <div className={`text-xs ${isOnline ? 'text-green-400' : 'text-gray-400'}`}>{isOnline ? 'Online' : 'Offline'}</div>
              </>
            );
          })()}
        </div>

        <div className="flex items-center gap-2">
          <button className="p-2 rounded-full hover:bg-[#0b2230] text-gray-200" title="Voice call" onClick={() => toast('Voice call not implemented yet')}><FiPhone /></button>
          <button className="p-2 rounded-full hover:bg-[#0b2230] text-gray-200" title="Video call" onClick={() => toast('Video call not implemented yet')}><FiVideo /></button>
          <button className="p-2 rounded-full hover:bg-[#0b2230] text-gray-200" title="Info" onClick={() => toast('Info not implemented yet')}><FiInfo /></button>
        </div>
      </div>

  <div ref={scrollRef} style={{ maxHeight: '576px' }} className="flex-1 overflow-y-auto p-4 md:p-6 space-y-4 scrollbar-thin scrollbar-thumb-[#1f2937] scrollbar-track-transparent">
        {loading && <div className="text-gray-300">Đang tải...</div>}

        {(!loading && loadError && messages.length === 0) && (
          <div className="text-center text-gray-400 py-8">{loadError}</div>
        )}

        {messages.map(m => {
          const mine = m.senderId === meId;
          return (
            <div key={m.id} className={`flex items-end ${mine ? 'justify-end' : 'justify-start'}`}>
              {!mine && (
                <div className="mr-3 w-10 h-10 rounded-full overflow-hidden bg-gray-600 flex-shrink-0">
                  {m.sender?.avatarUrl ? <img src={m.sender.avatarUrl} className="w-full h-full object-cover" /> : <div className="w-full h-full flex items-center justify-center text-white">{(m.sender?.username || 'U')[0]}</div>}
                </div>
              )}

              <div className={`max-w-[78%] ${mine ? 'text-right' : 'text-left'}`}>
                {!mine && m.sender?.username && <div className="text-xs text-gray-400 mb-1">{m.sender.username}</div>}
                <div className={`inline-block px-4 py-2 rounded-2xl break-words ${mine ? 'bg-gradient-to-tr from-indigo-600 to-purple-600 text-white' : 'bg-[#0b2330] text-gray-200'}`} style={{boxShadow: mine ? '0 8px 24px rgba(99,102,241,0.18)' : 'none'}}>
                  <div className="text-sm leading-relaxed">{m.content}</div>
                  <div className="text-xs text-gray-400 mt-1">{formatTime(m.createdAt)}</div>
                </div>
              </div>

              {mine && (
                <div className="ml-3 w-8 h-8 rounded-full overflow-hidden bg-indigo-600 flex-shrink-0" />
              )}
            </div>
          );
        })}
      </div>

      <div className="p-4 border-t border-[#0b2230] flex items-center gap-3 bg-[#06121a]">
        <div className="flex items-center gap-2">
          <button ref={emojiButtonRef} className="p-2 rounded-full hover:bg-[#0b2230] text-gray-200" title="Emoji" onClick={() => setShowEmoji(s => !s)}><FiSmile /></button>
          <button className="p-2 rounded-full hover:bg-[#0b2230] text-gray-200" title="Attach file" onClick={() => fileInputRef.current?.click()}><FiPaperclip /></button>
          <input ref={fileInputRef} type="file" className="hidden" onChange={(e) => { if (e.target.files && e.target.files.length > 0) { toast('File upload not implemented yet'); e.target.value = ''; } }} />
        </div>

        <input
          value={text}
          onChange={(e) => setText(e.target.value)}
          onKeyDown={(e) => { if (e.key === 'Enter') { e.preventDefault(); handleSend(); } }}
          className="flex-1 rounded-lg px-4 py-3 bg-[#061726] text-white outline-none placeholder-gray-400"
          placeholder="Gõ tin nhắn..."
        />
        <button onClick={handleSend} className="bg-indigo-500 hover:bg-indigo-600 text-white px-4 py-2 rounded-lg">Gửi</button>
        {showEmoji && (
          <div ref={emojiRef} className="absolute bottom-20 right-6 p-1 bg-[#0b1720] rounded-lg shadow-lg border border-[#10202a]">
            <Picker onEmojiClick={onEmojiClick} theme={("dark" as any)} />
          </div>
        )}
      </div>
    </div>
  );
}
