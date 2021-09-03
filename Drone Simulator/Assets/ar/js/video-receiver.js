let peerConnection = new RTCPeerConnection();

function receiveOffer(offer) {
    console.log("Received offer (JS): " + offer);

    peerConnection.setRemoteDescription(JSON.parse(offer));

    createAndSendAnswer();
}

function createAndSendAnswer() {
    peerConnection.createAnswer().then(
        function (answer) {
            peerConnection.setLocalDescription(answer);
            android.SendAnswer(JSON.stringify(answer));
        },
        function (err) {
            alert(err.name + ": " + err.message);
        });
}