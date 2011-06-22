using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightSimLibrary
{
    public class LevelMap
    {
        //string[][] - first array is objects, second is object properties
        public string[][] myObjectsWithProperties;
        //Extra information about each object - rotations
        //Loading list of meshes and textures
        public KeyValuePair<string, string>[] myLoadCommands;
        public static LevelMap sampleLevelMap()
        {
            LevelMap newMap = new LevelMap();
            newMap.myLoadCommands = new KeyValuePair<string, string>[2];
            newMap.myLoadCommands[0] = new KeyValuePair<string, string>("station", "Model");
            newMap.myLoadCommands[1] = new KeyValuePair<string, string>("simpleAsteroid", "Model");
            newMap.myObjectsWithProperties = new string[1][];
            newMap.myObjectsWithProperties[0] = new string[] { "WorldObject", "simpleAsteroid", "simpleShipTex" };
            return newMap;
        }
    }
}
