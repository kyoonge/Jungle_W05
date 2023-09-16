using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(PlayerDodge), typeof(PlayerMove), typeof(PlayerJump))]
public class Player : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	[SerializeField] private Animator anim;

	private PlayerAction input;

	private PlayerDodge dodge;
	private PlayerMove move;
	private PlayerJump jump;
	private PlayerAttack attack;
	private PlayerMagnet magnet;

	private bool isInvincible;
	[SerializeField] private bool isPressedDown;
	[SerializeField] public bool canAct = false;
	Vector2 RespawnPoint = new Vector2(-19f, 0.3f);



	
	#endregion

	#region PublicMethod
	public void Hit()
	{
		if (GameManager.instance.GetGameState() == GameManager.EGameState.battle || GameManager.instance.GetGameState() == GameManager.EGameState.tutorial)
		{
			if (isInvincible) return;
			GameManager.instance.BattleEnd();
			BodyGenerator.instance.SpawnBody(transform, false);
		}
	}
	public Animator GetAnimator() => anim;
	public void SetInvincibility(bool b) => isInvincible = b;
	public void CanAct() => canAct = true;
	public void CanNotAct()
	{
		canAct = false;
		move.MoveCanceled();
		attack.isAttack = false;
	}

	public void MoveToRespawnPoint()
	{
		transform.position = RespawnPoint;
	}
	#endregion
	private void OnParticleCollision()
	{
		Hit();
		SetInvincibility(true);
	}

	#region PrivateMethod
	private void Awake()
	{
		input = new PlayerAction();
		TryGetComponent(out dodge);
		TryGetComponent(out move);
		TryGetComponent(out jump);
		TryGetComponent(out attack);
		TryGetComponent(out magnet);
	}
    private void Start()
    {
		CanAct();
    }

    private void OnEnable()
	{
		input.Enable();
		input.Player.Move.performed += Move;
		input.Player.Move.canceled += MoveCanceled;
		input.Player.Attack.performed += Attack;
		input.Player.Attack.canceled += AttackCanceled;
		input.Player.Dodge.performed += Dodge;
		input.Player.Jump.performed += Jump;
		input.Player.Down.performed += Down;
		input.Player.Down.canceled += DownCanceled;
		input.Player.Magnet.performed += Magnet;
		input.Player.Magnet.canceled += MagnetCanceled;

	}
	private void OnDisable()
	{
		input.Player.Move.performed -= Move;
		input.Player.Move.canceled -= MoveCanceled;
		input.Player.Attack.performed -= Attack;
		input.Player.Attack.canceled -= AttackCanceled;
		input.Player.Dodge.performed -= Dodge;
		input.Player.Jump.performed -= Jump;
		input.Player.Down.performed -= Down;
		input.Player.Down.canceled -= DownCanceled;
		input.Player.Magnet.performed -= Magnet;
		input.Player.Magnet.canceled -= MagnetCanceled;
		input.Disable();
	}
	private void Move(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		move.Move((int)_context.ReadValue<Vector2>().x);
	}
	private void MoveCanceled(InputAction.CallbackContext _context)
	{
		move.MoveCanceled();
	}
	private void Jump(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		if (isPressedDown == true)
		{
			jump.DownJump();
		}
		else
		{
			Debug.Log("jump");
			jump.Jump();
		}
	}
	private void Attack(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;

		attack.isAttack = true;
	}

	private void AttackCanceled(InputAction.CallbackContext _context)
    {
		attack.isAttack = false;
	}
	private void Dodge(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		attack.isAttack = false;
		dodge.Dodge();
	}
	private void Down(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;
		isPressedDown = true;
	}
	private void DownCanceled(InputAction.CallbackContext _context)
	{
		isPressedDown = false;
	}

	private void Magnet(InputAction.CallbackContext _context)
	{
		if (canAct == false)
			return;

		Debug.Log("magnet on");
		magnet.Magnet();
	}
	private void MagnetCanceled(InputAction.CallbackContext _context)
	{
		magnet.MagnetCanceled();
	}

	#endregion
}
