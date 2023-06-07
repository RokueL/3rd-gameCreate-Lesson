using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text playerNameText;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Vector3 screenOffset = new Vector3(0, 30, 0);

    private PlayerManager target;
    float characterControllerHeight = 0f;
    Transform targetTransform;
    Vector3 targetPosition;


    public void SetTarget(PlayerManager _target)
    {
        if(_target == null)
        {
            Debug.LogError("<Color = red>**** _target is null ****</Color>");
            return;
        }
        this.target = _target;
        if(playerNameText != null)
        {
            playerNameText.text = target.photonView.owner.NickName;
        }

        var characterController = target.GetComponent<CharacterController>();
        if(characterController != null)
        {
            characterControllerHeight = characterController.height;
        }
    }

    private void Awake()
    {
        var parent = GameObject.Find("Canvas").GetComponent<Transform>();
        if (parent)
        {
            this.transform.SetParent(parent, false);
        }
        else
        {
            Debug.LogError("<Color=red>**** Canvas Missing ****</Color>");
        }
    }

    private void Update()
    {
        if (playerHealthSlider != null)
        {
            playerHealthSlider.value = target.Health;
        }

        if(target == null)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void LateUpdate()
    {
        if(targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }
}
