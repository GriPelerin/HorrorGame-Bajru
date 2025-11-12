using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string InteractableName { get; set; }
    public void Interact() { }

}
