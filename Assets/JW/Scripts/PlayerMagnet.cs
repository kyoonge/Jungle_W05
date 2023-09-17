using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMagnet : MonoBehaviour
{
	#region PublicVariables
	[SerializeField] public Utils.MagnetType magnetType;
	#endregion

	#region PrivateVariables
	private Animator anim;
	private Rigidbody2D rb;

	[SerializeField] private float magnetForce;
	[SerializeField] private Magnet magnet;

	[Header("Raycast")]
	public LayerMask collisionLayer;
	public float boxOffset;
	private Vector2 boxSize;

	[SerializeField]private bool isPlayerMagnetActive = false;
	private bool isDownJumping = false;

	[Header("HoldTime")]
	[SerializeField] private float inputHoldTime = 1f;
	[SerializeField] private float magnetInputHoldTime;


	#endregion

	#region PublicMethod
	public void Magnet()
	{
		Debug.Log("PlayerMagnet On");
		isPlayerMagnetActive = true;
		magnetInputHoldTime = Time.time;
	}

	public void MagnetCanceled()
    {
		isPlayerMagnetActive = false;
		//Tween curTween = magnet.moveTween;
		Debug.Log("Magnet Canceled()");
		if (magnet != null)
		{
			magnet.ExitMagnet();
		}
;    }

	#endregion

	 #region PrivateMethod

	private void Awake()
	{
		transform.Find("Renderer").TryGetComponent(out anim);
		TryGetComponent(out rb);
	}

    private void Start()
    {
		boxOffset = 5f;
		boxSize = new Vector2(2 * boxOffset, 2 * boxOffset);
	}

    private void Update()
	{

		if(isPlayerMagnetActive)
        {
			magnet = CheckMagnet();

			if (magnet != null)
            {
				//Debug.Log(magnet.name);
				magnet.CallMagnet(this);
			}

			if (Time.time >= magnetInputHoldTime + inputHoldTime)
			{
				MagnetCanceled();
			}
		}

	}
	#endregion

	private Magnet CheckMagnet()
	{
		Magnet curMagnet = null;
		//float closestDistance = 10f;
		Vector3 playerPosition = transform.position; // 플레이어의 위치
		Vector3 boxCenter = playerPosition; // 박스의 중심 위치

		// 박스와 레이어 충돌 검사를 수행합니다.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, collisionLayer);

		if (colliders.Length > 0)
		{
			// 충돌한 오브젝트가 있는 경우
			foreach (Collider2D collider in colliders)
			{
				if (collider != null)
				{
						curMagnet = collider.gameObject.GetComponent<Magnet>();
				}				
			}
		}
		return curMagnet;
	}

	private void OnDrawGsdfjkizmos()
	{
		Gizmos.color = Color.red;
		// 디버그용으로 검출 박스를 그리는 코드 (Scene 뷰에서만 보임)
		Gizmos.DrawWireCube(transform.position, boxSize);
	}

	private void movingMovableMagnet()
    {

    }

	private void movingUnmovableMagnet()
    {

    }
}
