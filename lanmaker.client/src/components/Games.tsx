import { useCombinedGames } from "../hooks/queries";
import { InstalledGameCard } from "./InstalledGameCard";

export const Games = () => {
  const games = useCombinedGames();

  return (
    <div className="container-fluid p-3">
      <h3>Games</h3>
      <div className="gap-3" style={{ flexWrap: "wrap", display: "flex" }}>
        {games
          .filter((x) => !!x.installed)
          .map((x) => {
            return <InstalledGameCard game={x} />;
          })}
      </div>
    </div>
  );
};
