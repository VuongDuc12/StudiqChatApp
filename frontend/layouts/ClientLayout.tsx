'use client';

import React, { useState, useEffect, useRef } from 'react';
import Link from 'next/link';
import { usePathname, useRouter } from 'next/navigation';
import { FiHome, FiInfo, FiBook, FiCalendar, FiUser, FiMenu, FiX, FiLogIn, FiZap, FiHeart, FiCoffee, FiUsers, FiTrendingUp, FiTarget, FiStar, FiSettings } from 'react-icons/fi';
// Bạn cần tạo components/Toast.tsx tương tự CustomToaster hoặc dùng react-hot-toast
import CustomToaster from '@/components/Toast';

interface ClientLayoutProps {
  children: React.ReactNode;
}

const navItems = [
  { to: '/', label: 'Trang chủ', icon: <FiHome className="w-5 h-5" />, color: 'from-blue-500 to-indigo-500' },
  { to: '/about', label: 'Giới thiệu', icon: <FiInfo className="w-5 h-5" />, color: 'from-purple-500 to-pink-500' },
  { to: '/courses', label: 'Khóa học', icon: <FiBook className="w-5 h-5" />, color: 'from-green-500 to-teal-500' },
  { to: '/planner', label: 'Lập kế hoạch', icon: <FiCalendar className="w-5 h-5" />, color: 'from-orange-500 to-red-500' },
  { to: '/exam-select', label: 'Ôn thi', icon: <FiStar className="w-5 h-5" />, color: 'from-yellow-400 to-pink-500' },
];

