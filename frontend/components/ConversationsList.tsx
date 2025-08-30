"use client";
import React from "react";

export interface SimpleUser {
  id: string;
  username?: string;
  fullName?: string;
  avatarUrl?: string | null;
}

export interface ConversationItem {
  id: string;
  type: number;
  provisional?: boolean;
  name?: string | null;
  avatarUrl?: string | null;
  lastMessage?: {
    id: string;
    content?: string | null;
    createdAt?: string | null;
    senderId?: string | null;
  } | null;
  lastMessageAt?: string | null;
  unreadCount?: number;
  members: SimpleUser[];
}

interface Props {
  conversations: ConversationItem[];
  selectedId?: string | null;
  onSelect: (conv: ConversationItem) => void;
  onRefresh?: () => void;
}

export default function ConversationsList({ conversations, selectedId, onSelect, onRefresh }: Props) {
  return (
    <aside className="w-96 max-w-xs bg-[#0f1720] border-r border-[#111827] flex flex-col">
      <div className="px-4 py-3 flex items-center justify-between border-b border-[#111827]">
        <h2 className="text-white text-lg font-semibold">Tin nhắn</h2>
        <button
          onClick={onRefresh}
          className="text-sm text-gray-300 bg-[#111827] px-3 py-1 rounded-md hover:bg-[#17202a]"
        >Làm mới</button>
      </div>

      <div className="p-3 overflow-y-auto flex-1">
        <ul className="space-y-3">
          {conversations.length === 0 && (
            <li className="text-gray-400 text-center py-8">Không có cuộc trò chuyện</li>
          )}

          {conversations.map(c => {
            const meId = typeof window !== 'undefined' ? localStorage.getItem('userId') || '' : '';
            const other = c.members.find(m => m.id !== meId) ?? c.members[0];
            const title = c.type === 0 ? (other?.username ?? c.name ?? 'Người dùng') : (c.name ?? 'Nhóm');
            const preview = c.lastMessage?.content ?? '';
            const time = c.lastMessageAt ? new Date(c.lastMessageAt).toLocaleTimeString([], {hour: '2-digit', minute: '2-digit'}) : '';
            const selected = selectedId === c.id;

            return (
              <li key={c.id}>
                <button
                  onClick={() => onSelect(c)}
                  className={`w-full flex items-center gap-3 p-3 rounded-xl transition ${selected ? 'bg-gradient-to-r from-indigo-600 to-purple-600 text-white' : 'hover:bg-[#071327] bg-transparent'}`}>
                  <div className="w-12 h-12 rounded-full overflow-hidden bg-gray-600 flex-shrink-0">
                    {other?.avatarUrl ? (
                      <img src={other.avatarUrl} alt={title} className="w-full h-full object-cover" />
                    ) : (
                      <div className="w-full h-full flex items-center justify-center text-white font-bold bg-[#334155]">{(title || 'U')[0].toUpperCase()}</div>
                    )}
                  </div>

                  <div className="flex-1 min-w-0">
                    <div className="flex items-center justify-between gap-3">
                      <div className="font-semibold truncate">{title}</div>
                      <div className="text-xs text-gray-400">{time}</div>
                    </div>
                    <div className="text-sm text-gray-300 truncate mt-1">{preview}</div>
                  </div>

                  {c.unreadCount && c.unreadCount > 0 && (
                    <div className="ml-2 bg-red-500 text-white text-xs font-bold rounded-full px-2 py-0.5">{c.unreadCount}</div>
                  )}
                </button>
              </li>
            );
          })}
        </ul>
      </div>
    </aside>
  );
}
