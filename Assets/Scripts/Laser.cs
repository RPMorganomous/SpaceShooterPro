using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    private bool _isTorpedo = false;
    private GameObject closestEnemy;
    private Rigidbody2D rb;

    [SerializeField]
    private Sprite _laser, _torpedo;
    private SpriteRenderer spriteRenderer;

    private GameObject currentEnemy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if ((_isTorpedo == true) & (_isEnemyLaser == false))
        {
            //spriteRenderer.sprite = _torpedo;
            FindClosestEnemy();
            if (closestEnemy != null)
            {
                TargetClosestEnemy();
            }
            else
            {
                MoveUp();
            }
        }
        else
        {
            //spriteRenderer.sprite = _laser;
        }
    }

    void Update()
    {

        if ((_isTorpedo == true) & (_isEnemyLaser == false))
        {
            FindClosestEnemy();
            if (closestEnemy != null)
            {
                TargetClosestEnemy();
            }
            else
            {
                MoveUp();
            }
        }
        else if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

        CheckBounds();

    }

    void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        closestEnemy = null;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
            currentEnemy.GetComponentInChildren<Reticule>().unTargeted();
            closestEnemy.GetComponentInChildren<Reticule>().unTargeted();
        }
        
    }

    void TargetClosestEnemy()
    {
        closestEnemy.GetComponentInChildren<Reticule>().Targeted();

        Vector2 direction = (Vector2)closestEnemy.transform.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * (_speed * 60);
        rb.velocity = transform.up * _speed * 2;
        //Debug.Log("closestEnemy = " + closestEnemy);
    }

    void MoveUp()
    {
        transform.Translate(
    Vector3.up * _speed * Time.deltaTime);

    }

    void MoveDown()
    {
        transform.Translate(
    Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void ArmTorpedo()
    {
        _isTorpedo = true;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _torpedo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }

    public void CheckBounds()
    {
        if (transform.position.y < -20f)
        {
            DestroyLaser();
        }

        if (transform.position.y > 20f)
        {
            DestroyLaser();
        }

        if (transform.position.x > 20f)
        {
            DestroyLaser();
        }

        if (transform.position.x < -20f)
        {
            DestroyLaser();
        }

    }

    public void DestroyLaser()
    {
        if (closestEnemy != null)
        {
            closestEnemy.GetComponentInChildren<Reticule>().unTargeted();
        }

        if (currentEnemy != null)
        {
            currentEnemy.GetComponentInChildren<Reticule>().unTargeted();
        }

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }

        Destroy(this.gameObject);
    }

}
