using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;


public class Launcher : PunBehaviour
{
    #region Private Serializable Fields

    #endregion

    #region Public Fields

    [SerializeField]
    private GameObject controlPanel;

    [SerializeField]
    private GameObject progressLabel;

    #endregion

    #region MonoBehaviour Callbacks

    #endregion

    #region Private Fields

    string gameVersion = "1";
    private bool isConnecting; //try to connect

    [Tooltip("방당 인원수")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }

    private void Start()
    {
        //Connect();

        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion

    #region Public Methods

    public void Connect()
    {
        isConnecting = true;
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.gameVersion = this.gameVersion;
            PhotonNetwork.ConnectUsingSettings(this.gameVersion);
            //PhotonNetwork.gameVersion = gameVersion;
        }

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

    }

    #endregion

    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {           
        Debug.Log("Connected On Master....");
        if (isConnecting)
        {
            //PhotonNetwork.JoinRandomRoom();

            PhotonNetwork.CreateRoom(null,
                  new RoomOptions { MaxPlayers = this.maxPlayersPerRoom },
                  TypedLobby.Default);
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("On disconnected from Photon");
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Pun connect failed : {0}", cause);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Pun random join failed");
        PhotonNetwork.CreateRoom(null, 
            new RoomOptions { MaxPlayers = this.maxPlayersPerRoom }, 
            TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Pun joined room");
        PhotonNetwork.LoadLevel("room1");
    }

    #endregion
}
