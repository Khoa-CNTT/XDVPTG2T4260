using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Managers;
using tuleeeeee.Utilities;
using UnityEngine;

public class ScreenCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        transform.position = HelperUtilities.GetMousePosition();
    }
}
