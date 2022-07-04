
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Xml;


public class ConfigHandler
{
    
    private static XmlDocument doc; 

    public static void init(){
        doc = new XmlDocument();
        doc.Load("Assets/Data/config.xml");
    }

    public static List<Dictionary<string, string>> readConfig(){

        List<Dictionary<string, string>> minions = new List<Dictionary<string, string>>();

        XmlNodeList nodelist = doc.SelectNodes("/character/minion");
        foreach (XmlNode node in nodelist) {
            var minion = new Dictionary<string, string>();
            foreach (XmlNode child in node.ChildNodes) {
                if (child.Name == "name") {
                    minion.Add("name", child.InnerText);
                } else if (child.Name == "x"){
                    minion.Add("xPos", child.InnerText);
                } else {
                    minion.Add("yPos", child.InnerText);
                }
            }
            minions.Add(minion);
        }

        return minions;
    }

}
