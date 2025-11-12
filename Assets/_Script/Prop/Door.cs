using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.Instance.OnCorrectPassword += OpenDoor;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnCorrectPassword -= OpenDoor;
    }

    private void OpenDoor()
    {
        AudioManager.Instance.PlaySound(SoundList.DoorSound);
        Destroy(this.gameObject);
    }
}
