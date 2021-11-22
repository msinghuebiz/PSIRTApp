using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace PSIRTApp.Models
{
    public class ExtractXML
    {

        public string GetExpaigetExpandoObjectValue ( string column, List<ExpandoObject> list )
        {
            var result = string.Empty;
            foreach (var item in list)
            {
                var expandoDict = (IDictionary<string, object>)item;

                if (expandoDict.ContainsKey(column))
                {
                    result = expandoDict[column].ToString();
                }
            }

            return result;
        }

        public Tuple<List<string>,Dictionary<int, List<ExpandoObject>>> GetXMLValue(string xmlLink)
        {
            var elementList = new List<string>();
            var dicValues = new Dictionary<int, List<ExpandoObject>>();

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlLink);
            XmlElement root = doc.DocumentElement;
            var counter = 1;
            foreach (XmlNode item in root)
            {
                AddNodeToTuple(item, elementList, dicValues, dicValues.Count() + 1);
            }

            return new Tuple<List<string>, Dictionary<int, List<ExpandoObject>>>(elementList, dicValues);

        }

        public void AddNodeToTuple(XmlNode elementNode, List<string> elementList, Dictionary<int, List<ExpandoObject>> dictValue, int counter)
        {
            if ((elementNode.HasChildNodes) && (elementNode.Attributes.Count > 0))
            {

                foreach (XmlNode node in elementNode.ChildNodes)
                {
                    var tempList = new Dictionary<int, List<ExpandoObject>>();
                    var addToList = new List<ExpandoObject>();

                    AddNodeToTuple(node, elementList, tempList, counter);

                    foreach (var item in tempList.Keys)
                    {
                        var expandedList = tempList[item];
                        foreach (var inneritem in expandedList)
                        {
                            addToList.Add(inneritem);
                        }
                    }

                    dictValue.Add(counter, addToList);

                    counter += 1;

                }
            }
            else
            {
                var currentNode = elementNode.LocalName;
                var currentvalue = elementNode.InnerText;

                if (!(elementList.Contains(currentNode)))
                {
                    elementList.Add(currentNode);
                }

                var list = new List<ExpandoObject>();
                var newList = new ExpandoObject();

                AddProperty(newList, currentNode, currentvalue);

                list.Add(newList);
                dictValue.Add(counter, list);

            }
        }

        private void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            propertyValue = (propertyValue == DBNull.Value) ? string.Empty : propertyValue;
            var expandoDict = (IDictionary<string, object>)expando;
            if(expandoDict.ContainsKey(propertyName))
            {
                expandoDict[propertyName] = propertyValue;
            }
            else
            {
                expandoDict.Add(propertyName, propertyValue);
            }
        }

        public Dictionary<string,object> GetXMLObject(string xmlLink)
        {
            var result = new Dictionary<string,object>();
                        
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlLink);
            XmlElement root = doc.DocumentElement;

            foreach (XmlElement item in root)
            {
                if (( item.HasChildNodes ) && (item.HasAttributes ))
                {
                    foreach (XmlElement itemInner in item.ChildNodes)
                    {
                        // there are multiple rows 
                        var resultInner = GetInnerRows(itemInner);

                        foreach (var itemDic in resultInner)
                        {
                            result.Add(itemDic.Key, itemDic.Value);

                        }
                    }
                   
                }
                else
                {
                    
                    // there is single note 
                    var expandoElement = CreateExpandoObject( item.LocalName  +"_0", item.InnerText);
                    result.Add(item.LocalName, item.InnerText);
                }

            }

            return result;
        }

        private Dictionary<string,object> GetInnerRows ( XmlElement element)
        {
            var result = new Dictionary<string, object>();
            foreach (XmlElement item in element)
            {
                if ((item.HasChildNodes) && (item.HasAttributes))
                {
                    foreach (XmlElement itemInner in item.ChildNodes)
                    {
                        // there are multiple rows 
                        var resultInner = GetInnerRows(itemInner);

                        foreach (var itemDic in resultInner)
                        {
                            result.Add(itemDic.Key, itemDic.Value);

                        }
                    }
                }
                else
                {

                    // there is single note 
                    var expandoElement = CreateExpandoObject(item.LocalName, item.InnerText);
                    result.Add(item.LocalName, item.InnerText);
                }

            }

            return result;
        }
        private  Dictionary<string,object> CreateExpandoObject(string columnName , string columnValue )
        {
            
            var result = new Dictionary<string, object>();
            result.Add(columnName, columnValue);

            return result; 

        }
    }
}
