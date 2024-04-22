import { useConfig } from "../hooks/queries";

export const Settings = () => {
  const { isPending, error, data } = useConfig();

  if (isPending) return "Loading...";

  if (error) return "An error has occurred: " + error.message;

  if (!data) return null;

  return (
    <div className="container p-3">
      <h3>Settings</h3>
      <form>
        <div className="row g-3">
          <div className="col-auto mb-3">
            <label htmlFor="manifestUrl" className="form-label">
              Manifest URL
            </label>
            <input
              className="form-control"
              id="manifestUrl"
              placeholder="https://your.manifest.url"
            />
          </div>
          <div className="row g-3">
            <div className="col-auto">
              <button type="submit" className="btn btn-primary mb-3">
                Save
              </button>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
};
