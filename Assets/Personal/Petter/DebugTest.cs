using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScreenEffect.StartTransitionIn(2);

        Invoke(nameof(Test), 5);
    }

    private void Test()
    { 
        ScreenEffect.StartTransitionOut(2);
    } 

}
