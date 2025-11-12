using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    [SerializeField]private Light flashlight;
    private bool _isFlashlightActive;

    private void Start()
    {
        _isFlashlightActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Flashlight();
        }
    }

    private void Flashlight()
    {
        if(GameManager.Instance.CurrentGameState != GameStates.GameActive) return;

        _isFlashlightActive = !_isFlashlightActive;
        AudioManager.Instance.PlaySound(SoundList.FlashlightSound);
        if(_isFlashlightActive)
        {
            flashlight.gameObject.SetActive(true);
        }
        else
        {
            flashlight.gameObject.SetActive(false);
        }
    }
}
