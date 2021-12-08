/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : FightMapItem.cs, Tout le script contenant les actions du combat.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Dofawa_Draw;
using Microsoft.Xna.Framework.Input;
using Dofawa_Pathfinding;
using Dofawa_Chars;
using Dofawa_Progressbar;
using Dofawa_AllPlayerPB;
using Dofawa_Camera;
using Dofawa_Button;
using Dofawa_Keyboard;
using Dofawa_SceneManager;

namespace Dofawa_FightMapItem
{
    /// <summary>
    /// Fonctions de la map de combat
    /// </summary>
    public static class FMapFunctions
    {
        /// <summary>
        /// Initialise le combat et la map de combat
        /// </summary>
        /// <param name="enemy">Un ennemi</param>
        public static void CreateFight(Chars.Enemy enemy)
        {
            if (!Dofawa_Maps.Singleton.Instance.maps[Dofawa_Maps.Singleton.Instance.Maplevel].fightCleared)
            {
                if (Dofawa_SceneManager.SceneFunctions.GetSceneByName("Fight").id != Dofawa_SceneManager.Singleton.Instance.currentSceneId)
                {
                    Vector2 posMap = Vector2.Zero;
                    int wMax = Dofawa_Tiles.Singleton.Instance.TotalTileW - 2;
                    int hMax = Dofawa_Tiles.Singleton.Instance.TotalTileH - 5;
                    FCase[,] fCases = new FCase[wMax, hMax];
                    // Calcule le nombres de cases
                    for (int h = 0; h < hMax; h++)
                    {
                        for (int w = 0; w < wMax; w++)
                        {
                            posMap.X = Dofawa_Tiles.Singleton.Instance.TileWPx * (w + 1);
                            posMap.Y = Dofawa_Tiles.Singleton.Instance.TileHPx * (h + 1);
                            fCases[w, h] = new FCase(posMap.ToPoint(), new Point(Dofawa_Tiles.Singleton.Instance.TileWPx, Dofawa_Tiles.Singleton.Instance.TileHPx), 3);
                        }
                    }
                    new FMap(fCases, Dofawa_Chars.Singleton.Instance.players[0], enemy, new Point(5, 8), new Point(24, 8), Dofawa_Maps.Singleton.Instance.Maplevel);
                    Dofawa_SceneManager.SceneFunctions.GetSceneByName("Fight").SetToCurrent();
                }
            }
        }
        /// <summary>
        /// Crée une map de combat avec l'ennemi par défaut
        /// </summary>
        public static void CreateFight()
        {
            if (Dofawa_SceneManager.SceneFunctions.GetSceneByName("Fight").id != Dofawa_SceneManager.Singleton.Instance.currentSceneId)
            {
                CreateFight(Dofawa_Chars.Singleton.Instance.DefaultEnemy);
            }
        }
    }
    /// <summary>
    /// La map de combat
    /// </summary>
    public class FMap
    {
        public FCase[,] map;
        public PathFinding pathFinding;
        public Chars.Player player;
        public Chars.Enemy enemy;
        public Point enemyPos;
        public Point playerPos;
        public Button finishButton;
        public KeyboardAction actionKBA;
        public string mapId;
        // true = tour du joueur, false = tour de l'ennemi
        public bool turn = true;
        // Vrai si il faut montrer les cases d'actions sinon faux
        public bool showActionCases = false;
        // Toutes les couleurs des cases
        public Color[] FCColors = new Color[]
        {
            new Color(255,255,255,20),
            new Color(255,255,255,100),
            new Color(101,174,53,255),
            new Color(234,65,65,255),
            new Color(80,126,213,130),
            new Color(101,174,53,114),
            new Color(235,30,30,114),
        };

