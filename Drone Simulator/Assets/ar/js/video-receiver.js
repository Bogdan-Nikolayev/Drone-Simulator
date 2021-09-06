function receiveOffer(offer) {
  console.log("Received offer (JS): " + offer);
  console.log("Received offer (JS, escaped): " + escapeCRLF(offer));

  peerConnection.setRemoteDescription(JSON.parse(escapeCRLF(offer))).then(
    function () {
      createAndSendAnswer();
      subscribeToRemoteMediaStream();
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

function subscribeToRemoteMediaStream() {
  const remoteStream = new MediaStream();
  const video = document.getElementById('arjs-video');
  video.srcObject = remoteStream;

  peerConnection.addEventListener('track', function (event) {
    console.log("New remote track has been added");

    remoteStream.addTrack(event.track, remoteStream);
  });
}