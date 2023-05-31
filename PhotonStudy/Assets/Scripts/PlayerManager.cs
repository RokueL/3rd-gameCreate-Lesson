using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PlayerManager : PunBehaviour
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

    private void Update()
    {
        ProcessInputs();
        if(beams != null && isFire != beams.activeInHierarchy)
        {
            beams.SetActive(isFire);
        }
    }
}
