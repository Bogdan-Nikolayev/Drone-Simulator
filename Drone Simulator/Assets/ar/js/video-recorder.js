navigator.mediaDevices
    .getUserMedia({video: {facingMode: "environment"}})
    .then(function (stream) {
        let myVideo = document.getElementsByTagName("video")[0];
        myVideo.srcObject = stream;

        let peerConnection = new RTCPeerConnection();
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
