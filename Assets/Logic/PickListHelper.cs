using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Logic
{
    public class PickListHelper: MonoBehaviour
    {
        public static List<Item> items = new List<Item>();
        private int counter = 0;


        void Start()
        {
            
        }
        public void InsertPickUpItemToPanel(string itemName, string locationNumber)
        {
            if (GameObject.Find("item") == null)
            {
                GameObject gameObject = new GameObject("item");
                gameObject.tag = "item";
                gameObject.layer = 5; // 5 = ui.
                gameObject.AddComponent<Text>().text = itemName;
                gameObject.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
                gameObject.GetComponent<Text>().color = Color.black;
                gameObject.GetComponent<Text>().fontSize = 20;
                gameObject.GetComponent<RectTransform>().transform.localScale += new Vector3(4, 0, 0);
                //gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1f,1f,1f);
                gameObject.transform.SetParent(GameObject.Find("pickList").transform, false);
                //These 2 lines moves the text in the middle of the panel and all the way to the top.
                gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                //Place the text the right place. This will move the text a bit down and to the left.
                gameObject.GetComponent<RectTransform>().localPosition -= new Vector3(250, 90f, 0f);
                items.Add(new Item(counter, itemName, 1, gameObject.transform, locationNumber));
                string plusser = "";
                for (int i = 0; i < items.Count; i++)
                {
                    plusser += items[i].Name + ", " + items[i].BelongsToLocation;
                }
                GameObject.Find("Item").GetComponent<Text>().text = plusser;
                gameObject.GetComponent<Text>().enabled = false;
                counter++;
            }
            else
            {
                int holder = 0;
                for (int i = 0; i < items.Count; i++)
                {
                    if (holder < items[i].Id)
                    {
                        holder = items[i].Id;
                    }
                }
                Debug.Log(items.Count);
                GameObject gameObject = new GameObject("item");
                gameObject.tag = "item";
                gameObject.layer = 5; // 5 = ui.
                gameObject.AddComponent<Text>().text = itemName;
                gameObject.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
                gameObject.GetComponent<Text>().color = Color.black;
                gameObject.GetComponent<Text>().fontSize = 20;
                gameObject.GetComponent<RectTransform>().transform.localScale += new Vector3(4, 0, 0);
                gameObject.transform.SetParent(GameObject.Find("pickList").transform, false);
                gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                Transform lowestGameOBject = items[holder].PanelPosition;
                gameObject.GetComponent<RectTransform>().localPosition = lowestGameOBject.GetComponent<RectTransform>().localPosition;
                gameObject.GetComponent<RectTransform>().localPosition -= new Vector3(0, 50, 0);
                items.Add(new Item(counter, itemName, 1, gameObject.transform, locationNumber));
                string plusser = "";
                for (int i = 0; i < items.Count; i++)
                {
                    //plusser += items[i].Name + ", " +  items[i].BelongsToLocation + "\n";
                    plusser += items[i].nameAndBelongs;
                }
                GameObject.Find("Item").GetComponent<Text>().text = plusser;
                gameObject.GetComponent<Text>().enabled = false;
                counter++;
            }

        }
    
        public IEnumerator UpdateItemsPanel()
        {
            //Delete all Gameobjects with tag "item"
            GameObject[] itemsFromHierarchy;
            itemsFromHierarchy = GameObject.FindGameObjectsWithTag("item");
            foreach (GameObject item in itemsFromHierarchy)
            {
                Destroy(item);

            }
            yield return new WaitForSeconds(1.5f);

            if (GameObject.FindGameObjectsWithTag("item").Length == 0)
            {
                GameObject gameObject = new GameObject("item");
                gameObject.tag = "item";
                gameObject.layer = 5; // 5 = ui.
                gameObject.AddComponent<Text>().text = items[0].Name;
                gameObject.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
                gameObject.GetComponent<Text>().color = Color.black;
                gameObject.GetComponent<Text>().fontSize = 20;
                gameObject.GetComponent<RectTransform>().transform.localScale += new Vector3(4, 0, 0);
                //gameObject.GetComponent<RectTransform>().transform.localScale = new Vector3(1f,1f,1f);
                gameObject.transform.SetParent(GameObject.Find("pickList").transform, false);
                //These 2 lines moves the text in the middle of the panel and all the way to the top.
                gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                //Place the text the right place. This will move the text a bit down and to the left.
                gameObject.GetComponent<RectTransform>().localPosition -= new Vector3(250, 90f, 0f);
                items[0].PanelPosition = gameObject.transform;
                string plusser = "";
                for (int i = 0; i < items.Count; i++)
                {
                    plusser += items[i].Name + ", " + items[i].BelongsToLocation;
                }
                GameObject.Find("Item").GetComponent<Text>().text = plusser;
                counter++;
            }
            for (int i = 1; i < items.Count; i++)
            {
                GameObject gameObject = new GameObject("item");
                Debug.Log(items.Count);
                gameObject.tag = "item";
                gameObject.layer = 5; // 5 = ui.
                gameObject.AddComponent<Text>().text = items[i].Name;
                gameObject.GetComponent<Text>().font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
                gameObject.GetComponent<Text>().color = Color.black;
                gameObject.GetComponent<Text>().fontSize = 20;
                gameObject.GetComponent<RectTransform>().transform.localScale += new Vector3(4, 0, 0);
                gameObject.transform.SetParent(GameObject.Find("pickList").transform, false);
                gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                Transform lowestGameOBject = items[i - 1].PanelPosition;
                gameObject.GetComponent<RectTransform>().localPosition = lowestGameOBject.GetComponent<RectTransform>().localPosition;
                gameObject.GetComponent<RectTransform>().localPosition -= new Vector3(0, 50, 0);
                items[i].PanelPosition = gameObject.transform;
            }
        }
    }
}
