import { useNavigate, useParams } from "react-router-dom";
import { useCombinedGames, useManifest } from "../hooks/queries";

export interface StoreGamePageProps {}

export const StoreGamePage = () => {
  const games = useCombinedGames();
  const { data: manifest } = useManifest();
  const navigate = useNavigate();

  const { gameId } = useParams();

  if (!manifest) return null;
  if (!games) return null;

  const game = games.find((x) => x.name === gameId);

  const navigateTo = () => {
    navigate("/store");
  };

  if (!game) return null;

  return (
    <div className="container-fluid p-3">
      <div className="mb-3">
        <a href="#" className="btn btn-primary" onClick={navigateTo}>
          Back to Store
        </a>
      </div>
      <div className="row align-items-start mb-3">
        <div className="col-3">
          <img
            src={`${manifest.rootUrl}/${game.name}/${game.posterUrl}`}
            className="card-img-top"
            style={{
              width: "100%",
              objectFit: "contain",
            }}
            alt="Poster"
          />
        </div>
        <div className="col">
          <h3>{game.title}</h3>
          <p>Publisher: {game.publisher}</p>
          <p>Released: {game.yearPublished}</p>
          <p>{game.description}</p>
          <a href="#" className="btn btn-primary">
            Install
          </a>
        </div>
      </div>
    </div>
  );
};
