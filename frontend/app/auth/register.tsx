"use client";
import React, { useState } from "react";
import { useRouter } from "next/navigation";

export default function RegisterPage() {
  const [form, setForm] = useState({
    username: "",
    email: "",
    password: "",
    fullName: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const router = useRouter();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError("");
    try {
      const res = await fetch("http://localhost:5210/api/Auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(form),
      });
      const data = await res.json();
      if (!res.ok || !data.success) throw new Error(data.message || "Đăng ký thất bại");
      router.push("/auth?registered=1");
    } catch (err: any) {
      setError(err.message || "Đăng ký thất bại");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-[#23272a]">
      <form onSubmit={handleSubmit} className="bg-[#36393f] p-8 rounded-xl shadow-lg w-full max-w-md flex flex-col gap-4">
        <h2 className="text-2xl font-bold text-white mb-2">Đăng ký tài khoản</h2>
        <input name="username" placeholder="Tên đăng nhập" value={form.username} onChange={handleChange} required className="px-4 py-2 rounded bg-[#23272a] text-white" />
        <input name="email" type="email" placeholder="Email" value={form.email} onChange={handleChange} required className="px-4 py-2 rounded bg-[#23272a] text-white" />
        <input name="fullName" placeholder="Họ tên" value={form.fullName} onChange={handleChange} required className="px-4 py-2 rounded bg-[#23272a] text-white" />
        <input name="password" type="password" placeholder="Mật khẩu" value={form.password} onChange={handleChange} required className="px-4 py-2 rounded bg-[#23272a] text-white" />
        {error && <div className="text-red-400 text-sm">{error}</div>}
        <button type="submit" className="bg-indigo-500 hover:bg-indigo-600 text-white font-bold py-2 rounded transition" disabled={loading}>
          {loading ? "Đang đăng ký..." : "Đăng ký"}
        </button>
        <div className="text-gray-400 text-sm text-center mt-2">
          Đã có tài khoản? <a href="/auth" className="text-indigo-400 hover:underline">Đăng nhập</a>
        </div>
      </form>
    </div>
  );
}
