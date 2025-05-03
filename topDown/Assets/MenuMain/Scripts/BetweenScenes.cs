using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BetweenScenes : MonoBehaviour
{
    private void Awake()
    {
        var notDestroyBetweenScenes = FindObjectsOfType<BetweenScenes>();
        if (notDestroyBetweenScenes.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
