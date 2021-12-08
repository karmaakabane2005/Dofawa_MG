/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Chars.cs, Classes des joueurs et ennemis.
 * Version : v1
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Dofawa_Keyboard;
using Dofawa_Level;
using Dofawa_Collision;
using Dofawa_Maps;
using Dofawa_Inventory;
using Dofawa_SkillTree;

namespace Dofawa_Chars
{
    public class CharsList
    {
        public List<Chars.Enemy> enemys = new List<Chars.Enemy>();
        public List<Chars.Ally> ally = new List<Chars.Ally>();
        public List<Chars.Player> players = new List<Chars.Player>();
        public Chars.Enemy DefaultEnemy
        {
            get
            {
                Random rand = new Random();
                return new Chars.Enemy(
                Vector2.Zero, // pos
                0, // roomindex
                Dofawa_Tiles.Singleton.Instance.enemyImages[rand.Next(0, Dofawa_Tiles.Singleton.Instance.enemyImages.Count)], // sprite
                0, // speed
                "Default", // name
                (int)(100f * (Dofawa_MapChanger.MapChanger.GetMainMapId(Dofawa_Maps.Singleton.Instance.Maplevel) / 6f + 1)), //basepv
                (int)(15f * (Dofawa_MapChanger.MapChanger.GetMainMapId(Dofawa_Maps.Singleton.Instance.Maplevel) /6f + 1)), //baseatk
                1, //size
                rand.Next(2,5), // mouvmentpoint
                rand.Next(3,7), // actionpoint
                rand.Next(4,7), // reach
                3, // attackPrice
                false, // isSpawned
                false, // isDied
                (rand.Next(0,15) == 0 ? true : false), // isBoss
                (int)(4f * (Dofawa_MapChanger.MapChanger.GetMainMapId(Dofawa_Maps.Singleton.Instance.Maplevel) / 1.5f + 1)) // xp
                );
            }
        }
    }
    public static class Chars
    {
        /// <summary>
        /// Equipement du joueur
        /// </summary>
        public class Equipment
        {
            public Inventory.Items casque;
            public Inventory.Items plastron;
            public Inventory.Items jambieres;
            public Inventory.Items bottes;
            /// <summary>
            /// Construit l'équipement du joueur
            /// </summary>
            /// <param name="casque">le casque</param>
            /// <param name="plastron">la plastron</param>
            /// <param name="jambieres">les jambières</param>
            /// <param name="bottes">les bottes</param>
            public Equipment(Inventory.Items casque, Inventory.Items plastron, Inventory.Items jambieres, Inventory.Items bottes)
            {
                this.casque = casque;
                this.plastron = plastron;
                this.jambieres = jambieres;
                this.bottes = bottes;
            }
            /// <summary>
            /// Verifie si l'item est équipé
            /// </summary>
            /// <param name="item">l'item</param>
            /// <returns>Vrai si il est équipé sinon faux</returns>
            public bool isEquip(Inventory.Items item)
            {
                if (item == casque || item == plastron || item == jambieres || item == bottes)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Squelette de base un Character
        /// </summary>
        public class Char
        {
            public Texture2D sprite;
            public string name;
            public Vector2 pos;
            public int speed;
            public int pv;
            public int maxPv;
            public int atk;
            public int maxAtk;
            public int baseatk;
            public int basepv;
            public int size;
            // Vrai si le character est sur le jeu
            public bool isSpawned;
            // Vrai si le character est mort
            public bool isDied;
            public int roomIndex;
            public int maxmouvmentPoint;
            public int mouvmentPoint;
            public int actionPoint;
            public int maxactionPoint;
            public int reach;
            public int attackPrice;
            public int bonuspv;
            public int bonusatk;
            public int bonuspm;
            public int bonuspa;
            public int basepm;
            public int basepa;
            public int maxpm;
            public int maxpa;
            /// <summary>
            /// Crée un character
            /// </summary>
            /// <param name="pos">La position</param>
            /// <param name="roomIndex">L'index de la salle</param>
            /// <param name="sprite">L'image</param>
            /// <param name="speed">La vitesse</param>
            /// <param name="name">Le nom</param>
            /// <param name="basepv">Les pv de base</param>
            /// <param name="baseatk">L'atk de base</param>
            /// <param name="size">La taille</param>
            /// <param name="mouvmentPoint">Les points de mouvements</param>
            /// <param name="actionPoint">Les points d'action</param>
            /// <param name="reach">Les points de viséé</param>
            /// <param name="attackPrice">Les points d'attaque</param>
            /// <param name="isSpawned">Vrai si il est dans le jeu</param>
            /// <param name="isDied">Vrai si il est mort</param>
            public Char(Vector2 pos, int roomIndex, Texture2D sprite, int speed, string name, int basepv, int baseatk, int size, int mouvmentPoint = 6, int actionPoint = 6, int reach = 6, int attackPrice = 3, bool isSpawned = true, bool isDied = false)
            {
                this.name = name;
                this.pos = pos;
                this.sprite = sprite;
                this.speed = speed;
                this.roomIndex = roomIndex;
                this.basepv = basepv;
                this.baseatk = baseatk;
                basepm = mouvmentPoint;
                basepa = actionPoint;
                this.size = size;
                this.isSpawned = isSpawned;
                this.isDied = isDied;
                maxmouvmentPoint = mouvmentPoint;
                this.mouvmentPoint = mouvmentPoint;
                maxactionPoint = actionPoint;
                this.actionPoint = actionPoint;
                this.reach = reach;
                this.attackPrice = attackPrice;
                maxPv = basepv;
                maxAtk = baseatk;
                atk = baseatk;
                pv = basepv;
            }
            /// <summary>
            /// Obtient le rectangle de collision
            /// </summary>
            public Rectangle collideBox
            {
                get => new Rectangle((int)pos.X, (int)pos.Y, Dofawa_Tiles.Singleton.Instance.TileWPx * size, Dofawa_Tiles.Singleton.Instance.TileHPx * size);
            }
            /// <summary>
            /// Move character to up
            /// </summary>
            /// <param name="px">The number of px the character will move</param>
            public void MoveUp(int px)
            {
                pos.Y -= px * Dofawa_Time.Singleton.Instance.deltaTime;
            }
            /// <summary>
            /// Move character to up
            /// </summary>
            /// <param name="px">The number of px the character will move</param>
            public void MoveRight(int px)
            {
                pos.X += px * Dofawa_Time.Singleton.Instance.deltaTime;
            }
            /// <summary>
            /// Move character to up
            /// </summary>
            /// <param name="px">The number of px the character will move</param>
            public void MoveDown(int px)
            {
                pos.Y += px * Dofawa_Time.Singleton.Instance.deltaTime;
            }
            /// <summary>
            /// Move character to up
            /// </summary>
            /// <param name="px">The number of px the character will move</param>
            public void MoveLeft(int px)
            {
                pos.X -= px * Dofawa_Time.Singleton.Instance.deltaTime;
            }
            /// <summary>
            /// This is the base draw for all the characters types
            /// </summary>
            /// <param name="_spriteBatch">The sprtie batch for draw the character</param>
            public void BaseDraw(SpriteBatch _spriteBatch)
            {
                _spriteBatch.Draw(this.sprite, pos, null, Color.White, 0, Vector2.Zero, size * 5, SpriteEffects.None, 0);
            }
        }
        public class Player : Char
        {
            public bool isControlable;
            public int playerIndex;
            public Level level;
            public float xpDropFactor = 1f;
            public KeyboardAction kbaUp = new KeyboardAction(Dofawa_Settings.Singleton.Instance.moveSettings.up,null, Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game"));
            public KeyboardAction kbaRight = new KeyboardAction(Dofawa_Settings.Singleton.Instance.moveSettings.right, null, Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game"));
            public KeyboardAction kbaDown = new KeyboardAction(Dofawa_Settings.Singleton.Instance.moveSettings.down, null, Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game"));
            public KeyboardAction kbaLeft = new KeyboardAction(Dofawa_Settings.Singleton.Instance.moveSettings.left, null, Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game"));
            public Equipment equipement;
            public SkillTree skillTree;

            /// <summary>
            /// Create a player
            /// </summary>
            /// <param name="pos">The position of pixel of the player</param>
            /// <param name="roomIndex">The index of room who the player is</param>
            /// <param name="sprite">The sprite of the player</param>
            /// <param name="name">The name of the player</param>
            /// <param name="basepv">The pv of player</param>
            /// <param name="baseatk">The attack damage of player</param>
            /// <param name="size">The size of the player</param>
            /// <param name="isSpawned">True if the player is spawned</param>
            /// <param name="isControlable">True if the player can be controlable by user</param>
            /// <param name="playerIndex">The index of player</param>
            /// <param name="isDied">True if the player is dead</param>
            /// <param name="level">The level + xp of the player</param>
            public Player(Vector2 pos, int roomIndex, Texture2D sprite, Level level, Equipment equipement, SkillTree skillTree, int speed = 300, int mouvmentPoint = 6, int actionPoint = 6, int reach = 6, int attackPrice = 3, string name = "Player", int basepv = 100, int baseatk = 10, int size = 1, bool isSpawned = true, bool isControlable = true, int playerIndex = 0, bool isDied = false) : base(pos, roomIndex, sprite, speed, name, basepv, baseatk, size, mouvmentPoint, actionPoint, reach, attackPrice, isSpawned, isDied)
            {
                this.isControlable = isControlable;
                this.playerIndex = playerIndex;
                this.level = level;
                this.equipement = equipement;
                this.skillTree = skillTree;
                UpdateStats();
                pv = maxPv;
                atk = maxAtk;
                mouvmentPoint = maxpm;
                actionPoint = maxpa;
            }
            /// <summary>
            /// Call this function in your global draw function for draw the character
            /// </summary>
            /// <param name="_spriteBatch">The sprite batch for draw the character</param>
            public void Draw(SpriteBatch _spriteBatch)
            {
                BaseDraw(_spriteBatch);
            }
            /// <summary>
            /// Update le joueur
            /// </summary>
            public void Update()
            {
                Vector2 previousPos = pos;
                UpdatePlayerForKeyboradEvents();
                List<Maps.MapItem> elementsCollided = Collision.ElementCollided(collideBox);
                if (Collision.isCollidedByAny(elementsCollided))
                {
                    pos = previousPos;
                    // Check which action the player need to do with the previous pos
                    elementsCollided = Collision.ElementCollided(collideBox);
                    foreach (Maps.MapItem item in elementsCollided)
                    {
                        item.eventOnCollide?.Invoke();
                    }
                }
                else
                {
                    foreach (Maps.MapItem item in elementsCollided)
                    {
                        item.eventOnCollide?.Invoke();
                    }
                }
                Dofawa_Door.Singleton.Instance.alreadyChange = false;
                UpdateStats();
                VerifyPv();
                level.VerifyLevel();
            }
            /// <summary>
            /// Change les statisitiques maximales du joueur
            /// </summary>
            public void UpdateStats()
            {
                maxPv = basepv + bonuspv + (equipement.casque != null ? equipement.casque.stats.pv : 0) + (equipement.plastron != null ? equipement.plastron.stats.pv : 0) + (equipement.jambieres != null ? equipement.jambieres.stats.pv : 0) + (equipement.bottes != null ? equipement.bottes.stats.pv : 0);
                maxAtk = baseatk + bonusatk + (equipement.casque != null ? equipement.casque.stats.atk : 0) + (equipement.plastron != null ? equipement.plastron.stats.atk : 0) + (equipement.jambieres != null ? equipement.jambieres.stats.atk : 0) + (equipement.bottes != null ? equipement.bottes.stats.atk : 0);
                maxmouvmentPoint = basepm + bonuspm + (equipement.casque != null ? equipement.casque.stats.pm : 0) + (equipement.plastron != null ? equipement.plastron.stats.pm : 0) + (equipement.jambieres != null ? equipement.jambieres.stats.pm : 0) + (equipement.bottes != null ? equipement.bottes.stats.pm : 0);
                maxactionPoint = basepa + bonuspa + (equipement.casque != null ? equipement.casque.stats.pa : 0) + (equipement.plastron != null ? equipement.plastron.stats.pa : 0) + (equipement.jambieres != null ? equipement.jambieres.stats.pa : 0) + (equipement.bottes != null ? equipement.bottes.stats.pa : 0);

            }
            /// <summary>
            /// Verifie la vie du joueur
            /// </summary>
            public void VerifyPv()
            {
                if (pv <= 0)
                {
                    isDied = true;
                    Dofawa_SceneManager.SceneFunctions.GetSceneByName("Dead").SetToCurrent();
                }
                else if (pv > maxPv)
                {
                    pv = maxPv;
                }
            }
            /// <summary>
            /// Update les actions de clavier du joueur
            /// </summary>
            public void UpdatePlayerForKeyboradEvents()
            {
                GameKeyboard.GetState();
                if (kbaUp.IsHold)
                {
                    MoveUp(speed);
                }
                if (kbaRight.IsHold)
                {
                    MoveRight(speed);
                }
                if (kbaDown.IsHold)
                {
                    MoveDown(speed);
                }
                if (kbaLeft.IsHold)
                {
                    MoveLeft(speed);
                }
            }
        }
        /// <summary>
        /// Ennemi
        /// </summary>
        public class Enemy : Char
        {
            public bool isBoss;
            public int xp;
            /// <summary>
            /// Create a enemy
            /// </summary>
            /// <param name="pos">The position of pixel of the character</param>
            /// <param name="roomIndex">The index of room who the character is</param>
            /// <param name="sprite">The sprite of the character</param>
            /// <param name="name">The name of the character</param>
            /// <param name="basepv">The pv of character</param>
            /// <param name="baseatk">The attack damage of character</param>
            /// <param name="size">The size of the player</param>
            /// <param name="isSpawned">True if the character is spawned</param>
            /// <param name="isDied">True if the character is dead</param>
            /// <param name="isBoss">True if the enemy is a boss</param>
            /// <param name="xp">How many xp the enemy give to character when dead</param>
            public Enemy(Vector2 pos, int roomIndex, Texture2D sprite, int speed, string name, int basepv, int baseatk, int size, int mouvmentPoint, int actionPoint, int reach, int attackPrice, bool isSpawned, bool isDied, bool isBoss, int xp) : base(pos, roomIndex, sprite, speed, name, basepv, baseatk, size, mouvmentPoint, actionPoint, reach, attackPrice, isSpawned, isDied)
            {
                this.isBoss = isBoss;
                this.xp = xp;
                if (this.isBoss)
                {
                    this.maxPv *= 2;
                    this.maxAtk *= 2;
                    this.maxpa += 3;
                    this.maxpm += 1;
                    this.xp *= 3;
                    UpdateStats();
                }
                Dofawa_Chars.Singleton.Instance.enemys.Add(this);
            }
            /// <summary>
            /// Call this function in your global draw function for draw the character
            /// </summary>
            /// <param name="_spriteBatch">The sprite batch for draw the character</param>
            public void Draw(SpriteBatch _spriteBatch)
            {
                BaseDraw(_spriteBatch);
            }
            /// <summary>
            /// Update les statistiques
            /// </summary>
            public void UpdateStats()
            {
                this.pv = maxPv;
                this.atk = maxAtk;
                this.actionPoint = maxactionPoint;
                this.mouvmentPoint = maxmouvmentPoint;
            }
            /// <summary>
            /// Update l'ennemi
            /// </summary>
            public void Update()
            {

            }
            /// <summary>
            /// Remove the enemy from the list
            /// </summary>
            public void KillEnemy()
            {
                Dofawa_Chars.Singleton.Instance.enemys.Remove(this);
            }
        }
        /// <summary>
        /// Allié
        /// </summary>
        public class Ally : Char
        {
            public bool isTalkable;
            public bool isShopable;
            public bool isWaypoint;
            public bool isLootable;
            /// <summary>
            /// Create a ally
            /// </summary>
            /// <param name="pos">The position of pixel of the character</param>
            /// <param name="roomIndex">The index of room who the character is</param>
            /// <param name="sprite">The sprite of the character</param>
            /// <param name="name">The name of the character</param>
            /// <param name="basepv">The pv of character</param>
            /// <param name="baseatk">The attack damage of character</param>
            /// <param name="size">The size of the player</param>
            /// <param name="isSpawned">True if the character is spawned</param>
            /// <param name="isDied">True if the character is dead</param>
            /// <param name="isLootable">True if the character can be lootable</param>
            /// <param name="isShopable">True if the character sell and buy things</param>
            /// <param name="isTalkable">True if character have dialogs</param>
            /// <param name="isWaypoint">True if character is a waypoint
            /// </param>
            /// 
            public Ally(Vector2 pos, int roomIndex, Texture2D sprite, int speed, string name, int basepv, int baseatk, int size, int mouvmentPoint, int actionPoint, int reach, int attackPrice, bool isSpawned, bool isDied, bool isTalkable, bool isShopable, bool isWaypoint, bool isLootable) : base(pos, roomIndex, sprite, speed, name, basepv, baseatk, size, mouvmentPoint, actionPoint, reach, attackPrice, isSpawned, isDied)
            {
                this.isTalkable = isTalkable;
                this.isShopable = isShopable;
                this.isWaypoint = isWaypoint;
                this.isLootable = isLootable;
            }
            /// <summary>
            /// Call this function in your global draw function for draw the character
            /// </summary>
            /// <param name="_spriteBatch">The sprite batch for draw the character</param>
            public void Draw(SpriteBatch _spriteBatch)
            {
                BaseDraw(_spriteBatch);
            }
            /// <summary>
            /// Update l'allié
            /// </summary>
            public void Update()
            {

            }
        }
        /// <summary>
        /// Add a player and add it to the list
        /// </summary>
        /// <param name="character">The player class</param>
        public static void PlayerIni(Player character)
        {
            Singleton.Instance.players.Add(character);
        }
        /// <summary>
        /// Add a ally ad att it to the list
        /// </summary>
        /// <param name="character">The ally class</param>
        public static void AllyIni(Ally character)
        {
            Singleton.Instance.ally.Add(character);
        }
    }
    /// <summary>
    /// Super Globales des characters
    /// </summary>
    public static class Singleton
    {
        public static CharsList Instance { get; } = new CharsList();
    }
}
