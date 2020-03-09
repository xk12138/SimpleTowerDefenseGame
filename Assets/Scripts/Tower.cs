using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float cool_down = 1;
    public float current_cool_down = 0;
    public int price = 100;
    public float range = 3.5f;
    public float range_2 = 0;

    public GameController gameController;

    void Start()
    {
        range_2 = range * range;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_cool_down >= cool_down)
        {
            GameObject target = GetTargetEnemy();
            if(target != null)
            {
                //发射子弹打向敌人
                Attack(target);
                current_cool_down -= cool_down;
            }
        }
        else
        {
            current_cool_down += Time.deltaTime;
        }
    }

    //从主控程序中获取目标（除了遍历好像没有好方法了）
    private GameObject GetTargetEnemy()
    {
        if(gameController.enemies == null || gameController.enemies.Count == 0)
        {
            return null;
        }
        Vector3 my_position = this.transform.position;
        foreach(GameObject enemy in gameController.enemies)
        {
            Vector3 enemy_position = enemy.transform.position;
            float x = my_position.x - enemy_position.x;
            float z = my_position.z - enemy_position.z;
            if(x * x + z * z <= range_2)
            {
                return enemy;
            }
        }
        return null;
    }

    public void Attack(GameObject enemy)
    {
        Object bullet_prefab = Resources.Load("Bullet");
        if(bullet_prefab == null)
        {
            Debug.Log("未能成功加载子弹的预设体");
            return;
        }
        GameObject bullet = bullet_prefab as GameObject;
        if(bullet == null)
        {
            Debug.Log("未能成功转化为游戏对象");
            return;
        }

        //设置子弹从头部发射
        Vector3 my_position = this.transform.position;
        Transform[] transforms = this.GetComponentsInChildren<Transform>();
        foreach(Transform transform in transforms)
        {
            if(transform.name == "Head")
            {
                my_position = transform.position;
            }
        }
        bullet = Instantiate(bullet, new Vector3(my_position.x, my_position.y, my_position.z), Quaternion.identity);
        bullet.GetComponent<Bullet>().SetTarget(enemy);
    }
    
    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }
}
