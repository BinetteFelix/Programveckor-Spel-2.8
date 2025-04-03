using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    public static EnemyAnimations Instance;
    Animator animator;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        AnimateWalking(7);
    }
    public void Die()
    {
        animator.SetBool("IsDead", true);
    }
    public void AnimateWalking(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
}