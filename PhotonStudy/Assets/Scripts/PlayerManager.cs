using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using ExitGames.Demos.DemoAnimator;

public class PlayerManager :  PunBehaviour, IPunObservable
{
    [Tooltip("로컬플레이어" )]
    public static GameObject LocalPlayerInstance;

    [SerializeField] private GameObject beams;
    [SerializeField] private GameObject playerUIPrefab;

    [Tooltip("플레이어의 현재 HP")]
    public float Health = 1f;
    public float maxHealth = 1f;
    private bool isFire;


    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine) return;
        if (!other.name.Contains("Beam")) return;

        Health -= 0.1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.isMine) return;
        if (!other.name.Contains("Beam")) return;

        Health -= 0.1f * Time.deltaTime;
    }

    private void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if ((!isFire)) isFire = true;
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if ((isFire)) isFire = false;
        }
    }

    private void Awake()
    {
        if(beams == null)
        {
            Debug.LogError("<Color=red>*****Missing Beams References*****</Color>",this);
        }
        else
        {
            beams.SetActive(false);
        }
        if (photonView.isMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        CameraWork cameraWork = this.GetComponent<CameraWork>();
        if(cameraWork != null)
        {
            if (photonView.isMine) cameraWork.OnStartFollowing();
        }
        else
        {
            Debug.LogError("<Color==red>***** 카메라 컴포넌트 없음 *****</Color>");
        }


#if UNITY_5_4_OR_NEWER
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) => {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        };
#endif
        if(playerUIPrefab != null)
        {
            var uiGo = Instantiate(playerUIPrefab);
            uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
        else
        {
            Debug.LogError("<Color=red>**** playerUIPrefab Missing ****</Color>",this);
        }
    }

#if UNITY_5_4_OR_NEWER
    private void OnLevelWasLoaded(int level)
{
    this.CalledOnLevelWasLoaded(level);
}
#endif
private void CalledOnLevelWasLoaded(int level)
{
    if(!Physics.Raycast(transform.position, -Vector3.up, 5f))
    {
        transform.position = new Vector3(0, 5, 0);
    }

        var uiGo = Instantiate(playerUIPrefab);
        uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }


private void Update()
    {
        if (photonView.isMine)
        {
            ProcessInputs();

            if(Health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }


            if (beams != null && isFire != beams.activeInHierarchy)
            {
                beams.SetActive(isFire);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(isFire);
            stream.SendNext(Health);
        }
        else
        {
            this.isFire = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }
}
