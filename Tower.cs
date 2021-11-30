using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Structure
{
    public Vector2 damageRange = new Vector2(2, 3);
    public float interval = 1.5f;
    public int range = 10;

    private float lastFireTime;

    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    public void Update()
    {
        FireAtTarget();
    }

    void FireAtTarget()
    {
        for (int index = 0; index < waveManager.aliveEnemies.Count; index++)
        {
            if (waveManager.aliveEnemies[index] != null &&
                Vector3.Distance(transform.position, waveManager.aliveEnemies[index].transform.position) <= range &&
                Time.time >= lastFireTime + interval)
            {
                // TODO: Deal damage / spawn projectile
                waveManager.aliveEnemies[index].GetComponent<Minion>().TakeDamage(Random.Range(damageRange.x, damageRange.y));
                lastFireTime = Time.time;
            }
        }
    }

}
