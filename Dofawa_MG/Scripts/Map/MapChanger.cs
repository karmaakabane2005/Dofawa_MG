/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : MapChanger.cs, Fonctions permettant de changer de salle.
 * Version : v1
 */
using System;
using System.Collections.Generic;
using System.Text;
using Dofawa_Door;

namespace Dofawa_MapChanger
{
    /// <summary>
    /// Enumérateur du sens de la map
    /// </summary>
    public enum MapSide
    {
        Up,
        Right,
        Down,
        Left
    }
    /// <summary>
    /// Fonctions utiles pour la map
    /// </summary>
    public static class MapChanger
    {
        /// <summary>
        /// Obtient l'id de map principale
        /// </summary>
        /// <param name="mapId">mapId</param>
        /// <returns>id de map principale</returns>
        public static int GetMainMapId(string mapId)
        {
            if (mapId.Contains('-'))
            {
                int iof = mapId.IndexOf('-');
                return int.Parse(mapId.Substring(0, iof));
            }
            return int.Parse(mapId);
        }
        /// <summary>
        /// Crée une map
        /// </summary>
        /// <param name="mapId">id de la map</param>
        /// <param name="width">largeur</param>
        /// <param name="height">hauteur</param>
        /// <param name="doorUp">porte du haut</param>
        /// <param name="doorRight">porte de droite</param>
        /// <param name="doorDown">porte du bas</param>
        /// <param name="doorLeft">porte de gauche</param>
        public static void CreateMap(string mapId, int width = 32, int height = 18, bool doorUp = false, bool doorRight = true, bool doorDown = false, bool doorLeft = false)
        {
            Dofawa_Maps.Singleton.Instance.maps.Add(mapId, Dofawa_Maps.Singleton.Instance.createDynamicMap(mapId, width, height, doorUp, doorRight, doorDown, doorLeft));
            DoorFunctions.CreateLockedDoors(mapId, doorUp, doorRight, doorDown, doorLeft);
        }
        /// <summary>
        /// Change de map
        /// </summary>
        /// <param name="mapId">Nouvelle map</param>
        public static void ChangeMap(string mapId)
        {
            Dofawa_Maps.Singleton.Instance.Maplevel = mapId;
        }
        /// <summary>
        /// Verifie si la map a déjà été crée
        /// </summary>
        /// <param name="mapId">map a tester</param>
        /// <returns>Vrai si elle est déjà crée</returns>
        public static bool isMapAlreadyCreated(string mapId)
        {
            bool res = Dofawa_Maps.Singleton.Instance.maps.ContainsKey(mapId);
            return res;
        }
        /// <summary>
        /// Obtient l'id de map secondaire
        /// </summary>
        /// <param name="mapId">id de la map</param>
        /// <returns>id de pas secondaire</returns>
        public static int GetSubMapId(string mapId)
        {
            if (mapId.Contains('-'))
            {
                int iof = mapId.IndexOf('-');
                return int.Parse(mapId.Substring(iof + 1, mapId.Length - iof - 2));
            }
            return 0;
        }
        /// <summary>
        /// Obtient l'indicateur de map (U ou B)
        /// </summary>
        /// <param name="mapId">id de la map</param>
        /// <returns>"U" ou "B"</returns>
        public static string GetUpDownMapIndicator(string mapId)
        {
            if (mapId.Contains('-'))
            {
                return mapId.Substring(mapId.Length - 1);
            }
            return "";
        }
        /// <summary>
        /// Obtient la prochaine id de map en fonction de l'id de la map et du sens
        /// </summary>
        /// <param name="currentMapId">id de la map current</param>
        /// <param name="side">sens</param>
        /// <returns>id de la map prochaine en fonction du sens</returns>
        public static string GetNextMapIdBySide(string currentMapId, MapSide side)
        {
            int maplv = GetMainMapId(currentMapId);
            int submaplv = GetSubMapId(currentMapId);
            string upDown = GetUpDownMapIndicator(currentMapId);
            switch (side)
            {
                case MapSide.Up:
                    if (upDown == "U")
                    {
                        return maplv.ToString() + "-" + (submaplv + 1).ToString() + upDown;
                    }
                    else if (upDown == "B")
                    {
                        return maplv.ToString() + (submaplv == 1 ? "" : "-" + (submaplv - 1).ToString() + upDown);
                    }
                    else
                    {
                        return maplv.ToString() + "-1U";
                    }
                case MapSide.Down:
                    if (upDown == "B")
                    {
                        return maplv.ToString() + "-" + (submaplv + 1).ToString() + upDown;
                    }
                    else if (upDown == "U")
                    {
                        return maplv.ToString() + (submaplv == 1 ? "" : "-" + (submaplv - 1).ToString() + upDown);
                    }
                    else
                    {
                        return maplv.ToString() + "-1B";
                    }
                case MapSide.Left:
                    return (maplv - 1).ToString() + (upDown.Length > 0 ? "-" + submaplv + upDown : "") ;
                case MapSide.Right:
                    return (maplv + 1).ToString() + (upDown.Length > 0 ? "-" + submaplv + upDown : "");
                default:
                    return "0";
            }
        }
    }
}
