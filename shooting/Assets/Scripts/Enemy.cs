using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float radius;

    private Vector3 startPos;
    private float time;
    [SerializeField] private float scale;

    private MeshRenderer meshRenderer;

    private Transform player;
    public void SetPlayer( Transform player )
    {
        this.player = player;
    }

    public Sphere GetSphere()
    {
        return new Sphere(transform.position, radius);
    }

    public void SetColor( Color color )
    {
        meshRenderer.material.SetColor("_Color", color);
    }


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        startPos = transform.position;
        time += Random.Range(0.0f, 12.0f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        var pos = transform.position;
        pos.x += Mathf.Cos(time) * scale;
        pos.y += Mathf.Lerp(-1, 1, Mathf.PingPong(time, 1)) * scale;
        pos.z += -Mathf.Cos(time) * scale;
        transform.position = pos;

        var dir = player.position - pos;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
