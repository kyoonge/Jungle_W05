using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
	private Rigidbody2D rb;
	public SpriteRenderer rendererPlayer;
    public SpriteRenderer rendererEffect;
    [SerializeField] private Color nColor;
	[SerializeField] private Color sColor;


	private bool isReady = true;

    private void Start()
    {
    }

    public void Dodge()
	{
		if (MagnetManager.Instance.playerMagnet.magnetType == Enums.MagnetType.N)
        {
            MagnetManager.Instance.playerMagnet.magnetType = Enums.MagnetType.S;
            rendererPlayer.color = sColor;
            rendererEffect.color = sColor;
        }
        else
        {

            MagnetManager.Instance.playerMagnet.magnetType = Enums.MagnetType.N;
            rendererPlayer.color = nColor;
            rendererEffect.color = nColor;
        }
	}
}