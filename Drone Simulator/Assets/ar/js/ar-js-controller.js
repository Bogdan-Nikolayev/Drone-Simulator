let camera;

window.onload = function () {
    camera = document.getElementsByTagName("a-camera")[0];
}

function receivePose(poseJSON) {
    // console.log("Received pose: " + poseJSON);
    let pose = JSON.parse(poseJSON);
    // setRotation(pose.RotationX, pose.RotationY, pose.RotationZ);
    setRotation(pose);
}

function setRotation(q) {
    if (camera) {
        const a = new THREE.Euler().setFromQuaternion(new THREE.Quaternion(q.RotationX, q.RotationY, q.RotationZ, q.RotationW), "ZYX");

        console.log(a.x + " " + a.y + " " + a.z);
        camera.parentElement.object3D.rotation.set(
            a.x,
            a.y,
            a.z
        );
    } else {
        console.log("Camera is not set");
    }
}