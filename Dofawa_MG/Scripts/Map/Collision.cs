/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Collision.cs, Fonctions relatives aux collisions.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Dofawa_Maps;
using System.Diagnostics;

namespace Dofawa_Collision
{
    class Collision
    {
        /// <summary>
        /// L'élément ou le point touche
        /// </summary>
        /// <param name="x">position x en pixel</param>
        /// <param name="y">position y en pixel</param>
        /// <param name="map">la map courrante</param>
        /// <returns>L'élément ou le point touche</returns>
        public static Maps.MapItem GetElementCollided(int x, int y, Maps.Map map)
        {
            Vector2 tileIndex = Dofawa_Tiles.TilesFunctions.PixelToTile(x,y);
            if ((int)tileIndex.X > map.map.GetLength(1)-1 || (int)tileIndex.Y > map.map.GetLength(0)-1 || (int)tileIndex.X < 0 || (int)tileIndex.Y < 0)
            {
                return new Maps.MapItem(0, true,null);
            }
            else
            {
               return map.map[(int)tileIndex.Y, (int)tileIndex.X];
            }
        }
        /// <summary>
        /// Obtient la liste des éléments ou le rectangle touche
        /// </summary>
        /// <param name="rect">La boîte de collision</param>
        /// <returns>Les éléments collisionés</returns>
        public static List<Maps.MapItem> ElementCollided(Rectangle rect)
        {
            List<Maps.MapItem> elementsCollided = new List<Maps.MapItem>();
            Maps.Map map = Dofawa_Maps.Singleton.Instance.maps[Dofawa_Maps.Singleton.Instance.Maplevel];

            Vector2 tilePosUpLeft = Dofawa_Tiles.TilesFunctions.PixelToTile(rect.Left, rect.Top);
            Vector2 tilePosDownRight = Dofawa_Tiles.TilesFunctions.PixelToTile(rect.Right, rect.Bottom);

            for (int i = (int)tilePosUpLeft.Y; i <= (int)tilePosDownRight.Y; i++)
            {
                for (int j = (int)tilePosUpLeft.X; j <= (int)tilePosDownRight.X; j++)
                {
                    Vector2 posPx = Dofawa_Tiles.TilesFunctions.TileToPixel(j,i);
                    elementsCollided.Add(GetElementCollided((int)posPx.X, (int)posPx.Y, map));
                }
            }

            return elementsCollided;
        }
        /// <summary>
        /// Verifie si les items ont une boite de collision
        /// </summary>
        /// <param name="collidedItems">Liste des items</param>
        /// <returns>Vrai si au moins 1 à une boîte de collision</returns>
        public static bool isCollidedByAny(List<Maps.MapItem> collidedItems)
        {
            foreach (Maps.MapItem item in collidedItems)
            {
                if (item.hasCollideBox)
                    return true;
            }
            return false;
        }
    }
}
