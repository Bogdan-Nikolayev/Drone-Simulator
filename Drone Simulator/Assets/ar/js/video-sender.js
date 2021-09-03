let peerConnection = new RTCPeerConnection();

navigator.mediaDevices
    .getUserMedia({video: {facingMode: "environment"}})
    .then(function (stream) {
        let video = document.getElementsByTagName("video")[0];
        if (video)
            video.srcObject = stream;

        peerConnection.addTrack(stream.getVideoTracks()[0]);
        peerConnection.createOffer().then(
            function (offer) {
                peerConnection.setLocalDescription(offer);
                let string = JSON.stringify(offer);
                android.SendOffer(string);
                console.log("Reparse offer: " + JSON.parse(string))
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
    console.log("Received answer (JS): " + answer);

    peerConnection.setRemoteDescription(JSON.parse(answer));
}