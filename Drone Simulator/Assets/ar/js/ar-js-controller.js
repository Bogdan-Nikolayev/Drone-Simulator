const aCamera = document.getElementsByTagName("a-camera")[0];

function receivePose(poseJSON) {
    console.log("Received pose: " + poseJSON);
    let pose = JSON.parse(poseJSON);
    setRotation(pose.RotationX, pose.RotationY, pose.RotationZ);
}

function setRotation(x, y, z) {
    console.log(x + " " + y + " " + z);

    aCamera.parentElement.object3D.rotation.set(
        x,
        THREE.Math.degToRad(e.target.value),
        z
    );
}