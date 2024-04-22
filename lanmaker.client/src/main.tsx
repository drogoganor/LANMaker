import "bootstrap/dist/css/bootstrap.css";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { Settings } from "./components/Settings.tsx";
import { Store } from "./components/Store.tsx";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Games } from "./components/Games.tsx";
import { InstalledGamePage } from "./components/InstalledGamePage.tsx";
import { StoreGamePage } from "./components/StoreGamePage.tsx";

const queryClient = new QueryClient();

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "/",
        element: <Games />,
      },
    ],
  },
  {
    path: "/games/:gameId",
    element: <App />,
    children: [
      {
        path: "/games/:gameId",
        element: <InstalledGamePage />,
      },
    ],
  },
  {
    path: "/store",
    element: <App />,
    children: [
      {
        path: "/store",
        element: <Store />,
      },
    ],
  },
  {
    path: "/store/:gameId",
    element: <App />,
    children: [
      {
        path: "/store/:gameId",
        element: <StoreGamePage />,
      },
    ],
  },
  {
    path: "/settings",
    element: <App />,
    children: [
      {
        path: "/settings",
        element: <Settings />,
      },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  </React.StrictMode>
);
