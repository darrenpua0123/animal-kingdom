using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHandMovementHandlerScript : MonoBehaviour, IDropHandler
{ 

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped");
        // TODO: Create a UI Dropzone in middle instead, detect the drop at there.
        // https://www.youtube.com/watch?v=QZUrHmqeZ_8&list=PL4j7SP4-hFDJvQhZJn9nJb_tVzKj7dR7M&index=17 21:00
        // https://www.youtube.com/watch?v=kWRyZ3hb1Vc
    }
}
