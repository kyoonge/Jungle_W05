using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetManager : MonoBehaviour
{
    public static MagnetManager Instance;

    #region PublicVariables
    public Transform player;
    public PlayerMagnet playerMagnet;
    #endregion
    #region PrivateVariables
    #endregion
    #region PublicMethod
    #endregion
    #region PrivateMethod
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1;
        player.GetComponent<Player>().canAct = true;
    }
    #endregion
}