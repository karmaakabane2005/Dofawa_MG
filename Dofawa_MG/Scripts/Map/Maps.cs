/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Maps.cs
 * Version : v1
 */
using System;
using System.Collections.Generic;
using Dofawa_Door;

namespace Dofawa_Maps
{
    public class Maps
    {
        public class Map
        {
            public MapItem[,] map;
            public string mapId;
            public bool doorUp;
            public bool doorRight;
            public bool doorDown;
            public bool doorLeft;
            public bool fightCleared = false;
            public int nbFightCleared;
            /// <summary>
            /// crée une map avec les paramètres suivants :
            /// </summary>
            /// <param name="map"></param>
            /// <param name="mapId"></param>
            /// <param name="doorUp"></param>
            /// <param name="doorRight"></param>
            /// <param name="doorDown"></param>
            /// <param name="doorLeft"></param>
            public Map(MapItem[,] map, string mapId, bool doorUp, bool doorRight, bool doorDown, bool doorLeft)
            {
                this.map = map;
                this.mapId = mapId;
                this.doorUp = doorUp;
                this.doorRight = doorRight;
                this.doorLeft = doorLeft;
                this.doorDown = doorDown;
            }
        }
        public class MapItem
        {
            public int spriteId;
            public bool hasCollideBox;
            public Action eventOnCollide;
            /// <summary>
            /// crée une case de la map et lui donne une colision(au besoin) et une action si colision
            /// </summary>
            /// <param name="spriteId"></param>
            /// <param name="hasCollideBox"></param>
            /// <param name="eventOnCollide"></param>
            public MapItem(int spriteId, bool hasCollideBox, Action eventOnCollide)
            {
                this.spriteId = spriteId;
                this.hasCollideBox = hasCollideBox;
                this.eventOnCollide = eventOnCollide;
                
            }
        }
        /// <summary>
        /// fonctions servant a faire une map dynamique selon la taille voulue
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="doorUp"></param>
        /// <param name="doorRight"></param>
        /// <param name="doorDown"></param>
        /// <param name="doorLeft"></param>
        /// <returns></returns>
        public Map createDynamicMap(string mapId, int width = 32, int height = 18, bool doorUp = false, bool doorRight = true, bool doorDown = false, bool doorLeft = false)
        {
            int wallUp = doorUp ? ((width - 6) / 2) : ((width - 2) / 2);
            int wallDown = doorDown ? ((width - 6) / 2) : ((width - 2) / 2);
            int wallRight = doorRight ? ((height - 5) / 2) : ((height - 2) / 2);
            int wallLeft = doorLeft ? ((height - 5) / 2) : ((height - 2) / 2);
            int wallWidth = (width - 6) / 2;
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            List<MapItem> line = new List<MapItem>();
            MapItem[,] map = new MapItem[height, width];
            Random rand = new Random();
            for (int i = 0; i < height; i++)
            {
                if (i == 0 || i == height - 1)
                {
                    if (i == 0)
                    {
                        if (doorUp)
                        {

                            line.Add(new MapItem(0, true, null));
                            for (int l = 0; l < wallUp; l++)
                            {
                                line.Add(new MapItem(1, true, null));
                            }

                            for (int l = 0; l < 4; l++)
                            {
                                line.Add(new MapItem(chooseRandom(), false, DoorFunctions.PassDoorUp));
                            }


                            for (int l = 0; l < wallUp; l++)
                            {
                                line.Add(new MapItem(1, true, null));
                            }
                            line.Add(new MapItem(3, true, null));
                        }
                        else
                        {
                            line.Add(new MapItem(0, true, null));

                            for (int l = 0; l < width - 2; l++)
                            {
                                line.Add(new MapItem(1, true, null));
                            }



                            line.Add(new MapItem(3, true, null));
                        }

                    }
                    if (i == height - 1)
                    {
                        if (doorDown)
                        {

                            line.Add(new MapItem(72, true, null));
                            for (int l = 0; l < wallDown; l++)
                            {
                                line.Add(new MapItem(1, true, null));
                            }

                            for (int l = 0; l < 4; l++)
                            {


                                line.Add(new MapItem(chooseRandom(), false, DoorFunctions.PassDoorDown));


                            }


                            for (int l = 0; l < width / 2 - 3; l++)
                            {
                                line.Add(new MapItem(1, true, null));
                            }
                            line.Add(new MapItem(73, true, null));
                        }
                        else
                        {
                            line.Add(new MapItem(72, true, null));
                            for (int l = 0; l < width - 2; l++)
                            {
                                line.Add(new MapItem(1, true, null));
                            }
                            line.Add(new MapItem(73, true, null));
                        }
                    }
                }
                else
                {
                    if (i >= halfHeight - 2 && i <= halfHeight + 1)
                    {
                        if (doorLeft)
                        {
                            line.Add(new MapItem(chooseRandom(), false, DoorFunctions.PassDoorLeft));
                        }
                        else
                        {
                            line.Add(new MapItem(14, true, null));
                        }
                        for (int l = 0; l < width - 2; l++)
                        {
                            line.Add(new MapItem(chooseRandom(), false, null));
                        }
                        if (doorRight)
                        {
                            line.Add(new MapItem(chooseRandom(), false, DoorFunctions.PassDoorRight));
                        }
                        else
                        {
                            line.Add(new MapItem(17, true, null));

                        }
                    }
                    else
                    {

                        line.Add(new MapItem(14, true, null));

                        for (int l = 0; l < width - 2; l++)
                        {
                            line.Add(new MapItem(chooseRandom(), false, null));
                        }



                        line.Add(new MapItem(17, true, null));


                    }
                }
                for (int key = 0; key < line.Count; key++)
                {
                    map[i, key] = line[key];

                }

                line.Clear();
            }
            return new Map(map, mapId, doorUp, doorRight, doorDown, doorLeft);
        }
        /// <summary>
        /// nombre random choisisant entre 2 nombre avec 1 chance sur 10 pour faire une salle plus acceuillante
        /// </summary>
        /// <param name="tileOne"></param>
        /// <param name="tileTwo"></param>
        /// <returns></returns>
        public int chooseRandom(int tileOne = 60, int tileTwo = 15)
        {
            Random rand = new Random();
            if (rand.Next(1, 10) == 1)
            {
                return tileOne;
            }
            else
            {
                return tileTwo;
            }

        }
        public IDictionary<string, Map> maps = new Dictionary<string, Map>();
        public string Maplevel = "0";

        

    }
    /// <summary>
    /// classes permetant d'accéder dynamiquement au variables de la class Maps si celles ci sont public    
    /// </summary>
    public static class Singleton
    {
        public static Maps Instance { get; } = new Maps();
    }
}
