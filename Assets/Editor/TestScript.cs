using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Logic;

namespace HoloLens
{


    public class TestScript {
        private Transform dummyTransform;
        private GameObject dummyGameobject;
        private BarcodeScanner fbs;
        private PickListHelper pickListHelper;
        private List<Item> items;
        private List<LocationNumber> locations;

        public void InsertData() {
            locations.Add(new LocationNumber(dummyGameobject, "B21"));
            locations.Add(new LocationNumber(dummyGameobject, "B18"));
            locations.Add(new LocationNumber(dummyGameobject, "B19"));
            locations.Add(new LocationNumber(dummyGameobject, "Baydoor"));
            items.Add(new Item(1, "Nike Shoes", 2, dummyTransform, "B21"));
            items.Add(new Item(2, "Adidas Shoes", 1, dummyTransform, "B21"));
            items.Add(new Item(3, "Lacoste Shoes", 1, dummyTransform, "B18"));

        }

        [Test]
        public void IsBaydoorAtTheEndOfTheList() {
            if (locations[locations.Count-1].Description.Equals("Baydoor")) {
                Assert.Pass("Baydoor exists at the end!");
                return;
            }
            throw new Exception("Baydoor does not exist as an end-station!");
        }

        [Test, Order(2)]
        public void IsItemsEmpty()
        {
            try { 
            Assert.AreEqual(3, items.Count);
            // Assert.AreNotEqual(locations.Count,3);
        } catch(Exception e)
            {
                throw new Exception("Something went wrong, one of the lists are empty");
    }
           

        }

        [Test, Order(1)]
        public void IsLocationsEmpty() {
           
            Assert.AreEqual(4, locations.Count);
        }

        [Test, Order(3)]
        public void HasLocationItem()
        {
            //This is the location that have been scanned
            string locationScanned = "B21";
            bool doesLocationExist = false;
            //check if location is in the list
            for (int i = 0; i < locations.Count; i++) {
                if (locations[i].Description.Equals(locationScanned)) {
                    doesLocationExist = true;
                    break;
                }
            }
            //if location didnt exist throw exception
            if (doesLocationExist == false) {
                throw new Exception("Location number doesnt exist");
            }
            //check if any items belong to the location
            for (int i = 0; i< items.Count; i++)
            {
                if (items[i].BelongsToLocation.Equals(locationScanned))
                {
                    Assert.Pass(items[i].Name + " was successfully found at : " + locationScanned);
                    return;
                }
                
            }
            throw new Exception("Item was not found");
        }

        [Test, Order(4)]
        public void PickUpItem() {
            string locationScanned = "B21"; //we have scanned b21
            string itemScanned = "Nike Shoes"; //then we try and scan nike shoes

            //question is if nike shoes belongs to b21?
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].BelongsToLocation.Equals(locationScanned) && items[i].Name.Equals(itemScanned)){
                    Assert.Pass(itemScanned + " belongs to " + locationScanned);
                    return;
            }
            }
            throw new Exception(itemScanned + " did not belongs to " + locationScanned);


        }

        [Test, Order(5)]
        public void IsBarcodeItemOrLocation() {
            //test fails if string is empty.
            string barcode = "abcdefg";
            string answer = BarcodeRecognizer.getItem(barcode);
            if (barcode.Equals("")) {
                throw new Exception("Barcode value not found!");
            }
            if (answer.Equals("")) {
                Assert.Pass("found value! (location number): " + barcode);
                return;
            } else
            {
                Assert.Pass("found value! (item): " + answer);
            }
            
        }


        [OneTimeSetUp]
        public void ThisFirst() {
            fbs = new BarcodeScanner();
            items = PickListHelper.items;
            locations = BarcodeScanner.locationNumbers;
            InsertData();
        }
        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        /*[UnityTest]
        public IEnumerator NewEditModeTestWithEnumeratorPasses() {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
        */
        [OneTimeTearDown]
        public void Cleanup()
        {
            //Clean up after all tests has run
           Debug.Log("Done with all tests");
        }
        
    }
}