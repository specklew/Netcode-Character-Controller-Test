using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerScripts
{
    public class SkyboxCamera : NetworkBehaviour
    { 
        //TODO: REFACTOR THIS FUCKING SHIT OF A SCRIPT.
        private GameObject _skyboxObject;
        private Camera _skyboxCamera;
        private Camera _playerCamera;
        private bool _stopCameraMovement;

        public override void OnNetworkSpawn()
        {
            if(!IsLocalPlayer) return;
            
            _stopCameraMovement = true;
            
            _playerCamera = gameObject.GetComponentInChildren<Camera>();
            StartCoroutine(WaitTillTheCameraIsFound());
        }
        
        private void Update()
        {
            if (_stopCameraMovement || !IsLocalPlayer) return;
            _skyboxObject.transform.rotation = _playerCamera.transform.rotation;
            _skyboxCamera.fieldOfView = _playerCamera.fieldOfView;
        }

        private void OnEnable()
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete += OnSceneLoaded;
            NetworkManager.Singleton.SceneManager.OnLoad += OnSceneSwitchStarted;
            GameplayManager.OnPlayerKilled += OnPlayerKilled;
        }


        private void OnDisable()
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnSceneLoaded;
            NetworkManager.Singleton.SceneManager.OnLoad -= OnSceneSwitchStarted;
            GameplayManager.OnPlayerKilled -= OnPlayerKilled;
        }
        
        private void OnSceneSwitchStarted(ulong clientid, string scenename, LoadSceneMode loadscenemode, AsyncOperation asyncoperation)
        {
            if(clientid == OwnerClientId) _stopCameraMovement = true;
        }
        
        private void OnSceneLoaded(ulong clientid, string scenename, LoadSceneMode loadscenemode)
        {
            if (clientid == OwnerClientId)
            {
                _stopCameraMovement = true;
                ConfigureSkyboxCam();
            }
        }


        private void OnPlayerKilled(ulong playerId)
        {
            if (playerId == OwnerClientId)
            {
                ConfigureSkyboxCam();
            }
        }
        
        private void ConfigureSkyboxCam()
        {
            _skyboxObject = GameObject.FindWithTag("SkyboxCamera") ?? GameObject.Find("SkyboxCam");
            //var cameraData = _skyboxObject.GetComponent<Camera>().GetUniversalAdditionalCameraData();
            //cameraData.cameraStack.Add(GetComponentInChildren<Camera>());
            _skyboxCamera = _skyboxObject.GetComponent<Camera>();
        }

        private IEnumerator WaitTillTheCameraIsFound()
        {
            while (_stopCameraMovement)
            {
                yield return new WaitForEndOfFrame();
                _skyboxObject = GameObject.FindWithTag("SkyboxCamera") ?? GameObject.Find("SkyboxCam");
                if (_skyboxObject != null) _stopCameraMovement = false;
            }
                
            Debug.Log(_skyboxObject);
            Debug.Log(IsLocalPlayer);

            //var cameraData = _skyboxObject.GetComponent<Camera>().GetUniversalAdditionalCameraData();
            //cameraData.cameraStack.Clear();
            //cameraData.cameraStack.Add(GetComponentInChildren<Camera>());
            _skyboxCamera = _skyboxObject.GetComponent<Camera>();

            _stopCameraMovement = false;
        }
    }
}
