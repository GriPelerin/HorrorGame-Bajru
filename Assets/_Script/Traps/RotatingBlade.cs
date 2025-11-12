using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingBlade : MonoBehaviour, ITrap
{
    [Range(1f, 5f)]
    [SerializeField] private float rotateSpeed;
    [SerializeField] private int damageAmount;
    [SerializeField] private float deactiveCooldown;

    private Tween _rotateTween;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerHealth>(out PlayerHealth player))
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
        _rotateTween.Kill();
        GameEvents.Instance.OnCardAbilityUsed -= Deactive;
    }
    private void Start()
    {
        Active();
    }

    public void Active()
    {
        if (_rotateTween == null)
        {
            // Ýlk defa oluþturuluyor
            _rotateTween = transform.DORotate(
                new Vector3(40, 0f, 0f),
                rotateSpeed,
                RotateMode.FastBeyond360
            ).SetLoops(-1, LoopType.Yoyo).SetRelative().SetEase(Ease.InOutExpo);
        }
        else
        {
            // Zaten varsa yeniden baþlatýyoruz
            _rotateTween.Play();
        }
    }
    public void Deactive(CardAbilityType ability, float duration)
    {
        if(ability == CardAbilityType.Gamble)
        {
            Debug.Log("Traps has been deactivated");
            StartCoroutine(DeactiveCooldown(ability, duration));
        }
    }

    private IEnumerator DeactiveCooldown(CardAbilityType ability, float duration)
    {
        _rotateTween.Pause();
        yield return new WaitForSeconds(duration);
        _rotateTween.Play();
    }

}