        /// <summary>
        /// Construit la map de combat
        /// </summary>
        /// <param name="map">les cases de combat</param>
        /// <param name="player">le joueur</param>
        /// <param name="enemy">l'ennemi</param>
        /// <param name="playerStartPos">la position de départ du joueur</param>
        /// <param name="enemyStartPos">la position de départ de l'ennemi</param>
        /// <param name="mapId">l'id de la map</param>
        public FMap(FCase[,] map, Chars.Player player, Chars.Enemy enemy, Point playerStartPos, Point enemyStartPos, string mapId)
        {
            this.map = map;
            this.player = player;
            enemyPos = enemyStartPos;
            playerPos = playerStartPos;
            this.enemy = enemy;
            this.mapId = mapId;

            // Crée le pathfinding
            pathFinding = new PathFinding(this);
            pathFinding.AddPoint(playerStartPos, map[playerStartPos.X, playerStartPos.Y]);
            
            Dofawa_FightMapItem.Singleton.Instance.fmap = this;
            Texture2D finishButtonText = Dofawa_FightMapItem.Singleton.Instance.fightUI[2];
            finishButton = new Button(DrawFunctions.AlignForm(Alignment.CenterBottom, new Point((int)(finishButtonText.Width / 2.5), (int)(finishButtonText.Height / 2.5)).ToVector2(), new Point((int)Dofawa_Settings.SettingsFunctions.GetCenterOfScreen().X, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 45).ToVector2()).ToPoint(), new Point((int)(finishButtonText.Width / 2.5), (int)(finishButtonText.Height / 2.5)), ChangeTurn, finishButtonText);
            
            // Crée une action de clavier A pour pour attaquer
            actionKBA = new KeyboardAction(Keys.A, ShowActionCases, SceneFunctions.GetSceneByName("Fight"));
            player.mouvmentPoint = player.maxmouvmentPoint;
            player.actionPoint = player.maxactionPoint;
            player.sprite = Dofawa_Tiles.Singleton.Instance.charImages[1];
        }
        /// <summary>
        /// Inverse le booleen qui indique si il faut montrer les cases d'actions
        /// </summary>
        public void ShowActionCases()
        {
            showActionCases = !showActionCases;
        }
        /// <summary>
        /// Dessine la map de combat
        /// </summary>
        /// <param name="font">La police d'écriture</param>
        /// <param name="_spriteBatch">le SpriteBatch du jeu</param>
        /// <param name="graphicsDeviceManager">le Manager graphique du jeu</param>
        /// <param name="camera">La camera</param>
        public void Draw(SpriteFont font, SpriteBatch _spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera camera)
        {
            // BlendState.NonPremultiplied utilisé pour le channel Alpha
            // SamplerState.PointClamp utilisé pour éviter les algorhitmes d'agrandissement d'image
            // camera.ViewMatix utilisé pour modifier la camera si besoin
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    // -------
                    // Attribue la couleur
                    FCase fcase = map[x, y];
                    Color color = FCColors[0];
                    if (fcase.isSelected)
                    {
                        color = FCColors[0];
                    }
                    else if (fcase.isHovered)
                    {
                        if (showActionCases)
                        {
                            if (fcase.isActionCase)
                            {
                                color = FCColors[5];
                            }
                            else
                            {
                                color = FCColors[6];
                            }
                        }
                        else
                        {
                            color = FCColors[1];
                        }
                    }
                    else if (fcase.inCase && !showActionCases)
                    {
                        if (pathFinding.canBeMove)
                        {
                            color = FCColors[2];
                        }
                        else
                        {
                            color = FCColors[3];
                        }
                    }
                    else if (fcase.isActionCase)
                    {
                        color = FCColors[4];
                    }

                    // -------
                    // Dessine la case

                    if (fcase.isSelected)
                    {
                        if (color != FCColors[0])
                        {
                            fcase.Draw(_spriteBatch, graphicsDeviceManager, color);
                        }
                        _spriteBatch.Draw(player.sprite, fcase.Rect, Color.White);
                    }
                    else if (new Point(x, y) == enemyPos)
                    {
                        if (color != FCColors[0])
                        {
                            fcase.Draw(_spriteBatch, graphicsDeviceManager, color);
                        }
                        _spriteBatch.Draw(enemy.sprite, fcase.Rect, Color.White);
                    }
                    else
                    {
                        fcase.Draw(_spriteBatch, graphicsDeviceManager, color);
                    }
                }
            }
            _spriteBatch.End();

