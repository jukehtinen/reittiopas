const getStops = async function () {
    try {
        let response = await fetch(`${process.env.VUE_APP_API}/stops`);
        let data = await response.json();
        return data;
    } catch (err) {
        return null;
    }
}

const getRoute = async function (from, to) {
    try {
        let response = await fetch(`${process.env.VUE_APP_API}/route?from=${from}&to=${to}`);
        let data = await response.json();
        return data;
    } catch (err) {
        return null;
    }
}

export const api = {
    getStops, getRoute
}
