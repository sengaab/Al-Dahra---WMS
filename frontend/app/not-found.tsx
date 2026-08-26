import Link from "next/link";
import Button from "../components/button";

export default function NotFound() {
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
          padding: "clamp(20px, 5vw, 30px)",
          borderRadius: "20px",
          gap: "10px",
          backgroundColor: "var(--beige)",
          boxSizing: "border-box",
          textAlign: "center",
        }}
      >
        <p className="big-text" style={{color: "var(--blood-red)"}}>Error 404</p>

        <p className="nav-item">Page Not Found</p>

        <p className="body-title">
          The page you're looking for doesn't exist.
        </p>

        <Link
          href="/dashboard"
          style={{
            textDecoration: "none",
          }}
        >
          <Button variant="secondary">
            Go back to Home
          </Button>
        </Link>
      </div>
    </div>
  );
}