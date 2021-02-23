using UnityEngine;

public class FinalGoal : MonoBehaviour
{
    KungFrogStatesAnimationSound kungFrog;
    bool isBossDone = true;
    Animator animator;

    private void Start()
    {
        kungFrog = FindObjectOfType<KungFrogStatesAnimationSound>();
        animator = GetComponent<Animator>();
        if (kungFrog != null)
        {
            isBossDone = false;
        }
    }

    private void Update()
    {
        if (kungFrog != null && !isBossDone)
        {
            isBossDone = kungFrog.life == 0;
            animator.SetBool("isBossDone", isBossDone);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        animator.SetTrigger("touched");
        if (other.tag == "Player" && isBossDone)
        {
            StageManager.InitializeNextStage();
        }
    }
}
