import axios from "axios";
import { CombinedGame, Config, Manifest } from "../models/manifest";
import { useQuery } from "@tanstack/react-query";

export const useConfig = () => {
  return useQuery<Config>({
    queryKey: ["Config"],
    queryFn: () =>
      axios.get("http://localhost:8001/Configuration").then((res) => res.data),
  });
};

export const useManifest = () => {
  return useQuery<Manifest>({
    queryKey: ["Manifest"],
    queryFn: () =>
      axios.get("http://localhost:8001/Manifest").then((res) => res.data),
  });
};

export const useRunGame = () => {
  return (name: string) =>
    axios.get("http://localhost:8001/GameRun", { params: { name } });
};

export const useCombinedGames = () => {
  const {
    isPending: isConfigPending,
    error: configError,
    data: configData,
  } = useConfig();

  const {
    isPending: isManifestPending,
    error: manifestError,
    data: manifestData,
  } = useManifest();

  if (isManifestPending || isConfigPending) return [];

  if (manifestError || configError) return [];

  return manifestData.games.map((m) => {
    const installedGame = configData.installedGames.find(
      (x) => x.name === m.name
    );

    return {
      ...m,
      ...installedGame,
      installed: !!installedGame,
    } as CombinedGame;
  });
};