export default function ClientLayout({ children }: ClientLayoutProps) {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const pathname = usePathname();
  const [user, setUser] = useState<any>(null);
  const [showUserMenu, setShowUserMenu] = useState(false);
  const userMenuRef = useRef<HTMLDivElement>(null);
  const avatarBtnRef = useRef<HTMLButtonElement>(null);
  const menuRef = useRef<HTMLDivElement>(null);
  const [menuPos, setMenuPos] = useState<{top: number, left: number}>({top: 0, left: 0});
  const router = useRouter();

  useEffect(() => {
    const userData = typeof window !== 'undefined' ? localStorage.getItem('user') : null;
    if (userData) {
      setUser(JSON.parse(userData));
    } else {
      setUser(null);
    }
  }, [pathname]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        menuRef.current &&
        !menuRef.current.contains(event.target as Node) &&
        avatarBtnRef.current &&
        !avatarBtnRef.current.contains(event.target as Node)
      ) {
        setShowUserMenu(false);
      }
    };
    if (showUserMenu) {
      document.addEventListener('click', handleClickOutside);
    } else {
      document.removeEventListener('click', handleClickOutside);
    }
    return () => document.removeEventListener('click', handleClickOutside);
  }, [showUserMenu]);

  const handleAvatarClick = () => {
    if (avatarBtnRef.current) {
      const rect = avatarBtnRef.current.getBoundingClientRect();
      setMenuPos({
        top: rect.bottom + window.scrollY + 8,
        left: rect.left + window.scrollX,
      });
    }
    setShowUserMenu(v => !v);
  };

  const isActive = (path: string) => {
    if (path === '/exam-select') {
      return pathname.startsWith('/exam-select') || pathname.startsWith('/exam');
    }
    return pathname === path;
  };

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      
      {/* Header */}
      <header className="bg-white/80 backdrop-blur-sm shadow-lg border-b border-white/20 sticky top-0 z-50 relative overflow-hidden">
        {/* Animated Background */}
        <div className="absolute inset-0 overflow-hidden pointer-events-none">
          <div className="absolute top-0 left-0 w-32 h-32 bg-gradient-to-r from-blue-400 to-purple-400 rounded-full mix-blend-multiply filter blur-xl opacity-10 animate-blob"></div>
          <div className="absolute top-0 right-0 w-24 h-24 bg-gradient-to-r from-purple-400 to-pink-400 rounded-full mix-blend-multiply filter blur-xl opacity-10 animate-blob animation-delay-2000"></div>
        </div>
        <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-20">
            {/* Logo */}
            <div className="flex items-center">
              <Link href="/" className="flex items-center space-x-4 group">
                <div className="w-12 h-12 bg-gradient-to-r from-blue-600 via-purple-600 to-pink-600 rounded-2xl flex items-center justify-center shadow-lg group-hover:scale-110 transition-transform duration-300">
                  <FiZap className="w-7 h-7 text-white" />
                </div>
                <div>
                  <h1 className="text-2xl font-bold bg-gradient-to-r from-blue-600 via-purple-600 to-pink-600 bg-clip-text text-transparent">
                    StudIQ
                  </h1>
                  <p className="text-xs text-gray-600 font-medium">Nền tảng học tập số toàn quốc</p>
                </div>
              </Link>
            </div>
            {/* Desktop Navigation */}
            <nav className="hidden md:flex items-center gap-1 ml-6">
              {navItems.map((item) => (
                <Link
                  key={item.to}
                  href={item.to}
                  className={`group flex items-center gap-2 px-3 py-1.5 rounded-full font-medium text-[14px] transition-all duration-200 relative
                    ${isActive(item.to)
                      ? 'bg-gradient-to-r from-purple-500 to-pink-500 text-white shadow scale-105'
                      : 'text-gray-800 hover:text-purple-700 hover:bg-gradient-to-r hover:from-purple-100 hover:to-pink-100'}
                  `}
                  style={{ minWidth: 90 }}
                >
                  <span className={`p-1.5 rounded-full flex items-center justify-center transition-all duration-200
                    ${isActive(item.to)
                      ? 'bg-white/20'
                      : `bg-gradient-to-r ${item.color} opacity-80 group-hover:opacity-100`}
                  `}>
                    {React.cloneElement(item.icon, { className: 'w-4 h-4' })}
                  </span>
                  <span>{item.label}</span>
                </Link>
              ))}
            </nav>
            {/* Auth Buttons */}
            <div className="hidden md:flex items-center gap-2 ml-4">
              <Link
                href="/login"
                className="px-3 py-1.5 rounded-full border-2 border-purple-500 text-purple-700 font-semibold bg-white hover:bg-purple-50 hover:text-purple-700 transition-all duration-200 text-[14px]"
              >
                Đăng nhập
              </Link>
              <Link
                href="/register"
                className="px-5 py-1.5 rounded-full bg-gradient-to-r from-blue-500 to-pink-500 text-white font-bold shadow hover:scale-105 transition-all duration-200 flex items-center gap-2 text-[14px]"
              >
                <FiZap className="w-4 h-4" /> Đăng ký
              </Link>
            </div>
            {/* Mobile menu button */}
            <div className="md:hidden">
              <button
                onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
                className="p-3 rounded-2xl text-gray-700 hover:text-purple-600 hover:bg-white/50 transition-all duration-300 group"
              >
                {isMobileMenuOpen ? (
                  <FiX className="w-6 h-6 group-hover:scale-110 transition-transform" />
                ) : (
                  <FiMenu className="w-6 h-6 group-hover:scale-110 transition-transform" />
                )}
              </button>
            </div>
          </div>
        </div>
        {/* Mobile Navigation */}
        {isMobileMenuOpen && (
          <div className="md:hidden border-t border-white/20 bg-white/90 backdrop-blur-sm">
            <div className="px-4 pt-4 pb-6 space-y-2">
              {navItems.map((item) => (
                <Link
                  key={item.to}
                  href={item.to}
                  onClick={() => setIsMobileMenuOpen(false)}
                  className={`group flex items-center space-x-3 px-4 py-3 rounded-2xl text-base font-medium transition-all duration-300 ${
                    isActive(item.to)
                      ? 'bg-gradient-to-r from-purple-600 via-pink-600 to-red-600 text-white shadow-lg'
                      : 'text-gray-700 hover:text-purple-600 hover:bg-white/50'
                  }`}
                >
                  <div className={`w-10 h-10 rounded-xl flex items-center justify-center ${
                    isActive(item.to)
                      ? 'bg-white/20'
                      : `bg-gradient-to-r ${item.color} opacity-60`
                  }`}>
                    <div className="text-white">
                      {item.icon}
                    </div>
                  </div>
                  <span>{item.label}</span>
                </Link>
              ))}
              <div className="pt-4 pb-3 border-t border-white/20 space-y-2">
                <Link
                  href="/login"
                  onClick={() => setIsMobileMenuOpen(false)}
                  className="flex items-center space-x-3 px-4 py-3 text-gray-700 hover:text-purple-600 hover:bg-white/50 rounded-2xl transition-all duration-300"
                >
                  <FiLogIn className="w-5 h-5" />
                  <span>Đăng nhập</span>
                </Link>
                <Link
                  href="/register"
                  onClick={() => setIsMobileMenuOpen(false)}
                  className="block bg-gradient-to-r from-blue-600 via-purple-600 to-pink-600 text-white px-4 py-3 rounded-2xl font-medium text-center hover:from-blue-700 hover:via-purple-700 hover:to-pink-700 transition-all duration-300 shadow-lg"
                >
                  Đăng ký
                </Link>
              </div>
            </div>
          </div>
        )}
      </header>
      {/* Main Content */}
      <main className="flex-1">
        {children}
      </main>
      {/* Footer */}
      <footer className="bg-gradient-to-r from-gray-900 via-purple-900 to-gray-900 text-white relative overflow-hidden">
        {/* Animated Background */}
        <div className="absolute inset-0 overflow-hidden pointer-events-none">
          <div className="absolute bottom-0 left-0 w-64 h-64 bg-gradient-to-r from-purple-400 to-pink-400 rounded-full mix-blend-multiply filter blur-xl opacity-10 animate-blob"></div>
          <div className="absolute top-0 right-0 w-48 h-48 bg-gradient-to-r from-blue-400 to-purple-400 rounded-full mix-blend-multiply filter blur-xl opacity-10 animate-blob animation-delay-4000"></div>
        </div>
        <div className="relative max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
            <div>
              <div className="flex items-center space-x-3 mb-6">
                <div className="w-12 h-12 bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500 rounded-2xl flex items-center justify-center shadow-lg">
                  <FiZap className="w-7 h-7 text-white" />
                </div>
                <div>
                  <h3 className="text-xl font-bold bg-gradient-to-r from-white to-purple-200 bg-clip-text text-transparent">
                    StudIQ
                  </h3>
                  <p className="text-purple-300 text-sm">Học tập thông minh – Ôn thi dễ dàng</p>
                </div>
              </div>
              <p className="text-gray-300 text-sm leading-relaxed">
                Nền tảng học tập số hóa giúp sinh viên đại học trên khắp cả nước tìm tài liệu học tập, luyện đề trắc nghiệm, tạo flashcards và kết nối với cộng đồng sinh viên cùng ngành.
              </p>
            </div>
            <div>
              <h4 className="font-bold mb-6 text-lg flex items-center">
                <FiTarget className="w-5 h-5 mr-2 text-purple-400" />
                Tính năng
              </h4>
              <ul className="space-y-3 text-sm text-gray-300">
                <li className="flex items-center hover:text-purple-300 transition-colors">
                  <FiStar className="w-4 h-4 mr-2 text-purple-400" />
                  Quản lý môn học thông minh
                </li>
                <li className="flex items-center hover:text-purple-300 transition-colors">
                  <FiStar className="w-4 h-4 mr-2 text-purple-400" />
                  Lập kế hoạch học tập AI
                </li>
                <li className="flex items-center hover:text-purple-300 transition-colors">
                  <FiStar className="w-4 h-4 mr-2 text-purple-400" />
                  Theo dõi tiến độ real-time
                </li>
                <li className="flex items-center hover:text-purple-300 transition-colors">
                  <FiStar className="w-4 h-4 mr-2 text-purple-400" />
                  Gamification & Huy hiệu
                </li>
              </ul>
            </div>
            <div>
              <h4 className="font-bold mb-6 text-lg flex items-center">
                <FiUsers className="w-5 h-5 mr-2 text-purple-400" />
                Hỗ trợ
              </h4>
              <ul className="space-y-3 text-sm text-gray-300">
                <li className="flex items-center hover:text-purple-300 transition-colors">
                  <FiCoffee className="w-4 h-4 mr-2 text-purple-400" />
                  Hướng dẫn sử dụng
                </li>
                <li className="flex items-center hover:text-purple-300 transition-colors">
                  <FiCoffee className="w-4 h-4 mr-2 text-purple-400" />
                  FAQ & Tài liệu
                </li>
                <li className="flex items-center hover:text-purple-300 transition-colors">
                  <FiCoffee className="w-4 h-4 mr-2 text-purple-400" />
                  Liên hệ hỗ trợ 24/7
                </li>
                <li className="flex items-center hover:text-purple-300 transition-colors">
                  <FiCoffee className="w-4 h-4 mr-2 text-purple-400" />
                  Phản hồi & Góp ý
                </li>
              </ul>
            </div>
            <div>
              <h4 className="font-bold mb-6 text-lg flex items-center">
                <FiHeart className="w-5 h-5 mr-2 text-purple-400" />
                Liên hệ
              </h4>
              <ul className="space-y-3 text-sm text-gray-300">
                <li className="flex items-center">
                  <FiTrendingUp className="w-4 h-4 mr-2 text-purple-400" />
                  support@tlustudyplanner.com
                </li>
                <li className="flex items-center">
                  <FiTrendingUp className="w-4 h-4 mr-2 text-purple-400" />
                  (024) 1234-5678
                </li>
                <li className="flex items-center">
                  <FiTrendingUp className="w-4 h-4 mr-2 text-purple-400" />
                  Đại học Thủy Lợi, Hà Nội
                </li>
                <li className="flex items-center">
                  <FiTrendingUp className="w-4 h-4 mr-2 text-purple-400" />
                  Giờ làm việc: 8:00 - 18:00
                </li>
              </ul>
            </div>
          </div>
          <div className="border-t border-white/10 mt-12 pt-8 text-center">
            <p className="text-gray-400 text-sm">
              &copy; 2024 TLU Study Planner. Tất cả quyền được bảo lưu. 
              <span className="text-purple-400 ml-2">Made with Ducdev04 for TLU Students</span>
            </p>
          </div>
        </div>
      </footer>
    </div>
  );
}
