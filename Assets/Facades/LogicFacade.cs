using Assets.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Facades
{
    //this class has references to all the methods that are needed from the logic-layer to the presentation layer.
    public class LogicFacade : MonoBehaviour
    {
        private BarcodeScanner bs = new BarcodeScanner();
        private BarcodeRecognizer br = new BarcodeRecognizer();
        private LineGuide lg = new LineGuide();
        private SpawnLines sl = new SpawnLines();
        private ScanCapacity sc = new ScanCapacity();

        //makes the scanned area usable for Unity Navigation
        public void Bake(Text guideText, List<NavMeshSurface> surfaces) {
            StartCoroutine(lg.waitBake(guideText, surfaces));
        }
        //start the scan of barcodes, and when areWeSCanning boolean is true scan again
        public void ScanRepeatingly(bool areWeScanning, Text guideText) {
            bs.ScanRepeatingly(areWeScanning, guideText);
        }
        //count the amount of triangles covering the scanned area, based on the wall, floor and ceiling.
        public void WallOrSurfaceScan(Text wall, Text surface, Text allTriangles) {
            sc.WallOrSurfaceScan(wall, surface, allTriangles);
        }
        //when we deliver items to the warehouse, check if barcode should be an item or location number. 
        //If location number then save it as spatial anchor and a string, if item save it as string and insert to items array
        public void AcceptBarcodeInPuttingMode(bool areWeScanning, bool allowToScanItems, GameObject cube, string copyBarcodeValue, string confirmBarcode, Text errors, Text guideText) {
            bs.AcceptBarcodeInPuttingMode(areWeScanning, allowToScanItems, cube, copyBarcodeValue, confirmBarcode, errors, guideText);
        }
        //when we pick up items from the warehoouse, check if barcode should be an item or location number.
        //if location number then allow the scanning of the items belonging to the location, if item check if it's in the picklist, and
        //if it is then remove from picklist.
        public void AcceptBarcodeInPickingMode(bool areWeScanning, string copyBarcodeValue, bool allowToScanItems, Text errors, Text debugText, Text itemText, GameObject microsoftLogo, Text guideText, GameObject pickListPanel)
        {
            bs.AcceptBarcodeInPickingMode(areWeScanning, copyBarcodeValue, allowToScanItems, errors, debugText, itemText, microsoftLogo, guideText, pickListPanel);
        }
        //creates a 3d text of the location bar code value and places it on where the user is looking (preferably on a wall, shelf etc.)
        public void PlaceLocationOnWall(bool allowToScanItems, Text errors, bool areWeScanning, string confirmBarcode)
        {
            bs.PlaceLocationOnWall(allowToScanItems, errors, areWeScanning, confirmBarcode);
        }
        //switches to picking-mode, meaning that the user will now see a picklist that shows which items need to be picked up.
        public void StartPickingMode(Text guideText, Text errors, bool areWeScanning, GameObject pickListPanel)
        {
            bs.StartPickingMode(guideText, errors, areWeScanning,pickListPanel);
        }
        //checks if the user is done scanning the required items from the picklist at a location, if
        //he is then change the direction of the guiding lines.
        public void DoneWithLocationItems(Text errors, bool allowToScanItems, bool areWeScanning)
        {
            bs.DoneWithLocationItems(errors, allowToScanItems, areWeScanning);
        }
        //confirms that the items belonging to the location have been accepted, and allows scanning of a new location number
        public void ConfirmItemsToLocation(Text guideText, Text errors, bool allowToScanItems, bool areWeScanning)
        {
            bs.ConfirmItemsToLocation( guideText,  errors,  allowToScanItems,  areWeScanning);
        }
        //saves the scanned area, so that it can be used for features in the app, also switches from putting mode to picking mode
        public void PrepareForPickUp(string copyBarcodeValue, bool areWePicking, bool allowToScanItems, bool areWeScanning, List<NavMeshSurface> surfaces, GameObject spatialMapping)
        {
            bs.PrepareForPickUp(copyBarcodeValue, areWePicking, allowToScanItems, areWeScanning, surfaces, spatialMapping);
        }
    }
}
