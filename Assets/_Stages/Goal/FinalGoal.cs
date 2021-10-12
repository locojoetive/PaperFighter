using UnityEngine;

public class FinalGoal : MonoBehaviour
{
    KungFrogStatesAnimationSound kungFrog;
    bool isBossDone = false;
    Animator animator;

    private void Start()
    {
        kungFrog = FindObjectOfType<KungFrogStatesAnimationSound>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (kungFrog == null)
        {
            isBossDone = true;
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
