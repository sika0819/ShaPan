using UnityEngine;
using System.Collections;
using System.Xml;
using System.Net;
public static class XmlTool {
    
    public static string ReadSingleNode(XmlDocument xmlDoc,string nodeName)
    {
        string Value = "";
        XmlNode root = xmlDoc.SelectSingleNode("Config");
        foreach (XmlNode item in root)
        {
            foreach (XmlNode child in item)
            {
                if (child.Name == nodeName)
                {
                    Value = child.InnerText;
                    Debug.Log(child.Name + ":" + Value);
                }
            }
        }
       
        return Value;
    }
    
    public static IPEndPoint[] ReadNodes(XmlDocument xmlDoc, string nodeName,string ipNodeName,string portNodeName)
    {
        IPEndPoint[] backPoint=new IPEndPoint[0];
        XmlNodeList nodeList = xmlDoc.SelectNodes(nodeName);
        backPoint = new IPEndPoint[nodeList.Count];
        for (int iLoop = 0; iLoop < nodeList.Count; iLoop++)
        {
            XmlNode ipNode = nodeList[iLoop].SelectSingleNode(ipNodeName);
            XmlNode portNode= nodeList[iLoop].SelectSingleNode(portNodeName);
            string ip = ipNode.InnerText;
            string port= portNode.InnerText;
            IPEndPoint newIpEP = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
        }
        return backPoint;
    }
}
