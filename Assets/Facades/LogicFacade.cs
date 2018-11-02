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
    public class LogicFacade : MonoBehaviour
    {
        private BarcodeScanner bs = new BarcodeScanner();
        private BarcodeRecognizer br = new BarcodeRecognizer();
        private LineGuide lg = new LineGuide();
        private SpawnLines sl = new SpawnLines();
        private ScanCapacity sc = new ScanCapacity();

        public void Bake(Text guideText, List<NavMeshSurface> surfaces) {
            StartCoroutine(lg.waitBake(guideText, surfaces));
        }
        public void ScanRepeatingly(bool areWeScanning) {
            bs.ScanRepeatingly(areWeScanning);
        }
        public void WallOrSurfaceScan(Text wall, Text surface, Text allTriangles) {
            sc.WallOrSurfaceScan(wall, surface, allTriangles);
        }
        public void AcceptBarcodeInPuttingMode(bool areWeScanning, bool allowToScanItems, GameObject cube, string copyBarcodeValue, string confirmBarcode, Text errors, Text guideText) {
            bs.AcceptBarcodeInPuttingMode(areWeScanning, allowToScanItems, cube, copyBarcodeValue, confirmBarcode, errors, guideText);
        }

        public void AcceptBarcodeInPickingMode(bool areWeScanning, string copyBarcodeValue, bool allowToScanItems, Text errors, Text debugText, Text itemText, GameObject microsoftLogo, Text guideText, GameObject pickListPanel)
        {
            bs.AcceptBarcodeInPickingMode(areWeScanning, copyBarcodeValue, allowToScanItems, errors, debugText, itemText, microsoftLogo, guideText, pickListPanel);
        }

        public void PlaceLocationOnWall(bool allowToScanItems, Text errors, bool areWeScanning, string confirmBarcode)
        {
            bs.PlaceLocationOnWall(allowToScanItems, errors, areWeScanning, confirmBarcode);
        }

        public void StartPickingMode(Text guideText, Text errors, bool areWeScanning, GameObject pickListPanel)
        {
            bs.StartPickingMode(guideText, errors, areWeScanning,pickListPanel);
        }

        public void DoneWithLocationItems(Text errors, bool allowToScanItems, bool areWeScanning)
        {
            bs.DoneWithLocationItems(errors, allowToScanItems, areWeScanning);
        }

        public void ConfirmItemsToLocation(Text guideText, Text errors, bool allowToScanItems, bool areWeScanning)
        {
            bs.ConfirmItemsToLocation( guideText,  errors,  allowToScanItems,  areWeScanning);
        }

        public void PrepareForPickUp(string copyBarcodeValue, bool areWePicking, bool allowToScanItems, bool areWeScanning, List<NavMeshSurface> surfaces, GameObject spatialMapping)
        {
            bs.PrepareForPickUp(copyBarcodeValue, areWePicking, allowToScanItems, areWeScanning, surfaces, spatialMapping);
        }
    }
}
