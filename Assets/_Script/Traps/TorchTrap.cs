using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchTrap : MonoBehaviour, ITrap
{
    [Header("Trap Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    private BoxCollider _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        GameEvents.Instance.OnCardAbilityUsed += Deactive;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnCardAbilityUsed -= Deactive;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Active();
        }
    }

    public void Active()
    {
        AudioManager.Instance.PlaySound(SoundList.ProjectileTrapSound);
        Instantiate(projectilePrefab, firePoint.position, firePoint.transform.localRotation);
    }

    public void Deactive(CardAbilityType ability, float duration)
    {
        if (ability == CardAbilityType.Gamble)
        {
            StartCoroutine(DeactivateTemporarily(duration));
        }
    }

    private IEnumerator DeactivateTemporarily(float duration)
    {
        _boxCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        _boxCollider.enabled = true;
    }
}
