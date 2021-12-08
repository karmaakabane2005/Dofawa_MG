/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : PathFinding.cs, Fonction pour dessiner et calculer la minimap.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Dofawa_Forms;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Draw;
using Dofawa_Settings;
using Dofawa_Maps;
using Dofawa_MapChanger;

namespace Dofawa_MiniMap
{
    /// <summary>
    /// Fonction de la minimap
    /// </summary>
    public static class MiniMapFunctions
    {
        /// <summary>
        /// Obtient la position de la map sur la minimap
        /// </summary>
        /// <param name="mapId">id de la map</param>
        /// <param name="roomSize">Taille de la map</param>
        /// <param name="doorSize">Taille de la porte</param>
        /// <returns></returns>
        public static Vector2 GetRoomPos(string mapId, Vector2 roomSize, Vector2 doorSize)
        {
            int mainMapId = Dofawa_MapChanger.MapChanger.GetMainMapId(mapId);
            int subMapId = Dofawa_MapChanger.MapChanger.GetSubMapId(mapId);
            string UB = Dofawa_MapChanger.MapChanger.GetUpDownMapIndicator(mapId);
            int UBMultiply = UB == "U" ? -1 : (UB == "B" ? 1 : -1);
            Vector2 rectPos = new Vector2(mainMapId * (roomSize.X + doorSize.X), subMapId * (roomSize.Y + doorSize.Y) * UBMultiply);
            return rectPos;
        }
        /// <summary>
        /// Obtient le rectangle de la salle sur la minimap
        /// </summary>
        /// <param name="size">Taille de la salle</param>
        /// <param name="pos">Position de la salle</param>
        /// <param name="posOffset">Décalage de position de la salle</param>
        /// <returns></returns>
        public static Rectangle RoomRect(Vector2 size, Vector2 pos, Vector2 posOffset)
        {
            Vector2 centerOfScreen = SettingsFunctions.GetCenterOfScreen() - posOffset;
            return new Rectangle((int)pos.X + (int)centerOfScreen.X, (int)pos.Y + (int)centerOfScreen.Y, (int)size.X, (int)size.Y);
        }
        /// <summary>
        /// Affiche la minimap
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        public static void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphicsDeviceManager)
        {
            Vector2 currentRoomPos = GetRoomPos(Dofawa_Maps.Singleton.Instance.Maplevel, Dofawa_MiniMap.Singleton.Instance.roomSize, Dofawa_MiniMap.Singleton.Instance.doorSize);
            graphicsDeviceManager.GraphicsDevice.Clear(Dofawa_MiniMap.Singleton.Instance.backColor);
            Color roomColor;
            foreach (string mapid in Dofawa_Maps.Singleton.Instance.maps.Keys)
            {
                if (mapid != Dofawa_Maps.Singleton.Instance.Maplevel)
                {
                    roomColor = Dofawa_MiniMap.Singleton.Instance.roomColor;
                }
                else
                {
                    roomColor = Dofawa_MiniMap.Singleton.Instance.currentRoomColor;
                }
                Forms.DrawRectangle(
                    RoomRect(
                        Dofawa_MiniMap.Singleton.Instance.roomSize, 
                        GetRoomPos(
                            mapid, Dofawa_MiniMap.Singleton.Instance.roomSize,Dofawa_MiniMap.Singleton.Instance.doorSize),
                               currentRoomPos), 
                        roomColor, graphicsDeviceManager, _spriteBatch, 
                        Alignment.Center);
                DrawDoors(mapid, graphicsDeviceManager, _spriteBatch);
            }
        }
        /// <summary>
        /// Obtient la couleur de la porte
        /// </summary>
        /// <param name="mapId">id de la map</param>
        /// <param name="mapSide">sens de la map</param>
        /// <returns>Couleur de la map</returns>
        public static Color GetDoorColor(string mapId, MapSide mapSide)
        {
            string nextmapid = MapChanger.GetNextMapIdBySide(mapId, mapSide);
            if (Dofawa_Maps.Singleton.Instance.maps.ContainsKey(nextmapid))
            {
                if (Dofawa_Maps.Singleton.Instance.maps[nextmapid].nbFightCleared > 0)
                {
                    return new Color(49,158,69);
                }
            }
            if (Dofawa_Maps.Singleton.Instance.maps[mapId].nbFightCleared > 0)
            {
                return new Color(49,158,69);
            }
            return new Color(158,49,49);
        }
        /// <summary>
        /// Dessine les portes de la minimap
        /// </summary>
        /// <param name="mapId">id de la map</param>
        /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        public static void DrawDoors(string mapId, GraphicsDeviceManager graphicsDeviceManager, SpriteBatch _spriteBatch)
        {
            Vector2 currentRoomPos = GetRoomPos(Dofawa_Maps.Singleton.Instance.Maplevel, Dofawa_MiniMap.Singleton.Instance.roomSize, Dofawa_MiniMap.Singleton.Instance.doorSize);
            Vector2 origin = Dofawa_MiniMap.Singleton.Instance.roomSize * 0.5f;
            Maps.Map map = Dofawa_Maps.Singleton.Instance.maps[mapId];
            if (map.doorUp)
            {
                Forms.DrawRectangle(
                    RoomRect(
                        Dofawa_MiniMap.Singleton.Instance.doorSize,
                        GetRoomPos(
                            mapId, Dofawa_MiniMap.Singleton.Instance.roomSize, Dofawa_MiniMap.Singleton.Instance.doorSize),
                            currentRoomPos + new Vector2(0,origin.Y)),
                    GetDoorColor(mapId, MapSide.Up), graphicsDeviceManager, _spriteBatch,
                    Alignment.CenterBottom
                        );
            }
            if (map.doorRight)
            {
                Forms.DrawRectangle(
                    RoomRect(
                        Dofawa_MiniMap.Singleton.Instance.doorSize,
                        GetRoomPos(
                            mapId, Dofawa_MiniMap.Singleton.Instance.roomSize, Dofawa_MiniMap.Singleton.Instance.doorSize),
                            currentRoomPos + new Vector2(-origin.X,0)),
                    GetDoorColor(mapId, MapSide.Right), graphicsDeviceManager, _spriteBatch,
                    Alignment.CenterLeft
                        );
            }
            if (map.doorDown)
            {
                Forms.DrawRectangle(
                    RoomRect(
                        Dofawa_MiniMap.Singleton.Instance.doorSize,
                        GetRoomPos(
                            mapId, Dofawa_MiniMap.Singleton.Instance.roomSize, Dofawa_MiniMap.Singleton.Instance.doorSize),
                            currentRoomPos + new Vector2(0,-origin.Y)),
                    GetDoorColor(mapId, MapSide.Down), graphicsDeviceManager, _spriteBatch,
                    Alignment.CenterTop
                        );
            }
            if (map.doorLeft)
            {
                Forms.DrawRectangle(
                    RoomRect(
                        Dofawa_MiniMap.Singleton.Instance.doorSize,
                        GetRoomPos(
                            mapId, Dofawa_MiniMap.Singleton.Instance.roomSize, Dofawa_MiniMap.Singleton.Instance.doorSize),
                            currentRoomPos + new Vector2(origin.X,0)),
                    GetDoorColor(mapId, MapSide.Left), graphicsDeviceManager, _spriteBatch,
                    Alignment.CenterRight
                        );
            }
        }
        /// <summary>
        /// Change la scene entre "Game" et "Minimap"
        /// </summary>
        public static void ChangeScene()
        {
            int minimapSceneId = Dofawa_SceneManager.SceneFunctions.GetSceneByName("Minimap").id;
            int gameSceneId = Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game").id;
            if (Dofawa_SceneManager.Singleton.Instance.currentSceneId == gameSceneId || Dofawa_SceneManager.Singleton.Instance.currentSceneId == minimapSceneId)
            {
                if (Dofawa_SceneManager.Singleton.Instance.currentSceneId == minimapSceneId)
                {
                    Dofawa_SceneManager.Singleton.Instance.currentSceneId = gameSceneId;
                }
                else
                {
                    Dofawa_SceneManager.Singleton.Instance.currentSceneId = minimapSceneId;
                }
            }
        }
    }
    public class MiniMap
    {
        public Color roomColor = Color.GhostWhite;
        public Color currentRoomColor = new Color(75,120,179);
        public Vector2 roomSize = new Vector2(300, 200);
        public Vector2 doorSize = new Vector2(30, 30);
        public Color backColor = new Color(37,36,36);
    }
    public static class Singleton
    {
        public static MiniMap Instance { get; } = new MiniMap();
    }
}
