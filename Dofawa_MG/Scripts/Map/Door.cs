/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Door.cs, Fonctions relatives aux portes des salles.
 * Version : v1
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Dofawa_MapChanger;
using Dofawa_Maps;
using Microsoft.Xna.Framework;
using Dofawa_Tiles;
using System.Linq;

namespace Dofawa_Door
{
    /// <summary>
    /// Fonctions des portes
    /// </summary>
    public static class DoorFunctions
    {
        /// <summary>
        /// Verifie si la porte n'existe déjà pas dans la liste des portes non ouvertes et si elle y exsite pas alors ajoute 1 aux portes non ouvertes
        /// </summary>
        /// <param name="mapSide">Id de la map</param>
        public static void CheckUnopenedDoorPointer(string mapSide)
        {
            if (!Dofawa_Door.Singleton.Instance.lockedDoors.ContainsKey(mapSide))
            {
                Dofawa_Door.Singleton.Instance.unopenedDoors++;
            }
        }
        /// <summary>
        /// Rergarde si la porte du sens donnée exsite
        /// </summary>
        /// <param name="mapId">l'id de la map</param>
        /// <param name="mapSide">le sens de la porte</param>
        /// <returns></returns>
        public static bool CheckTravelDoor(string mapId, MapSide mapSide)
        {
            string nextMapId = MapChanger.GetNextMapIdBySide(mapId, mapSide);
            bool doorToTravel = false;
            if (Dofawa_Maps.Singleton.Instance.maps.ContainsKey(nextMapId))
            {
                switch (mapSide)
                {
                    case MapSide.Up:
                        doorToTravel = Dofawa_Maps.Singleton.Instance.maps[nextMapId].doorDown;
                        break;
                    case MapSide.Right:
                        doorToTravel = Dofawa_Maps.Singleton.Instance.maps[nextMapId].doorLeft;
                        break;
                    case MapSide.Down:
                        doorToTravel = Dofawa_Maps.Singleton.Instance.maps[nextMapId].doorUp;
                        break;
                    case MapSide.Left:
                        doorToTravel = Dofawa_Maps.Singleton.Instance.maps[nextMapId].doorRight;
                        break;
                }
            }
            return doorToTravel;
        }
        /// <summary>
        /// Passe la porte
        /// </summary>
        /// <param name="side">Sens</param>
        public static void PassDoor(MapSide side)
        {
            Random rand = new Random();
            string nextMapId = MapChanger.GetNextMapIdBySide(Dofawa_Maps.Singleton.Instance.Maplevel, side);
            Vector2 pos = Dofawa_Chars.Singleton.Instance.players[0].pos;
            switch (side)
            {
                case MapSide.Up:
                    Dofawa_Chars.Singleton.Instance.players[0].pos = new Vector2(pos.X, TilesFunctions.TileToPixel(0, Dofawa_Tiles.Singleton.Instance.TotalTileH - 2).Y - 10);
                    break;
                case MapSide.Down:
                    Dofawa_Chars.Singleton.Instance.players[0].pos = new Vector2(pos.X, Dofawa_Tiles.Singleton.Instance.TileHPx + 10);
                    break;
                case MapSide.Right:
                    Dofawa_Chars.Singleton.Instance.players[0].pos = new Vector2(Dofawa_Tiles.Singleton.Instance.TileWPx + 10, pos.Y);
                    break;
                case MapSide.Left:
                    Dofawa_Chars.Singleton.Instance.players[0].pos = new Vector2(TilesFunctions.TileToPixel(Dofawa_Tiles.Singleton.Instance.TotalTileW - 2, 0).X - 10, pos.Y);
                    break;
            }
            if (!MapChanger.isMapAlreadyCreated(nextMapId))
            {
                UpdateLockedDoors(nextMapId);
                bool[] doorRands = new bool[4];
                for (int i = 0; i < 4; i++)
                {
                    if (i == (((int)side) + 2) % 4)
                    {
                        doorRands[i] = true;
                    }
                    else
                    {
                        string doubleNextMapIdByDoor = MapChanger.GetNextMapIdBySide(nextMapId, (MapSide)i);
                        bool containsKey = Dofawa_Maps.Singleton.Instance.maps.ContainsKey(doubleNextMapIdByDoor);
                        bool checkTravelDoor = CheckTravelDoor(nextMapId, (MapSide)i);
                        if (checkTravelDoor)
                        {
                            doorRands[i] = true;
                        }
                        else if (MapChanger.GetMainMapId(nextMapId) == 0 && (MapSide)i == MapSide.Left || (containsKey && !checkTravelDoor))
                        {
                            doorRands[i] = false;
                        }
                        else
                        {
                            bool isOpenDoor = rand.Next(0, 2) == 0 ? false : true;
                            if (isOpenDoor && !containsKey)
                            {
                                CheckUnopenedDoorPointer(doubleNextMapIdByDoor);
                            }
                            doorRands[i] = isOpenDoor;
                        }
                    }
                }
                if (Dofawa_Door.Singleton.Instance.unopenedDoors <= 0)
                {
                    while (true)
                    {
                        int random = rand.Next(0, doorRands.Length);
                        string doubleNextMapIdByDoor = MapChanger.GetNextMapIdBySide(nextMapId, (MapSide)random);
                        bool containsKey = Dofawa_Maps.Singleton.Instance.maps.ContainsKey(doubleNextMapIdByDoor);
                        if (!doorRands[random] && (MapChanger.GetMainMapId(nextMapId) != 0 || (MapSide)random != MapSide.Left) && (!containsKey || CheckTravelDoor(nextMapId, (MapSide)random)))
                        {
                            doorRands[random] = true;
                            if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(doubleNextMapIdByDoor))
                            {
                                CheckUnopenedDoorPointer(doubleNextMapIdByDoor);
                            }
                            break;
                        }
                    }
                }
                MapChanger.CreateMap(nextMapId, 32, 18, doorRands[0], doorRands[1], doorRands[2], doorRands[3]);
            }
            MapChanger.ChangeMap(nextMapId);
            Dofawa_Door.Singleton.Instance.alreadyChange = true;
        }
        /// <summary>
        /// Passe la porte du haut
        /// </summary>
        public static void PassDoorUp()
        {
            if (!Dofawa_Door.Singleton.Instance.alreadyChange)
            {
                string nextmapid = MapChanger.GetNextMapIdBySide(Dofawa_Maps.Singleton.Instance.Maplevel, MapSide.Up);
                if (Dofawa_Maps.Singleton.Instance.maps[Dofawa_Maps.Singleton.Instance.Maplevel].nbFightCleared > 0)
                {
                    PassDoor(MapSide.Up);
                }
                else if (Dofawa_Maps.Singleton.Instance.maps.ContainsKey(nextmapid))
                {
                    if (Dofawa_Maps.Singleton.Instance.maps[nextmapid].nbFightCleared > 0)
                    {
                        PassDoor(MapSide.Up);
                    }
                }
            }
        }
        /// <summary>
        /// Passe la porte de droite
        /// </summary>
        public static void PassDoorRight()
        {
            if (!Dofawa_Door.Singleton.Instance.alreadyChange)
            {
                string nextmapid = MapChanger.GetNextMapIdBySide(Dofawa_Maps.Singleton.Instance.Maplevel, MapSide.Right);
                if (Dofawa_Maps.Singleton.Instance.maps[Dofawa_Maps.Singleton.Instance.Maplevel].nbFightCleared > 0)
                {
                    PassDoor(MapSide.Right);
                }
                else if (Dofawa_Maps.Singleton.Instance.maps.ContainsKey(nextmapid))
                {
                    if (Dofawa_Maps.Singleton.Instance.maps[nextmapid].nbFightCleared > 0)
                    {
                        PassDoor(MapSide.Right);
                    }
                }
            }
        }
        /// <summary>
        /// Passe la porte du bas
        /// </summary>
        public static void PassDoorDown()
        {
            if (!Dofawa_Door.Singleton.Instance.alreadyChange)
            {
                string nextmapid = MapChanger.GetNextMapIdBySide(Dofawa_Maps.Singleton.Instance.Maplevel, MapSide.Down);
                if (Dofawa_Maps.Singleton.Instance.maps[Dofawa_Maps.Singleton.Instance.Maplevel].nbFightCleared > 0)
                {
                    PassDoor(MapSide.Down);
                }
                else if (Dofawa_Maps.Singleton.Instance.maps.ContainsKey(nextmapid))
                {
                    if (Dofawa_Maps.Singleton.Instance.maps[nextmapid].nbFightCleared > 0)
                    {
                        PassDoor(MapSide.Down);
                    }
                }
            }
        }
        /// <summary>
        /// Pas la porte de gauche
        /// </summary>
        public static void PassDoorLeft()
        {
            if (!Dofawa_Door.Singleton.Instance.alreadyChange)
            {
                string nextmapid = MapChanger.GetNextMapIdBySide(Dofawa_Maps.Singleton.Instance.Maplevel, MapSide.Left);
                if (Dofawa_Maps.Singleton.Instance.maps[Dofawa_Maps.Singleton.Instance.Maplevel].nbFightCleared > 0)
                {
                    PassDoor(MapSide.Left);
                }
                else if (Dofawa_Maps.Singleton.Instance.maps.ContainsKey(nextmapid))
                {
                    if (Dofawa_Maps.Singleton.Instance.maps[nextmapid].nbFightCleared > 0)
                    {
                        PassDoor(MapSide.Left);
                    }
                }
            }
        }
        /// <summary>
        /// Crée une porte bloqué pour calculer le nombres de portes réelements ouvertes et non bloqués par d'autres portes
        /// </summary>
        /// <param name="mapId">Id de la map</param>
        /// <param name="doorUp">Porte du haut</param>
        /// <param name="doorRight">Porte du droite</param>
        /// <param name="doorDown">Porte du bas</param>
        /// <param name="doorLeft">Porte du gauche</param>
        public static void CreateLockedDoors(string mapId, bool doorUp, bool doorRight, bool doorDown, bool doorLeft)
        {
            if (doorUp)
            {
                string subMapId = MapChanger.GetNextMapIdBySide(mapId, MapSide.Up);
                if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(subMapId))
                {
                    List<string> tmpList = new List<string>();
                    string leftmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Left);
                    string upmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Up);
                    string rightmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Right);
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(leftmpid) || MapChanger.GetMainMapId(leftmpid) >= 0)
                    {
                        tmpList.Add(leftmpid);
                    }
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(upmpid) || MapChanger.GetMainMapId(upmpid) >= 0)
                    {
                        tmpList.Add(upmpid);
                    }
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(rightmpid) || MapChanger.GetMainMapId(rightmpid) >= 0)
                    {
                        tmpList.Add(rightmpid);
                    }
                    Dofawa_Door.Singleton.Instance.lockedDoors.TryAdd(subMapId, tmpList);
                }
            }
            if (doorRight)
            {
                string subMapId = MapChanger.GetNextMapIdBySide(mapId, MapSide.Right);
                if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(subMapId))
                {
                    List<string> tmpList = new List<string>();
                    string rightmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Right);
                    string upmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Up);
                    string downmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Down);
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(rightmpid) || MapChanger.GetMainMapId(rightmpid) >= 0)
                    {
                        tmpList.Add(rightmpid);
                    }
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(upmpid) || MapChanger.GetMainMapId(upmpid) >= 0)
                    {
                        tmpList.Add(upmpid);
                    }
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(downmpid) || MapChanger.GetMainMapId(downmpid) >= 0)
                    {
                        tmpList.Add(downmpid);
                    }
                    Dofawa_Door.Singleton.Instance.lockedDoors.TryAdd(subMapId, tmpList);
                }
            }
            if (doorDown)
            {
                string subMapId = MapChanger.GetNextMapIdBySide(mapId, MapSide.Down);
                if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(subMapId))
                {
                    List<string> tmpList = new List<string>();
                    string rightmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Right);
                    string downmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Down);
                    string leftmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Left);
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(rightmpid) || MapChanger.GetMainMapId(rightmpid) >= 0)
                    {
                        tmpList.Add(rightmpid);
                    }
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(downmpid) || MapChanger.GetMainMapId(downmpid) >= 0)
                    {
                        tmpList.Add(downmpid);
                    }
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(leftmpid) || MapChanger.GetMainMapId(leftmpid) >= 0)
                    {
                        tmpList.Add(leftmpid);
                    }
                    Dofawa_Door.Singleton.Instance.lockedDoors.TryAdd(subMapId, tmpList);
                }
            }
            if (doorLeft)
            {
                string subMapId = MapChanger.GetNextMapIdBySide(mapId, MapSide.Left);
                if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(subMapId))
                {
                    List<string> tmpList = new List<string>();
                    string upmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Up);
                    string downmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Down);
                    string leftmpid = MapChanger.GetNextMapIdBySide(subMapId, MapSide.Left);
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(upmpid) || MapChanger.GetMainMapId(upmpid) >= 0)
                    {
                        tmpList.Add(upmpid);
                    }
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(downmpid) || MapChanger.GetMainMapId(downmpid) >= 0)
                    {
                        tmpList.Add(downmpid);
                    }
                    if (!Dofawa_Maps.Singleton.Instance.maps.ContainsKey(leftmpid) || MapChanger.GetMainMapId(leftmpid) >= 0)
                    {
                        tmpList.Add(leftmpid);
                    }
                    Dofawa_Door.Singleton.Instance.lockedDoors.TryAdd(subMapId, tmpList);
                }
            }
        }
        /// <summary>
        /// Update les portes bloqués pour savoir le nombres de portes réels le joueur lui reste
        /// </summary>
        /// <param name="mapId">ID de la map</param>
        public static void UpdateLockedDoors(string mapId)
        {
            if (Dofawa_Door.Singleton.Instance.lockedDoors.ContainsKey(mapId))
            {
                Dofawa_Door.Singleton.Instance.lockedDoors.Remove(mapId);
                Dofawa_Door.Singleton.Instance.unopenedDoors--;
            }
            foreach (KeyValuePair<string, List<string>> lkdoor in Dofawa_Door.Singleton.Instance.lockedDoors)
            {
                foreach (string lkdoormapid in lkdoor.Value.ToList())
                {
                    if (lkdoormapid == mapId)
                    {
                        lkdoor.Value.Remove(lkdoormapid);
                        if (lkdoor.Value.Count <= 0)
                        {
                            Dofawa_Door.Singleton.Instance.unopenedDoors--;
                            UpdateLockedDoors(lkdoormapid);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// Super globales des portes
    /// </summary>
    public class Door
    {
        public IDictionary<string, List<string>> lockedDoors = new Dictionary<string, List<string>>();
        public int unopenedDoors = 1;
        // Vrai si le joueur a déjà passé une porte cette frame
        public bool alreadyChange = false;
    }
    /// <summary>
    /// Donne l'instance des super globales de door
    /// </summary>
    public static class Singleton
    {
        public static Door Instance { get; } = new Door();
    }
}
