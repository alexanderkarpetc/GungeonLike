using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.Import;
using UnityEngine;

public class JsonParserScript : MonoBehaviour
{
    void Start()
    {
        var files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            if(fileInfo.Extension.Equals(".json"))
                JsonToTpsheetParser.Parse(file);
        }
    }
}
