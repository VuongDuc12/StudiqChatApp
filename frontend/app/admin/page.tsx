'use client';
import toast from "react-hot-toast";

export default function AdminHomePage() {
  return <div><button
  className="px-6 py-3 bg-gradient-to-r from-blue-600 via-purple-600 to-pink-600 text-white rounded-2xl font-medium hover:from-blue-700 hover:via-purple-700 hover:to-pink-700 transition-all duration-300 shadow-lg"
  onClick={() => toast.success('Toast hoạt động thành công!')}
>
  Test Toast
</button>


  </div>;
} 