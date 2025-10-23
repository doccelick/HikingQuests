const serverUrl = "/api/version";

type VersionData = {
    version: string;
}

export async function getAppVersion(): Promise<VersionData> {

    try {
        const response = await fetch(serverUrl);

        if (!response.ok) {
            const errorBody = await response.json().catch(() => ({}));
            throw new Error(errorBody.message || `HTTP ${response.status}`);
        }

        return await response.json() as VersionData;
    }
    catch (error) {
        console.error("Error fetching version: ", error)
        throw error;
    }
}