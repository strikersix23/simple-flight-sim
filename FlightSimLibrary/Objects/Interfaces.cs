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
    public interface GameObject
    {
        void Initalize();
        void Update(GameTime gameTime);
        Vector3 getCoords();
        Matrix getWorldTransform();
        DrawCommand[] GetDrawCommands();
        bool hasCollidedWith(GameObject other);
        BoundingSphere collisionSphere();
    }
    public interface DrawCommand
    {
        Texture2D getTexture();
        Model getModel();
        Matrix getWorldTransform();
        GameObject getParent();
    }
    public struct DrawParameters
    {
        public bool isSprite;
        //techniques and other stuff
    }
}
