import { useConfig } from "../hooks/queries";

export const Settings = () => {
  const { isPending, error, data } = useConfig();

  if (isPending) return "Loading...";

  if (error) return "An error has occurred: " + error.message;

  return (
    <div className="container p-3">
      <h3>Settings</h3>
      <p>Test</p>
      <p>{JSON.stringify(data)}</p>
    </div>
  );
};
