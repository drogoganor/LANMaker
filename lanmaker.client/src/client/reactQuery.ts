import { useQuery } from "@tanstack/react-query";
import axios from "axios";

export const useSettingsQuery = () => {
  const query = useQuery({
    queryKey: ["Settings"],
    queryFn: () => {
      axios
        .get("http://localhost:8001/WeatherForecast")
        .then((res) => res.data);
    },
  });

  return query;
};