            // Dessine mais cette fois sans SamplerState.PointClamp pour éviter une mauvaise qualité
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, null, null, camera.ViewMatrix);

            Texture2D star = Dofawa_FightMapItem.Singleton.Instance.fightUI[0];
            Texture2D starpm = Dofawa_FightMapItem.Singleton.Instance.fightUI[1];

            Rectangle pmpos = new Rectangle(DrawFunctions.AlignForm(Alignment.BottomLeft, new Point(starpm.Width / 4, starpm.Height / 4).ToVector2(), new Point(300, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 50).ToVector2()).ToPoint(), new Point(starpm.Width / 4, starpm.Height / 4));
            Rectangle papos = new Rectangle(DrawFunctions.AlignForm(Alignment.BottomLeft, new Point(star.Width / 4, star.Height / 4).ToVector2(), new Point(850, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 50).ToVector2()).ToPoint(), new Point(star.Width / 4, star.Height / 4));

            // Dessine l'étoile des points de mouvements
            _spriteBatch.Draw(star, papos, Color.White);
            _spriteBatch.DrawString(font, player.actionPoint.ToString(), DrawFunctions.AlignForm(Alignment.Center, font.MeasureString(player.actionPoint.ToString()), new Vector2(papos.X, papos.Y) + (papos.Size.ToVector2() / 2).ToPoint().ToVector2() + new Vector2(1, 8)), Color.White);

            // Dessine l'écoile des point d'actions
            _spriteBatch.Draw(starpm, pmpos, Color.White);
            _spriteBatch.DrawString(font, player.mouvmentPoint.ToString(), DrawFunctions.AlignForm(Alignment.Center, font.MeasureString(player.mouvmentPoint.ToString()), new Vector2(pmpos.X, pmpos.Y) + (pmpos.Size.ToVector2() / 2).ToPoint().ToVector2() + new Vector2(1, 3)), Color.White);

            // Dessime le boutton
            finishButton.Draw(_spriteBatch);

            ProgressBar lifePB = Dofawa_AllPlayerPB.Singleton.Instance.lifePBDict[player].lifePB.progressBar;

            // Dessine la vie du joueur
            lifePB.Draw(graphicsDeviceManager, _spriteBatch, new Percent(0, player.maxPv, player.pv), DrawFunctions.AlignForm(Alignment.Center, new Vector2(225, 30), new Vector2(155, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 80)).ToPoint(), new Point(225, 30), Color.Black, 5);
            _spriteBatch.DrawString(Dofawa_Fonts.Singleton.Instance.fonts[3], "Joueur", DrawFunctions.AlignForm(Alignment.Center, Dofawa_Fonts.Singleton.Instance.fonts[3].MeasureString("Joueur"), new Vector2(155, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 80)) - new Vector2(0, 50), Color.White);

            // Dessine la vie de l'ennemi
            if (enemy.isBoss)
            {
                lifePB.Draw(graphicsDeviceManager, _spriteBatch, new Percent(0, enemy.maxPv, enemy.pv), DrawFunctions.AlignForm(Alignment.Center, new Vector2(225, 30), new Vector2(Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth - 155, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 80)).ToPoint(), new Point(225, 30), Color.Purple, Color.Black, 5);
                _spriteBatch.DrawString(Dofawa_Fonts.Singleton.Instance.fonts[3], "Boss", DrawFunctions.AlignForm(Alignment.Center, Dofawa_Fonts.Singleton.Instance.fonts[3].MeasureString("Boss"), new Vector2(Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth - 155, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 80)) - new Vector2(0, 50), Color.Gold);

            }
            else
            {
                lifePB.Draw(graphicsDeviceManager, _spriteBatch, new Percent(0, enemy.maxPv, enemy.pv), DrawFunctions.AlignForm(Alignment.Center, new Vector2(225, 30), new Vector2(Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth - 155, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 80)).ToPoint(), new Point(225, 30), Color.Black, 5);
                _spriteBatch.DrawString(Dofawa_Fonts.Singleton.Instance.fonts[3], "Ennemi", DrawFunctions.AlignForm(Alignment.Center, Dofawa_Fonts.Singleton.Instance.fonts[3].MeasureString("Ennemi"), new Vector2(Dofawa_Settings.Singleton.Instance.PreferredBackBufferWidth - 155, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight - 80)) - new Vector2(0, 50), Color.White);

            }

            _spriteBatch.End();
        }
        /// <summary>
        /// Fonction qui inverse le booleen du tour
        /// </summary>
        public void ChangeTurn()
        {
            turn = !turn;
        }
        /// <summary>
        /// Fonction qui modifie le tour de l'ennemi
        /// </summary>
        public void ChangeEnemyPos()
        {
            Random rand = new Random();
            int radius = enemy.maxmouvmentPoint;
            Point randMove;
            int totalEnemyReach = enemy.maxmouvmentPoint + enemy.reach;
            int mostFarOfPlayer = -1;
            List<Point> mostFarOfPlayerPos = new List<Point>();

            // break si la position de l'ennemi n'est pas celle du joueur
            while (true)
            {
                // Si l'ennemi à un moyen d'attaquer le joueur alors on rentre dans le if
                if (CheckPlayerReachableByEnemy(totalEnemyReach, enemyPos))
                {
                    // fait une boucle de deuxième dimention sphérique
                    for (int x = enemy.maxmouvmentPoint; x >= -enemy.maxmouvmentPoint; x--)
                    {
                        for (int y = enemy.maxmouvmentPoint; y >= -enemy.maxmouvmentPoint; y--)
                        {
                            Point movementPos = new Point(x, y);
                            Point EnemyPosAndMouvementPos = enemyPos + movementPos;
                            // Si la position reste dans la map
                            if (EnemyPosAndMouvementPos.X <= map.GetLength(0) - 1 && EnemyPosAndMouvementPos.X > -1
                            && EnemyPosAndMouvementPos.Y <= map.GetLength(1) - 1 && EnemyPosAndMouvementPos.Y > -1)
                            {
                                // Si la position est dans la sphère
                                if (Math.Abs(movementPos.X) <= enemy.maxmouvmentPoint - Math.Abs(movementPos.Y)
                                && Math.Abs(movementPos.Y) <= enemy.maxmouvmentPoint - Math.Abs(movementPos.X))
                                {
                                    // Si en utilisant juste la portée de l'ennemi on arrive a toucher le joueur
                                    if (CheckPlayerReachableByEnemy(enemy.reach, enemyPos + new Point(x, y)))
                                    {
                                        int tmp_mostFarOfPlayer = Math.Abs(EnemyPosAndMouvementPos.X - playerPos.X) + Math.Abs(EnemyPosAndMouvementPos.Y - playerPos.Y);
                                        // Si la distance de l'ennemi est plus lointaine qu'avant
                                        if (tmp_mostFarOfPlayer > mostFarOfPlayer)
                                        {
                                            // On supprime les variables stoquées dans la liste
                                            mostFarOfPlayerPos.Clear();
                                            mostFarOfPlayer = tmp_mostFarOfPlayer;
                                            mostFarOfPlayerPos.Add(enemyPos + new Point(x, y));
                                        }
                                        else if (tmp_mostFarOfPlayer == mostFarOfPlayer)
                                        {
                                            // On ajoute la position possible de l'ennemi dans la liste
                                            mostFarOfPlayerPos.Add(enemyPos + new Point(x, y));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // On tire une des position possibles de l'ennemi de la liste
                    Point newEnemyPos = mostFarOfPlayerPos[rand.Next(0, mostFarOfPlayerPos.Count)];
                    radius -= Math.Abs(newEnemyPos.X - enemyPos.X) + Math.Abs(newEnemyPos.Y - enemyPos.Y);
                    enemyPos = newEnemyPos;
                    int actionPoint = enemy.actionPoint;

                    // Attque le joueur
                    while (actionPoint >= enemy.attackPrice)
                    {
                        player.pv -= enemy.atk;
                        actionPoint -= enemy.attackPrice;
                    }
                }
                if (enemyPos != playerPos)
                {
                    break;
                }
            }
            // Dépence le reste de ses points de mouvement aléatoirement
            while (true)
            {
                randMove = new Point(rand.Next(-radius, radius + 1), rand.Next(-radius, radius + 1));
                if (Math.Abs(randMove.X) <= radius - Math.Abs(randMove.Y)
                    && Math.Abs(randMove.Y) <= radius - Math.Abs(randMove.X)
                    && enemyPos.X + randMove.X <= map.GetLength(0) - 1 && enemyPos.X + randMove.X > -1
                    && enemyPos.Y + randMove.Y <= map.GetLength(1) - 1 && enemyPos.Y + randMove.Y > -1
                    && enemyPos + randMove != playerPos)
                {
                    break;
                }
            }
            enemyPos += randMove;
        }
        /// <summary>
        /// Verifie si le joueur peut être touché par la portée de l'ennemi
        /// </summary>
        /// <param name="totalEnemyReach">portée</param>
        /// <param name="enemyPos">Position de l'ennemi</param>
        /// <returns></returns>
        public bool CheckPlayerReachableByEnemy(int totalEnemyReach, Point enemyPos)
        {
            //Distance de l'ennemi face au joueur
            Point dt = new Point(Math.Abs(playerPos.X - enemyPos.X), Math.Abs(playerPos.Y - enemyPos.Y));
            if (Math.Abs(dt.X) <= totalEnemyReach - Math.Abs(dt.Y)
                && Math.Abs(dt.Y) <= totalEnemyReach - Math.Abs(dt.X))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Update la map de combat
        /// </summary>
        public void Update()
        {

            pathFinding.Reset();
            finishButton.Update(SceneFunctions.GetSceneByName("Fight").screenMouseActions.camera);
            if (!turn)
            {
                // Tour de l'ennemi
                ChangeEnemyPos();
                ChangeTurn();
                player.mouvmentPoint = player.maxmouvmentPoint;
                player.actionPoint = player.maxactionPoint;
            }
            else
            {
                // Tour du joueur
                MouseState ms = Mouse.GetState();
                Point mp = new Point(ms.X, ms.Y);
                if (showActionCases)
                {
                    // Sphère d'action
                    for (int x = player.reach; x >= -player.reach; x--)
                    {
                        for (int y = player.reach; y >= -player.reach; y--)
                        {
                            if (Math.Abs(x) <= player.reach - Math.Abs(y)
                                && Math.Abs(y) <= player.reach - Math.Abs(x)
                                && playerPos.X + x <= map.GetLength(0) - 1 && playerPos.X + x > -1
                                && playerPos.Y + y <= map.GetLength(1) - 1 && playerPos.Y + y > -1)
                            {
                                map[playerPos.X + x, playerPos.Y + y].isActionCase = true;
                            }
                        }
                    }
                }
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        // Déplacement
                        FCase item = map[x, y];
                        if (map[x, y].Clicked(mp, ms))
                        {
                            if (!item.isClicked && !showActionCases && pathFinding.canBeMove
                                && new Point(x, y) != enemyPos
                                && new Point(x, y) != playerPos)
                            {
                                item.isClicked = true;
                                pathFinding.AddPoint(new Point(x, y), map[x, y]);
                                playerPos = new Point(x, y);
                                player.mouvmentPoint -= pathFinding.nbFC - 1;
                            }
                            else if (!item.isClicked && showActionCases && map[x, y].isActionCase)
                            {
                                item.isClicked = true;
                                if (player.actionPoint - player.attackPrice >= 0)
                                {
                                    player.actionPoint -= player.attackPrice;
                                    if (new Point(x, y) == enemyPos)
                                    {
                                        enemy.pv -= player.atk;
                                    }
                                }
                            }
                        }
                        else
                        {
                            item.isClicked = false;
                        }
                        if (map[x, y].Hover(mp))
                        {
                            pathFinding.FC2 = map[x, y];
                            pathFinding.FP2 = new Point(x, y);
                        }
                    }
                }
                // Update la diagonale de cases entre A et B
                pathFinding.Update(Dofawa_Chars.Singleton.Instance.players[0]);
            }
            // Si le combat est fini
            if (enemy.pv <= 0)
            {
                enemy.KillEnemy();
                KeyboardFunction.RemoveKeyboardAction(actionKBA);
                Dofawa_Maps.Singleton.Instance.maps[mapId].nbFightCleared += 1;
                if (Dofawa_Maps.Singleton.Instance.maps[mapId].nbFightCleared == 3)
                {
                    Dofawa_Maps.Singleton.Instance.maps[mapId].fightCleared = true;
                }
                Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game").SetToCurrent();
                player.actionPoint = player.maxactionPoint;
                player.mouvmentPoint = player.maxmouvmentPoint;
                player.pv = player.maxPv;
                player.level.xp += (int)(enemy.xp * player.xpDropFactor);
                Random rand = new Random();
                // 1 chance sur 5
                if (rand.Next(0,5) == 0 || enemy.isBoss)
                {
                    player.level.skillPoint += 1;
                }
            }
        }
    }
    /// <summary>
    /// Cases de combat
    /// </summary>
    public class FCase
    {
        public Point pos;
        public Point size;
        // Vrai si la case est survolée
        public bool isHovered;
        // Vrai si la case est dans la diagonale entre A et B
        public bool inCase;
        // Vrai si la case est cliqué
        public bool isClicked;
        // Vrai si la case est séléctionné ( Par défaut c'est le joueur)
        public bool isSelected;
        // Vrai si la case est dans la sphère d'action et que la sphère d'action est dessinable
        public bool isActionCase;
        public int border;
        /// <summary>
        /// Rectangle de la case
        /// </summary>
        public Rectangle Rect
        {
            get => new Rectangle(pos + new Point(3), size - new Point(6));
        }
        /// <summary>
        /// Constructeur de la case de combat
        /// </summary>
        /// <param name="pos">position</param>
        /// <param name="size">taille</param>
        /// <param name="border">bordure</param>
        public FCase(Point pos, Point size, int border)
        {
            this.pos = pos;
            this.size = size;
            this.border = border;
        }
        /// <summary>
        /// Dessine la case de combat
        /// </summary>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="graphicsDeviceManager">Manageur graphique du jeu</param>
        /// <param name="color">Couleur de la case</param>
        public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Color color)
        {
            Forms.DrawRectangle(Rect, color, graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
        }
        /// <summary>
        /// Verifie si la case est survolée
        /// </summary>
        /// <param name="mp">position de la souris</param>
        /// <returns>Vrai si elle est survolée</returns>
        public bool Hover(Point mp)
        {
            if (Rect.Contains(mp))
            {
                isHovered = true;
                return true;
            }
            else
            {
                isHovered = false;
                return false;
            }
        }
        /// <summary>
        /// Verifie si la case est cliqué
        /// </summary>
        /// <param name="mp">Position de la souris</param>
        /// <param name="ms">état de la souris</param>
        /// <returns></returns>
        public bool Clicked(Point mp, MouseState ms)
        {
            if (Rect.Contains(mp) && ms.LeftButton == ButtonState.Pressed && !Dofawa_Mouse.Singleton.Instance.isHolded)
                return true;
            return false;
        }
    }
    /// <summary>
    /// Super globales de FightMapItem
    /// </summary>
    public class FMapItemVars
    {
        public List<Texture2D> fightUI = new List<Texture2D>();
        public FMap fmap;
    }
    /// <summary>
    /// Crée l'instance des Super globales
    /// </summary>
    public static class Singleton
    {
        public static FMapItemVars Instance { get; } = new FMapItemVars();
    }
}
