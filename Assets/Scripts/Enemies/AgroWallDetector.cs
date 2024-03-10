using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AgroWallDetector : MonoBehaviour
{
    private bool inArea;
    // Start is called before the first frame update

    private void Start()
    {
        inArea = false;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = false;
        }
    }

    public bool GetInArea()
    {
        return inArea;
    }
}
