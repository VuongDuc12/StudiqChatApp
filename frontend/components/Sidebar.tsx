'use client';

import React from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { 
  FiHome, 
  FiUsers, 
  FiBookOpen, 
  FiTag, 
  FiFileText, 
  FiCalendar,
  FiX,
  FiLogOut
} from 'react-icons/fi';

interface SidebarProps {
  sidebarOpen: boolean;
  setSidebarOpen: (open: boolean) => void;
}

const Sidebar: React.FC<SidebarProps> = ({ sidebarOpen, setSidebarOpen }) => {
  const pathname = usePathname();

  const menuItems = [
    {
      name: 'Dashboard',
      icon: FiHome,
      href: '/admin/dashboard',
      description: 'Tổng quan hệ thống'
    },
    {
      name: 'Người dùng',
      icon: FiUsers,
      href: '/admin/users',
      description: 'Quản lý người dùng'
    },
    {
      name: 'Môn học',
      icon: FiBookOpen,
      href: '/admin/subjects',
      description: 'Quản lý môn học'
    },
    {
      name: 'Khóa học',
      icon: FiBookOpen,
      href: '/admin/courses',
      description: 'Quản lý khóa học'
    },
    {
      name: 'Topic môn học',
      icon: FiTag,
      href: '/admin/topics',
      description: 'Quản lý topic môn học'
    },
    {
      name: 'Mẫu kế hoạch',
      icon: FiFileText,
      href: '/admin/plan-templates',
      description: 'Quản lý mẫu kế hoạch'
    },
    {
      name: 'Kế hoạch học tập',
      icon: FiCalendar,
      href: '/admin/studyplans',
      description: 'Quản lý kế hoạch học tập'
    }
  ];

  const isActive = (href: string) => {
    if (href === '/admin/dashboard') {
      return pathname === '/admin/dashboard' || pathname === '/admin';
    }
    return pathname.startsWith(href);
  };

  return (
    <>
      {/* Mobile overlay */}
      {sidebarOpen && (
        <div 
          className="fixed inset-0 z-40 bg-gray-600 bg-opacity-75 lg:hidden"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Sidebar */}
      <div className={`
        fixed inset-y-0 left-0 z-50 w-64 bg-white shadow-lg transform transition-transform duration-300 ease-in-out lg:translate-x-0 lg:static lg:inset-0
        ${sidebarOpen ? 'translate-x-0' : '-translate-x-full'}
      `}>
        <div className="flex flex-col h-full">
          {/* Header */}
          <div className="flex items-center justify-between h-16 px-6 border-b border-gray-200">
            <div className="flex items-center">
              <div className="w-8 h-8 bg-blue-600 rounded-lg flex items-center justify-center">
                <span className="text-white font-bold text-lg">S</span>
              </div>
              <span className="ml-3 text-xl font-bold text-gray-900">StudIQ</span>
            </div>
            <button
              onClick={() => setSidebarOpen(false)}
              className="lg:hidden p-2 rounded-lg text-gray-500 hover:text-gray-700 hover:bg-gray-100"
            >
              <FiX className="h-5 w-5" />
            </button>
          </div>

          {/* Navigation */}
          <nav className="flex-1 px-4 py-6 space-y-2 overflow-y-auto">
            {menuItems.map((item) => {
              const Icon = item.icon;
              const active = isActive(item.href);
              
              return (
                <Link
                  key={item.href}
                  href={item.href}
                  className={`
                    flex items-center px-4 py-3 text-sm font-medium rounded-xl transition-all duration-200 group
                    ${active 
                      ? 'bg-blue-50 text-blue-700 border border-blue-200' 
                      : 'text-gray-700 hover:bg-gray-50 hover:text-gray-900'
                    }
                  `}
                  onClick={() => setSidebarOpen(false)}
                >
                  <Icon className={`
                    h-5 w-5 mr-3 transition-colors duration-200
                    ${active ? 'text-blue-600' : 'text-gray-400 group-hover:text-gray-600'}
                  `} />
                  <div className="flex-1">
                    <div className="font-medium">{item.name}</div>
                    <div className={`
                      text-xs mt-0.5 transition-colors duration-200
                      ${active ? 'text-blue-500' : 'text-gray-400 group-hover:text-gray-500'}
                    `}>
                      {item.description}
                    </div>
                  </div>
                </Link>
              );
            })}
          </nav>

          {/* Footer */}
          <div className="p-4 border-t border-gray-200">
            <div className="flex items-center px-4 py-3">
              <div className="w-8 h-8 bg-gray-300 rounded-full flex items-center justify-center">
                <span className="text-gray-600 font-medium text-sm">A</span>
              </div>
              <div className="ml-3 flex-1">
                <div className="text-sm font-medium text-gray-900">Admin User</div>
                <div className="text-xs text-gray-500">admin@studyiq.com</div>
              </div>
            </div>
            
            <button className="w-full mt-3 flex items-center px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50 hover:text-gray-900 rounded-xl transition-all duration-200">
              <FiLogOut className="h-4 w-4 mr-3 text-gray-400" />
              Đăng xuất
            </button>
          </div>
        </div>
      </div>
    </>
  );
};

export default Sidebar; 