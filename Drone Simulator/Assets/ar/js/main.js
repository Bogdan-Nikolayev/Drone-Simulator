let peerConnection = createPeerConnection();

function createPeerConnection() {
  let config = {
    iceServers: [{
      urls: "stun:stun.l.google.com:19302"
    }]
  };
  let peerConnection = new RTCPeerConnection(config);
  // let peerConnection = new RTCPeerConnection();

  peerConnection.addEventListener('icecandidate', function (event) {
    if (event.candidate) {
      console.log("Sending ICE candidate: " + JSON.stringify(event.candidate));
      android.SendIceCandidate(JSON.stringify(event.candidate));
    }
  });
  peerConnection.addEventListener('connectionstatechange', event => {
    console.log("Changed connection state: " + peerConnection.connectionState);
  });

  return peerConnection;
}

function receiveIceCandidate(iceCandidate) {
  console.log("Received ICE candidate: " + iceCandidate);

  peerConnection.addIceCandidate(JSON.parse(iceCandidate));
}

function alertError(error) {
  alert("ERROR! " + error.name + ": " + error.message);
}

// https://stackoverflow.com/a/27725393
function escapeCRLF(string) {
  return string
    .replace(/\r/g, "\\r")
    .replace(/\n/g, "\\n");
}
