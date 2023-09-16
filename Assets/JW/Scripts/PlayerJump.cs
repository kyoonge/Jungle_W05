using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Animator anim;
	private Rigidbody2D rb;

	[SerializeField] private float jumpForce;
	[SerializeField] private float grondCheckRayLength;
	[SerializeField] private float downJumpRayLength;

	private bool isDownJumping = false;
	#endregion

	#region PublicMethod
	public void Jump()
	{
		if (anim.GetBool("jump") == true)
			return;

		
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		Debug.Log(rb.velocity);
		anim.SetBool("jump", true);
	}
	public void DownJump()
	{
		if (anim.GetBool("jump") == true || isDownJumping == true)
			return;

		isDownJumping = true;
		Invoke(nameof(DownJumpReady), 0.5f);
		anim.SetBool("jump", true);
		Vector2 origin = (Vector2)transform.position + Vector2.down * 1.5f;
		RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, downJumpRayLength, 1 << LayerMask.NameToLayer("Ground"));
		Debug.DrawRay(origin, Vector2.down * downJumpRayLength, Color.red);
		if(hit.collider != null)
		{
			transform.position = new Vector2(transform.position.x, transform.position.y - 0.3f);
		}
	}
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		transform.Find("Renderer").TryGetComponent(out anim);
		TryGetComponent(out rb);
	}
	private void Update()
	{
		CheckGround();
	}
	private void CheckGround()
	{
		if(rb.velocity.y > 0)
		{
			return;
		}
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, grondCheckRayLength, 1 << LayerMask.NameToLayer("Ground"));
		Debug.DrawRay(transform.position, Vector2.down * grondCheckRayLength, Color.red);

		if (hit.collider != null)
		{
			anim.SetBool("jump", false);
		}
	}
	private void DownJumpReady()
	{
		isDownJumping = false;
	}
	#endregion
}
