import { Sidebar } from "./components/Sidebar";
import { Main } from "./components/Main";

function App() {
  return (
    <div className="container-fluid">
      <div className="row align-items-start">
        <div className="col-2 gx-0 vh-100">
          <Sidebar />
        </div>
        <div className="col bg-light vh-100">
          <Main />
        </div>
      </div>
    </div>
  );
}

export default App;
