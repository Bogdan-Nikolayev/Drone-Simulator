// Sometimes the connection happens before the recipient subscribes
// to a new track ("arjs-video-loaded" event is triggered late).
// A timeout is required to ensure that the connection is performed
// only after the recipient subscribes to the new track.;
setTimeout(function () {
  startVideoStreaming();
}, 3000)

function startVideoStreaming() {
  navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } }).then(
    function (stream) {
      let video = document.getElementsByTagName("video")[0];
      if (video) video.srcObject = stream;

      peerConnection.addTrack(stream.getVideoTracks()[0]);

      createAndSendOffer();
    },
    alertError);
}

function createAndSendOffer() {
  peerConnection.createOffer().then(
    function (offer) {
      peerConnection.setLocalDescription(offer);

      console.log("Sending offer: " + JSON.stringify(offer));
      webRtcAndroidInterface.SendOffer(JSON.stringify(offer));
    },
    alertError);
}

function receiveAnswer(answer) {
  console.log("Received answer: " + escapeCRLF(answer));

  peerConnection.setRemoteDescription(JSON.parse(escapeCRLF(answer)));
}