import React, { useEffect, useState } from "react";
import { getAppVersion } from "../services";

export const AppVersion: React.FC = () => {

    const [version, setVersion] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        getAppVersion()
            .then(data => setVersion(data.version))
            .catch(err => setError(err.message));
    }, []);

    if (error) {
        return <p className="error-message">v{error}</p>;
    }

    if (!version) {
        return <p className="info-message">v: Loading...</p>;
    }

    return <p className="app-version">v{version}</p>;
};