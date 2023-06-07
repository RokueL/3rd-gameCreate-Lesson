using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PlayerAnimatorManager : PunBehaviour
{
    [SerializeField]private Animator animator;
    [SerializeField] private float directionDampTime = 0.2f;

    #region STRING
    private const string RunLayer = "Base Layer.Run";
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";
    private const string FireButton = "Fire2";
    private int JumpAnimHash;
    private int SpeedAnimHash;
    private int DirectionAnimHash;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        if (!animator)
        {
            Debug.LogError("NULL Animator");
        }
       JumpAnimHash = Animator.StringToHash("Jump");
       SpeedAnimHash = Animator.StringToHash("Speed");
       DirectionAnimHash = Animator.StringToHash("Direction");
    }

    private void Update()
    {
        if(photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }

        if (!animator) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName(RunLayer))
        {
            if (Input.GetButtonDown(FireButton))
                animator.SetTrigger(JumpAnimHash);
        }

        float hInput = Input.GetAxis(HorizontalInput);
        float vInput = Input.GetAxis(VerticalInput);
        if (vInput < 0) vInput = 0f;

        animator.SetFloat(SpeedAnimHash, hInput * hInput + vInput * vInput);
        animator.SetFloat(DirectionAnimHash, hInput, directionDampTime, Time.deltaTime);    
    }
}
