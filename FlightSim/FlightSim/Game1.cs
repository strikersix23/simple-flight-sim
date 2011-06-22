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

using FlightSimLibrary;

namespace FlightSim
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //Raw objects
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Main objects to address groups of objects
        List<DrawCommand> mainDrawCommands;
        List<GameObject> mainGameObjects;
        InputHandler inHandler;
        //Specific objects we'll want to pay attention to
        Ship playerShip;
        Quaternion myView;
        Vector3 myPos;
        Vector3 baseViewPos;
        Vector3 lookPos;
        Vector3 camUp;
        Matrix View;
        Matrix Proj;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mainDrawCommands = new List<DrawCommand>();
            mainGameObjects = new List<GameObject>();
            playerShip = new Ship();
            mainGameObjects.Add(playerShip);
            myPos = new Vector3(0, -2, 2);
            baseViewPos = myPos;
            myView = Quaternion.Identity;
            inHandler = new InputHandler();
            inHandler.CurrentKeyMapping = InputHandler.getDefaultKeymapping();
            lookPos = Vector3.Zero;
            camUp = Vector3.UnitZ;
            LevelLoader.InitalizeLevelLoader(this);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// 
        /// Kaushik: The content pipeline in XNA is pretty complicated. Essentially
        /// your files are converted to an easily understood file, which are then
        /// loaded on run-time - this you don't "load" a file as you normally would.
        /// The reason I have my code split into three projects (Content, Library, and
        /// game) is because I need to have references to each of the other ones - if
        /// I did it with two projects, they'd reference each other, and you'd get a
        /// compile error. I can go into more on the content pipeline if you want -
        /// this project won't really be dealing with it too much, except for maybe one
        /// or two things (levels, for example).
        /// 
        /// Also, the way I do loading is a little different than the way XNA does
        /// loading (because it commandeers all the threads to do loading, and so the
        /// game "freezes"). I do it dynamically - so, eventually I'll have a "load manager"
        /// which will watch what files I've loaded, unload some of them, and load
        /// more in - this is sort of a work-around manual garbage collector at a more
        /// abstract level. The load manager is multi-threaded, so I can run a
        /// simple loading screen while it's loading. As such this method isn't really
        /// my "load" function - it loads the bare necessisities, and gets the real
        /// loading process started.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerShip.Mesh = LevelLoader.LoadInitially("scuttlebutmk2", "Model") as Model;
            playerShip.MeshTexture = LevelLoader.LoadInitially("simpleShipTex", "Texture2D") as Texture2D;
            playerShip.Initalize();
            LevelMap sampleMap = LevelMap.sampleLevelMap();
            LevelLoader.LoadLevel(sampleMap);
            while (!LevelLoader.HasFinishedLoading()) ;
            for (int i = 0; i < sampleMap.myObjectsWithProperties.GetLength(0); i++)
            {
                switch (sampleMap.myObjectsWithProperties[i][0])
                {
                    case "WorldObject":
                        WorldObject newWorldObj = new WorldObject();
                        newWorldObj.Mesh = LevelLoader.loadedAssets[sampleMap.myObjectsWithProperties[i][1]].Value as Model;
                        newWorldObj.MeshTexture = LevelLoader.loadedAssets[sampleMap.myObjectsWithProperties[i][2]].Value as Texture2D;
                        newWorldObj.Initalize();
                        mainGameObjects.Add(newWorldObj);
                        break;
                }
            }
            //WorldObject rock = new WorldObject();
            ////rock.Mesh = Content.Load<Model>("simpleAsteroid");
            //rock.MeshTexture = playerShip.MeshTexture;
            //string loadedType = LevelLoader.loadedAssets["simpleAsteroid"].Key;
            //switch(loadedType)
            //{
            //    case "Model":
            //        rock.Mesh = LevelLoader.loadedAssets["simpleAsteroid"].Value as Model;
            //        break;
            //}
            //rock.Initalize();
            //mainGameObjects.Add(rock);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// 
        /// Kaushik: The update method is a little skimpy right now, primarily
        /// because we'll be adding in alot more of the specific code to
        /// handle things in here. What I like to do is to seperate the
        /// "Update" function into several child functions for the seperate
        /// game-states - there'll be a menu update function, a game update
        /// function, and so on. What's here is the basics I needed to get
        /// started.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //Handle input and output
            inHandler.Update(gameTime);
            if (inHandler.isKeyDown(FlightSimLibrary.Action.Up))
                playerShip.rotation = Quaternion.CreateFromAxisAngle(playerShip.rightVector, 0.1f) * playerShip.rotation;
            else if (inHandler.isKeyDown(FlightSimLibrary.Action.Down))
                playerShip.rotation = Quaternion.CreateFromAxisAngle(playerShip.rightVector, -0.1f) * playerShip.rotation;
            if (inHandler.isKeyDown(FlightSimLibrary.Action.Left))
                playerShip.rotation = Quaternion.CreateFromAxisAngle(playerShip.forwardVector, 0.1f) * playerShip.rotation;
            else if (inHandler.isKeyDown(FlightSimLibrary.Action.Right))
                playerShip.rotation = Quaternion.CreateFromAxisAngle(playerShip.forwardVector, -0.1f) * playerShip.rotation;
            if (inHandler.isKeyDown(FlightSimLibrary.Action.Accelerate))
                playerShip.Accelerate();
            myPos = Vector3.Transform(baseViewPos, Matrix.CreateFromQuaternion(playerShip.rotation));
            myPos += playerShip.getCoords();
            camUp = Vector3.Transform(Vector3.UnitZ, Matrix.CreateFromQuaternion(playerShip.rotation));
            //lookPos = Vector3.Transform(lookPos, Matrix.CreateTranslation(-playerShip.getCoords()));
            View = Matrix.CreateLookAt(myPos, playerShip.getCoords(), camUp);

            //if (inHandler.isKeyDown(FlightSimLibrary.Action.Up))
            //    myPos -= 0.1f * Vector3.UnitX;
            //else if (inHandler.isKeyDown(FlightSimLibrary.Action.Down))
            //    myPos += 0.1f * Vector3.UnitX;
            //if (inHandler.isKeyDown(FlightSimLibrary.Action.Left))
            //    myPos -= 0.1f * Vector3.UnitY;
            //else if (inHandler.isKeyDown(FlightSimLibrary.Action.Right))
            //    myPos += 0.1f * Vector3.UnitY;
            //Set the view and projection matricies
            //View = Matrix.CreateLookAt(myPos, Vector3.Zero, Vector3.UnitZ);//Typically in game 3space, Y is up - this is more of that bethesda influence, as in their games Z is always up.
            Proj = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2, graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight, 0.1f, 10000.0f);
            //Perform update operations
            foreach (GameObject g in mainGameObjects)
                g.Update(gameTime);
            mainDrawCommands.Clear();//Clears the draw queue
            foreach (GameObject g in mainGameObjects)
                mainDrawCommands.AddRange(g.GetDrawCommands());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// 
        /// Kaushik: what I do here with a "draw loop" is not something you'll see in any tutorials.
        /// It's something I've moved to out of necessity rather than code cleaniness. In the tutorials
        /// you read, you'll see alot of actual calls to draw commands and other stuff. Because the
        /// draw and update functions run in two seperate threads in XNA, this can lead to some strange
        /// concurrency issues - the most common of which is "drifting" of objects when the player moves
        /// around. By doing it the way I'm doing here, we first off eliminate that "drifting", as well
        /// as increasing effeciency - it really cuts back on lag (XNA doesn't give me a very big overhead
        /// when it comes to using processing power).
        /// 
        /// If it's too complicated, I can walk through my reasoning. Essentially, think back to every
        /// lesson we've had on concurrency - this is a "consumer" thread, whereas the program is a
        /// "producer" thread - if the producer is adding things in to the consumer's plate or modifying
        /// them, we can get weird effects like the drifting I talked about.
        /// 
        /// I'll include a link to an example of "drifting" in a game in the readme.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);//Draw Loop, in it's simplest state - not sure if everyone does this, but I find it alot easier to abstract this.
            foreach (DrawCommand d in mainDrawCommands)//First, go through all the draw commands that I've given myself in the update - this prevents concurrency issues.
            {
                foreach (ModelMesh m in d.getModel().Meshes)//Next, nab each of the meshes comprising the model (if there are seperate - typically only two to three)
                {
                    foreach (BasicEffect e in m.Effects)//This is the basic effect that XNA comes with - RIEMER's uses a custom effect, and I'll be writing my own shaders later.
                    {
                        e.World = d.getWorldTransform();//Get the world transform associated with the mesh (rotation, translation, scaling, etc. done here) and pass it to the effect.
                        e.View = View;//Set the effect's view matrix to the one we're calculating - mathematically, this transforms the matrices to meet the specific view point.
                        e.Projection = Proj;//Set the projection matrix - this is what creates a "fish eye" effect, or a narrow field of view.
                        e.Texture = d.getTexture();//Pass our texture into the effect
                        //e.EnableDefaultLighting();//This just creates a basic 3-point lighting system - I don't really need it for testing.
                        e.TextureEnabled = true;//This tells the effect that yes, we WOULD like to use our texture.
                    }
                    m.Draw();//After all the effects associated with m have been updated, we tell it to go ahead and draw itself.
                }
            }
            //Alongside this main draw loop we'll also have subsidary draw loops - for example, shadows (not done yet), and "post draw effects" such as the UI have a seperate draw list and loop (so as to ensure they're on top).

            base.Draw(gameTime);//Calls the draw command of the parent - not really needed in most cases, but best not to delete.
        }
    }
}
