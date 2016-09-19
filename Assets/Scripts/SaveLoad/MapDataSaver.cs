using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace saveload
{
    [Serializable]
    public class MapDataSaver
    {

        Dictionary<string,  System.DateTime> savedUrls;
        private int maxDataSaves = 100;
        public MapDataSaver() {
            if (File.Exists(Application.persistentDataPath + "/MapData/general.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/MapData/general.dat", FileMode.Open);
                MapDataSaver data = (MapDataSaver)bf.Deserialize(file);
                file.Close();
                savedUrls = data.savedUrls;
            }
            else
            {
                savedUrls = new Dictionary<string, System.DateTime>();
                if (!Directory.Exists(Application.persistentDataPath + "/MapData/"))
                {
                    //if it doesn't, create it
                    Directory.CreateDirectory(Application.persistentDataPath + "/MapData/");

                }
            }
        }
        public string getMapData(string url)
        {
            if (!savedUrls.ContainsKey(url) || !File.Exists(Application.persistentDataPath + "/MapData/" + url + ".dat")) return null;
            savedUrls[url] = DateTime.Now;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MapData/"+ url+".dat", FileMode.Open);
            UrlData data = (UrlData)bf.Deserialize(file);
            file.Close();
            return data.data;
        }

        public void saveMapData(string url, string data)
        {
            if (savedUrls.Count > 100)
            {
                KeyValuePair<string, DateTime> oldesUrl = new KeyValuePair<string, DateTime>("Error", System.DateTime.MinValue);
                foreach (KeyValuePair<string, DateTime> someUrl in savedUrls)
                {
                    if(someUrl.Value > oldesUrl.Value) {
                        oldesUrl = someUrl;
                    }
                }
                if(oldesUrl.Key == "Error") {
                    Debug.Log("Error deleting oldest mapdata");
                    return;
                }
                File.Delete(Application.persistentDataPath + "/MapData/" + oldesUrl.Key + ".dat");
                savedUrls.Remove(oldesUrl.Key);
            }
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MapData/"+url+".dat", FileMode.OpenOrCreate);
            UrlData urlData = new UrlData(data);
            bf.Serialize(file, urlData);
            file.Close();
            savedUrls.Add(url, DateTime.Now);
            save();
        }
        public void save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/MapData/general.dat", FileMode.OpenOrCreate);
            bf.Serialize(file, this);
            file.Close();
        }
        [Serializable]
        public class UrlData
        {
            public string data;
            public UrlData(string data)
            {
                this.data = data;
            }
        } 
    }
}