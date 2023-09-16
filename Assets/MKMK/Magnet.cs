using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    #region PublicVariables
    [SerializeField] public Utils.MagnetType magnetType;
    public bool isMovable = false;
    private Vector3 originPosition;
    private Vector3 targetPosition, targetDirection;

    #endregion
    #region PrivateVariables
    #endregion
    #region PublicMethod



    public Magnet(Utils.MagnetType _magnetType, bool _isMovable)
    {
        this.magnetType = _magnetType;
        this.isMovable = _isMovable;
    }
    public void CallMagnet()
    {
        Debug.Log("It's Magnet Object");
    }
    #endregion
    #region PrivateMethod
    private void Start()
    {
        targetPosition = MagnetManager.Instance.player.position;

        switch (magnetType)
        {
            case Utils.MagnetType.N:
                break;
            case Utils.MagnetType.S:
                break;
        }
    }



    #endregion
}