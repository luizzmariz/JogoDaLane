using UnityEngine;
using UnityEngine.UI;

public class CreativeButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
