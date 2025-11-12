using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ForceInvariantCulture : MonoBehaviour
{
    static ForceInvariantCulture()
    {
        // Set the culture for the editor thread to the invariant culture.
        // The invariant culture is culture-agnostic and uses the dot as a decimal separator.
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }
}
