import { useParams } from "react-router-dom";
import { useCombinedGames } from "../hooks/queries";

export interface InstalledGamePageProps {}

export const InstalledGamePage = () => {
  const games = useCombinedGames();

  const { gameId } = useParams();

  if (!games) return null;

  const game = games.find((x) => x.name === gameId);

  if (!game) return null;

  return (
    <div className="container-fluid p-3">
      <h3>{game.title}</h3>
      <p>Publisher: {game.publisher}</p>
      <p>Released: {game.yearPublished}</p>
      <p>{game.description}</p>
      <a href="#" className="btn btn-primary">
        Play
      </a>
    </div>
  );
};
