using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using Photon.Realtime;
public class GameManager : PunBehaviour
{
    public static GameManager Instance;

    private void Start()
    {
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
            Debug.Log("<Color==blue>PUN room for 1�� �ε��մϴ�</Color>");
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
            Debug.LogError("������ Ŭ���̾�Ʈ�� ����");
            return;
        }
        Debug.Log($"PUN �ε� ���� {PhotonNetwork.room.PlayerCount}");
        PhotonNetwork.LoadLevel($"room{PhotonNetwork.room.PlayerCount}");
    }
    #endregion
}
