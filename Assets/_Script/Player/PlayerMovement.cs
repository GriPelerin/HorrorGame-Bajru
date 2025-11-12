using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float sprintMultiplier = 2;

    [SerializeField] private float footstepInterval = 0.4f; // Time between footsteps
    private float footstepTimer;

    [Header("Look Sensitivity")]
    [SerializeField] private float mouseSensitivity = 2.5f;
    [SerializeField] private float upDownRange = 80f;

    [Header("Input Customization")]
    [SerializeField] private string horizontalMoveInput = "Horizontal";
    [SerializeField] private string verticalMoveInput = "Vertical";

    [SerializeField] private string mouseXInput = "Mouse X";
    [SerializeField] private string mouseYInput = "Mouse Y";
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Space(20)]
    [Header("Player Hand")]
    [SerializeField] private GameObject playerHand;

    private float _verticalRotation;
    private float _speedMultiplier;
    private Vector3 _currentMovement = Vector3.zero;
    Vector3 _horizontalMovement;
    private CharacterController _characterController;
    private Camera _mainCamera;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        GameEvents.Instance.TriggerPlayerSpawned(this);
        _mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        GameEvents.Instance.OnCardAbilityUsed += ApplyPlayerAbilityEffect;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnCardAbilityUsed -= ApplyPlayerAbilityEffect;
    }
    void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameStates.GameActive) return;
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        _speedMultiplier = Input.GetKey(sprintKey) ? sprintMultiplier : 1;

        _horizontalMovement = new Vector3(Input.GetAxisRaw(horizontalMoveInput), 0, Input.GetAxisRaw(verticalMoveInput)).normalized;
        _horizontalMovement = transform.rotation * _horizontalMovement;

        _currentMovement.x = _horizontalMovement.x * walkSpeed * _speedMultiplier;
        _currentMovement.z = _horizontalMovement.z * walkSpeed * _speedMultiplier;

        _characterController.Move(_currentMovement * Time.deltaTime);

        FootstepSound(_horizontalMovement);

    }
    private void HandleRotation()
    {
        float mouseXRotation = Input.GetAxisRaw(mouseXInput) * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);
        _verticalRotation -= Input.GetAxisRaw(mouseYInput) * mouseSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -upDownRange, upDownRange);
        _mainCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    private void FootstepSound(Vector3 movement)
    {
        if (movement.magnitude > 0.1f)
        {
            float interval = Input.GetKey(sprintKey) ? footstepInterval / 1.5f : footstepInterval;
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                AudioManager.Instance.PlaySound(SoundList.FootstepSound);
                footstepTimer = interval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void ApplyPlayerAbilityEffect(CardAbilityType type, float duration)
    {
        switch (type)
        {
            case CardAbilityType.Blessing:
                StartCoroutine(ApplyBlessingCO(duration));
                break;
            case CardAbilityType.Curse:
                StartCoroutine(ApplyCurseCO(duration));
                break;
            default:
                break;
        }
    }

    IEnumerator ApplyBlessingCO(float duration)
    {
        float originalSpeed = walkSpeed;
        walkSpeed *= 2.5f;

        yield return new WaitForSeconds(duration);

        walkSpeed = originalSpeed;
    }
    IEnumerator ApplyCurseCO(float duration)
    {
        float originalHeight = _characterController.height;
        Vector3 originalCenter = _characterController.center;
        Vector3 originalCamPos = _mainCamera.transform.localPosition;

        _characterController.height = 1f;
        _characterController.center = new Vector3(originalCenter.x, 0.5f, originalCenter.z);
        _mainCamera.transform.localPosition = new Vector3(0, 0.5f, 0);

        yield return new WaitForSeconds(duration);

        _characterController.height = originalHeight;
        _characterController.center = originalCenter;
        _mainCamera.transform.localPosition = originalCamPos;
    }
}
