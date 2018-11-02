
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Logic
{
    public class SpawnLines: MonoBehaviour
    {
        //We need to drag the robot in, because the script is depending on a prefab
        public GameObject robot;
        public static Transform startPosition;
        public static int counter = 0;
  
        private IEnumerator InstantiateRobotCooldown(List<LocationNumber> locations)
        {
            while (true)
            {
                Instantiate(robot);
                startPosition = locations[counter].LocNumOnWall.transform;
                robot.transform.position = startPosition.position;
                yield return new WaitForSeconds(1.5F);
            }
        }

        public void PrepareLines(List<LocationNumber> locations)
        {
            locations = BarcodeScanner.locationNumbers;
            StartCoroutine(InstantiateRobotCooldown(locations));
            GameObject.Find("Errors").GetComponent<Text>().text = "";//"we should be spawning robots now. First is: " + locations[counter].LocNumOnWall.name;
        }
    }

}