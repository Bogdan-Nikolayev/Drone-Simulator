// window.onload = subscribeToRemoteMediaStream;
this.addEventListener("arjs-video-loaded", () => {
    subscribeToRemoteMediaStream();
});

function subscribeToRemoteMediaStream() {
  let remoteStream = new MediaStream();
  let video = document.getElementById('arjs-video');
  video.srcObject = remoteStream;

  peerConnection.addEventListener('track', function (event) {
    console.log("Adding remote track");
    remoteStream.addTrack(event.track);
  });
}

function receiveOffer(offer) {
  console.log("Received offer: " + escapeCRLF(offer));

  peerConnection.setRemoteDescription(JSON.parse(escapeCRLF(offer))).then(
    createAndSendAnswer,
    alertError);
}

function createAndSendAnswer() {
  peerConnection.createAnswer().then(
    function (answer) {
      peerConnection.setLocalDescription(answer);

      console.log("Sending answer: " + JSON.stringify(answer));
      webRtcAndroidInterface.SendAnswer(JSON.stringify(answer));
    },
    alertError);
}