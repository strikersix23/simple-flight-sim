using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace FlightSimLibrary
{
    public class LevelLoader
    {
        public static Dictionary<string, KeyValuePair<string, Object>> loadedAssets;
        private static Game myParent;
        private static Queue<LevelMap> toLoad;
        private static List<Thread> loadingThreads;
        public static void InitalizeLevelLoader(Game parent)
        {
            myParent = parent;
            loadedAssets = new Dictionary<string, KeyValuePair<string, object>>();
            toLoad = new Queue<LevelMap>();
            loadingThreads = new List<Thread>();
        }
        public static object LoadInitially(string asset, string type)
        {
            object loadedAsset;
            switch (type)
            {
                case "Model":
                    loadedAsset = myParent.Content.Load<Model>(asset);
                    loadedAssets.Add(asset, new KeyValuePair<string,object>("Model", loadedAsset));
                    return loadedAsset;
                case "Texture2D":
                    loadedAsset = myParent.Content.Load<Texture2D>(asset);
                    loadedAssets.Add(asset, new KeyValuePair<string,object>("Texture2D", loadedAsset));
                    return loadedAsset;
                default:
                    return null;
            }
        }
        public static void LoadLevel(LevelMap levelToLoad)
        {
            toLoad.Enqueue(levelToLoad);
            Thread newThread = new Thread(new ThreadStart(LoaderThread));
            loadingThreads.Add(newThread);
            newThread.Start();
        }
        public static bool HasFinishedLoading()
        {
            return loadingThreads.Count == 0;
        }
        public static void LoaderThread()
        {
            if (toLoad.Count == 0)
            {
                loadingThreads.Remove(Thread.CurrentThread);
                return;
            }
            LevelMap levelToLoad = toLoad.Dequeue();
            for (int i = 0; i < levelToLoad.myLoadCommands.Length; i++)
            {
                if (!loadedAssets.ContainsKey(levelToLoad.myLoadCommands[i].Key))
                {
                    KeyValuePair<string, Object> asset = new KeyValuePair<string,object>();
                    bool assetInitalized = false;
                    Object assetVal;
                    switch(levelToLoad.myLoadCommands[i].Value)
                    {
                        case "Model": 
                            assetVal = myParent.Content.Load<Model>(levelToLoad.myLoadCommands[i].Key);
                            asset = new KeyValuePair<string,object>("Model", assetVal);
                            assetInitalized = true;
                            break;
                        case "Texture2D":
                            assetVal = myParent.Content.Load<Texture2D>(levelToLoad.myLoadCommands[i].Key);
                            asset = new KeyValuePair<string, object>("Texture2D", assetVal);
                            assetInitalized = true;
                            break;
                    }
                    if (assetInitalized)
                        loadedAssets.Add(levelToLoad.myLoadCommands[i].Key, asset);
                }
            }
            loadingThreads.Remove(Thread.CurrentThread);
            return;
        }
    }
}
