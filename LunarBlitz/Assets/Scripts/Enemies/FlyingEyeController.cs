using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeController : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        if (animator == null)
        {
            Debug.LogError("No animator attached!");
        }
    }

    public override void moveEnemyTowards(Vector3 targetDest)
    {
        base.moveEnemyTowards(targetDest);
        animator.SetBool("IsMoving", true);
    }

    protected override void checkPosition()
    {
        base.checkPosition();
    }

    protected override void die()
    {
        base.die();
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

    }
}
