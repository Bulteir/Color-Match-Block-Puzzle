using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsMenu_ChangeUserNameConfirm_Btn : MonoBehaviour
{
    public TMP_InputField userNameTextBox;
    public Button confirmButton;
    public GameObject LeaderboardMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private async void OnEnable()
    {
        try
        {
            string currentPlayerName = await AuthenticationService.Instance.GetPlayerNameAsync();

            TextMeshProUGUI placeholder = (TextMeshProUGUI)userNameTextBox.placeholder;

            currentPlayerName = currentPlayerName.Substring(0, currentPlayerName.IndexOf('#'));
            placeholder.text = currentPlayerName;
        }
        catch
        {
            TextMeshProUGUI placeholder = (TextMeshProUGUI)userNameTextBox.placeholder;
            placeholder.text = LocalizationSettings.StringDatabase.GetLocalizedString("Localizations", "EnterText");
        }
    }

    public void OnClick()
    {
        string newUserName = userNameTextBox.text;
        LeaderboardMenu.GetComponent<LeaderboardController>().SetPlayerName(newUserName);
        userNameTextBox.text = string.Empty;
        TextMeshProUGUI placeholder = (TextMeshProUGUI)userNameTextBox.placeholder;
        placeholder.text = newUserName;
        confirmButton.interactable = false;

    }
}
