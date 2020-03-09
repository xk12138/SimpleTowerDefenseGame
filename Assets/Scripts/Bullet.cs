using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public Vector3 target_position;

    public int power = 30;
    public int speed = 10;

    private void Update()
    {
        if (target != null)
        {
            target_position = target.transform.position;
            Vector3 my_position = transform.position;
            float x = target_position.x - my_position.x;
            float y = target_position.y - my_position.y;
            float z = target_position.z - my_position.z;
            if (x * x + y * y + z * z < 0.05)
            {
                //Boom!
                Boom();
            }
            else
            {
                //Move
                float sum = Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z);
                this.transform.Translate(new Vector3(x / sum * speed * Time.deltaTime, y / sum * speed * Time.deltaTime, z / sum * speed * Time.deltaTime));
            }
        }
        else if (target_position != null)       //如果子弹打中敌人前敌人已经死亡，就emmm鞭尸?
        {
            Vector3 my_position = transform.position;
            float x = target_position.x - my_position.x;
            float y = target_position.y - my_position.y;
            float z = target_position.z - my_position.z;
            if (x * x + y * y + z * z < 0.05)
            {
                //Destroy
                Destroy(this.gameObject);
            }
            else
            {
                //Move
                float sum = Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z);
                this.transform.Translate(new Vector3(x / sum * speed * Time.deltaTime, y / sum * speed * Time.deltaTime, z / sum * speed * Time.deltaTime));
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Boom()
    {
        if (target != null)
        {
            target.GetComponent<Enemy>().Attacked(power);
        }
        Destroy(this.gameObject);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
