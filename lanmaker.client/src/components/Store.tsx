import { useCombinedGames } from "../hooks/queries";
import { StoreGameCard } from "./StoreGameCard";

export const Store = () => {
  const games = useCombinedGames();

  return (
    <div className="container p-3">
      <h3>Store</h3>
      <div className="gap-3" style={{ flexWrap: "wrap", display: "flex" }}>
        {games
          .filter((x) => !x.installed)
          .map((x) => (
            <StoreGameCard game={x} />
          ))}
      </div>
    </div>
  );
};
