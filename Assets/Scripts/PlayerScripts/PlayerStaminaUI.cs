using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerScripts
{
    public class PlayerStaminaUI : MonoBehaviour
    {
        private ProgressBar _progressBar;
        [SerializeField] private CharacterControllerPlayerMovement playerMovement;
        [SerializeField] private UIDocument uiDocument;

        public void Start()
        {
            playerMovement ??= GetComponent<CharacterControllerPlayerMovement>();
            uiDocument ??= GetComponentInChildren<UIDocument>();
            
            _progressBar = uiDocument.rootVisualElement.Q<ProgressBar>("stamina");
        }

        private void Update()
        {
            _progressBar.value = playerMovement.Stamina;
        }
    }
}
