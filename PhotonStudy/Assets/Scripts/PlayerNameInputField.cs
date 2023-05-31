using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    #region Private Constants

    const string playerNamePrefKey = "PlayerName";

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        string defaultName = string.Empty;
        InputField inputField = this.GetComponent<InputField>();
        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }
        }
        PhotonNetwork.playerName = defaultName;
    }

    #endregion

    #region Public Methods

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player name is null or empty");
            return;
        }

        PhotonNetwork.playerName = value;
        PlayerPrefs.SetString(playerNamePrefKey, value);

        Debug.Log($"Pun player name is {value}");
    }

    #endregion
}
