import { Stack } from "react-bootstrap";
import { Link } from "react-router-dom";

export const Sidebar = () => {
  return (
    <div className="navbar navbar-dark bg-dark box-shadow gx-0 vh-100">
      <div className="container h-100">
        <Stack>
          <Link to={`/`} className="navbar-brand d-flex align-items-top">
            Store
          </Link>
          <Link
            to={`/settings`}
            className="navbar-brand d-flex align-items-top"
          >
            Settings
          </Link>
        </Stack>
      </div>
    </div>
  );
};
