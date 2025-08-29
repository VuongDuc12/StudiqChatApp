'use client';
import { toast } from 'react-hot-toast';

export default function HomePage() {
  return (
    <div className="p-8 text-center">
      <h1 className="text-3xl font-bold mb-4">Trang ClientLayout Test</h1>
      <p className="mb-6">Đây là trang test cho area client. Layout, menu, footer, toast... sẽ được kế thừa tự động.</p>
      <button
        className="px-6 py-3 bg-gradient-to-r from-blue-600 via-purple-600 to-pink-600 text-white rounded-2xl font-medium hover:from-blue-700 hover:via-purple-700 hover:to-pink-700 transition-all duration-300 shadow-lg"
        onClick={() => toast.success('Toast hoạt động thành công!')}
      >
        Test Toast
      </button>
    </div>
  );
} 