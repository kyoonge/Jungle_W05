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
	[SerializeField] private float grondCheckRayLength;
	[SerializeField] private float downJumpRayLength;
	[SerializeField] private Magnet magnet;

	[Header("Raycast")]
	public LayerMask collisionLayer;
	private Vector2 boxSize = new Vector2(2f, 2f);

	[SerializeField]private bool isPlayerMagnetActive = false;
	private bool isDownJumping = false;

	[Header("HoldTime")]
	[SerializeField] private float inputHoldTime = 1f;
	[SerializeField] private float magnetInputHoldTime;

	[Header("MagnetMove")]
	[SerializeField]private Tween moveTween;
	[SerializeField] private float moveDuration = 1f;
	#endregion

	#region PublicMethod
	public void Magnet()
	{
		Debug.Log("PlayerMagnet On");
		isPlayerMagnetActive = true;
		magnetInputHoldTime = Time.time;
		//if (anim.GetBool("jump") == true)
		//	return;


		


		//rb.AddForce(Vector2.up * magnetForce, ForceMode2D.Impulse);
		//Debug.Log(rb.velocity);
		//anim.SetBool("jump", true);
	}

	public void MagnetCanceled()
    {
		isPlayerMagnetActive = false;
		Debug.Log("Magnet Canaeled()");
		if (moveTween != null)
		{
			// Magnet이 없을 때, 이동 중인 Tween을 취소하고 위치를 즉시 멈춥니다.
			moveTween.Kill();
			moveTween = null;
		}
;    }
	//public void DownJump()
	//{
	//	if (anim.GetBool("jump") == true || isDownJumping == true)
	//		return;

	//	isDownJumping = true;
	//	//Invoke(nameof(DownJumpReady), 0.5f);
	//	anim.SetBool("jump", true);
	//	Vector2 origin = (Vector2)transform.position + Vector2.down * 1.5f;
	//	RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, downJumpRayLength, 1 << LayerMask.NameToLayer("Ground"));
	//	Debug.DrawRay(origin, Vector2.down * downJumpRayLength, Color.red);
	//	if(hit.collider != null)
	//	{
	//		transform.position = new Vector2(transform.position.x, transform.position.y - 0.3f);
	//	}
	//}
	#endregion

	 #region PrivateMethod
	private void Awake()
	{
		transform.Find("Renderer").TryGetComponent(out anim);
		TryGetComponent(out rb);
	}
	private void Update()
	{
		Magnet currentMagnet = CheckMagnet();

		if(isPlayerMagnetActive)
        {
			if (currentMagnet != null)
			{
				Vector3 targetPosition = currentMagnet.transform.position;

				// 이동 중인 Tween을 취소하고 새로운 Tween을 시작
				if (moveTween != null)
				{
					moveTween.Kill();
				}

				// DOTween을 사용하여 오브젝트의 위치를 부드럽게 이동시킵니다.
				moveTween = transform.DOMove(targetPosition, moveDuration)
					.SetEase(Ease.Linear)
					.OnComplete(() =>
					{
					// 이동이 완료된 후 실행할 코드
					Debug.Log("Player reached Magnet");
						MagnetCanceled();
					});
			}

			if (Time.time >= magnetInputHoldTime + inputHoldTime)
			{
				MagnetCanceled();
			}
		}


		
		//      if (isPlayerMagnetActive)
		//      {
		//          if (CheckMagnet()!=null)
		//          {
		//		magnet.CallMagnet();
		//          }

		//	if(Time.time >= magnetInputHoldTime + inputHoldTime)
		//          {
		//		MagnetCanceled();

		//	}
		//}


	}
	//private void CheckMagnet()
	//{
	//	if(rb.velocity.y > 0)
	//	{
	//		return;
	//	}
	//	RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, grondCheckRayLength, 1 << LayerMask.NameToLayer("Magnet"));

	//	Debug.DrawRay(transform.position, Vector2.down * grondCheckRayLength, Color.yellow);

	//	if (hit.collider != null)
	//	{
	//		anim.SetBool("jump", false);
	//	}
	//}

	//private void DownJumpReady()
	//{
	//	isDownJumping = false;
	//}
	#endregion

	private Magnet CheckMagnet()
	{
		Magnet curMagnet = null;
		float closestDistance = 10f;
		Vector3 playerPosition = transform.position; // 플레이어의 위치
		Vector3 boxCenter = playerPosition; // 박스의 중심 위치

		// 박스와 레이어 충돌 검사를 수행합니다.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize / 2, 0f, collisionLayer);

		if (colliders.Length > 0)
		{
			// 충돌한 오브젝트가 있는 경우
			foreach (Collider2D collider in colliders)
			{
				if (collider != null)
				{
					// 마그넷 컴포넌트를 가진 경우
					float distance = Vector3.Distance(playerPosition, collider.transform.position);
					if (distance < closestDistance)
					{
						closestDistance = distance;
						curMagnet = collider.gameObject.GetComponent<Magnet>();
					}
				}
				
			}
		}
		magnet = curMagnet;

		return curMagnet;
	}

	private void OnDrawGizmos()
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
