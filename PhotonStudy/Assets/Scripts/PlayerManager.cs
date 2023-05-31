using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.Demos.DemoAnimator;

public class PlayerManager : PunBehaviour, IPunObservable
{
    [SerializeField] private GameObject beams;

    [Tooltip("플레이어의 현재 HP")]
    public float Health = 1f;

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
