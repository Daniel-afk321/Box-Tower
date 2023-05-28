using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveRuble : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        Destroy(col.gameObject);
    }
}
