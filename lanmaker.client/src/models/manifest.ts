export interface Manifest {
  games: StoreGame[];
}

export interface StoreGame {
  name: string;
}

export interface Settings {
  manifestUrl: string;
}
