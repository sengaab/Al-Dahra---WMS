import Button from "./components/button";

export default function Home() {
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
          textAlign:"center"
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
        </div>

        {/* Login Button */}
        <Button
          variant="primary"
          style={{
            width: "100%",
          }}
        >
          Login to wms
        </Button>
      </div>
    </div>
  );
}