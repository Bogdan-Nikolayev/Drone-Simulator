window.onload = subscribeToRemoteMediaStream;

function subscribeToRemoteMediaStream() {
  let remoteStream = new MediaStream();
  let video = document.getElementById('arjs-video');
  video.srcObject = remoteStream;

  peerConnection.addEventListener('track', function (event) {
    log("Added remote track");

    remoteStream.addTrack(event.track, remoteStream);
  });
}

function receiveOffer(offer) {
  log("Received offer: " + escapeCRLF(offer));

  peerConnection.setRemoteDescription(JSON.parse(escapeCRLF(offer))).then(
    createAndSendAnswer,
    alertError);
}

function createAndSendAnswer() {
  peerConnection.createAnswer().then(
    function (answer) {
      peerConnection.setLocalDescription(answer);

      log("Sending answer: " + JSON.stringify(answer));
      android.SendAnswer(JSON.stringify(answer));
    },
    alertError);
}