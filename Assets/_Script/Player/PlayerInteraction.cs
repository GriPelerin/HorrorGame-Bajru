using Bajru.Inventory;
using TMPro;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    private PlayerUI _playerUI;

    [Header("Interaction Fields")]
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private float rayLength;

    private RaycastHit _hit;

    private void Awake()
    {
        _playerUI = GetComponent<PlayerUI>();
    }
    private void Update()
    {
        InteractionRay();
    }
    private void InteractionRay()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Camera.main.transform.forward * rayLength, Color.red);
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Camera.main.transform.forward, out _hit, rayLength, interactableMask))
        {
            if (_hit.collider.gameObject.CompareTag("Obstacle")) return;

            if (_hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                _playerUI.InteractText.text = interactable.InteractableName;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
        }
        else
        {
            _playerUI.InteractText.text = "";
            _playerUI.MapImage.gameObject.SetActive(false);
        }
    }
}
