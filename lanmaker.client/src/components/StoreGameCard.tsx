import { useNavigate } from "react-router-dom";
import { useManifest } from "../hooks/queries";
import { StoreGame } from "../models/manifest";

export interface StoreGameCardProps {
  game: StoreGame;
}

export const StoreGameCard = ({ game }: StoreGameCardProps) => {
  const { data } = useManifest();
  const navigate = useNavigate();

  if (!data) return null;

  const navigateTo = () => {
    navigate(`/store/${game.name}`);
  };

  return (
    <div className="card" style={{ width: "18rem" }} onClick={navigateTo}>
      <img
        src={`${data.rootUrl}/${game.name}/${game.posterUrl}`}
        className="card-img-top"
        style={{
          width: "100%",
          maxHeight: "12rem",
          objectFit: "contain",
        }}
        alt="Poster"
      />
      <div className="card-body">
        <h5 className="card-title">{game.title}</h5>
      </div>
    </div>
  );
};
