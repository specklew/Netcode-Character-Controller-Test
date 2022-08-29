using Unity.Netcode;
using UnityEngine;


public class SkyboxCam : MonoBehaviour
{
    private Camera _skyboxCamera;
    private Camera _playerCamera;
    
    void Start()
    {
        _skyboxCamera = GetComponent<Camera>();
        _playerCamera = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponentInChildren<Camera>();
        
        //var cameraData = _skyboxCamera.GetComponent<Camera>().GetUniversalAdditionalCameraData();
        //cameraData.cameraStack.Add(_playerCamera);
    }
    
    void Update()
    {
        transform.rotation = _playerCamera.transform.rotation;
        _skyboxCamera.fieldOfView = _playerCamera.fieldOfView;
    }
}
