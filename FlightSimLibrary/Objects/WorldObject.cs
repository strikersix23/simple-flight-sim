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

namespace FlightSimLibrary
{
    public class WorldObject : GameObject
    {
        private class objDraw : DrawCommand
        {
            private Model myMesh;
            private Texture2D myTex;
            private GameObject myParent;
            public Matrix myWorld;
            public objDraw(Model mesh, Texture2D tex, WorldObject parent, Matrix World)
            {
                myParent = parent; myTex = tex; myMesh = mesh; myWorld = World;
            }
            public Texture2D getTexture()
            {
                return myTex;
            }
            public Model getModel()
            {
                return myMesh;
            }
            public Matrix getWorldTransform()
            {
                return myWorld;
            }
            public GameObject getParent()
            {
                return myParent;
            }
        }
        public BoundingSphere myCollision { get; set; }
        public Model Mesh { get; set; }
        public Texture2D MeshTexture { get; set; }
        private objDraw mainDrawCommand;
        private Vector3 rot;
        public WorldObject()
        {
            rot = Vector3.Zero;
        }
        public void Initalize()
        {
            mainDrawCommand = new objDraw(Mesh, MeshTexture, this, Matrix.Identity);
        }
        public void Update(GameTime gameTime)
        {
            rot += (float)gameTime.ElapsedGameTime.TotalSeconds * 0.1f * (Vector3.UnitX - Vector3.UnitY + 0.5f * Vector3.UnitZ);
            mainDrawCommand.myWorld = getWorldTransform();
        }
        public Vector3 getCoords()
        {
            return new Vector3(2, 2, 0);
        }
        public Matrix getWorldTransform()
        {
            return Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateTranslation(getCoords());
        }
        public DrawCommand[] GetDrawCommands()
        {
            return new DrawCommand[]{mainDrawCommand};
        }
        public bool hasCollidedWith(GameObject other)
        {
            if (myCollision.Intersects(other.collisionSphere()))
                return true;
            return false;
        }
        public BoundingSphere collisionSphere()
        {
            return myCollision;
        }
    }
}
