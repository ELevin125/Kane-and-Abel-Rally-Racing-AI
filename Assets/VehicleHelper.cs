using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHelper : MonoBehaviour
{
    public static Transform FindCarRoot(Transform currentTransform)
    {
        if (currentTransform.CompareTag("car"))
        {
            return currentTransform; // Found the "car" ancestor
        }

        if (currentTransform.parent != null)
        {
            return FindCarRoot(currentTransform.parent);
        }

        return null;
    }
}
