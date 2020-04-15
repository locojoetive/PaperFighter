using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public int hp = 1;
    public bool isEnemy = false;

    public void Damage(int damageCount)
    {
        hp -= damageCount;
        if (hp <= 0)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D otherColider)
    {
        ShurikenScript shot = otherColider.gameObject.GetComponent<ShurikenScript>();
        if(shot != null)
        {
            if(shot.isEnemyShot != isEnemy)
            {
                Damage(shot.damage);

                //Shuriken dispose
                Destroy(shot.gameObject);
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
