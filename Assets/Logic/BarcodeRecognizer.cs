using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Logic
{

    public class BarcodeRecognizer : MonoBehaviour
    {

        //can be used for:
        //(1) convert barcode to item name, if barcode exists in the if statements
        //(2) if no items existed according to the barcode, then the barcode might be a location number.
        public static string getItem(string barcode)
        {
            string test = "";
            if (barcode.Equals("012378A93L2N"))
            {
                test = "Tommy Jeans";
            }
            if (barcode.Equals("245123B23A22"))
            {
                test = "Lloyd";
            }
            if (barcode.Equals("abcdefg"))
            {
                test = "Nike Shoes";
            }
            if (barcode.Equals("12921j3"))
            {
                test = "Adidas Shoes";
            }
            if (barcode.Equals("hgerget33"))
            {
                test = "Lacoste Shoes";
            }

            if (!test.Equals(""))
            {
                return test;
            }
            return "";
        }
        public string ItemOrLocationNumber(string barcode)
        {
            if (BarcodeRecognizer.getItem(barcode).Equals(""))
            {
                return barcode;
            }
            if (!BarcodeRecognizer.getItem(barcode).Equals(""))
            {
                return BarcodeRecognizer.getItem(barcode);
            }
            return "";
        }
    }
}