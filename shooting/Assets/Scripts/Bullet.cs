using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 10;
    public float deleteTime;
    public float count;
    public float radius;

    public Sphere GetSphere()
    {
        return new Sphere(transform.position, radius);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
        if( count >= deleteTime )
        {
            GameObject.Destroy(gameObject);
        }
    }
}
