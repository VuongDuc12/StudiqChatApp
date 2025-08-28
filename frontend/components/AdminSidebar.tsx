'use client';

import { useState } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { FiHome, FiBookOpen, FiUsers, FiBarChart2, FiSettings, FiChevronDown, FiLogOut } from 'react-icons/fi';

interface SidebarItem {
  name: string;
  href: string;
  icon: React.ReactNode;
  badge?: string;
  children?: Omit<SidebarItem, 'children'>[];
}

interface AdminSidebarProps {
  collapsed?: boolean;
  onToggle?: () => void;
}

const sidebarItems: SidebarItem[] = [
  {
    name: 'Dashboard',
    href: '/admin',
    icon: <FiHome size={18} />,
  },
  {
    name: 'Quản lý khóa học',
    href: '/admin/courses',
    icon: <FiBookOpen size={18} />,
    children: [
      { name: 'Tất cả khóa học', href: '/admin/courses', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
      { name: 'Thêm khóa học', href: '/admin/courses/add', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
      { name: 'Danh mục', href: '/admin/courses/categories', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
    ],
  },
  {
    name: 'Quản lý học viên',
    href: '/admin/students',
    icon: <FiUsers size={18} />,
    badge: 'New',
    children: [
      { name: 'Tất cả học viên', href: '/admin/students', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
      { name: 'Thêm học viên', href: '/admin/students/add', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
      { name: 'Phân tích', href: '/admin/students/analytics', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
    ],
  },
  {
    name: 'Báo cáo & Thống kê',
    href: '/admin/reports',
    icon: <FiBarChart2 size={18} />,
    children: [
      { name: 'Tổng quan', href: '/admin/reports', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
      { name: 'Doanh thu', href: '/admin/reports/revenue', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
      { name: 'Hiệu suất', href: '/admin/reports/performance', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
    ],
  },
  {
    name: 'Cài đặt',
    href: '/admin/settings',
    icon: <FiSettings size={18} />,
    children: [
      { name: 'Tổng quan', href: '/admin/settings', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
      { name: 'Bảo mật', href: '/admin/settings/security', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
      { name: 'Thông báo', href: '/admin/settings/notifications', icon: <div className="w-2 h-2 bg-gray-400 rounded-full" /> },
    ],
  },
];

export default function AdminSidebar({ collapsed = false, onToggle }: AdminSidebarProps) {
  const [expandedItems, setExpandedItems] = useState<string[]>([]);
  const pathname = usePathname();

  const toggleExpanded = (itemName: string) => {
    setExpandedItems(prev =>
      prev.includes(itemName)
        ? prev.filter(name => name !== itemName)
        : [...prev, itemName]
    );
  };

  const isActive = (href: string) => {
    if (href === '/admin') {
      return pathname === '/admin';
    }
    return pathname.startsWith(href);
  };

  return (
    <aside className="h-full bg-white border-r border-purple-100 flex flex-col shadow-xl min-w-[220px] max-w-[250px]">
      {/* Header */}
      <div className="flex items-center justify-between h-14 px-4 border-b border-purple-100 flex-shrink-0">
        {!collapsed && (
          <div className="flex items-center gap-3">
            {/* Logo nhỏ */}
            <div className="w-9 h-9 bg-gradient-to-br from-purple-600 to-purple-700 rounded-xl flex items-center justify-center shadow-md">
              <img src="/favicon.ico" alt="logo" className="w-6 h-6 rounded" />
            </div>
            <span className="text-lg font-bold text-purple-700 tracking-wide">StudyIQ Admin</span>
          </div>
        )}
        <button
          onClick={onToggle}
          className="p-1.5 rounded-lg hover:bg-purple-50 transition-colors"
        >
          <svg className="w-4 h-4 text-purple-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
          </svg>
        </button>
      </div>

      {/* Navigation - Scrollable */}
      <nav className="flex-1 px-2 py-3 space-y-1 overflow-y-auto custom-scrollbar">
        {sidebarItems.map((item) => {
          const isItemActive = isActive(item.href);
          const isExpanded = expandedItems.includes(item.name);
          const hasChildren = item.children && item.children.length > 0;

          return (
            <div key={item.name}>
              {/* Main Item */}
              <div className="relative">
                <Link
                  href={item.href}
                  className={`group flex items-center px-3 py-2 text-[15px] font-medium rounded-lg transition-all duration-200 tracking-wide select-none
                    ${isItemActive
                      ? 'bg-gradient-to-r from-purple-100 to-purple-50 text-purple-700 border-l-4 border-purple-600 shadow'
                      : 'text-gray-700 hover:bg-purple-50 hover:text-purple-700'}
                  `}
                  onClick={() => hasChildren && toggleExpanded(item.name)}
                >
                  <div className={`flex items-center justify-center w-7 h-7 mr-2 rounded-lg transition-colors duration-200
                    ${isItemActive ? 'bg-purple-100 text-purple-700' : 'bg-gray-100 text-gray-400 group-hover:bg-purple-200 group-hover:text-purple-600'}
                  `}>
                    {item.icon}
                  </div>
                  {!collapsed && (
                    <>
                      <span className="flex-1 truncate">{item.name}</span>
                      {item.badge && (
                        <span className="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-pink-100 text-pink-700 ml-2">
                          {item.badge}
                        </span>
                      )}
                      {hasChildren && (
                        <FiChevronDown
                          className={`w-4 h-4 ml-2 transition-transform duration-200 ${isExpanded ? 'rotate-180' : ''}`}
                        />
                      )}
                    </>
                  )}
                </Link>
              </div>

              {/* Sub Items */}
              {!collapsed && hasChildren && isExpanded && (
                <div className="ml-7 mt-1 space-y-1">
                  {item.children!.map((child) => {
                    const isChildActive = isActive(child.href);
                    return (
                      <Link
                        key={child.name}
                        href={child.href}
                        className={`group flex items-center px-3 py-1.5 text-[14px] rounded-md transition-all duration-200 select-none
                          ${isChildActive
                            ? 'bg-purple-50 text-purple-700 font-semibold'
                            : 'text-gray-600 hover:bg-purple-50 hover:text-purple-700'}
                        `}
                      >
                        <div className={`w-2 h-2 rounded-full mr-2
                          ${isChildActive ? 'bg-purple-600' : 'bg-gray-400 group-hover:bg-purple-400'}
                        `} />
                        <span className="truncate">{child.name}</span>
                      </Link>
                    );
                  })}
                </div>
              )}
            </div>
          );
        })}
      </nav>

      {/* Footer */}
      <div className="p-3 border-t border-purple-100 flex-shrink-0">
        {!collapsed && (
          <div className="flex items-center gap-3 p-2 bg-gradient-to-r from-purple-50 to-white rounded-lg shadow-sm">
            <img
              src="https://ui-avatars.com/api/?name=Admin+User&background=7c3aed&color=fff&size=128"
              alt="avatar"
              className="w-9 h-9 rounded-full object-cover shadow-md border-2 border-purple-200"
            />
            <div className="flex-1 min-w-0">
              <p className="text-[15px] font-semibold text-purple-700 truncate">Admin User</p>
              <p className="text-xs text-gray-500 truncate">admin@studyiq.com</p>
            </div>
            <button className="p-1.5 rounded-lg hover:bg-purple-100 transition-colors" title="Đăng xuất">
              <FiLogOut className="w-5 h-5 text-purple-600" />
            </button>
          </div>
        )}
      </div>
    </aside>
  );
} 