const aCamera = document.getElementsByTagName("a-camera")[0];

function setRotation(x, y, z) {
    console.log(x + " " + y + " " + z);

    aCamera.parentElement.object3D.rotation.set(
        0,
        THREE.Math.degToRad(e.target.value),
        0
    );
}