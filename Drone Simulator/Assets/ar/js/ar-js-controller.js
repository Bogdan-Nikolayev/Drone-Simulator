let camera;

window.onload = function (){
    camera = document.getElementsByTagName("a-camera")[0];
}

function receivePose(poseJSON) {
    // console.log("Received pose: " + poseJSON);
    let pose = JSON.parse(poseJSON);
    setRotation(pose.RotationX, pose.RotationY, pose.RotationZ);
}

function setRotation(x, y, z) {
    if (camera)
    {
        // x = THREE.Math.radToDeg(x);
        // y = THREE.Math.degToRad(y);
        
        console.log(x + " " + y + " " + z);
        
        camera.parentElement.object3D.rotation.set(
            x,
            y,
            z
        );
    }
    else
    {
        console.log("Camera is not set");
    }
}