import type { Metadata } from "next";
import { Roboto, Roboto_Mono, Roboto_Serif } from "next/font/google";
import "../styles/globals.css";
import "../styles/variables.css";

const roboto = Roboto({
  variable: "--font-roboto",
  subsets: ["latin"],
});

const robotoMono = Roboto_Mono({
  variable: "--font-roboto-mono",
  subsets: ["latin"],
});

const robotoSerif = Roboto_Serif({
  variable: "--font-roboto-serif",
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "Al Dahra WMS",
  description: "Al Dahra Warehouse Management System",
};

export default function RootLayout({ children }: LayoutProps<"/">) {
  return (
    <html lang="en">
      <body
        className={`${roboto.variable} ${robotoMono.variable} ${robotoSerif.variable}`}
      >
        {children}
      </body>
    </html>
  );
}
