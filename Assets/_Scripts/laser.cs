using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    private float _damage;

    public float Damage { get => _damage; set => _damage = value; }

    void Awake()
    {
        Destroy(this.gameObject, 0.5f);
    }
    // Start is called before the first frame update
    void Start()
    {
        Damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().Health -= Damage;
            Destroy(this.gameObject);
        }
    }
}
