import { Card } from "react-bootstrap";
import { useManifest } from "../hooks/queries";

export const Store = () => {
  const { isPending, error, data } = useManifest();

  if (isPending) return "Loading...";

  if (error) return "An error has occurred: " + error.message;

  return (
    <div className="container p-3">
      <h3>Store</h3>
      {data.games.map((x) => (
        <Card>
          <h3>{x.title}</h3>
        </Card>
      ))}
    </div>
  );
};
