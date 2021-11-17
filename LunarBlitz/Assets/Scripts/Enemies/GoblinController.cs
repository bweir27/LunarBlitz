using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : Enemy
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

    protected override void moveEnemy()
    {
        base.moveEnemy();
        //animator.SetBool("IsMoving", true);
    }

    protected override void checkPosition()
    {
        base.checkPosition();
    }

    protected override void die()
    {
        //remove before animation so towers stop targeting it
        Enemies.enemies.Remove(gameObject);
        movementSpeed = 0;

        // reward gold 
        playerController.AddMoney(killReward);
        //Debug.Log("Goblin killed, rewarded +" + killReward);
        //TODO: animate death
        //animator.SetBool("IsDead", true);
        Destroy(transform.gameObject);
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

    }
}
