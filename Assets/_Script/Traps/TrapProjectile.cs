using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int damageAmount;
    [SerializeField] private int lifeTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void FixedUpdate()
    {
        MoveProjectile();
    }

    public void MoveProjectile()
    {
        transform.position += new Vector3(0,0, -1 * projectileSpeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.TakeDamage(damageAmount);
            Destroy(gameObject);
        }
    }
}
