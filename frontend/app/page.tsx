"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Button from "@/components/button";
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
      {/* =========================
          Left Section
          ========================= */}

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

      {/* =========================
          Login Section
          ========================= */}

      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "stretch",
          justifyContent: "center",
          width: "clamp(360px, 35vw, 520px)",
          minHeight: "100vh",
          padding: "clamp(30px, 5vw, 70px)",
          gap: "var(--space-8)",
          backgroundColor: "var(--beige)",
          boxSizing: "border-box",
        }}
      >
        {/* =========================
            Welcome
            ========================= */}

        <div
          style={{
            textAlign: "center",
          }}
        >
          <h1
            style={{
              color: "var(--midnight-blue)",
            }}
          >
            Welcome to
          </h1>

          <h4
            style={{
              marginTop: "var(--space-2)",
              color: "var(--midnight-blue)",
              lineHeight: 1.5,
            }}
          >
            Eldahra's Warehouse Management System
          </h4>
        </div>

        {/* =========================
            Form
            ========================= */}

        <div
          style={{
            display: "flex",
            flexDirection: "column",
            gap: "var(--space-5)",
            width: "100%",
          }}
        >
          {/* Email */}

          <div
            style={{
              width: "100%",
            }}
          >
            <label
              htmlFor="email"
              style={{
                display: "block",
                marginBottom: "var(--space-2)",
                fontFamily: "var(--font-roboto-serif)",
                fontSize: "var(--font-section-title)",
                fontWeight: "var(--weight-section-title)",
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
                height: "var(--input-height)",
                borderRadius: "var(--input-radius)",
                border: "var(--border-default)",
                borderColor: "var(--dark-green)",
                padding: "0 var(--input-padding-x)",
                boxSizing: "border-box",
                fontFamily: "var(--font-roboto)",
                fontSize: "var(--font-placeholder)",
                fontWeight: "var(--weight-placeholder)",
                backgroundColor: "var(--beige)",
                color: "var(--dark-green)",
                outline: "none",
              }}
            />
          </div>

          {/* Password */}

          <div
            style={{
              width: "100%",
            }}
          >
            <label
              htmlFor="password"
              style={{
                display: "block",
                marginBottom: "var(--space-2)",
                fontFamily: "var(--font-roboto-serif)",
                fontSize: "var(--font-section-title)",
                fontWeight: "var(--weight-section-title)",
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
                height: "var(--input-height)",
                borderRadius: "var(--input-radius)",
                border: "var(--border-default)",
                borderColor: "var(--dark-green)",
                padding: "0 var(--input-padding-x)",
                boxSizing: "border-box",
                fontFamily: "var(--font-roboto)",
                fontSize: "var(--font-placeholder)",
                fontWeight: "var(--weight-placeholder)",
                backgroundColor: "var(--beige)",
                color: "var(--dark-green)",
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

        {/* =========================
            Login Button
            ========================= */}

        <Button
          variant="primary"
          onClick={handleLogin}
          disabled={loading}
          style={{
            width: "100%",
            minHeight: "var(--button-height)",
          }}
        >
          {loading ? "Logging in..." : "Login to wms"}
        </Button>
      </div>
    </div>
  );
}