using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableWood : MonoBehaviour
{
    [SerializeField] private GameObject collectableWood;
    private bool isCallFunction = true;
    void Update()
    {
        // The woods respawn 5 seconds after they are destroyed.
        if (!collectableWood.gameObject.activeInHierarchy)
        {
            if (isCallFunction)
            {
                StartCoroutine("ActivateWoodObject");
                isCallFunction = false;
            }
        }
    }
    IEnumerator ActivateWoodObject()
    {
        yield return new WaitForSeconds(5f);
        collectableWood.gameObject.SetActive(true);
        isCallFunction = true;
    }
}
