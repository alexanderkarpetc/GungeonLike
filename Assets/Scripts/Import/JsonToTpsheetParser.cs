using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DefaultNamespace.Import
{

  public class JsonToTpsheetParser
  {
    // Example {"objs":[{"name":"sa","age":10}]};
    public const string StupidEnd = "0.5;0.5; 0;0;0;0; 0;0;0;0;0;0;0;0;0; 0;0;0;0;0;0;0";
    public static void Parse(string inPath)
    {
      var lines = File.ReadAllText(inPath);
      lines = ("{ \"objs\":") + lines + "}";
      var json = JsonUtility.FromJson<WholeFile>(lines).objs.Where(x => x.height != 0 && x.width != 0).ToList();
      var tpsheet = new StringBuilder($":texture={new FileInfo(inPath).Name.Substring(0,new FileInfo(inPath).Name.Length- ".json".Length)}.png\n\n");
      json.ForEach(line =>
      {
        tpsheet.AppendLine(line.name + ";" + line.x + ";" + line.y + ";" + line.width + ";" + line.height + "; " + StupidEnd);
      });
      File.WriteAllText(inPath.Substring(0,inPath.Length-".json".Length)+".tpsheet", tpsheet.ToString());
    }
  }
  [Serializable]
  class WholeFile
  {
    public List<GungeonJsonStructure> objs;
  }
  [Serializable]
  class GungeonJsonStructure
  {
    [Serializable]
    public class AttPoint
    {
      public string name;
      public string size;
    }
    public string name;
    public int x;
    public int y;
    public int width;
    public int height;
    public int flip;
    // public List<AttPoint> attachPoints;
    
  }
  [Serializable]
  class A
  {
    public string name;
    public int age;
  }
}