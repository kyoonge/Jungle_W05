using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
	#region PublicVariables
	public static CameraController instance;
	#endregion

	#region PrivateVariables
	[SerializeField] private Vector3 defaultCameraPosition;
	[SerializeField] private float offsetY;
	[SerializeField] private List<Vector2> stageCameraPosition = new List<Vector2>();
	[SerializeField] private Vector3 respawnPoint;
	[SerializeField] private CameraShaker shaker;
	private Transform player;

	//private Transform player;
	private Enums.EStage stage;
	#endregion

	#region PublicMethod
	public void MoveToRespawnPoint()
	{
		Camera.main.transform.localPosition = Vector2.zero;
		transform.position = respawnPoint;
	}
	public void MoveToStage(Enums.EStage stageNumber)
	{
		Vector3 result = Vector3.zero;
		switch(stageNumber)
		{
			case Enums.EStage.Stage1:
				result = stageCameraPosition[0];
				break;
			case Enums.EStage.Stage2:
				result = stageCameraPosition[1];
				break;
			case Enums.EStage.Stage3:
				result = stageCameraPosition[2];
				break;
		}
		result.z = -10f;
		transform.DOMove(result, 0.5f)
			.OnComplete(()=>{
				StartFollowToPlayer(stageNumber);
			});
	}
	public void HitShake()
	{
		shaker.StartCameraShake(CameraShaker.ECameraShakingType.playerHit);
	}
	public void ThunderShake()
	{
		shaker.StartCameraShake(CameraShaker.ECameraShakingType.thunder);
	}

	public void StartFollowToPlayer(Enums.EStage _stage)
    {
		stage = _stage;
    }

	public void EndFollowToPlayer()
    {
		stage = Enums.EStage.main;
    }
	#endregion

	#region PrivateMethod
	private void Awake()
	{
		if(instance == null)
			instance = this;
	}

    private void Start()
    {
		//player = GameManager.instance.GetPlayer().transform;
		//player = GameObject.Find("Player").transform;
		player = MagnetManager.Instance.player;
	}
    private void FollowToPlayer()
    {
		if (stage == Enums.EStage.title 
			|| stage == Enums.EStage.main)
			return;

        if (stage == Enums.EStage.Stage1)
        {
			Vector3 cameraPos = defaultCameraPosition;
			float posY = Mathf.Clamp(player.position.y + offsetY, 4f, 6f);
			cameraPos.y = posY;
			cameraPos.z = -10;
			transform.position = cameraPos;
		}
		else if(stage == Enums.EStage.Stage2)
		{
			Vector3 cameraPos = defaultCameraPosition;
			float posX = Mathf.Clamp(player.position.x, -45f, -40f);
			float posY = 2f;
			cameraPos.x = posX;
			cameraPos.y = posY;
			cameraPos.z = -10;
			transform.position = cameraPos;
		}
		else if(stage == Enums.EStage.Stage3)
		{
			Vector3 cameraPos = defaultCameraPosition;
			float posY = Mathf.Clamp(player.position.y + offsetY, -18.5f, -10f);
			cameraPos.x = -19f;
			cameraPos.y = posY;
			cameraPos.z = -10;
			transform.position = cameraPos;
		}
		
    }

    private void Update()
    {
		FollowToPlayer();
    }

    #endregion
}
