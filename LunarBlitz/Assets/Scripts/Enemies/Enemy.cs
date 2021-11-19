using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Made in part by following tutorial: https://www.youtube.com/watch?v=iKtxC4mzpaI&t=0s
public class Enemy : MonoBehaviour
{
    [SerializeField] public float remainingHealth;
    [SerializeField] public float startHealth; // TODO: Will be used for healthbar

    public GameObject healthBarUI;
    public Slider healthBarSlider;

    [SerializeField] public float movementSpeed;
    public float DistanceCovered { get; set; }
    public int killReward; // The amount of money the played gets when this enemy is killed
    public int livesCost; // The amount of damage the enemy does when it reached the end
    //public float minTimeBetweenSpawns;

    [SerializeField] protected Sprite mobSkin;
    [SerializeField] protected Animator animator; //TODO: attach animations to mobs

    protected PlayerController playerController;
    
    protected bool hasReachedEnd = false;
    

    //public GameObject targetTile;

    void Awake()
    {
        remainingHealth = startHealth;
        Enemies.enemies.Add(gameObject);
        healthBarUI.GetComponent<Canvas>().worldCamera = Camera.main;
        
    }

    public virtual void Start()
    {
        initEnemy();
        playerController = FindObjectOfType<PlayerController>();

        //hide healthbar by default
        if(healthBarUI.GetComponent<Canvas>().enabled)
        {
            showHealthBar(false);
        }
        animator.SetBool("IsMoving", true);
    }

    public virtual void Update()
    {
        healthBarSlider.value = CalculateHealth();

        // If in the future I want a mob that heals others, prevent from having >100% health
        if(remainingHealth > startHealth)
        {
            remainingHealth = startHealth;
        }
    }

    protected virtual void initEnemy()
    {
        //targetTile = MapGenerator.startTile;
        if(mobSkin == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = mobSkin;
        }
        healthBarSlider.value = CalculateHealth();
        DistanceCovered = 0f;
    }

    // returns percentage of health remaining
    protected float CalculateHealth()
    {
        return remainingHealth / (float)startHealth;
    }

    // listen for getting hit by a bullet
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            //Debug.Log("Collision with Projectile detected! " + collision.gameObject.name);
            Bullet b = collision.gameObject.GetComponent<Bullet>();

            if(b != null && b.target != null)
            {
                // prevent "splash" damage when sprites may overlap
                if (b.target.name.Equals(gameObject.name))
                {
                    takeDamage(b.damage);
                }
            }
        }
    }

    public virtual void takeDamage(float amount)
    {
        // only show healthbar once damage has been taken for the first time
        if (remainingHealth == startHealth && amount > 0)
        {
            showHealthBar(true);
            //Debug.Log("Showing " + gameObject.name + "\'s Healthbar");
        }


        remainingHealth -= amount;

        if (remainingHealth <= 0)
        {
            die();
        }
    }

    protected virtual void showHealthBar(bool isShown)
    {
        healthBarUI.SetActive(isShown);
        healthBarUI.GetComponent<Canvas>().enabled = isShown;
    }

    protected virtual void die()
    {
        //Debug.Log(gameObject.name + " die()");
        // reward gold 
        playerController.AddMoney(killReward);
        //TODO: animate death
        Enemies.enemies.Remove(gameObject);
        movementSpeed = 0;
        showHealthBar(false);

        //animate death
        animator.SetTrigger("Death");


        // wait for animaton to finish before destroying
        float destroyDelay = animator.GetCurrentAnimatorStateInfo(0).length + 0.25f; // + animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //Debug.Log("Destroy delay: " + destroyDelay);
        Destroy(gameObject, destroyDelay);
    }

    private IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public virtual void moveEnemyTowards(Vector3 targetDest)
    {
        if (targetDest != null && gameObject != null)
        {
            float thisMoveDist = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetDest,
                thisMoveDist);

            // Update distance covered to be used in Tower targeting system
            DistanceCovered = DistanceCovered + thisMoveDist;
        }
    }

    protected virtual void checkPosition()
    {
        
    }
}
