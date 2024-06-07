using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyRagdoll());
    }

    IEnumerator DestroyRagdoll() // For dissppaering the Ragdoll after Die
    {
        yield return new WaitForSeconds(10f);
        DestroyImmediate(gameObject);
    }
}
