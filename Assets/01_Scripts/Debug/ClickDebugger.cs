using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ClickDebugger : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                Debug.Log("감지: " + result.gameObject.name);
            }
        }
    }
}