const config = {
  iceServers: []
}
let peerConnection = new RTCPeerConnection(config);
peerConnection.addEventListener('icecandidate', function (event) {
  if (event.candidate) {
    android.SendIceCandidate(JSON.stringify(event.candidate));
  }
});
peerConnection.addEventListener('connectionstatechange', event => {
  if (peerConnection.connectionState === 'connected') {
    console.log("Connection has been established")
  }
});
console.log("Subscribed");

function receiveIceCandidate(iceCandidate) {
  console.log("Received ICE candidate (JS): " + iceCandidate);
  console.log("Received ICE candidate (JS, escaped): " + escapeCRLF(iceCandidate));

  peerConnection.addIceCandidate(iceCandidate);
}

function showError(error) {
  alert(error.name + ": " + error.message);
}

// https://stackoverflow.com/a/27725393
function escapeCRLF(string) {
  return string
    .replace(/\r/g, "\\r")
    .replace(/\n/g, "\\n");
}
