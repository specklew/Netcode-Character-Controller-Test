using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerScripts
{
    [RequireComponent(typeof(Player))]
    public class PlayerUIController : MonoBehaviour
    {
        private VisualElement _interactionElement;
        private Label _interactionTextLabel;

        private void OnEnable()
        {
            _interactionElement = GetComponentInChildren<UIDocument>().rootVisualElement.Q<VisualElement>("interaction");

            _interactionElement.visible = false;
        
            _interactionTextLabel = _interactionElement.Q<Label>("interaction_text");
        }

        public void ShowInteractionText(String text)
        {
            _interactionTextLabel.text = text;
            _interactionElement.visible = true;
        }

        public void HideInteractionText()
        {
            _interactionElement.visible = false;
        }
    }
}
