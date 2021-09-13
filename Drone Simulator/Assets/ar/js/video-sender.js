setTimeout(function () {
    startVideoStreaming();
}, 5000);

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
  // TODO: Remove if not required.
    var mediaConstraints = {
        'offerToReceiveAudio': true,
        'offerToReceiveVideo': true
    };

  peerConnection.createOffer(mediaConstraints).then(
    function (offer) {
      peerConnection.setLocalDescription(offer);

      log("Sending offer: " + JSON.stringify(offer));
      android.SendOffer(JSON.stringify(offer));
    },
    alertError);
}

function receiveAnswer(answer) {
  log("Received answer: " + escapeCRLF(answer));

  peerConnection.setRemoteDescription(JSON.parse(escapeCRLF(answer)));
}