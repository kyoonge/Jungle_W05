using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private bool isPlayerMagnetActive = false;
	private bool isDownJumping = false;

	[Header("HoldTime")]
	[SerializeField] private float inputHoldTime = 1f;
	[SerializeField] private float magnetInputHoldTime;
	#endregion

	#region PublicMethod
	public void Magnet()
	{
		isPlayerMagnetActive = true;
		magnetInputHoldTime = Time.time;
		//if (anim.GetBool("jump") == true)
		//	return;


		//�ڼ� �������


		//rb.AddForce(Vector2.up * magnetForce, ForceMode2D.Impulse);
		//Debug.Log(rb.velocity);
		//anim.SetBool("jump", true);
	}

	public void MagnetCanceled()
    {
		isPlayerMagnetActive = false;
		Debug.Log("Magnet Canaeled()");
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
        if (isPlayerMagnetActive)
        {
            if (CheckMagnet()!=null)
            {
				magnet.CallMagnet();
            }

			if(Time.time >= magnetInputHoldTime + inputHoldTime)
            {
				MagnetCanceled();

			}
		}


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
		Vector3 playerPosition = transform.position; // �÷��̾��� ��ġ
		Vector3 boxCenter = playerPosition; // �ڽ��� �߽� ��ġ

		// �ڽ��� ���̾� �浹 �˻縦 �����մϴ�.
		Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, boxSize / 2, 0f, collisionLayer);

		if (colliders.Length > 0)
		{
			// �浹�� ������Ʈ�� �ִ� ���
			foreach (Collider2D collider in colliders)
			{
				if (collider != null)
				{
					// ���׳� ������Ʈ�� ���� ���
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
		// ����׿����� ���� �ڽ��� �׸��� �ڵ� (Scene �信���� ����)
		Gizmos.DrawWireCube(transform.position, boxSize);
	}
}
