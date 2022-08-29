using Networking;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private VisualElement _root;
    
    private VisualElement _main;
    private VisualElement _join;
    private VisualElement _options;
    void OnEnable()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        _root = uiDocument.rootVisualElement;

        _main = _root.Q<VisualElement>("Main");
        _join = _root.Q<VisualElement>("Join");
        _options = _root.Q<VisualElement>("Options");

        //Main buttons:

        _main.Q<Button>("host-button").clicked += MainHostClicked;
        _main.Q<Button>("join-button").clicked += MainJoinClicked;
        _main.Q<Button>("options-button").clicked += MainOptionsClicked;

        //Join buttons:

        _join.Q<Button>("join-button").clicked += JoinJoinClicked;
        _join.Q<Button>("back-button").clicked += JoinBackClicked;

        //Option buttons:

        _options.Q<Button>("back-button").clicked += OptionsBackClicked;

    }

    #region Main screen

    void MainHostClicked()
    {
        GameNetPortal.Instance.StartHost();
    }
    
    void MainJoinClicked()
    {
        _main.style.display = DisplayStyle.None;
        _join.style.display = DisplayStyle.Flex;
    }
    
    void MainOptionsClicked()
    {
        _main.style.display = DisplayStyle.None;
        _options.style.display = DisplayStyle.Flex;
    }

    #endregion

    #region Join screen

    private void JoinJoinClicked()
    {
        ClientGameNetPortal.Instance.StartClient(_join.Q<TextField>("ipaddress").value);
    }
    
    private void JoinBackClicked()
    {
        _main.style.display = DisplayStyle.Flex;
        _join.style.display = DisplayStyle.None;
    }

    #endregion

    #region Options screen

    private void OptionsBackClicked()
    {
        _main.style.display = DisplayStyle.Flex;
        _options.style.display = DisplayStyle.None;
    }

    #endregion
    
}