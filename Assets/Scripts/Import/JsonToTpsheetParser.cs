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
    public static void Parse(string inPath, bool gungeonStructure)
    {
      if (gungeonStructure)
      {
        var lines = File.ReadAllText(inPath);
        lines = ("{ \"objs\":") + lines + "}";
        var objs = JsonUtility.FromJson<WholeGungeonFile>(lines).objs.Where(x => x.height != 0 && x.width != 0).ToList();
        var tpsheet =
          new StringBuilder(
            $":texture={new FileInfo(inPath).Name.Substring(0, new FileInfo(inPath).Name.Length - ".json".Length)}.png\n\n");
        objs.ForEach(line =>
        {
          tpsheet.AppendLine(line.name + ";" + line.x + ";" + line.y + ";" + line.width + ";" + line.height + "; " +
                             StupidEnd);
        });
        File.WriteAllText(inPath.Substring(0, inPath.Length - ".json".Length) + ".tpsheet", tpsheet.ToString());
      }
      else
      {
        var lines = File.ReadAllText(inPath);
        var frames = JsonUtility.FromJson<WholeTexturePackerFile>(lines).frames;
        var meta = JsonUtility.FromJson<WholeTexturePackerFile>(lines).meta;
        var tpsheet =
          new StringBuilder(
            $":texture={new FileInfo(inPath).Name.Substring(0, new FileInfo(inPath).Name.Length - ".json".Length)}.png\n\n");
        frames.ForEach(line =>
        {
          tpsheet.AppendLine(line.filename + ";" + line.frame.x + ";" + (meta.size.h - line.frame.y - line.frame.h)  + ";" + line.frame.w + ";" + line.frame.h + "; " +
                             StupidEnd);
        });
        File.WriteAllText(inPath.Substring(0, inPath.Length - ".json".Length) + ".tpsheet", tpsheet.ToString());
      }

    }
  }
  [Serializable]
  class WholeGungeonFile
  {
    public List<GungeonJsonStructure> objs;
  }
  [Serializable]
  class WholeTexturePackerFile
  {
    public List<TexturePackerJsonStructure> frames;
    public TexturePackerMeta meta;
  }
  [Serializable]
  class GungeonJsonStructure
  {
    public string name;
    public int x;
    public int y;
    public int width;
    public int height;
    public int flip;
    // public List<AttPoint> attachPoints;
    
  }  
  [Serializable]
  class TexturePackerJsonStructure
  {
    [Serializable]
    public class TexturePackerFrame
    {
      public int x;
      public int y;
      public int w;
      public int h;
    }
    public string filename;
    public TexturePackerFrame frame;
  }
  [Serializable]
  class TexturePackerMeta
  {
    [Serializable]
    public class TexturePackerMetaSize
    {
      public int w;
      public int h;
    }
    public TexturePackerMetaSize size;
  }
}