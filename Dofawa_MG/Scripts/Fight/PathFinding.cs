/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : PathFinding.cs, Permet de donner les coordonnées des cases formant la diagonale d'un point A à un point B.
 * Version : v1
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_FightMapItem;
using Dofawa_Chars;

namespace Dofawa_Pathfinding
{
    /// <summary>
    /// Algorhitme de Pathfinding essentiel
    /// </summary>
    public class PathFinding
    {
        public FMap map;
        public Point FP1;
        public Point FP2;
        public FCase FC1;
        public FCase FC2;
        // Vrai si on a assez de point de mouvement pour se déplacer
        public bool canBeMove = true;
        public int nbFC;
        /// <summary>
        /// Construit le pathfinding
        /// </summary>
        /// <param name="map">la map de combat</param>
        public PathFinding(FMap map)
        {
            this.map = map;
        } 
        /// <summary>
        /// Ajoute un point de séléction
        /// </summary>
        /// <param name="p">position</param>
        /// <param name="FC">Case de combat</param>
        public void AddPoint(Point p, FCase FC)
        {
            FP1 = p;
            if (FC1 != null)
            {
                FC1.isSelected = false;
            }
            FC1 = FC;
            FC1.isSelected = true;
        }
        /// <summary>
        /// Update le pathfinding
        /// </summary>
        /// <param name="player">Joueur</param>
        public void Update(Chars.Player player)
        {
            if (FC1 != null && FC2 != null)
            {
                PlayerPathFindingCases(player, FP1.X, FP1.Y, FP2.X, FP2.Y);
            }
        }
        /// <summary>
        /// Reset les vraiables du pathfinding
        /// </summary>
        public void Reset()
        {
            foreach (FCase fcase in map.map)
            {
                fcase.inCase = false;
                fcase.isActionCase = false;
            }
        }
        /// <summary>
        /// Obtient les cases de pathfinding en fonction du joueur et de deux cases
        /// </summary>
        /// <param name="player">Joueur</param>
        /// <param name="FCStartX">Position de départ X</param>
        /// <param name="FCStartY">Position de départ Y</param>
        /// <param name="FCStopX">Position de arrivé X</param>
        /// <param name="FCStopY">Position de arrivé Y</param>
        public void PlayerPathFindingCases(Chars.Player player, int FCStartX, int FCStartY, int FCStopX, int FCStopY)
        {
            List<Point> vals = PathFindingCases(FCStartX, FCStartY, FCStopX, FCStopY);
            foreach (Point p in vals)
            {
                map.map[p.X, p.Y].inCase = true;
            }
            if (nbFC > player.mouvmentPoint + 1)
            {
                canBeMove = false;
            }
            else
            {
                canBeMove = true;
            }
        }
        /// <summary>
        /// Obtient une liste de point des position des cases dans la diagonale
        /// </summary>
        /// <param name="FCStartX">Position de départ X</param>
        /// <param name="FCStartY">Position de départ Y</param>
        /// <param name="FCStopX">Position de arrivé X</param>
        /// <param name="FCStopY">Position de arrivé Y</param>
        /// <returns></returns>
        public List<Point> PathFindingCases(int FCStartX, int FCStartY, int FCStopX, int FCStopY)
        {
            List<Point> vals = new List<Point>();
            nbFC = 0;
            if (FP1 != FP2)
            {
                // 0 = BottomRight
                // 1 = TopRight
                // 2 = TopLeft
                // 3 = BottomLeft
                byte posId = 0;
                if (FP2.X >= FP1.X && FP2.Y < FP1.Y)
                {
                    posId = 1;
                }
                if (FP2.X < FP1.X && FP2.Y >= FP1.Y)
                {
                    posId = 2;
                }
                if (FP2.X < FP1.X && FP2.Y < FP1.Y)
                {
                    posId = 3;
                }
                float lx = Math.Abs(FCStopX - FCStartX)+1;
                float ly = Math.Abs(FCStopY - FCStartY)+1;
                int lastIdX = -1;
                int lastIdY = -1;
                for (int x = 0; x < lx; x++)
                {
                    for (int y = 0; y < ly; y++)
                    {
                        // Fonction permettant de déterminer si la case est dans la diagonale
                        if ((ly * x / lx >= y && ly * x / lx < y+1) || (ly * (x+1) / lx >= y && ly * (x+1) / lx < y+1))
                        {
                            nbFC++;
                            switch (posId)
                            {
                                case 0:
                                    vals.Add(new Point(x + FP1.X, y + FP1.Y));
                                    break;
                                case 1:
                                    vals.Add(new Point(x + FP1.X, FP1.Y - y));
                                    break;
                                case 2:
                                    vals.Add(new Point(FP1.X - x, y + FP1.Y));
                                    break;
                                case 3:
                                    vals.Add(new Point(FP1.X - x, FP1.Y - y));
                                    break;
                            }
                            if (lastIdX == x)
                            {
                                for (int z = lastIdY + 1; z < y; z++)
                                {
                                    nbFC++;
                                    switch (posId)
                                    {
                                        case 0:
                                            vals.Add(new Point(x + FP1.X, z + FP1.Y));
                                            break;
                                        case 1:
                                            vals.Add(new Point(x + FP1.X, FP1.Y - z));
                                            break;
                                        case 2:
                                            vals.Add(new Point(FP1.X - x, z + FP1.Y));
                                            break;
                                        case 3:
                                            vals.Add(new Point(FP1.X - x, FP1.Y - z));
                                            break;
                                    }
                                }
                            }
                            lastIdX = x;
                            lastIdY = y;
                        }
                    }
                }
                for (int y = lastIdY + FP1.Y + 1; y <= FP2.Y; y++)
                {
                    nbFC++;
                    vals.Add(new Point(FP2.X, y));
                }
                for (int y = FP2.Y; y < FP1.Y - lastIdY; y++)
                {
                    nbFC++;
                    vals.Add(new Point(FP2.X, y));
                }
            }
            else
            {
                vals.Add(new Point(FP1.X, FP1.Y));
                nbFC = 1;
            }
            return vals;
        }
    }
}
