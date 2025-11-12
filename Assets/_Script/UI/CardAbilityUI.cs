using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
public class CardAbilityUI : MonoBehaviour
{
    [SerializeField]private Image _abilityIcon;

    [Header("Ability Sprites")]
    [SerializeField] private Sprite curseSprite;
    [SerializeField] private Sprite stallSprite;
    [SerializeField] private Sprite blessingSprite;
    [SerializeField] private Sprite revealSprite;
    [SerializeField] private Sprite gambleSprite;

    [Header("Full Screen Effect")]
    [SerializeField] private ScriptableRendererFeature fullScreenAbility;
    [SerializeField] private Material material;

    private int _effectColor = Shader.PropertyToID("_VignetteColor");
    private void Start()
    {
        fullScreenAbility.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.Instance.OnCardAbilityUsed += ShowAbilityIcon;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnCardAbilityUsed -= ShowAbilityIcon;
    }
    private void ShowAbilityIcon(CardAbilityType type, float dur)
    {
        Sprite selectedSprite = null;

        switch (type)
        {
            case CardAbilityType.Curse:
                selectedSprite = curseSprite;
                StartCoroutine(FullScreenEffect(dur, Color.red));
                break;
            case CardAbilityType.Stall:
                StartCoroutine(FullScreenEffect(dur, new Color(1, 0.5f, 0)));
                selectedSprite = stallSprite;
                break;
            case CardAbilityType.Blessing:
                StartCoroutine(FullScreenEffect(dur, Color.green));
                selectedSprite = blessingSprite;
                break;
            case CardAbilityType.Reveal:
                StartCoroutine(FullScreenEffect(dur, Color.grey));
                selectedSprite = revealSprite;
                break;
            case CardAbilityType.Gamble:
                StartCoroutine(FullScreenEffect(dur, Color.blue));
                selectedSprite = gambleSprite;
                break;
        }

        if (selectedSprite == null) return;


        _abilityIcon.sprite = selectedSprite;
        StartCoroutine(HideAfterDelay(dur));
    
           
    }
    private IEnumerator FullScreenEffect(float duration, Color color)
    {
        fullScreenAbility.SetActive(true);
        material.SetColor(_effectColor, color);
        yield return new WaitForSeconds(duration);
        fullScreenAbility.SetActive(false);
    }
    private IEnumerator HideAfterDelay(float duration)
    {
        _abilityIcon.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        _abilityIcon.gameObject.SetActive(false);
    }
}
