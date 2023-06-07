using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Realtime;
public class GameManager : PunBehaviour
{
    public static GameManager Instance;

    [Tooltip("Set using player prefab")]
    public GameObject playerPrefab;
    private void Start()
    {
        Instance = this;

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=red>**** player prefab is missing ****</Color>");
        }
        else
        {
            Debug.Log($"We are Instantiating LocalPlayer from {SceneManager.GetActiveScene().name}");

            if (PlayerManager.LocalPlayerInstance == null)
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity, 0);

            }
            else
            {
                Debug.Log($"Ignoring scene load for {SceneManager.GetActiveScene().name}");
            }
        }

    }

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.room.PlayerCount == 1)
        {
            Debug.Log("<Color==blue>PUN room for 1을 로딩합니다</Color>");
            PhotonNetwork.LoadLevel("room1");
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log($"PUN new Player entered.. : {newPlayer.NickName}");
        if (PhotonNetwork.isMasterClient)
        {
            LoadArena();
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log($"PUN player Disconnected : {otherPlayer.NickName}");
        if (PhotonNetwork.isMasterClient)
        {
            LoadArena();
        }
    }
    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Private Methods

    private void LoadArena()
    {
        if(!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("마스터 클라이언트만 진행");
            return;
        }
        Debug.Log($"PUN 로딩 레벨 {PhotonNetwork.room.PlayerCount}");
        PhotonNetwork.LoadLevel($"room{PhotonNetwork.room.PlayerCount}");
    }
    #endregion
}
