using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMagnet : MonoBehaviour
{
	#region PublicVariables
	[SerializeField] public Enums.MagnetType magnetType;
	#endregion

	#region PrivateVariables
	private Animator anim;
	private Rigidbody2D rb;
	public GameObject effect;

	[SerializeField] private float magnetForce;
	private Magnet m_curMagnet;
	[SerializeField] private Magnet magnet;

	[Header("Raycast")]
	public LayerMask collisionLayer;
	public float boxOffset;
	private Vector2 boxSize;
	private float buttonCheckRayLength = 1f;
	[SerializeField] private bool canButtonJump = false;


	[SerializeField]private bool isPlayerMagnetActive = false;
	private bool isDownJumping = false;

	[Header("HoldTime")]
	[SerializeField] public float inputHoldTime = 1f;
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
		effect.SetActive(false);
		rb.gravityScale = 3;
		isPlayerMagnetActive = false;
		//Tween curTween = magnet.moveTween;
		canButtonJump = false;
		
		if (magnet != null)
		{
			Debug.Log("Magnet Canceled()");
			magnet.ExitMagnet();
			magnet = null;
		}
;    }

	#endregion

	 #region PrivateMethod

	private void Awake()
	{
		transform.Find("@Renderer").TryGetComponent(out anim);
		TryGetComponent(out rb);
	}

    private void Start()
    {
		boxOffset = 5f;
		boxSize = new Vector2(2 * boxOffset, 2 * boxOffset);
		buttonCheckRayLength = 1f;
	}

    private void Update()
	{


		if(isPlayerMagnetActive)
        {
			m_curMagnet = CheckMagnet();
			CheckButton();

            if (canButtonJump == true)
            {
				magnet.CallMagnetButton(this);
				KeepCheckButton();
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
			float closestDistance = float.MaxValue;
			// 충돌한 오브젝트가 있는 경우
			foreach (Collider2D collider in colliders)
			{
				if (collider != null)
				{
                    if (collider.CompareTag("MagnetObject"))
                    {
						//curMagnet = collider.gameObject.GetComponent<Magnet>();
						//magnet = curMagnet;
						//magnet.CallMagnet(this);

						if(magnet!= null)
                        {
							float distance1 = Vector3.Distance(playerPosition, collider.gameObject.transform.position);
							float distance2 = Vector3.Distance(playerPosition, magnet.transform.position);

							if (distance1 <= distance2)
							{
								curMagnet = collider.gameObject.GetComponent<Magnet>();
								magnet = curMagnet;
							}
						}
                        else
                        {
							curMagnet = collider.gameObject.GetComponent<Magnet>();
							magnet = curMagnet;
						}

					
						
						magnet.CallMagnet(this);
					}

				}				
			}
		}
		return curMagnet;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		// 디버그용으로 검출 박스를 그리는 코드 (Scene 뷰에서만 보임)
		Gizmos.DrawWireCube(transform.position, boxSize);
	}

	private void CheckButton()
	{
        if (rb.velocity.y > 0)
        {
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, buttonCheckRayLength, 1 << LayerMask.NameToLayer("Magnet"));
        Debug.DrawRay(transform.position, Vector2.down * buttonCheckRayLength, Color.yellow);
        if(hit.collider != null)
        {
			if (hit.collider.CompareTag("MagnetButton"))
			{
				canButtonJump = true;
				print("magnet button");
				magnet = hit.collider.transform.GetComponent<Magnet>();
				//magnet.CallMagnetButton(this);
			}
		}

	}

	private void KeepCheckButton()
    {
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 10f, 1 << LayerMask.NameToLayer("Magnet"));
		Debug.DrawRay(transform.position, Vector2.down * 10f, Color.blue);
		bool isButtonExist = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.CompareTag("MagnetButton"))
            {
				isButtonExist = true;
				break;
            }
        }

        if (!isButtonExist)
        {
			MagnetCanceled();
        }
	}

}
