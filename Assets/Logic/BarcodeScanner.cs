using Assets.Logic;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Logic
{
    //this class represents the functions that to do with the system from putting goods,
    //to picking goods again. Also the scanning of a barcode happens here
    public class BarcodeScanner : MonoBehaviour
    {
        public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
        public Text guideText, errors, debugText, itemText;
        public GameObject pickListPanel;
        public GameObject spatialMapping;
        public Material combinedMeshMaterial;
        public Camera cam;

        public GameObject microsoftLogo, correctImage, wrongImage;
        private bool areWePicking = false;
        public static bool firstTimeYes, areWeScanning, allowToScanItems = false;
        private PickListHelper pickListHelper = new PickListHelper();
        private GameObject cube = null;
        private string copyBarcodeValue, confirmBarcode = "";
        public static List<LocationNumber> locationNumbers = new List<LocationNumber>();
        private LineGuide lineGuide = new LineGuide();
        private SpawnLines spawnLines = new SpawnLines();

        public void PrepareForPickUp(string copyBarcodeValue, bool areWePicking, bool allowToScanItems, bool areWeScanning, List<NavMeshSurface> surfaces, GameObject spatialMapping)
        {
            Create3DTextOnWall(copyBarcodeValue);
            areWePicking = true;
            allowToScanItems = false;
            areWeScanning = false;
            //combine child meshes in spatialmapping
            MeshFilter[] meshFilters = GameObject.Find("SpatialMapping").GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 1;
            while (i < meshFilters.Length)
            {

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }
            spatialMapping.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            spatialMapping.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            spatialMapping.transform.gameObject.SetActive(true);
            spatialMapping.GetComponent<Renderer>().material = combinedMeshMaterial;

            //spatialMapping.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(lineGuide.waitBake(guideText, surfaces));
        }

        public void ConfirmItemsToLocation(Text guideText, Text errors, bool allowToScanItems, bool areWeScanning)
        {
            guideText.text = "find a new location-number to scan";
            errors.text = "you have succesfully added items to a location";
            allowToScanItems = false;
            areWeScanning = true;
        }

        public void DoneWithLocationItems(Text errors, bool allowToScanItems, bool areWeScanning)
        {
            bool anyItemsLeft = true;
            for (int i = 0; i < PickListHelper.items.Count; i++)
            {

                if (PickListHelper.items[i].BelongsToLocation.Equals(confirmBarcode))
                {
                    anyItemsLeft = true;
                    errors.text = "You are not done with Scanning items! - Go further";
                    break; //break because you are not done with scanning items.
                }
                else
                {
                    anyItemsLeft = false;
                }

            }
            if (PickListHelper.items.Count == 0)
            {
                anyItemsLeft = false;
            }
            if (anyItemsLeft == false)
            {
                // Change direction of the Robot with the counter. 
                SpawnLines.counter++;
                allowToScanItems = false;
                errors.text = "items have been picked up, now go to: " + locationNumbers[SpawnLines.counter].Description;
            }
            areWeScanning = true;
        }

        public void StartPickingMode(Text guideText, Text errors, bool areWeScanning, GameObject pickListPanel)
        {
            //Print the name of the LocationNumber
            guideText.text = "follow the line and scan the location number: " + locationNumbers[0].Description;
            errors.text = "we are in picking mode.";
            spawnLines.PrepareLines(locationNumbers);
            areWeScanning = true;
            pickListPanel.GetComponent<Image>().enabled = true;
            foreach (Transform child in pickListPanel.transform)
            {
                child.gameObject.GetComponent<Text>().enabled = false;
            }
        }

        public void PlaceLocationOnWall(bool allowToScanItems, Text errors, bool areWeScanning, string confirmBarcode)
        {
            string isConfirmedLocationOrNot = BarcodeRecognizer.getItem(confirmBarcode);
            if (isConfirmedLocationOrNot.Equals(""))
            {
                Create3DTextOnWall(confirmBarcode);
                allowToScanItems = true;
            }
            else
            {
                StartCoroutine(ShowCorrectLogo(false));
                errors.text = "you did not scan a location number";
                areWeScanning = true;
            }
        }

        public void ScanRepeatingly(bool areWeScanning, Text guideText) {
#if !UNITY_EDITOR
    MediaFrameQrProcessing.Wrappers.ZXingQrCodeScanner.ScanFirstCameraForQrCode(
        result =>
        {
          UnityEngine.WSA.Application.InvokeOnAppThread(() =>
          {
              if (areWeScanning == true) {
                  copyBarcodeValue = result;
                  guideText.text = "got result: " + copyBarcodeValue + ", say scan to confirm";
              }
            
          }, 
          false);
        },
        null);
#endif
        }

        public void AcceptBarcodeInPuttingMode(bool areWeScanning, bool allowToScanItems, GameObject cube, string copyBarcodeValue, string confirmBarcode, Text errors, Text guideText) {
            areWeScanning = false;
            if (allowToScanItems == false)
            {
                if (copyBarcodeValue.Equals("A17"))
                {
                    copyBarcodeValue = "Baydoor";
                    errors.text = "";
                    guideText.text = "baydoor scanned, say 'end' if you wish to proceed to picking, say 'back' to cancel";
                    return;
                }
                confirmBarcode = copyBarcodeValue;
                guideText.text = "You have scanned: " + copyBarcodeValue + ", say 'yes' to confirm the location";
                StartCoroutine(ShowCorrectLogo(true));
            }
            if (allowToScanItems == true)
            {
                if (BarcodeRecognizer.getItem(copyBarcodeValue).Equals(""))
                {
                    StartCoroutine(ShowCorrectLogo(false));
                    errors.text = "you did not scan an item, try again, if you're done with items say: 'done'";
                    areWeScanning = true;
                    return;
                }
                StartCoroutine(ShowCorrectLogo(true));
                copyBarcodeValue = BarcodeRecognizer.getItem(copyBarcodeValue);
                guideText.text = copyBarcodeValue + " added to: " + confirmBarcode;
                errors.text = "Item scanned: " + copyBarcodeValue + ", say 'scan' to add more items to location, say 'done' to confirm the items to location";
                pickListHelper.InsertPickUpItemToPanel(copyBarcodeValue, confirmBarcode);
                areWeScanning = true;
            }
            string isConfirmedLocationOrNot = BarcodeRecognizer.getItem(confirmBarcode);
            if (cube == null && isConfirmedLocationOrNot.Equals("") && !copyBarcodeValue.Equals(""))
            {
                //if cube is null then we instantiate a cube that is used as postition for text meshes, also follow camera.
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<TapToPlace>();
                cube.GetComponent<TapToPlace>().IsBeingPlaced = true;
                cube.GetComponent<TapToPlace>().DefaultGazeDistance = 10;
                cube.GetComponent<TapToPlace>().AllowMeshVisualizationControl = false;
                cube.GetComponent<Transform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
                cube.transform.position = cam.transform.position + cam.transform.forward * 2;
                cube.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (cube != null && isConfirmedLocationOrNot.Equals("") && !copyBarcodeValue.Equals(""))
            {//if cube exists, then just make it follow the camera.
                cube.GetComponent<TapToPlace>().IsBeingPlaced = true;
            }
        }
        public void AcceptBarcodeInPickingMode(bool areWeScanning, string copyBarcodeValue, bool allowToScanItems, Text errors, Text debugText, Text itemText, GameObject microsoftLogo, Text guideText, GameObject pickListPanel) {
            areWeScanning = false;
            if (copyBarcodeValue.Equals("A17") && SpawnLines.startPosition.GetComponent<TextMesh>().text.Equals("Baydoor") && allowToScanItems == false)
            {
                copyBarcodeValue = "Baydoor";
                errors.text = "";
                debugText.text = "";
                itemText.text = "";
                microsoftLogo.SetActive(true);
                pickListPanel.SetActive(false);
                guideText.text = "there are no tasks left and you have scanned baydoor, job done! go home!";
                GameObject.Find("myControls").GetComponent<SpawnLines>().enabled = false;
                return;
            }
            if (BarcodeRecognizer.getItem(copyBarcodeValue).Equals("") && locationNumbers[SpawnLines.counter].Description.Equals(copyBarcodeValue) && allowToScanItems == false)
            {
                StartCoroutine(ShowCorrectLogo(true));
                errors.text = "You have scanned the correct location number, now scan the items";
                confirmBarcode = copyBarcodeValue;
                allowToScanItems = true;
                areWeScanning = true;
                return;
            }
            else
            {
                if (copyBarcodeValue.Equals("A17"))
                {
                    StartCoroutine(ShowCorrectLogo(false));
                    errors.text = "You have scanned baydoor but your job is not done! Scan the correct location number.";
                }
                else
                {
                    StartCoroutine(ShowCorrectLogo(false));
                    errors.text = "Wrong location number scanned. Try again.";
                }


            }
            if (allowToScanItems == true)
            {
                if (BarcodeRecognizer.getItem(copyBarcodeValue).Equals(""))
                {
                    StartCoroutine(ShowCorrectLogo(false));
                    errors.text = "you did not scan an item, try again, if you're done picking up items here say 'done'";
                    areWeScanning = true;
                    return;
                }
                copyBarcodeValue = BarcodeRecognizer.getItem(copyBarcodeValue);
                //errors.text = "something went wrong? item scanned was: " + copyBarcodeValue + ", confirmed barcode was: " + confirmBarcode;
                for (int i = 0; i < PickListHelper.items.Count; i++)
                {

                    // first checks, if the scanned barcode exist in the list, and if is belongs to the current locationnumber
                    if (PickListHelper.items[i].Name.Equals(copyBarcodeValue) && PickListHelper.items[i].BelongsToLocation.Equals(confirmBarcode))
                    {
                        errors.text = "You have picked up an item : " + copyBarcodeValue + ", say 'scan' to pickup more items from location, say 'done' for next location";
                        PickListHelper.items.Remove(PickListHelper.items[i]);
                        StartCoroutine(pickListHelper.UpdateItemsPanel()); //update the panel, so that it shows that we have picked up an item, now pickup shows the remanining items
                                                                        //StartCoroutine(ShowCorrectLogo(true));
                    }
                }
            }
            areWeScanning = true;
        }
        public void Create3DTextOnWall(string barcodeResult)
        {
            if (!copyBarcodeValue.Equals(""))
            {
                cube.GetComponent<TapToPlace>().IsBeingPlaced = false;
                GameObject textMesh = new GameObject("TextOnWall");
                textMesh.AddComponent<TextMesh>();
                textMesh.AddComponent<TapToPlace>();
                textMesh.GetComponent<TextMesh>().text = barcodeResult;
                textMesh.transform.position = GameObject.Find("DefaultCursor").transform.position;
                textMesh.transform.rotation = cam.transform.rotation;
                textMesh.GetComponent<TextMesh>().anchor = UnityEngine.TextAnchor.MiddleCenter;
                textMesh.GetComponent<TextMesh>().fontSize = 1024;
                textMesh.GetComponent<TextMesh>().characterSize = 0.001f;
                textMesh.AddComponent<BoxCollider>();

                locationNumbers.Add(new LocationNumber(textMesh, barcodeResult));
                string combine = "";
                for (int i = 0; i < locationNumbers.Count; i++)
                {
                    combine += "Locations scanned: " + locationNumbers[i].Description + ", ";
                }
                debugText.text = combine;
                if (!copyBarcodeValue.Equals("Baydoor"))
                {
                    guideText.text = "placement complete, now scan the items";
                    allowToScanItems = true;
                }
                areWeScanning = true; //allow barcode-scanning again.
            }
            else
            {
                guideText.text = "something was wrong with the barcode(value is empty) Try again to scan";
                areWeScanning = true;
            }
        }

        private IEnumerator ShowCorrectLogo(bool whichCheckmark)
        {
            if (whichCheckmark == true)
            {
                correctImage.SetActive(true);
                yield return new WaitForSeconds(2f);
                correctImage.SetActive(false);
            }
            if (whichCheckmark == false)
            {
                wrongImage.SetActive(true);
                yield return new WaitForSeconds(2f);
                wrongImage.SetActive(false);
            }

        }
    }
}
