using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerUI : MonoBehaviour
{

    public static playerUI Singleton
    {
        get
            {
            if (singleton == null)
                singleton = GameObject.FindObjectOfType<playerUI>();
            return singleton;
            }

    }


    private static playerUI singleton;

    [SerializeField] Text playerHP;
    [SerializeField] Slider barHP;

    public void SetPlayerHP(int hp)
    {
        playerHP.text = "Player HP : " + hp.ToString();
        barHP.value = (float)hp / (float)100;
        Debug.Log(barHP.value);
    }

    private void OnDestroy()
    {
        singleton = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
