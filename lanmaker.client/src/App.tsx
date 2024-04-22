import { Sidebar } from "./components/Sidebar";
import { Main } from "./components/Main";

function App() {
  return (
    <div className="container-fluid">
      <div className="row align-items-start">
        <div className="col-2 gx-0 vh-100">
          <Sidebar />
        </div>
        <div className="col-10 bg-light vh-100" style={{ overflowY: "scroll" }}>
          <Main />
        </div>
      </div>
    </div>
  );
}

export default App;
