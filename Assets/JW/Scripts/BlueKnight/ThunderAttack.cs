using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttack : MonoBehaviour
{
    [SerializeField] private string animationName;
    [SerializeField] private Animator attackAnim;
    [SerializeField] private Collider2D thunderCollider;
    [SerializeField] private float AttackTime;
    

    private void OnEnable()
    {
        StartCoroutine(nameof(AttackPlay));
    }
    public IEnumerator AttackPlay()
    {
        thunderCollider.enabled = true;
		CameraController.instance.ThunderShake();
		yield return new WaitForSeconds(AttackTime);
        Destroy(this.gameObject);
    }

    public void InitAttack()
    {
        StopCoroutine(nameof(AttackPlay));
        thunderCollider.enabled = false;

    }
   
}
