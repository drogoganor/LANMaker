export interface Manifest {
  rootUrl: string;
  manifestFile: string;
  games: StoreGame[];
}

export interface StoreGame {
  name: string;
  title: string;
  version: string;
  multiplayer: boolean;
  portable: boolean;
  posterUrl: string;
  exeName: string;
  zipUrl: string;
  screenshotUrls: string[];
  description: string;
  publisher: string;
  yearPublished: number;
}

export interface Config {
  manifestUrl: string;
  timeoutMinutes: number;
  storagePath: string;
  themeName: string;
  installedGames: InstalledGame[];
}

export interface InstalledGame {
  name: string;
  installedVersion: string;
  installPath: string;
  exePath: string;
}

export interface CombinedGame extends InstalledGame, StoreGame {
  installed: boolean;
}
