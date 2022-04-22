using UnityEngine;
using System.Collections;

public class Capture : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
                //Debug.Log("destroy");
            }
        }
    }
}
