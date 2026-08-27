"use client";

import React, { useState } from "react";

type ButtonVariant = "primary" | "secondary" | "outline" | "ghost";

interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  children: React.ReactNode;
}

export default function Button({
  variant = "primary",
  children,
  style,
  ...props
}: ButtonProps) {
  const [isHovered, setIsHovered] = useState(false);

  const variants: Record<
    ButtonVariant,
    {
      normal: React.CSSProperties;
      hover: React.CSSProperties;
    }
  > = {
    primary: {
      normal: {
        backgroundColor: "var(--midnight-blue)",
        color: "var(--beige)",
      },
      hover: {
        backgroundColor: "#171E24",
      },
    },

    secondary: {
      normal: {
        backgroundColor: "var(--dark-green)",
        color: "var(--beige)",
      },
      hover: {
        backgroundColor: "var(--dahra-green)",
        color: "white",
      },
    },

    outline: {
      normal: {
        backgroundColor: "transparent",
        color: "var(--dark-green)",
        border: "2px solid var(--dark-green)",
      },
      hover: {
        backgroundColor: "var(--dark-green)",
        color: "white",
      },
    },

    ghost: {
      normal: {
        backgroundColor: "transparent",
        color: "var(--dark-green)",
      },
      hover: {
        backgroundColor: "rgba(0, 0, 0, 0.05)",
      },
    },
  };

  const currentVariant = variants[variant];

  return (
    <button
      {...props}
      onMouseEnter={(e) => {
        setIsHovered(true);
        props.onMouseEnter?.(e);
      }}
      onMouseLeave={(e) => {
        setIsHovered(false);
        props.onMouseLeave?.(e);
      }}
      className="body-title"
      style={{
        ...currentVariant.normal,
        ...(isHovered ? currentVariant.hover : {}),

        borderRadius: "8px",
        fontWeight: 400,
        cursor: "pointer",
        transition: "all 0.2s ease",
        textWrap: "nowrap",
        height: "var(--input-height)",
        paddingInline: "var(--input-padding-x)",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        gap: "var(--space-2)",
        border: variant === "outline"
          ? "2px solid var(--dark-green)"
          : "none",

        outline: "none",

        ...style,
      }}
    >
      {children}
    </button>
  );
}