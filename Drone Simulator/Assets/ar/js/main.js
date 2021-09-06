const config = {
  iceServers: []
}
let peerConnection = new RTCPeerConnection(config);
peerConnection.addEventListener('icecandidate', function (event) {
  setTimeout(() => {
    android.SendIceCandidate(JSON.stringify(event.candidate));
  }, Math.floor(Math.random() * 11000));
});
peerConnection.addEventListener('connectionstatechange', event => {
  console.log("Connection state has been changed: " + peerConnection.connectionState)
});
console.log("Subscribed");

function receiveIceCandidate(iceCandidate) {
  console.log("Received ICE candidate (JS): " + iceCandidate);
  console.log("Received ICE candidate (JS, escaped): " + escapeCRLF(iceCandidate));

  peerConnection.addIceCandidate(JSON.parse(iceCandidate));
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
