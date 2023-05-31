using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform muzzle;

    [SyncVar]
    [SerializeField]private int playerHP = 100;

    private Vector3 moveDir;

    private Camera camera;
    public Transform camAnchor;
    private float rotation;
    private float forward;
    private const float FIRE_DELAY = 0.2f;
    [SyncVar] private float fireTime;

    private NetworkIdentity networkdIdentity;
    private SpawnPointManager spawnPointManager;

    public Vector3 MuzzleDir
    {
        get => muzzle.TransformDirection(Vector3.forward);
    }

    private void Awake()
    {
        try
        {
            Init();
        }
        catch (System.ArgumentOutOfRangeException ex)
        {

        }
        catch (System.DivideByZeroException ex)
        {

        }
        catch (System.Exception ex)
        {
            Debug.Log(ex);  // 소비
        }
        /*
        catch (System.Exception)
        { 
            throw // 패스
        }
        */
    }

    private void Init()
    {
        networkdIdentity = this.GetComponent<NetworkIdentity>();
        spawnPointManager = FindObjectOfType<SpawnPointManager>();

        Debug.Log("Spawn");
        spawnPlayer();

        if (networkdIdentity == null)
            throw new System.Exception();   // 생성
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (!camera)
        {
            camera = Camera.main;
            //camera.transform.position = camAnchor.position;
            //camera.transform.rotation = camAnchor.rotation;
            camera.transform.parent = camAnchor;
            camera.transform.localPosition = Vector3.zero;
            camera.transform.localRotation = Quaternion.identity;
        }


        if (Input.GetMouseButtonDown(0) && ShouldFire())
            Fire();
        // rotation
        rotation = Input.GetAxisRaw("Horizontal");

        // forward, backward
        forward = Input.GetAxisRaw("Vertical");

        // Move
        if (forward < -.1f)
            forward = -1f;
        else if (forward > .1f)
            forward = 1f;
        else
            forward = 0f;

        moveDir = new Vector3(0f, 0f, forward);
        //moveDir.Normalize();

        this.transform.Translate(moveSpeed * moveDir * Time.deltaTime);
        this.transform.Rotate(Vector3.up * rotationSpeed * rotation * Time.deltaTime);
    }

    private bool ShouldFire()
    {
        return Time.time >= fireTime;
    }

    [Command]
    private void Fire()
    {
        var bullet = GetBullet();
        var bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetFireData(networkdIdentity);

        fireTime = Time.time + FIRE_DELAY;
    }

    private GameObject GetBullet()
    {
        var bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation) as GameObject;
        NetworkServer.Spawn(bullet);

        return bullet;
    }

    [ServerCallback]

    private void OnTriggerEnter(Collider other)
    {
        var netID = other.GetComponent<NetworkIdentity>();
        if (netID == null || netID.gameObject == this.gameObject) return;

        playerHP -= 30 + Random.Range(0, 10);
        playerHP = (playerHP > 0) ? playerHP : 0;

        Debug.Log("Hit , HP = " + playerHP);

        RPCSetPlayerHP(playerHP);

        if (playerHP <= 0)
        {
            spawnPlayer();
            //NetworkServer.Destroy(this.gameObject);
        }
    }
    [ClientRpc]
    private void spawnPlayer()
    {
        var pos = spawnPointManager.GetNextStartPosition();
        this.transform.position = pos.position;
        playerHP = 100;
        RPCSetPlayerHP(playerHP);
    }


    private void OnDestroy()
    {
        camera.transform.parent = null;
    }

    [ClientRpc]
    private void RPCSetPlayerHP(int playerHP)
    {
        if (isLocalPlayer)
        {
            playerUI.Singleton.SetPlayerHP(playerHP);
        }
    }

    /*
    [Command]
    private void Cmd_Func()
    { 
    
    }

    [ClientRpc]
    private void Rpc_Func2()
    { 
    
    }
    */

    #region Server Client Func
    public override void OnStartLocalPlayer()
    {
        SetupPlayerName();
        playerUI.Singleton.SetPlayerHP(playerHP);
    }

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
        {
            SetupPlayerName();
        }
    }

    private void SetupPlayerName()
    {
        var playerName = $"Player{this.netId}";
        this.gameObject.name = playerName;
        Debug.Log($"{playerName}");
    }
    #endregion
}