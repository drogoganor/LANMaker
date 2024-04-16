import { useQuery } from "@tanstack/react-query";
import axios from "axios";

export const Settings = () => {
  const { isPending, error, data } = useQuery({
    queryKey: ["Settings"],
    queryFn: () =>
      axios
        .get("http://localhost:8001/WeatherForecast")
        .then((res) => res.data),
  });

  if (isPending) return "Loading...";

  if (error) return "An error has occurred: " + error.message;

  return (
    <div className="container p-3">
      <h3>Settings</h3>
      <p>Test</p>
      <p>{JSON.stringify(data)}</p>
    </div>
  );
};
