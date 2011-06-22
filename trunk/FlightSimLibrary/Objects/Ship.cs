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
    public class Ship : GameObject
    {
        private class ShipDraw : DrawCommand
        {
            private Model myMesh;
            private Texture2D myTex;
            private GameObject myParent;
            public Matrix myWorld;
            public ShipDraw(Model mesh, Texture2D tex, Ship parent, Matrix World)
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
        public Model Mesh { get; set; }
        public Texture2D MeshTexture { get; set; }
        private ShipDraw mainDrawCommand;
        public Quaternion rotation { get; set; }
        public Vector3 forwardVector { get; set; }
        public Vector3 rightVector { get; set; }
        public Vector3 curPosition { get; set; }
        public float speed = 0.0f;

        public void Initalize() 
        {
            mainDrawCommand = new ShipDraw(Mesh, MeshTexture, this, Matrix.Identity);
            forwardVector = Vector3.UnitY;
            rightVector = Vector3.UnitX;
            curPosition = Vector3.Zero;
            rotation = Quaternion.Identity;
        }
        public void Update(GameTime gameTime)
        {
            forwardVector = Vector3.Transform(Vector3.UnitY, Matrix.CreateFromQuaternion(rotation));
            rightVector = Vector3.Transform(Vector3.UnitX, Matrix.CreateFromQuaternion(rotation));
            curPosition += Vector3.Normalize(forwardVector) * speed;
            mainDrawCommand.myWorld = getWorldTransform();
        }
        public DrawCommand[] GetDrawCommands()
        {
            return new DrawCommand[]{mainDrawCommand};
        }
        public Vector3 getCoords()
        {
            return curPosition;
        }
        public Matrix getWorldTransform()
        {
            return Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(getCoords());
        }
        public bool hasCollidedWith(GameObject other)
        {
            return false;
        }
        public BoundingSphere collisionSphere()
        {
            return new BoundingSphere();
        }
        public void Accelerate()
        {
            speed += 0.001f;
        }
    }
}
