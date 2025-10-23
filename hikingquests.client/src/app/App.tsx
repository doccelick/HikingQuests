import { AppVersion, QuestList } from "../features/quests";
import "../assets/styles/globals.css";

const App: React.FC = () => {
    return (
        <div className="app-container">
            <h1>Hiking Quests <br></br> Dashboard</h1>
            <AppVersion />
            <QuestList />
        </div >
    );
};

export default App;