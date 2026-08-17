"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Button from "./components/button";
import { createClient } from "@/lib/supabase/client";

export default function Home() {
  const router = useRouter();
  const supabase = createClient();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async () => {
    setError("");
    setLoading(true);

    if (!email || !password) {
      setError("Please enter your email and password.");
      setLoading(false);
      return;
    }

    // =========================
    // Supabase Login
    // =========================

    const { data, error } = await supabase.auth.signInWithPassword({
      email,
      password,
    });

    if (error) {
      console.error("Supabase login error:", error);
      setError(error.message);
      setLoading(false);
      return;
    }

    // =========================
    // Get Session
    // =========================

    const session = data.session;

    if (!session) {
      setError("Login succeeded but no session was created.");
      setLoading(false);
      return;
    }

    // =========================
    // Get Access Token
    // =========================

    console.log("SESSION:", session);

    console.log("ACCESS TOKEN:", session.access_token);

    // =========================
    // Test .NET Backend
    // =========================

    try {
      const response = await fetch(
        "http://localhost:5171/api/Auth/me",
        {
          method: "GET",
          headers: {
            Authorization: `Bearer ${session.access_token}`,
            "Content-Type": "application/json",
          },
        }
      );

      const responseText = await response.text();

      console.log("Backend status:", response.status);
      console.log("Backend raw response:", responseText);

      if (!response.ok) {
        setError(
          `Backend authentication failed (${response.status})`
        );

        setLoading(false);
        return;
      }

      let backendData = null;

      if (responseText) {
        try {
          backendData = JSON.parse(responseText);
        } catch (error) {
          console.error("Invalid JSON from backend:", error);
        }
      }

      console.log("Backend response:", backendData);

      // =========================
      // Login Successful
      // =========================

      console.log("Authentication successful!");

      router.push("/dashboard");
    } catch (backendError) {
      console.error("Backend connection error:", backendError);

      setError(
        "Could not connect to the WMS backend."
      );

      setLoading(false);
    }
  };

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "row",
        alignItems: "stretch",
        width: "100%",
        minHeight: "100vh",
        backgroundColor: "var(--dark-green)",
        boxSizing: "border-box",
        overflow: "hidden",
      }}
    >
      {/* Left Section */}
      <div
        style={{
          position: "relative",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          minWidth: 0,
          flex: 1,
          minHeight: "100vh",
          backgroundImage: "url('/bg.svg')",
          backgroundRepeat: "no-repeat",
          backgroundSize: "cover",
          backgroundPosition: "center",
          overflow: "hidden",
        }}
      >
        <img
          src="/logo.svg"
          alt="Aldahra WMS"
          style={{
            width: "clamp(180px, 30vw, 400px)",
            height: "auto",
            zIndex: 1,
          }}
        />
      </div>

      {/* Login Section */}
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "stretch",
          justifyContent: "center",
          width: "clamp(360px, 35vw, 520px)",
          minHeight: "100vh",
          padding: "clamp(30px, 5vw, 70px)",
          gap: "30px",
          backgroundColor: "var(--beige)",
          boxSizing: "border-box",
        }}
      >
        {/* Welcome */}
        <div
          style={{
            textAlign: "center",
          }}
        >
          <h3
            style={{
              color: "var(--midnight-blue)",
              margin: 0,
            }}
          >
            Welcome to
          </h3>

          <p
            style={{
              margin: "8px 0 0",
              color: "var(--midnight-blue)",
              fontFamily: "var(--font-roboto)",
              fontSize: "16px",
              lineHeight: 1.5,
            }}
          >
            Eldahra's Warehouse Management System
          </p>
        </div>

        {/* Form */}
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            gap: "20px",
            width: "100%",
          }}
        >
          {/* Email */}
          <div style={{ width: "100%" }}>
            <label
              htmlFor="email"
              style={{
                display: "block",
                marginBottom: "8px",
                fontFamily: "var(--font-roboto-serif)",
                fontSize: "14px",
                fontWeight: "600",
                color: "var(--dark-green)",
              }}
            >
              Email
            </label>

            <input
              id="email"
              type="email"
              placeholder="Enter your email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  handleLogin();
                }
              }}
              style={{
                width: "100%",
                height: "42px",
                borderRadius: "5px",
                border: "1px solid var(--dark-green)",
                padding: "0 12px",
                boxSizing: "border-box",
                fontFamily: "var(--font-roboto)",
                fontSize: "14px",
                backgroundColor: "var(--beige)",
                outline: "none",
              }}
            />
          </div>

          {/* Password */}
          <div style={{ width: "100%" }}>
            <label
              htmlFor="password"
              style={{
                display: "block",
                marginBottom: "8px",
                fontFamily: "var(--font-roboto-serif)",
                fontSize: "14px",
                fontWeight: "600",
                color: "var(--dark-green)",
              }}
            >
              Password
            </label>

            <input
              id="password"
              type="password"
              placeholder="Enter your password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Enter") {
                  handleLogin();
                }
              }}
              style={{
                width: "100%",
                height: "42px",
                borderRadius: "5px",
                border: "1px solid var(--dark-green)",
                padding: "0 12px",
                boxSizing: "border-box",
                fontFamily: "var(--font-roboto)",
                fontSize: "14px",
                backgroundColor: "var(--beige)",
                outline: "none",
              }}
            />
          </div>

          {/* Error */}
          {error && (
            <p
              style={{
                margin: "-5px 0 0",
                color: "#B42318",
                fontFamily: "var(--font-roboto)",
                fontSize: "13px",
              }}
            >
              {error}
            </p>
          )}
        </div>

        {/* Login Button */}
        <Button
          variant="primary"
          onClick={handleLogin}
          disabled={loading}
          style={{
            width: "100%",
          }}
        >
          {loading ? "Logging in..." : "Login to wms"}
        </Button>
      </div>
    </div>
  );
}