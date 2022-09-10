using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IGameScene
{
    void ShotBullet(Vector3 pos);
}

public struct Sphere
{
    public Vector3 pos;
    public float r;

    public Sphere( Vector3 pos, float r )
    {
        this.pos = pos;
        this.r = r;
    }

    public static bool IsHit( Sphere a, Vector3 p )
    {
        var v = p - a.pos;
        return v.sqrMagnitude <= a.r * a.r;
    }

    public static bool IsHit(Sphere a, Sphere b)
    {
        var v = a.pos - b.pos;
        var r = a.r + b.r;
        return v.sqrMagnitude <= r * r;
    }
}

public struct AABB
{
    public Vector3 pos;
    public Vector3 size;    //íÜêSì_Ç©ÇÁÇÃëÂÇ´Ç≥

    public AABB( Vector3 pos, Vector3 size )
    {
        this.pos = pos;
        this.size = size;
    }

    public static bool IsHit( AABB a, Vector3 p )
    {
        var v = p - a.pos;
        return v.x <= a.size.x && v.y <= a.size.y && v.z <= a.size.z;
    }

    public static bool IsHit(AABB a, AABB b)
    {
        var v = b.pos - a.pos;
        var size = a.size + b.size;
        return v.x <= size.x && v.y <= size.y && v.z <= size.z;
    }
}

public struct Segment
{
    public Vector3 s;
    public Vector3 v;
    public Vector3 e => s + v;
    public Vector3 GetPos(float t) => s + v * t;

    public Segment( Vector3 s, Vector3 v )
    {
        this.s = s;
        this.v = v;
    }

    public Vector3 GetNearPos(Vector3 pos)
    {
        var d = pos - s;
        var n = v.normalized;
        var t = Vector3.Dot(d, n) / v.magnitude;
        if (t < 0.0f) return s;
        else if (t > 1.0f) return e;
        return GetPos(t);
    }

    public static bool IsHit(Segment seg, Sphere s)
    {
        var pos = seg.GetNearPos(s.pos);
        return Sphere.IsHit(s, pos);
    }

    public static bool IsHit( Segment seg, AABB box )
    {
        var pos = seg.GetNearPos(box.pos);
        return AABB.IsHit(box, pos);
    }
}

public class GameScene : MonoBehaviour, IGameScene
{
    public void ShotBullet( Vector3 pos )
    {
        var go = GameObject.Instantiate(bulletPrefab, pos, Quaternion.identity);
        var bullet = go.GetComponent<Bullet>();
        bulletList.Add(bullet);
    }

    public Player player;
    public List<Enemy> enemyList;
    public List<Bullet> bulletList;

    public GameObject bulletPrefab;
    public GameObject enemyPrefab;
    public Transform enemyPopPos;
    public float popRange;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<Player>();
        enemyList = GetComponentsInChildren<Enemy>().ToList();
        foreach( var enemy in enemyList )
        {
            enemy.SetPlayer(player.transform);
        }
        bulletList = new List<Bullet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        if( Input.GetKeyDown(KeyCode.X))
        {
            var pos = enemyPopPos.position;
            pos.x += Random.Range(-popRange, popRange);
            var go = GameObject.Instantiate(enemyPrefab, pos, Quaternion.identity);
            var e = go.GetComponent<Enemy>();
            enemyList.Add(e);
            e.SetPlayer(player.transform);
            e.transform.SetParent(transform);
        }

        //ìñÇΩÇËîªíËÇçsÇ§
        for (int i = 0; i < enemyList.Count; ++i)
        {
            if (enemyList[i] == null) continue;

            if (Sphere.IsHit(player.GetSphere(), enemyList[i].GetSphere()))
            {
                GameObject.Destroy(player.gameObject );
                break;
            }
        }

        var ray = new Segment(player.transform.position, player.transform.forward * 10);
        foreach( var enemy in enemyList )
        {
            if (enemy == null) continue;
            if (Segment.IsHit(ray, enemy.GetSphere()))
            {
                enemy.SetColor(Color.red);
            }
            else
            {
                enemy.SetColor(Color.white);
            }

        }

        bool enemyDeleteFlag = false;
        foreach ( var bullet in bulletList )
        {
            if (bullet == null) continue;
            foreach( var enemy in enemyList )
            {
                if (enemy == null) continue;

                if (Sphere.IsHit(bullet.GetSphere(), enemy.GetSphere()))
                {
                    GameObject.Destroy(enemy.gameObject);
                    enemyDeleteFlag = true;
                }
            }
        }
        if( enemyDeleteFlag )
        {
            enemyList = GetComponentsInChildren<Enemy>().ToList();
        }


    }
}
