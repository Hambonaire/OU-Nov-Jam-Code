using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    GameManager gameManager;
    BaseManager baseManager;
    WaveManager waveManager;

    public bool isEnemy = false;
    public bool isGate = false;

    public bool isStationaryAlly = true;
    [HideInInspector]
    public Transform stationaryPoint;

    Transform[] pathWaypoints;
    int waypointIndex = 0;
    int nextWaypointIndex = 1;

    [SerializeField]
    private float waypointOffset = 0;

    Minion minionTarget;

    public int goldReward = 10;
    public int healthDamage = 1;

    public float maxHealth = 10;
    public float health = 10;
    [SerializeField]
    float moveSpeed = 4;
    [SerializeField]
    float damage = 2;
    [SerializeField]
    float attackInterval = 1;
    [SerializeField]
    float attackRange = 2;
    [SerializeField]
    float aggroRange = 4;

    float lastAttackTime;
    bool isAttacking;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager._instance;
        baseManager = BaseManager._instance;
        waveManager = WaveManager._instance;

        Initialize(waveManager.pathWaypoints);
    }

    public void Initialize(Transform[] waypoints)
    {
        if (isEnemy)
            waveManager.aliveEnemies.Add(gameObject);
        else
            waveManager.aliveAllies.Add(gameObject);

        if (isGate)
            return;

        this.pathWaypoints = waypoints;

        if (isEnemy)
            waypointIndex = 0;
        else
        {
            waypointIndex = pathWaypoints.Length;
            nextWaypointIndex = waypointIndex - 1;
        }

        waypointOffset = Random.Range(-0.3f, 0.3f);

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGate)
            return;

        GetTarget();

        Rotate();

        MoveAttack();

        if (Vector3.Distance(transform.position, pathWaypoints[nextWaypointIndex].position) < 0.5f)
        {
            if (isEnemy && nextWaypointIndex == pathWaypoints.Length - 1)
            {
                baseManager.TakeHealthDamage(healthDamage);
                //Destroy(gameObject);
                Die(false);
            }
            else if (!isEnemy && !isStationaryAlly && nextWaypointIndex == 0)
            {
                //Destroy(gameObject);
                Die(false);
            }

            if (isEnemy)
            {
                waypointIndex++;
                nextWaypointIndex++;
            }
            else
            {
                waypointIndex--;
                nextWaypointIndex--;
            }
        }

    }

    void Rotate()
    {
        float angle;
        Vector3 direction = Vector3.forward;

        if (isEnemy)
        {
            if (minionTarget == null)
                direction = pathWaypoints[nextWaypointIndex].position + (pathWaypoints[nextWaypointIndex].right * waypointOffset)
                    - transform.position;
            else
                direction = minionTarget.transform.position - transform.position;
        }
        else
        {
            if (minionTarget == null)
            {
                if (isStationaryAlly)
                    angle = Vector3.Angle(transform.position, stationaryPoint.position);
                else
                    direction = pathWaypoints[nextWaypointIndex].position + (pathWaypoints[nextWaypointIndex].right * waypointOffset)
                                        - transform.position;
            }
            else
                direction = minionTarget.transform.position - transform.position;
        }

        Quaternion toRotation = Quaternion.FromToRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 5 * Time.deltaTime);
        //transform.Rotate(Vector3.up, -angle * Time.deltaTime * 5);
    }

    void MoveAttack()
    {
        isAttacking = false;

        if (minionTarget != null)
        {
            print(Vector3.Distance(transform.position, minionTarget.transform.position));

            if (Vector3.Distance(transform.position, minionTarget.transform.position) <= attackRange)
            {
                if (Time.time < lastAttackTime + attackInterval)
                    return;

                Debug.Log("Attacking!");

                isAttacking = true;
                minionTarget.TakeDamage(damage);

                lastAttackTime = Time.time;
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed/2);
            }
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
    }

    void GetTarget()
    {
        if (minionTarget != null && !minionTarget.gameObject.activeSelf)
            minionTarget = null;

        if (minionTarget == null)
        {
            float shortest = 99.9f;
            GameObject closest = null;

            List<GameObject> listToIter = null;

            if (isEnemy)
                listToIter = WaveManager._instance.aliveAllies;
            else
                listToIter = WaveManager._instance.aliveEnemies;

            foreach (GameObject enemy in listToIter)
            {
                if (enemy == null || !enemy.activeSelf)
                    continue;

                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < aggroRange && dist < shortest)
                {
                    shortest = dist;
                    closest = enemy;
                }
            }

            if (closest != null)
                NegotiateTarget(closest.GetComponent<Minion>());
        }

    }

    public bool NegotiateTarget(Minion otherMinion)
    {
        if (otherMinion.isGate)
        {
            minionTarget = otherMinion;
            Debug.Log("Got Target!");
            return true;
        }

        if (otherMinion.minionTarget == null)
        {
            otherMinion.minionTarget = this;
            minionTarget = otherMinion;
            Debug.Log("Got/Set Target!");
            return true;
        }

        return false;
    }

    public void TakeDamage(float dam)
    {
        health -= dam;

        if (health <= 0)
            Die(true);
    }

    public void MeleeTrigger()
    {
        if (minionTarget != null)
            minionTarget.GetComponent<Minion>().TakeDamage(damage);
    }

    public void RangeTrigger()
    {

    }

    void Die(bool killed)
    {
        //if (health <= 0)
        //{
            if (isGate)
                gameObject.SetActive(false);
            else
            {
                Destroy(gameObject);

                if (isEnemy)
                    waveManager.aliveEnemies.Remove(gameObject);
                else
                    waveManager.aliveAllies.Remove(gameObject);

                if (isEnemy && killed)
                    gameManager.AddGold(goldReward);
            }

        //}

    }
}
