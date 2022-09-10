using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private IGameScene gameScene;

    public float radius;

    public Sphere GetSphere()
    {
        return new Sphere(transform.position, radius);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameScene = GetComponentInParent<IGameScene>();
        
    }

    // Update is called once per frame
    void Update()
    {
        var vx = Input.GetAxis("Horizontal");
        var vz = Input.GetAxis("Vertical");

        var v = new Vector3(vx, 0, vz) * moveSpeed * Time.deltaTime;

        transform.Translate(v);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            gameScene.ShotBullet(transform.position);
        }
    }
}
