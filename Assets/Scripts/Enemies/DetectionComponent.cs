using UnityEngine;

namespace TP.Common
{
    [RequireComponent(typeof(Collider))]
    public class DetectionComponent : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("entered" + other.gameObject.name);

            if (other.CompareTag("Player"))
            {
                Debug.LogError("Player detected:" + other.gameObject.name);
            }
        }
    }
}
