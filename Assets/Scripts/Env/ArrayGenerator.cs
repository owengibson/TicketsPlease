using UnityEngine;
using Sirenix.OdinInspector;

public class ArrayGenerator : MonoBehaviour
{
    public GameObject prefab;
    public int count = 10;

    [ToggleLeft]
    public bool autoSpacing = true;

    [DisableIf(nameof(autoSpacing))]
    public Vector3 spacing = new Vector3(1f, 0f, 0f);

    [HideInInspector]
    public GameObject[] generatedObjects;
}