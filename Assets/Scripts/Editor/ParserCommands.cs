
using System;
using System.IO;
using DefaultNamespace.Import;
using UnityEditor;
using UnityEngine;

public class ParserCommands
{
    [MenuItem("Gungeon/ParseEnterStructure")]
    public static void ParseEnterStructure()
    {
        var files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            if(fileInfo.Extension.Equals(".json"))
                JsonToTpsheetParser.Parse(file, true);
        }
    }
    [MenuItem("Gungeon/ParseNormalStructure")]
    public static void ParseNormalStructure()
    {
        var files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            if(fileInfo.Extension.Equals(".json"))
                JsonToTpsheetParser.Parse(file, false);
        }
    }
}
