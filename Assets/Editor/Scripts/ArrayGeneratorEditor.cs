using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArrayGenerator))]
public class ArrayGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ArrayGenerator generator = (ArrayGenerator)target;

        if (GUILayout.Button("Generate Array"))
        {
            GenerateArray(generator);
        }
    }

    void GenerateArray(ArrayGenerator generator)
    {
        if (generator.prefab == null || generator.count <= 0)
        {
            Debug.LogWarning("Assign a prefab and set count > 0.");
            return;
        }

        // 🧹 Clear old objects
        for (int i = generator.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(generator.transform.GetChild(i).gameObject);
        }

        generator.generatedObjects = new GameObject[generator.count];

        Vector3 finalSpacing = generator.spacing;

        // 📦 Auto spacing from bounds
        if (generator.autoSpacing)
        {
            Renderer renderer = generator.prefab.GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                Bounds bounds = renderer.bounds;
                finalSpacing = bounds.size; // size in world units
            }
            else
            {
                Debug.LogWarning("No Renderer found on prefab, falling back to manual spacing.");
            }
        }

        for (int i = 0; i < generator.count; i++)
        {
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(generator.prefab);

            obj.name = generator.prefab.name + "_" + i;
            obj.transform.SetParent(generator.transform);

            obj.transform.localPosition = finalSpacing * i;

            generator.generatedObjects[i] = obj;
        }

        Debug.Log("Generated " + generator.count + " objects.");
    }
}