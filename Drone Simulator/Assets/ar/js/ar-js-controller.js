let camera;

window.onload = function () {
    camera = document.getElementsByTagName("a-camera")[0];
}

function receivePose(poseJSON) {
    // console.log("Received pose: " + poseJSON);
    let pose = JSON.parse(poseJSON);
    setRotation(pose.RotationX, pose.RotationY, pose.RotationZ);
}

function setRotation(x, y, z) {
    if (camera) {
        // x = THREE.Math.radToDeg(x);
        // y = THREE.Math.degToRad(y);
        let xn = x * 180;
        let yn = y * 180;
        let zn = z * 180;

        console.log(xn + " " + yn + " " + zn);

        camera.parentElement.object3D.rotation.set(
            THREE.Math.degToRad(xn),
            THREE.Math.degToRad(yn),
            THREE.Math.degToRad(zn)
        );
    } else {
        console.log("Camera is not set");
    }
}