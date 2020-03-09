using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongHoldController : MonoBehaviour
{
    public bool hasUsed = false;

    public GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void OnMouseDown()
    {
        if (!hasUsed)
        {
            CreateTower();
        }
    }

    private void CreateTower()
    {
        Object tower_prefab = Resources.Load("Tower");
        if(tower_prefab == null)
        {
            Debug.Log("未能成功加载预设体");
            return;
        }
        GameObject tower = tower_prefab as GameObject;
        if(tower == null)
        {
            Debug.Log("未能成功转化为游戏对象");
            return;
        }

        //检测是否可以购买
        int price = tower.GetComponent<Tower>().price;
        if (gameController.CanIBuy(price))
        {
            tower = Instantiate<GameObject>(tower, this.transform.position, Quaternion.identity);
            tower.GetComponent<Tower>().SetGameController(gameController);
            gameController.CutGlod(price);
        }
    }
}
