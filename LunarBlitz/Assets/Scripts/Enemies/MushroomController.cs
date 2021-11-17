using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : Enemy
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
        //remove before animation so towers stop targeting it
        Enemies.enemies.Remove(gameObject);
        movementSpeed = 0;

        // reward gold 
        playerController.AddMoney(killReward);
        //Debug.Log("Mushroom killed, rewarded + " + killReward);
        //TODO: animate death
        animator.SetBool("IsDead", true);
        //Animation[] animations = animator.GetComponents<Animation>();
        // wait for death animation to finish: https://answers.unity.com/questions/1208395/animator-wait-until-animation-finishes.html
        //yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //Animation animation = animator.GetAn
        //StartCoroutine("WaitForAnimation");
        Destroy(transform.gameObject);
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

    }
}
