import { useManifest } from "../hooks/queries";
import { CombinedGame } from "../models/manifest";

export interface InstalledGameCardProps {
  game: CombinedGame;
}

export const InstalledGameCard = ({ game }: InstalledGameCardProps) => {
  const { data } = useManifest();

  if (!data) return null;

  return (
    <div>
      <div className="card" style={{ width: "18rem" }}>
        <img
          src={`${data.rootUrl}/${game.name}/${game.posterUrl}`}
          className="card-img-top"
          style={{ width: "18rem" }}
          alt="Poster"
        />
        <div className="card-body">
          <h5 className="card-title">{game.title}</h5>
        </div>
      </div>
    </div>
  );
};
