let peerConnection = new RTCPeerConnection();

navigator.mediaDevices
    .getUserMedia({video: {facingMode: "environment"}})
    .then(function (stream) {
        let myVideo = document.getElementsByTagName("video")[0];
        if (myVideo)
            myVideo.srcObject = stream;

        peerConnection.addTrack(stream.getVideoTracks()[0]);
        peerConnection.createOffer().then(
            function (offer) {
                peerConnection.setLocalDescription(offer);
                android.SendOffer(JSON.stringify(offer));
            },
            function (err) {
                alert(err.name + ": " + err.message);
            }
        );
    })
    .catch(function (err) {
        alert(err.name + ": " + err.message);
    });

function receiveAnswer(answer) {
    console.log("Receiving answer: " + answer);
    peerConnection.setRemoteDescription(answer);
}