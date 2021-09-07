let peerConnection = createPeerConnection();

function createPeerConnection()
{
  let config = {
    iceServers: [{
      urls: "stun:stun.l.google.com:19302"
    }]
  };
  let peerConnection = new RTCPeerConnection(config);

  peerConnection.addEventListener('icecandidate', function (event) {
    if (event.candidate) {
      setTimeout(() => {
        log("Sending ICE candidate: " + JSON.stringify(event.candidate))
        android.SendIceCandidate(JSON.stringify(event.candidate));
      }, 3000);
    }
  });
  peerConnection.addEventListener('connectionstatechange', event => {
    log("Changed connection state: " + peerConnection.connectionState);
  });

  return peerConnection;
}

function receiveIceCandidate(iceCandidate) {
  log("Received ICE candidate: " + iceCandidate);

  peerConnection.addIceCandidate(JSON.parse(iceCandidate));
}

function log(message) {
  console.log("[JavaScript] " + message);
}

function alertError(error) {
  alert("ERROR. " + error.name + ": " + error.message);
}

// https://stackoverflow.com/a/27725393
function escapeCRLF(string) {
  return string
    .replace(/\r/g, "\\r")
    .replace(/\n/g, "\\n");
}
