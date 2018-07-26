using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenOrientation : MonoBehaviour {


    /// <summary>
    /// Obsolete Script
    /// </summary>

    public bool isVertical;
    public bool isHorizontal;
    public bool shouldRotate;

    private void Update()
    {
        if (isVertical)
        {

        }
        else if (isHorizontal)
        {

        }
        else if (shouldRotate)
        {

        }
    }
    /*
    public int GetScreenResolution(string lenghtsOf) {
        Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            print(res.width + "x" + res.height);
        }
        //Screen.SetResolution(resolutions[0].width, resolutions[0].height, true)

        if (lenghtsOf == "width")
        {
            return resolutions[];
        }


    }
    */

}
