let peerConnection = new RTCPeerConnection();

function receiveOffer(offer) {
  console.log("Received offer (JS): " + offer);
  console.log("Received offer (JS, escaped): " + escapeCRLF(offer));

  peerConnection.setRemoteDescription(JSON.parse(escapeCRLF(offer))).then(
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