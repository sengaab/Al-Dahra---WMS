"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { createClient } from "@/lib/supabase/client";
import Button from "../components/button";

export default function Dashboard() {
  const router = useRouter();
  const supabase = createClient();

  const [email, setEmail] = useState("");
  const [accessToken, setAccessToken] = useState("");
  const [loading, setLoading] = useState(true);
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    const getSession = async () => {
      const {
        data: { session },
      } = await supabase.auth.getSession();

      if (!session) {
        router.push("/");
        return;
      }

      setEmail(session.user.email ?? "");
      setAccessToken(session.access_token);
      setLoading(false);
    };

    getSession();
  }, [router, supabase]);

  const handleCopy = async () => {
    if (!accessToken) return;

    await navigator.clipboard.writeText(accessToken);

    setCopied(true);

    setTimeout(() => {
      setCopied(false);
    }, 2000);
  };

  const handleLogout = async () => {
    await supabase.auth.signOut();

    router.push("/");
  };

  if (loading) {
    return (
      <div
        style={{
          minHeight: "100vh",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          backgroundColor: "var(--beige)",
          color: "var(--dark-green)",
          fontFamily: "var(--font-roboto)",
        }}
      >
        Loading dashboard...
      </div>
    );
  }

  return (
    <div
      style={{
        minHeight: "100vh",
        backgroundColor: "var(--beige)",
        padding: "clamp(20px, 5vw, 60px)",
        boxSizing: "border-box",
        fontFamily: "var(--font-roboto)",
      }}
    >
      {/* Header */}
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          gap: "20px",
          marginBottom: "40px",
          flexWrap: "wrap",
        }}
      >
        <div>
          <h1
            style={{
              margin: 0,
              color: "var(--dark-green)",
              fontFamily: "var(--font-roboto-serif)",
            }}
          >
            WMS Dashboard
          </h1>

          <p
            style={{
              margin: "8px 0 0",
              color: "var(--midnight-blue)",
            }}
          >
            Welcome, {email}
          </p>
        </div>

        <Button
          variant="outline"
          onClick={handleLogout}
        >
          Logout
        </Button>
      </div>

      {/* Authentication Card */}
      <div
        style={{
          width: "100%",
          maxWidth: "1000px",
          margin: "0 auto",
          padding: "clamp(20px, 4vw, 40px)",
          backgroundColor: "white",
          borderRadius: "12px",
          boxSizing: "border-box",
          boxShadow: "0 4px 20px rgba(0, 0, 0, 0.08)",
        }}
      >
        <h2
          style={{
            margin: "0 0 10px",
            color: "var(--dark-green)",
            fontFamily: "var(--font-roboto-serif)",
          }}
        >
          Authentication
        </h2>

        <p
          style={{
            margin: "0 0 25px",
            color: "var(--midnight-blue)",
            fontSize: "14px",
          }}
        >
          Your Supabase authentication information.
        </p>

        {/* Email */}
        <div
          style={{
            marginBottom: "25px",
          }}
        >
          <label
            style={{
              display: "block",
              marginBottom: "8px",
              fontWeight: "600",
              color: "var(--dark-green)",
            }}
          >
            Logged-in User
          </label>

          <div
            style={{
              padding: "12px",
              border: "1px solid #ccc",
              borderRadius: "6px",
              backgroundColor: "var(--beige)",
              wordBreak: "break-word",
            }}
          >
            {email}
          </div>
        </div>

        {/* Access Token */}
        <div>
          <div
            style={{
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              gap: "10px",
              marginBottom: "8px",
              flexWrap: "wrap",
            }}
          >
            <label
              style={{
                fontWeight: "600",
                color: "var(--dark-green)",
              }}
            >
              Supabase Access Token
            </label>

            <button
              type="button"
              onClick={handleCopy}
              style={{
                border: "none",
                background: "transparent",
                color: "var(--dark-green)",
                cursor: "pointer",
                fontFamily: "var(--font-roboto)",
                fontWeight: "600",
              }}
            >
              {copied ? "Copied!" : "Copy Token"}
            </button>
          </div>

          <textarea
            value={accessToken}
            readOnly
            rows={8}
            style={{
              width: "100%",
              boxSizing: "border-box",
              padding: "14px",
              border: "1px solid #ccc",
              borderRadius: "6px",
              backgroundColor: "#f5f5f5",
              color: "#333",
              fontFamily: "monospace",
              fontSize: "12px",
              lineHeight: "1.5",
              resize: "vertical",
              outline: "none",
              wordBreak: "break-all",
            }}
          />

          <p
            style={{
              marginTop: "10px",
              fontSize: "12px",
              color: "#777",
            }}
          >
            Development/testing only. This is your user's access token.
          </p>
        </div>
      </div>
    </div>
  );
}