using UnityEngine;
using System.Threading;
using UnityCommon;

public abstract class BossPattern : MonoBehaviour
{
	#region PublicVariables
	#endregion

	#region PrivateVariables
	private Boss main;
	protected Animator anim;
	[SerializeField] protected string animationStateName;
	[SerializeField] protected int preDelayMilliSeconds;
	[SerializeField] protected int postDelayMilliSeconds;

	protected CancellationTokenSource preDelaySource = new CancellationTokenSource();
	protected CancellationTokenSource postDelaySource = new CancellationTokenSource();
	#endregion

	#region PublicMethod
	public async UniTaskVoid Act()
	{
		PreProcessing();
		PlayAnimation();
		await UniTask.Delay(preDelayMilliSeconds, cancellationToken: preDelaySource.Token);
		ActionContext();
		await UniTask.Delay(postDelayMilliSeconds, cancellationToken: postDelaySource.Token);
		PostProcessing();

		CallNextAction();
	}
	public void CallNextAction()
	{
		main.PatternNext();
	}
	public void ShutdownAction()
	{
		preDelaySource.Cancel();
		postDelaySource.Cancel();
	}
	#endregion

	#region PrivateMethod
	protected virtual void Awake()
	{
		TryGetComponent(out main);
	}
	private void Start()
	{
		anim = main.GetAnimator();
	}
	protected virtual void OnEnable()
	{
		if (preDelaySource != null)
			preDelaySource.Dispose();
		preDelaySource = new CancellationTokenSource();
		if(postDelaySource != null)
			postDelaySource.Dispose();
		postDelaySource = new CancellationTokenSource();
	}
	protected virtual void OnDisable()
	{
		ShutdownAction();
	}
	private void PlayAnimation()
	{
		if (animationStateName != "")
		{
			transform.Find("renderer").TryGetComponent(out anim);
			anim.Play(animationStateName);
		}
	}
	protected virtual void PreProcessing()
	{

	}
	protected virtual void PostProcessing()
	{

	}
	protected abstract void ActionContext();
	#endregion
}
