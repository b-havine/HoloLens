using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Logic
{
    //This class has to do with gettings some stats on how large an area can be scanned,
    //this is accomplished by counting the total amount of traingles that are being shown when scannning.
    public class ScanCapacity : MonoBehaviour
    {


        // Use this for initialization


        // Update is called once per frame

        
        private void LogSurfaceState(Text wall, Text surface, Text allTriangles)
        {
            IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
            if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) != 0)
            {
                var stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
                //Debug.Log(stats.TotalSurfaceArea);
                wall.GetComponent<Text>().text = "Wall count: " + stats.WallSurfaceArea.ToString();
                surface.GetComponent<Text>().text = "Surface: " + stats.HorizSurfaceArea.ToString();
                allTriangles.GetComponent<Text>().text = "Total: " + stats.TotalSurfaceArea.ToString();

            }
        }

        public IEnumerator WallOrSurfaceScan(Text wall, Text surface, Text allTriangles)
        {
            while (true)
            {
                switch (SpatialUnderstanding.ScanStates.Scanning)
                {
                    case SpatialUnderstanding.ScanStates.Scanning:
                        this.LogSurfaceState(wall, surface, allTriangles); //indirect recursion
                        break;
                }
                yield return new WaitForSeconds(0.1f);
            }

        }
    }
}