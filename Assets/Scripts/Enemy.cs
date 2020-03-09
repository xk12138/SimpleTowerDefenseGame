using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Vector3[] route = null;
    int current_route_index = 0;
    Vector3 target;
    private GameController gameController;

    public bool isAlive = false, dying = false, hasArrive = false, hasRemoved = false;
    public float speed = 3;

    public int health = 100;
    public int shield = 10;

    public int reword = 12;

    void Update()
    {
        if(isAlive)
        {
            //判断是否在目标位置
            Vector3 position = this.transform.position;
            float x = target.x - position.x, z = target.z - position.z;
            if (x * x + z * z < 0.1)
            {
                GetNextTarget();
            }

            float sum = Mathf.Abs(x) + Mathf.Abs(z);

            //开始移动
            this.transform.Translate(new Vector3(x / sum * speed * Time.deltaTime, 0, z / sum * speed * Time.deltaTime), Space.World);
        }
        else if(dying)
        {
            Invoke("Destroy", 3);       //三秒后摧毁自己的游戏对象
            Dead();
            dying = false;
        }
        else if(hasArrive)
        {
            Arrive();
            Destroy(this.gameObject);       //可能要放几帧动画
        }
        this.transform.Rotate(new Vector3(20 * Time.deltaTime, 0, 0));       //否则不停旋转
    }

    private void GetNextTarget()
    {
        if (route == null)
            return;

        if(route.Length <= current_route_index)
        {
            //Debug.Log("敌人已经到达了终点");
            isAlive = false;
            hasArrive = true;
            return;
        }

        target = route[current_route_index++];
        isAlive = true;
    }

    public void StartMove(Vector3[] route)
    {
        this.route = route;
        GetNextTarget();
    }

    public void Attacked(int power)
    {
        if (!hasRemoved)
        {
            power -= shield;
            if (power <= 0)       //防止越打血越多
            {
                power = 1;
            }
            health -= power;
            if (health <= 0)
            {
                isAlive = false;
                dying = true;
                hasRemoved = true;
            }
        }
    }

    public void Dead()
    {
        gameController.EnemyDec();
        gameController.enemies.Remove(this.gameObject);
        gameController.AddGold(reword);
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void setGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void Arrive()
    {
        gameController.LifeDec();
        gameController.EnemyDec();
        gameController.enemies.Remove(this.gameObject);
    }

}
