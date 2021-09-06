let peerConnection = new RTCPeerConnection();
startWebRTC();

function startWebRTC() {
    navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } }).then(
        function (stream) {
            let video = document.getElementsByTagName("video")[0];
            if (video) video.srcObject = stream;

            peerConnection.addTrack(stream.getVideoTracks()[0]);

            createAndSendOffer();
        },
        showError);
}

function createAndSendOffer() {
    peerConnection.createOffer().then(
        function (offer) {
            peerConnection.setLocalDescription(offer);

            console.log("Sending offer (JS): " + offer);
            android.SendOffer(JSON.stringify(offer));
        },
        showError);
}

function receiveAnswer(answer) {
    console.log("Received answer (JS): " + answer);

    peerConnection.setRemoteDescription(JSON.parse(answer));
}