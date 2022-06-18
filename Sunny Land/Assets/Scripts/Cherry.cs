using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    public void Eliminate()
    {
        FindObjectOfType<CharacterControl>().CherryCount();
        Destroy(gameObject);
    }
}
