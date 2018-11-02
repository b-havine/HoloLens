using Assets.Facades;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Presentation
{
    class VoiceCommands : MonoBehaviour, ISpeechHandler
    {
        public Text guideText, wallText, wallSurface, wallAllTriangles, errors, debugText, itemText;
        public GameObject pickListPanel, microsoftLogo, spatialUnderstanding, spatialMapping;
        private bool startedScanning , areWeScanning, areWePicking, allowToScanItems = false;
        public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
        private GuideTitles guideTitles;
        private LogicFacade facade;
        private GameObject cube = null;
        private string copyBarcodeValue, confirmBarcode = "";


        public void OnSpeechKeywordRecognized(SpeechEventData eventData)
        {
            if (eventData.RecognizedText.Equals("yes") && guideText.text.Equals(guideTitles.yesToStart))
            {
                Debug.Log("hey?");
                microsoftLogo.SetActive(false);
                guideText.text = guideTitles.workOrCapacity;
                /* !! not from facade, ask teacher if we can make persistence facade for logic layer?*/
                SpatialMappingManager.Instance.DrawVisualMeshes = false;
                SpatialMappingManager.Instance.StopObserver();
            }
            if (eventData.RecognizedText.Equals("one") && guideText.text.Equals(guideTitles.workOrCapacity))
            {
                spatialUnderstanding.SetActive(true); //we need spatial understanding to regocnize walls, ceiling/floor etc...
                wallText.gameObject.SetActive(true);
                wallSurface.gameObject.SetActive(true);
                facade.WallOrSurfaceScan(wallText, wallSurface, wallAllTriangles);
            }
            if (eventData.RecognizedText.Equals("two") && guideText.text.Equals(guideTitles.workOrCapacity))
            {
                guideText.text = "You can now scan barcodes, go and scan your first location number!";
                startedScanning = true;
                pickListPanel.SetActive(true);
                facade.ScanRepeatingly(areWeScanning);
            }
            if (eventData.RecognizedText.Equals("scan") && areWePicking == false && startedScanning == true)
            {
                facade.AcceptBarcodeInPuttingMode(areWeScanning, allowToScanItems, cube, copyBarcodeValue, confirmBarcode, errors, guideText);
            }
            if (eventData.RecognizedText.Equals("scan") && areWePicking == true && startedScanning == true)
            {
                facade.AcceptBarcodeInPickingMode(areWeScanning, copyBarcodeValue, allowToScanItems, errors, debugText, itemText, microsoftLogo, guideText, pickListPanel);
            }
            if (eventData.RecognizedText.Equals("yes") && areWePicking == false && startedScanning == true && allowToScanItems == false)
            {
                facade.PlaceLocationOnWall(allowToScanItems, errors, areWeScanning, confirmBarcode);
            }
            if (eventData.RecognizedText.Equals("go") && guideText.text.Equals(guideTitles.startPicking) && areWePicking == true)
            {
                facade.StartPickingMode(guideText, errors, areWeScanning, pickListPanel);
            }
            if (eventData.RecognizedText.Equals("done") && areWePicking == false && allowToScanItems == true)
            {
                facade.ConfirmItemsToLocation( guideText,  errors,  allowToScanItems,  areWeScanning);
            }
            if (eventData.RecognizedText.Equals("done") && areWePicking == true && allowToScanItems == true)
            {
                facade.DoneWithLocationItems(errors, allowToScanItems, areWeScanning);
            }
            if (eventData.RecognizedText.Equals("back") && guideText.text.Equals(guideTitles.baydoorScanned))
            {
                areWeScanning = true;
                errors.text = "baydoor canceled, scan missing location numbers!";
            }
            if (eventData.RecognizedText.Equals("end") && guideText.text.Equals(guideTitles.baydoorScanned))
            {
                facade.PrepareForPickUp(copyBarcodeValue, areWePicking, allowToScanItems, areWeScanning, surfaces, spatialMapping);
            }
        }

        private void Start()
        {
            guideTitles = new GuideTitles();
            facade = new LogicFacade();
            guideText.text = guideTitles.yesToStart;
        }
    }
}
