"use client";

import { useEffect } from "react";
import Button from "../components/button";

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    console.error(error);
  }, [error]);

  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        backgroundImage: "url('/bg.svg')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        backgroundRepeat: "no-repeat",
        width: "100%",
        minHeight: "100vh",
        padding: "20px",
        boxSizing: "border-box",
      }}
    >
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          width: "100%",
          maxWidth: "525px",
          padding: "clamp(30px, 5vw, 45px)",
          borderRadius: "20px",
          gap: "10px",
          backgroundColor: "var(--beige)",
          boxSizing: "border-box",
          color: "black",
          textAlign: "center",
        }}
      >
        <p className="big-text"
          style={{
            margin: 0,
            color:"var(--blood-red)",
          }}
        >
         Error 500
        </p>

        <p
          className="nav-item"
        >
          Something Went Wrong
        </p>

        <p
          className="body-title"
          style={{
            margin: "5px 0 15px",
          }}
        >
          Something went wrong while processing your request. Please try
          again.
        </p>

        <Button
          variant="secondary"
          onClick={() => reset()}
        >
          Try Again
        </Button>
      </div>
    </div>
  );
}