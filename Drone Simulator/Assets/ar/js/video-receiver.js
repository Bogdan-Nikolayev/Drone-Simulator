let peerConnection = new RTCPeerConnection();

function receiveOffer(offer) {
  console.log("Received offer (JS): " + offer);
  console.log("Received offer (JS, escaped): " + escapeJson(offer));

  peerConnection.setRemoteDescription(JSON.parse(escapeJson(offer))).then(
    function () {
      createAndSendAnswer();
    },
    showError);
}

function createAndSendAnswer() {
  peerConnection.createAnswer().then(
    function (answer) {
      peerConnection.setLocalDescription(answer);

      console.log("Sending answer (JS): " + answer);
      android.SendAnswer(JSON.stringify(answer));
    },
    showError);
}