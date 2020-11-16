using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// Dialog containing the text to be displayed and the options available as responses.
/// </summary>
public class DataCategory : IComparable
{
    [XmlArray("DataCategory"), XmlArrayItem("Data")]
    public Data[] DataCategorys;	//The data available as responses.

    [XmlAttribute("id")] public string Id;	//The ID of the GameObject.

    /// <summary>
    /// Compares this object to the given object, returns an integer greater than 0 if the given object precedes this object,
    /// an integer less than 0 if the given object follows this object, and 0 if they have the same position in the sort order.
    /// </summary>
    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }

        DataCategory other = obj as DataCategory;
        if (other != null)
        {
            return Id.CompareTo(other.Id);
        }

        throw new ArgumentException("Object is not Data");
    }
}

/// <summary>
/// Represents an option given to the player during a dialog.
/// </summary>
[XmlRoot("Data")]
public class Data : IComparable
{
    [XmlAttribute("number")] public string Number;	// The options ordinal number among the other options for their containing Dialog

    public string SendToId;	// The ID of the next dialog to send the DialogHandler to if this option is selected. Set to "EoF" if the option should close the dialog.
    public string Text; // The text to be displayed as the description of the option.

    /// <summary>
    /// Compares this object to the given object, returns an integer greater than 0 if the given object precedes this object,
    /// an integer less than 0 if the given object follows this object, and 0 if they have the same position in the sort order.
    /// </summary>
    public int CompareTo(object obj)
    {
        if (obj == null)
        {
            return 1;
        }

        Data other = obj as Data;
        if (other != null)
        {
            return Number.CompareTo(other.Number);
        }

        throw new ArgumentException("Object is not Data");
    }
}

/// <summary>
/// A collection containing all of the games data.
/// </summary>
[XmlRoot("DataCollection")]
public class DataCollection
{
    public static TextAsset textFile;
    public static StringReader xmlFile;

    [XmlArray("Dialogs"), XmlArrayItem("Dialog")]
    public List<DataCategory> Datas;

    //public int Data
    //{
    //    get { return Data.Count; }
    //}

    /// <summary>
    /// Returns the Dialog in the DialogCollection with the given id.
    /// </summary>
    public DataCategory GetData(string id)
    {
        return Datas[GetIndexOf(id)];
    }

    /// <summary>
    ///	Gets the index of the Dialog with the given id.
    /// </summary>
    public int GetIndexOf(string id)
    {
        for (int i = 0; i < Datas.Count; i++)
        {
            if (Datas[i].Id == id)
            {
                return i;
            }
        }

        throw new ArgumentException("No data with id" + id + "found.");
    }

    /// <summary>
    /// Saves the DialogCollection object as an XML file.
    /// OBS!!! WARNING: Untested.
    /// </summary>
    public void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DataCollection));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    /// <summary>
    /// Loads the XML file containing the DialogCollection from the given path.
    /// </summary>
    private static DataCollection Load(StringReader path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DataCollection));

        return serializer.Deserialize(path) as DataCollection;
    }

    /// <summary>
    /// Loads and returns the DataCollection from disk. PATH start needs to be in Resources folder in UNITY!
    /// </summary>
    /// <returns>The games dialogues in the form of a DataCollection</returns>
    public static DataCollection GetDataCollection()
    {
        textFile = Resources.Load<TextAsset>("testing xml");
        xmlFile = new StringReader(textFile.text);

        return
            Load(xmlFile); //Example: @"C:\Users\erikb\Desktop\testing xml.xml", path doesn't need to be absolute, can be relative.
    }
}

/*
The XML document. Use as a reference when coding.

 <DataCollection>
 	<DataCategorys>
 		<DataCategory id="a">
 			<Text>5</Text>
			<Datas>
				<Data number="1">
					<Text>Here we say what option 1 is.</Text>
					<SendToId>200</SendToId>
				</Data>
				<Data number="2">
					<Text>Here we say what option 2 is.</Text>
					<SendToId>200</SendToId>
				</Data>
			</Datas>
		</DataCategory>
 		<DataCategory id="b">
 			<Text>You shall not pass! said the knight pointing his lance at you from atop his horse.</Text>
			<Options>
				<Option number="1">
					<Text>1. "Sir knight! Stay your lanse i wish no quarrel just to sell my food, what if we split the profits seventy thirty to you of course brave sir knight."</Text>
					<SendToId>200</SendToId>
				</Option>
				<Option number="2">
					<Text>2. Leave.</Text>
					<SendToId>EoF</SendToId>
				</Option>
			</Options>
		</DataCategory>
 	</DataCategorys>
 </DataCollection>
 */
