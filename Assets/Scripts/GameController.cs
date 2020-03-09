using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //游戏的得分、金钱、当前剩余敌人数和当前剩余生命数由游戏控制器保存与使用
    [SerializeField]
    private int score = 0;
    [SerializeField]
    private int gold = 400;
    [SerializeField]
    private int enemy = 0;
    [SerializeField]
    private int life = 5;

    //几个3D Text的实例，用于在数据发生变化后修改界面上的显示
    public TextMesh Score;
    public TextMesh GoldCount;
    public TextMesh EnemyCount;
    public TextMesh LifeCount;

    //与敌人生成器相互绑定，减少搜索时间
    public EnemySpawnController enemySpawnController;
    public List<GameObject> enemies = new List<GameObject>();
    
    void Start()
    {
        if(enemySpawnController != null)
        {
            enemySpawnController.setGameController(this);
            enemySpawnController.Begin();                   //先使用直接开始第一波的形式便于测试功能是否都完成
        }
    }

    //为了方便以后添加一些其他的，比如说等级、科技树之类的，这里对是否能买进行封装
    public bool CanIBuy(int price)
    {
        return price <= gold;
    }


    //剩下的是一些基础的参数修改
    public void AddScore(int new_score)
    {
        score += new_score;
        DrawScore();
    }
    public void AddGold(int new_gold)
    {
        gold += new_gold;
        DrawGoldCount();
    }
    public void CutGlod(int price)
    {
        gold -= price;
        DrawGoldCount();
    }
    public void SetEnemy(int enemy)
    {
        this.enemy = enemy;
        DrawEnemyCount();
    }
    public void EnemyDec()
    {
        enemy--;
        DrawEnemyCount();
    }
    public void LifeDec()
    {
        life--;
        DrawLifeCount();
        if(life <= 0)
        {
            //Lose();
        }
    }
    public bool allClear()      //封装一下场上是否还有敌人的函数
    {
        return enemy == 0;    
    }
    private void DrawScore()
    {
        if(Score != null)
        {
            Score.text = string.Format("Score: {0:D}", score);
        }
    }
    private void DrawGoldCount()
    {
        if(GoldCount != null)
        {
            GoldCount.text = string.Format("Gold: {0:D}", gold);
        }
    }
    private void DrawEnemyCount()
    {
        if(EnemyCount != null)
        {
            EnemyCount.text = string.Format("Enemy: {0:D}", enemy);
        }
    }
    private void DrawLifeCount()
    {
        if(LifeCount != null)
        {
            LifeCount.text = string.Format("Life: {0:D}", life);
        }
    }

    public void Win()
    {
        Debug.Log("YOU WIN");
        Time.timeScale = 0;
    }
    public void Lose()
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0;
    }

    public void AddEnemy(GameObject enemy)
    {
        if (enemies != null)
        {
            enemies.Add(enemy);
        }
        else
        {
            Debug.Log("敌人列表为空");
        }
    }
}
