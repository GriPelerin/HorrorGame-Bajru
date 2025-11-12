using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyTrap : MonoBehaviour, ITrap
{
    [SerializeField] private float spikeSpeed;
    [SerializeField] private float spikeRange;
    [SerializeField] private int damageAmount;

    private Tween _upDownTween;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.TakeDamage(damageAmount);
        }
    }
    private void OnEnable()
    {
        GameEvents.Instance.OnCardAbilityUsed += Deactive;
    }
    private void OnDisable()
    {
        _upDownTween.Kill();
        GameEvents.Instance.OnCardAbilityUsed -= Deactive;
    }
    private void Start()
    {
        Active();
    }
    public void Active()
    {
        _upDownTween = transform.DOMove(new Vector3(0, spikeRange, 0), spikeSpeed).SetLoops(-1, LoopType.Yoyo).SetRelative().SetEase(Ease.InOutElastic);
    }

    public void Deactive(CardAbilityType ability, float duration)
    {
        StartCoroutine(DeactiveCooldown(ability, duration));
    }

    private IEnumerator DeactiveCooldown(CardAbilityType ability, float duration)
    {
        _upDownTween.Pause();
        yield return new WaitForSeconds(duration);
        _upDownTween.Play();
    }
}
