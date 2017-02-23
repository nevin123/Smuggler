using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {

    Camera playerCamera;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float speed;
    [SerializeField] float smoothTime = 0.3F;

    private Vector3 newPos;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start() {
        //Destoy Script if this is not the local player
        if(!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        //Get the camera
        playerCamera = Camera.main;

        //Reset the camere
        GL.invertCulling = false;
        playerCamera.ResetProjectionMatrix();

        //Rotate the camera
        if (!isServer)
        {
            playerCamera.transform.rotation = Quaternion.Euler(0, 180, 0);
            
            Matrix4x4 mat = playerCamera.projectionMatrix;

            mat *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
            playerCamera.projectionMatrix = mat;

            //Fix the inverted scene
            GL.invertCulling = true;
        }
        else
        {
            newPos = transform.position + new Vector3(cameraOffset.x, cameraOffset.y, -cameraOffset.z);
            playerCamera.transform.rotation = Quaternion.identity;
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if (playerCamera == null)
            return;

        if (transform.position.z < 0)
            newPos = transform.position + cameraOffset;
        else
            newPos = transform.position + new Vector3(-cameraOffset.x, cameraOffset.y, -cameraOffset.z);


        playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, newPos, ref velocity, smoothTime);
    }
}
