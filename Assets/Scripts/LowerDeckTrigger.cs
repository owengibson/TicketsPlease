using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerDeckTrigger : MonoBehaviour
{
    [SerializeField] private GameObject upperDeck;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            upperDeck.GetComponent<MeshRenderer>().enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            upperDeck.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
