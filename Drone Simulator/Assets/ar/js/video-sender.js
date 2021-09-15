startVideoStreaming();

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