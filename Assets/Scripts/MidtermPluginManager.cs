using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MidtermPluginManager : MonoBehaviour
{
    //Import the functions I need
    [DllImport("EnginesMidtermDLLRandomPos")]
    private static extern void runOnLaunch();

    [DllImport("EnginesMidtermDLLRandomPos")]
    private static extern float RandFloat(float min, float max);

    static bool hasRun = false;

    public static Vector3 generateEnemyPosition(Vector2 xRange, Vector3 yRange, Vector3 zRange, Vector3 existingPos)
    {
        if (!hasRun)
        {
            //it will execute the try if the dll exists, and the catch if it does not, needed to allow the dll swapping
            try
            {
                //will generate the seed
                runOnLaunch();
            }
            catch
            {
                //do nothing cause the dll isn't there
            }

            //set has run to true
            hasRun = true;
        }

        Vector3 pos = Vector3.zero;

        //if the dll exists, make each value a random number from the dll
        try
        {
            pos.x = RandFloat(xRange.x, xRange.y);
            pos.y = RandFloat(yRange.x, yRange.y);
            pos.z = RandFloat(zRange.x, zRange.y);
        }
        //if it does not, just leave it on the existing pos
        catch
        {
            pos = existingPos;
        }

        return pos;
    }
}
