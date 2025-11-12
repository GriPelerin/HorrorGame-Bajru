using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 newPosition = GameManager.Instance.Player.transform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
