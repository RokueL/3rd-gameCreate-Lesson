using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    private Player fireOwner;
    private Vector3 targetDir;
    private float moveSpeed = 150f;

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), 10f);
    }

    [ClientRpc]
    public void SetFireData(NetworkIdentity networkIdentity)
    {
        var player = networkIdentity.gameObject.GetComponent<Player>();
        this.fireOwner = player;
        this.targetDir = player.MuzzleDir;
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(this.gameObject);
    }



    #region Unity CallBack

    private void Awake()
    {
        //Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        var pos = this.transform.position;
        pos.x += targetDir.x * moveSpeed * Time.deltaTime;
        pos.z += targetDir.z * moveSpeed * Time.deltaTime;
        this.transform.position = pos;

        // this.transform.Translate(moveSpeed * targetDir * Time.deltaTime, Space.World);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Destroy(this.gameObject);
    //}

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        var netID = other.GetComponent<NetworkIdentity>();
        if (netID == null || netID.gameObject == fireOwner.gameObject) return;
        else if (other.gameObject.layer == LayerMask.GetMask("cube"))
        {
            Destroy(other.gameObject);
        }
        DestroySelf();
    }

    #endregion
}