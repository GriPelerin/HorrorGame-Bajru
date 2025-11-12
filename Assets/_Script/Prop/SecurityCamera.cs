using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform SCHead;

    [Header("Settings")]
    [SerializeField] private float rotateSpeed;


    private void Update()
    {
        RotateCamera();
    }
    private void RotateCamera()
    {
        Vector3 targetDir = GameManager.Instance.Player.transform.position - SCHead.position;
        targetDir.y = 0f;

        if (targetDir == Vector3.zero)
            return;

        // Hedef rotasyonu hesapla
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        // Smooth dönüþ
        SCHead.rotation = Quaternion.Slerp(SCHead.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

}
