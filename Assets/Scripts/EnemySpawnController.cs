using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    //敌人生成器
    //分析：
    //  生成一个敌人： CreateEnemy，认为出生点就是本身
    //  计数：生成的敌人个数和要生成的敌人总数
    //  保存可用路径：可用路径坐标和总数

    GameController gameController;

    Vector3[] route = new Vector3[5]
    {
        new Vector3(5.15f, 0, 6.35f),
        new Vector3(5.15f, 0, 1f),
        new Vector3(-5.82f, 0, 1f),
        new Vector3(-5.82f, 0, -6.0f),
        new Vector3(9.5f, 0, -6.0f)
    };

    //每个敌人生成的间隔
    public float interval = 0.3f;           
    public float current_interval = 0;
    //敌人的最大数量和当前数量，应该如何处理多波敌人？
    public List<int> enemy_counts = new List<int>()
    { 10, 15, 50 };
    public int current_wave = 0;
    public int current_enemy_max_counts;
    public int enemy_count = 0;

    //一波敌人的准备时间
    public bool isComing = false;
    public float wave_interval = 5;
    public float current_wave_interval = 0;

    public bool isCreating = false;
    
    void Update()
    {
        if (isCreating)
        {
            current_interval += Time.deltaTime;
            if (current_interval >= interval)
            {
                CreateEnemy();
                current_interval -= interval;
            }
        }
        else if(isComing)
        {
            current_wave_interval += Time.deltaTime;
            if(current_wave_interval >= wave_interval)
            {
                isComing = false;
                Next();
                current_wave_interval = 0;
            }
        }
        else
        {
            if(gameController.allClear())
            {
                isComing = true;
            }
        }
    }  

    public void Begin()
    {
        isComing = true;
    }

    public void Next()
    {
        if(current_wave >= enemy_counts.Count)
        {
            this.gameController.Win();
            return;
        }
        enemy_count = 0;
        current_interval = 0;
        isCreating = true;
        this.gameController.SetEnemy(enemy_counts[current_wave]);
    }

    public void CreateEnemy()
    {
        Object enemy = Resources.Load("Enemy");
        if(enemy == null)
        {
            Debug.Log("未能正确加载预设体");
            return;
        }
        GameObject enemy_go = enemy as GameObject;
        if(enemy_go == null)
        {
            Debug.Log("未能将预设体转换为游戏对象");
        }
        enemy_go = Instantiate(enemy_go, this.transform.position, Quaternion.identity);
        Enemy enemyController = enemy_go.GetComponent<Enemy>();
        enemyController.setGameController(gameController);
        enemyController.StartMove(route);
        gameController.AddEnemy(enemy_go);

        //控制生成的敌人的数量，全部生成完毕时停止再生成敌人
        enemy_count++;
        if(enemy_count >= enemy_counts[current_wave])
        {
            isCreating = false;
            current_wave++;
        }
    }
    
    public void setGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

}
