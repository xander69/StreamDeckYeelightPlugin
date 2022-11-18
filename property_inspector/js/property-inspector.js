var websocket = null,
    uuid = null,
    inInfo = null,
    actionInfo = {},
    settingsModel = {
    };

function connectElgatoStreamDeckSocket(inPort, inUUID, inRegisterEvent, inInfo, inActionInfo) {
    uuid = inUUID;
    actionInfo = JSON.parse(inActionInfo);
    inInfo = JSON.parse(inInfo);
    websocket = new WebSocket('ws://localhost:' + inPort);

    loadConfiguration(actionInfo.payload.settings.settingsModel);

    websocket.onopen = function () {
        var json = { event: inRegisterEvent, uuid: inUUID };
        websocket.send(JSON.stringify(json));
    };

    websocket.onmessage = function (evt) {
        var jsonObj = JSON.parse(evt.data);
        var sdEvent = jsonObj['event'];
        switch (sdEvent) {
            case "didReceiveSettings":
                loadConfiguration(jsonObj.payload.settings.settingsModel);
                break;
            default:
                break;
        }
    };
}

function loadConfiguration(payload) {
    for (var key in payload) {
        settingsModel[key] = payload[key];
        document.getElementById(key).value = payload[key];
    }
}

const setSettings = (value, param) => {
    if (websocket) {
        settingsModel[param] = value;
        var json = {
            "event": "setSettings",
            "context": uuid,
            "payload": {
                "settingsModel": settingsModel
            }
        };
        websocket.send(JSON.stringify(json));
    }
};
