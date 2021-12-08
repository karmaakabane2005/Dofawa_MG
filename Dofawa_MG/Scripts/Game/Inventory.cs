/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Inventory.cs
 * Version : v1
 */
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Forms;
using Microsoft.Xna.Framework;
using Dofawa_Settings;
using Dofawa_Button;

namespace Dofawa_Inventory
{
    public class Inventory
    {
        public class Items
        {
            public enum TypeArmure
            {
                Casque,
                Plastron,
                Jambieres,
                Bottes
            }

            public string nameItem;
            public int level;
            public ItemStats stats;
            public TypeArmure type;

            /// <summary>
            /// type Items contenant un nameItem (doit ètre unique), int level donnant le level minimum auxquelles on peut équiper cet item, Un ItemStats contenant les stats qu'apportent l'item et le TypeArmure contenant le type(casque, plastron, jamibières, bottes) 
            /// </summary>
            /// <param name="nameItem"></param>
            /// <param name="level"></param>
            /// <param name="stats"></param>
            /// <param name="type"></param>
            public Items(string nameItem, int level, ItemStats stats, TypeArmure type)
            {
                this.nameItem = nameItem;
                this.level = level;
                this.stats = stats;
                this.type = type;

            }
        }
        public class ItemStats
        {
            public int atk;
            public int pv;
            public int pa;
            public int pm;
            /// <summary>
            /// type ItemStats servant a stocker les statistiques des items
            /// </summary>
            /// <param name="atk"></param>
            /// <param name="pv"></param>
            public ItemStats(int atk, int pv, int pa, int pm)
            {
                this.atk = atk;
                this.pv = pv;
                this.pa = pa;
                this.pm = pm;

            }
        }
    }
    public class ItemButton
    {
        public Inventory.Items item;
        public Button button;
        /// <summary>
        /// créé un bonton lié a un item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="button"></param>
        public ItemButton(Inventory.Items item, Button button)
        {
            this.item = item;
            this.button = button;
        }
    }
    /// <summary>
    /// class qui stock toutes les variables
    /// </summary>
    public class InventoryVars
    {
        public IDictionary<string, Inventory.Items> items = new Dictionary<string, Inventory.Items>();
        //public List<Inventory.Items> items = new List<Inventory.Items>();
        public bool drawInventory = false;
        public Vector2 tailleInterface = new Vector2(490, 340);
        public List<Color> ColorEquiped = new List<Color>()
        {
            Color.SlateGray,
            Color.Gray
        };
        public List<ItemButton> inventoryButton = new List<ItemButton>();
        public List<Inventory.Items> itemDropables = new List<Inventory.Items>();
        public Inventory.Items UiToDraw = null;
        public List<Texture2D> UiInventory = new List<Texture2D>();
        public Button ItemEquiped;
        public Vector2 mesureTXT;
    }
    /// <summary>
    /// class qui stock les fonctions
    /// </summary>
    public static class InventoryFunctions
    {
        /// <summary>
        /// Fonction permettant de fermer/ouvrir l'inventaire
        /// </summary>
        public static void IsToDrawInventory()
        {
            Dofawa_Inventory.Singleton.Instance.drawInventory = !Dofawa_Inventory.Singleton.Instance.drawInventory;
        }
        /// <summary>
        /// fonction qui dessine l'entiereté de l'inventaire
        /// </summary>
        /// <param name="_spritebatch"></param>
        /// <param name="_graphics"></param>
        /// <param name="font"></param>
        public static void drawInventory(SpriteBatch _spritebatch, GraphicsDeviceManager _graphics, SpriteFont font)
        {
            if (Dofawa_Inventory.Singleton.Instance.drawInventory)
            {
                int ysize = 20;
                int nItems = 0;
                int sizeoffset = 0;
                int nItemEquip = 0;
                int nitemNoEquip = 0;
                foreach (ItemButton itemButton in Dofawa_Inventory.Singleton.Instance.inventoryButton)
                {
                    Inventory.Items item = itemButton.item;
                    if (Dofawa_Chars.Singleton.Instance.players[0].equipement.isEquip(item))
                    {
                        sizeoffset += ysize;
                    }
                }
                Forms.DrawRectangle(new Rectangle(new Point((int)SettingsFunctions.GetCenterOfScreen().X, (int)SettingsFunctions.GetCenterOfScreen().Y), new Point(550, 500)), Color.Gray, _graphics, _spritebatch, Dofawa_Draw.Alignment.Center);
                Forms.DrawRectangle(new Rectangle(new Point((int)SettingsFunctions.GetCenterOfScreen().X, (int)SettingsFunctions.GetCenterOfScreen().Y), new Point((int)Singleton.Instance.tailleInterface.X+50, (int)Singleton.Instance.tailleInterface.Y + 150)), new Color(28, 26, 26), _graphics, _spritebatch, Dofawa_Draw.Alignment.Center);
                foreach (ItemButton itemButton in Dofawa_Inventory.Singleton.Instance.inventoryButton)
                {
                    Point posPointItemRect = new Point((int)SettingsFunctions.GetCenterOfScreen().X - ((int)Singleton.Instance.tailleInterface.X / 2), (int)(SettingsFunctions.GetCenterOfScreen().Y - ((int)Singleton.Instance.tailleInterface.Y / 2)));
                    Button btn = itemButton.button;
                    Inventory.Items item = itemButton.item;
                    bool isEquip = Dofawa_Chars.Singleton.Instance.players[0].equipement.isEquip(item);
                    if (isEquip)
                    {
                        posPointItemRect.Y = posPointItemRect.Y + (20 * nItemEquip);
                        nItemEquip++;
                    }
                    else
                    {
                        posPointItemRect.Y = posPointItemRect.Y + sizeoffset + (ysize * nitemNoEquip);
                        nitemNoEquip++;
                    }
                    Rectangle itemRect = new Rectangle(new Point(posPointItemRect.X-25, posPointItemRect.Y - 75), new Point((int)Singleton.Instance.tailleInterface.X / 2+25, 18));
                    Forms.DrawRectangle(itemRect, Dofawa_Inventory.Singleton.Instance.ColorEquiped[isEquip ? 0 : 1], _graphics, _spritebatch, Dofawa_Draw.Alignment.TopLeft);
                    btn.pos = new Point(itemRect.X, itemRect.Y);
                    btn.size = new Point(itemRect.Width, itemRect.Height);
                    _spritebatch.DrawString(font, item.nameItem, new Vector2(itemRect.X, itemRect.Y), Color.Black);
                    nItems++;
                }
                if (Dofawa_Inventory.Singleton.Instance.UiToDraw != null)
                {
                    Dofawa_Inventory.Singleton.Instance.mesureTXT = font.MeasureString(Dofawa_Inventory.Singleton.Instance.UiToDraw.nameItem);
                    Forms.DrawRectangle(new Rectangle(new Point((int)SettingsFunctions.GetCenterOfScreen().X + 2, (int)SettingsFunctions.GetCenterOfScreen().Y), new Point((int)Singleton.Instance.tailleInterface.X / 2 - 4+25, (int)Singleton.Instance.tailleInterface.Y - 4 + 150)), Color.Gray, _graphics, _spritebatch, Dofawa_Draw.Alignment.CenterLeft);
                    _spritebatch.DrawString(font, Dofawa_Inventory.Singleton.Instance.UiToDraw.nameItem, new Vector2((int)SettingsFunctions.GetCenterOfScreen().X + 2 + ((((int)Singleton.Instance.tailleInterface.X / 2 - 4+25) - Dofawa_Inventory.Singleton.Instance.mesureTXT.X) / 2), (int)SettingsFunctions.GetCenterOfScreen().Y - (((int)
                  Singleton.Instance.tailleInterface.Y - 4) / 2)), Color.Black);
                    _spritebatch.DrawString(font, "lvl : " + Dofawa_Inventory.Singleton.Instance.UiToDraw.level, new Vector2(SettingsFunctions.GetCenterOfScreen().X + 100, SettingsFunctions.GetCenterOfScreen().Y - 140), Dofawa_Inventory.Singleton.Instance.UiToDraw.level <= Dofawa_Chars.Singleton.Instance.players[0].level.level ? Color.Black : Color.Red);
                    _spritebatch.DrawString(font, "atk : " + Dofawa_Inventory.Singleton.Instance.UiToDraw.stats.atk, new Vector2(SettingsFunctions.GetCenterOfScreen().X +100,SettingsFunctions.GetCenterOfScreen().Y -100), Color.Black);
                    _spritebatch.DrawString(font, "pv  : " + Dofawa_Inventory.Singleton.Instance.UiToDraw.stats.pv, new Vector2(SettingsFunctions.GetCenterOfScreen().X + 100, SettingsFunctions.GetCenterOfScreen().Y - 80), Color.Black);
                    _spritebatch.DrawString(font, "pa  : " + Dofawa_Inventory.Singleton.Instance.UiToDraw.stats.pa, new Vector2(SettingsFunctions.GetCenterOfScreen().X + 100, SettingsFunctions.GetCenterOfScreen().Y - 60), Color.Black);
                    _spritebatch.DrawString(font, "pm : " + Dofawa_Inventory.Singleton.Instance.UiToDraw.stats.pm, new Vector2(SettingsFunctions.GetCenterOfScreen().X + 100, SettingsFunctions.GetCenterOfScreen().Y - 40), Color.Black);
                    Dofawa_Inventory.Singleton.Instance.ItemEquiped.Draw(_spritebatch);
                }
            }
        }
        /// <summary>
        /// fonction contenant tout les test
        /// </summary>
        public static void updateInventory()
        {
            if (Dofawa_Inventory.Singleton.Instance.drawInventory)
            {
                foreach (ItemButton btn in Dofawa_Inventory.Singleton.Instance.inventoryButton)
                {
                    if (btn.button.Click() && Dofawa_Inventory.Singleton.Instance.drawInventory)
                    {
                        Dofawa_Inventory.Singleton.Instance.UiToDraw = btn.item;
                    }
                }
                if (Dofawa_Inventory.Singleton.Instance.ItemEquiped == null)
                {
                    Dofawa_Inventory.Singleton.Instance.ItemEquiped = new Button(new Point((int)SettingsFunctions.GetCenterOfScreen().X + 50, (int)SettingsFunctions.GetCenterOfScreen().Y), new Point(150, 75), InventoryFunctions.changeItemsEquiped, Dofawa_Inventory.Singleton.Instance.UiInventory[(Dofawa_Chars.Singleton.Instance.players[0].equipement.isEquip(Dofawa_Inventory.Singleton.Instance.UiToDraw) ? 1 : 0)]);
                }
                if (Dofawa_Inventory.Singleton.Instance.UiToDraw != null)
                {
                    bool isequiped = Dofawa_Chars.Singleton.Instance.players[0].equipement.isEquip(Dofawa_Inventory.Singleton.Instance.UiToDraw);
                    Dofawa_Inventory.Singleton.Instance.ItemEquiped.image = Dofawa_Inventory.Singleton.Instance.UiInventory[isequiped ? 1 : 0];
                }

                if (Dofawa_Inventory.Singleton.Instance.ItemEquiped.Click())
                {
                    InventoryFunctions.changeItemsEquiped();
                }
            }
        }
        /// <summary>
        /// fonction qui change l'items si on a le niveau requis
        /// </summary>
        public static void changeItemsEquiped()
        {
            if (Dofawa_Inventory.Singleton.Instance.UiToDraw.level <= Dofawa_Chars.Singleton.Instance.players[0].level.level)
            {
                Inventory.Items.TypeArmure typeArmure = Dofawa_Inventory.Singleton.Instance.UiToDraw.type;
                foreach (var type in Enum.GetValues(typeof(Inventory.Items.TypeArmure)))
                {

                    if (type.ToString() == typeArmure.ToString())
                    {
                        bool isequiped = Dofawa_Chars.Singleton.Instance.players[0].equipement.isEquip(Dofawa_Inventory.Singleton.Instance.UiToDraw);
                        Inventory.Items playerItem;
                        if (isequiped)
                        {
                            playerItem = null;
                        }
                        else
                        {
                            playerItem = Dofawa_Inventory.Singleton.Instance.UiToDraw;
                        }
                        switch (typeArmure)
                        {
                            case Inventory.Items.TypeArmure.Casque:
                                Dofawa_Chars.Singleton.Instance.players[0].equipement.casque = playerItem;
                                break;
                            case Inventory.Items.TypeArmure.Plastron:
                                Dofawa_Chars.Singleton.Instance.players[0].equipement.plastron = playerItem;
                                break;
                            case Inventory.Items.TypeArmure.Jambieres:
                                Dofawa_Chars.Singleton.Instance.players[0].equipement.jambieres = playerItem;
                                break;
                            case Inventory.Items.TypeArmure.Bottes:
                                Dofawa_Chars.Singleton.Instance.players[0].equipement.bottes = playerItem;
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// fonction qui initialise les variables de l inventaire requises
        /// </summary>
        public static void iniInventory()
        {
            int nItems = 0;
            foreach (Inventory.Items item in Dofawa_Inventory.Singleton.Instance.items.Values)
            {
                addInventoryButton(nItems, item);
                nItems++;
            }
        }
        /// <summary>
        /// Fonction qui crée un item de type ItemButton
        /// </summary>
        /// <param name="nItems"></param>
        /// <param name="item"></param>
        public static void addInventoryButton(int nItems, Inventory.Items item)
        {
            Dofawa_Inventory.Singleton.Instance.inventoryButton.Add(new ItemButton(item, new Dofawa_Button.Button(new Point((int)SettingsFunctions.GetCenterOfScreen().X - ((int)Singleton.Instance.tailleInterface.X / 2), (int)(SettingsFunctions.GetCenterOfScreen().Y / 2 + 10) + 20 * nItems), new Point((int)Singleton.Instance.tailleInterface.X / 2, 18))));
        }
    }
    /// <summary>
    /// classes permetant d'accéder dynamiquement au variables de la class InventoryVars si celles ci sont public
    /// </summary>
    public static class Singleton
    {
        public static InventoryVars Instance { get; } = new InventoryVars();

    }

}
