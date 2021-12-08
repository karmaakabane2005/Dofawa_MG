/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : SkillTree.cs, Squelette d'un arbre de compétence et de ses composants.
 * Version : v1
 */
using System;
using System.Collections.Generic;
using System.Text;
using Dofawa_Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dofawa_Button;
using Dofawa_Camera;
using Dofawa_Draw;
using Dofawa_Forms;
using Dofawa_Chars;
using Microsoft.Xna.Framework.Input;

namespace Dofawa_SkillTree
{
    /// <summary>
    /// Squelette d'un arbre de compétence et de ses composants.
    /// </summary>
    public class SkillTree
    {
        public List<Component> components = new List<Component>();
        // Vrai si l'interface d'achat est ouverte
        public bool anyComponentOppened = false;
        public Component oppenedComponent;
        public Button button;
        public Point defaultUISize = new Point(400, 200);
        public Point UISize = new Point(400, 200);
        public Point UIPos;
        /// <summary>
        /// Construit un squelette d'un arbre de compétence et de ses composants.
        /// </summary>
        public SkillTree()
        {
            UIPos = DrawFunctions.AlignForm(Alignment.CenterBottom, UISize.ToVector2(), new Vector2(Dofawa_Settings.SettingsFunctions.GetCenterOfScreen().X, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight)).ToPoint();
            Rectangle btnRect = new Rectangle(UIPos + new Point(100, 125), UISize - new Point(200, 150));
            button = new Button(new Point(btnRect.X, btnRect.Y), new Point(btnRect.Width, btnRect.Height));
        }
        /// <summary>
        /// Construit un composant et l'ajoute à la liste.
        /// </summary>
        /// <param name="title">Le titre</param>
        /// <param name="description">La description</param>
        /// <param name="cost">Le prix</param>
        /// <param name="pos">La position</param>
        /// <param name="nobuyedColor">La couleur quand à pas acheté</param>
        /// <param name="buyedColor">La couleur quand on à acheté</param>
        /// <param name="buyFunction">La méthode quand on achete</param>
        /// <param name="tag">Le tag</param>
        /// <param name="childOf">Les parents</param>
        /// <param name="size">La taille</param>
        /// <returns></returns>
        public Component IniComponent(string title, string description, int cost, Point pos, Color nobuyedColor, Color buyedColor, Action buyFunction, string tag = "", List<Component> childOf = null, Point size = default)
        {
            Component component = new Component(title, description, cost, this, pos, nobuyedColor, buyedColor, buyFunction, tag, childOf, size);
            components.Add(component);
            return component;
        }
        /// <summary>
        /// Update le skill tree
        /// </summary>
        /// <param name="camera">La camera</param>
        /// <param name="level">Le level</param>
        public void Update(Camera camera, Level level)
        {
            Vector2 mousePosWorldSpace = Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(camera.ViewMatrix));
            int posx = (int)mousePosWorldSpace.X;
            int posy = (int)mousePosWorldSpace.Y;
            if (button.Click())
            {
                if (level.skillPoint >= oppenedComponent.cost && !oppenedComponent.buyed)
                {
                    level.skillPoint -= oppenedComponent.cost;
                    oppenedComponent.buyed = true;
                    oppenedComponent.buyFunction?.Invoke();
                }
            }
            foreach (Component component in components)
            {
                component.Update();
                if (component.button.Click(posx,posy) && component.IsToDraw())
                {
                    anyComponentOppened = true;
                    oppenedComponent = component;
                }
            }
        }
        /// <summary>
        /// Dessine le skill tree
        /// </summary>
        /// <param name="titleFont">La police d'écriture du titre</param>
        /// <param name="font">La polcie d'écriture</param>
        /// <param name="_spriteBatch">SpriteBatch du jeu</param>
        /// <param name="graphicsDeviceManager">Manager grahique du jeu</param>
        /// <param name="camera">La camera</param>
        public void Draw(SpriteFont titleFont, SpriteFont font, SpriteBatch _spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera camera)
        {
            graphicsDeviceManager.GraphicsDevice.Clear(new Color(37, 36, 36));
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.ViewMatrix);
            if (components.Count > 0)
            {
                foreach (Component component in components)
                {
                    if (component == oppenedComponent)
                    {
                        component.Draw(_spriteBatch, graphicsDeviceManager, Color.DodgerBlue);
                    }
                    else
                    {
                        component.Draw(_spriteBatch, graphicsDeviceManager);
                    }

                }
            }

            _spriteBatch.End();
            if (anyComponentOppened)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
                Vector2 descSize = font.MeasureString(oppenedComponent.description);
                if (descSize.X > defaultUISize.X - 20)
                {
                    UISize.X = (int)descSize.X + 20;
                    UIPos = DrawFunctions.AlignForm(Alignment.CenterBottom, UISize.ToVector2(), new Vector2(Dofawa_Settings.SettingsFunctions.GetCenterOfScreen().X, Dofawa_Settings.Singleton.Instance.PreferredBackBufferHeight)).ToPoint();
                }
                Forms.DrawRectangle(new Rectangle(UIPos - new Point(10), UISize + new Point(20)), new Color(30, 30, 30), graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
                Forms.DrawRectangle(new Rectangle(UIPos, UISize), new Color(60,60,60), graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);

                Vector2 titleRectPos = DrawFunctions.AlignForm(Alignment.CenterTop, titleFont.MeasureString(oppenedComponent.title), new Vector2(UIPos.X + UISize.X / 2, UIPos.Y + 10));
                _spriteBatch.DrawString(titleFont, oppenedComponent.title, titleRectPos, Color.Black);

                Rectangle btnRect = new Rectangle(button.pos, button.size);
                Forms.DrawRectangle(new Rectangle(new Point(button.pos.X, button.pos.Y) - new Point(4), new Point(btnRect.Width, btnRect.Height) + new Point(8)), new Color(45,45,45), graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
                Forms.DrawRectangle(btnRect, new Color(190, 190, 190), graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
                _spriteBatch.DrawString(font, "Acheter", DrawFunctions.AlignForm(Alignment.Center, font.MeasureString("Acheter"), btnRect.Center.ToVector2()),Color.Gray);

                Point descPos = new Point(UIPos.X + UISize.X/2, UIPos.Y + 65);
                Vector2 descAlignPos = DrawFunctions.AlignForm(Alignment.Center, font.MeasureString(oppenedComponent.description), descPos.ToVector2());
                _spriteBatch.DrawString(font, oppenedComponent.description, descAlignPos, Color.White);
                _spriteBatch.DrawString(font, "Prix : " + oppenedComponent.cost + " point(s)", descAlignPos + new Vector2(0, 40), Color.White);
                _spriteBatch.End();
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            _spriteBatch.DrawString(font, Dofawa_Chars.Singleton.Instance.players[0].level.skillPoint.ToString() + " P", new Vector2(30, 30), Color.DodgerBlue);
            _spriteBatch.End();
        }
        /// <summary>
        /// Composants du skilltree
        /// </summary>
        public class Component
        {
            public string title;
            public string description;
            public string tag;
            public int cost;
            // Vrai si le composant est acheté
            public bool buyed = false;
            public Texture2D texture;
            public List<Component> parentOf;
            public List<Component> childOf = new List<Component>();
            public Vector2 size;
            public Vector2 pos;
            public Button button;
            public Color nobuyedColor;
            public Color buyedColor;
            public Action buyFunction;
            public Rectangle rect = new Rectangle(0,0,0,0);
            /// <summary>
            /// Construit un composant
            /// </summary>
            /// <param name="title">Le titre</param>
            /// <param name="description">La description</param>
            /// <param name="cost">Le prix</param>
            /// <param name="pos">La position</param>
            /// <param name="nobuyedColor">La couleur quand à pas acheté</param>
            /// <param name="buyedColor">La couleur quand on à acheté</param>
            /// <param name="buyFunction">La méthode quand on achete</param>
            /// <param name="tag">Le tag</param>
            /// <param name="childOf">Les parents</param>
            /// <param name="size">La taille</param>
            public Component(string title, string description, int cost, SkillTree parent, Point pos, Color nobuyedColor, Color buyedColor, Action buyFunction, string tag = "", List<Component> childOf = null, Point size = default)
            {
                this.title = title;
                this.description = description;
                this.cost = cost;
                if (childOf != null)
                {
                    this.childOf = childOf;
                }
                parentOf = new List<Component>();
                this.pos = DrawFunctions.AlignForm(Alignment.CenterBottom, size.ToVector2(), Dofawa_Settings.SettingsFunctions.GetCenterOfScreen() + pos.ToVector2());
                this.tag = tag;
                this.nobuyedColor = nobuyedColor;
                this.buyedColor = buyedColor;
                this.buyFunction = buyFunction;

                if (size == default)
                {
                    this.size = new Vector2(50, 50);
                }
                else
                {
                    this.size = size.ToVector2();
                }
                button = new Button(this.pos.ToPoint(), this.size.ToPoint());
                if (childOf != null)
                {
                    foreach (Component component in childOf)
                    {
                        component.parentOf.Add(this);
                    }
                }
            }
            /// <summary>
            /// Update le composant
            /// </summary>
            public void Update()
            {
                rect.X = (int)pos.X;
                rect.Y = (int)pos.Y;
                rect.Width = (int)size.X;
                rect.Height = (int)size.Y;
            }
            /// <summary>
            /// Est-ce que le composant doit être dessiné
            /// </summary>
            /// <returns>Vrai si il doit être dessiné</returns>
            public bool IsToDraw()
            {
                bool needToDraw = false;
                if (childOf.Count == 0)
                {
                    needToDraw = true;
                }
                else
                {
                    bool allParentsUnlocked = true;
                    foreach (Component parent in childOf)
                    {
                        if (!parent.buyed)
                        {
                            allParentsUnlocked = false;
                            break;
                        }
                    }
                    needToDraw = allParentsUnlocked;
                }
                return needToDraw;
            }
            /// <summary>
            /// Dessine le composant
            /// </summary>
            /// <param name="_spriteBatch">SpriteBatch du jeu</param>
            /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
            public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphicsDeviceManager)
            {
                Color color;
                if (buyed)
                {
                    color = buyedColor;
                }
                else
                {
                    color = nobuyedColor;
                }
                Draw(_spriteBatch, graphicsDeviceManager, color);
            }
            /// <summary>
            /// Dessome le composant
            /// </summary>
            /// <param name="_spriteBatch">SpriteBatch du jeu</param>
            /// <param name="graphicsDeviceManager">Manager graphique du jeu</param>
            /// <param name="specialColor">Couleur du composant</param>
            public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Color specialColor)
            {
                if (IsToDraw())
                {
                    if (parentOf.Count > 0 && buyed)
                    {
                        foreach (Component child in parentOf)
                        {
                            Forms.DrawLine(graphicsDeviceManager, _spriteBatch, pos + size / 2, child.pos + child.size / 2, new Color(150, 150, 150, 255), 3);
                        }
                    }
                    Forms.DrawRectangle(rect, specialColor, graphicsDeviceManager, _spriteBatch, Alignment.TopLeft);
                }
            }
        }
    }
    /// <summary>
    /// Fonctions du skilltree
    /// </summary>
    public static class SkillTreeFunctions
    {
        /// <summary>
        /// Obtient le composant par son titre
        /// </summary>
        /// <param name="name">le titre</param>
        /// <param name="skillTree">Le parent</param>
        /// <returns>Le composant</returns>
        public static SkillTree.Component getComponentByName(string name, SkillTree skillTree)
        {
            foreach (SkillTree.Component component in skillTree.components)
            {
                if (component.title == name)
                {
                    return component;
                }
            }
            throw new Exception("Component not found");
        }
        /// <summary>
        /// Obtient le composant par son tag
        /// </summary>
        /// <param name="tag">Le tag</param>
        /// <param name="skillTree">Le parent</param>
        /// <returns></returns>
        public static SkillTree.Component getComponentByTag(string tag, SkillTree skillTree)
        {
            foreach (SkillTree.Component component in skillTree.components)
            {
                if (component.tag == tag)
                {
                    return component;
                }
            }
            throw new Exception("Component not found");
        }
        /// <summary>
        /// Alterne les scènes entre SkillTree et Game
        /// </summary>
        public static void ChangeScene()
        {
            int SkillTreeSceneId = Dofawa_SceneManager.SceneFunctions.GetSceneByName("SkillTree").id;
            int gameSceneId = Dofawa_SceneManager.SceneFunctions.GetSceneByName("Game").id;
            if (Dofawa_SceneManager.Singleton.Instance.currentSceneId == gameSceneId || Dofawa_SceneManager.Singleton.Instance.currentSceneId == SkillTreeSceneId)
            {
                if (Dofawa_SceneManager.Singleton.Instance.currentSceneId == SkillTreeSceneId)
                {
                    Dofawa_SceneManager.Singleton.Instance.currentSceneId = gameSceneId;
                }
                else
                {
                    Dofawa_SceneManager.Singleton.Instance.currentSceneId = SkillTreeSceneId;
                }
            }
        }
        /// <summary>
        /// Execute la méthode pour le dernier composant du skill tree
        /// </summary>
        public static void BuyOmniscient()
        {
            Chars.Player player = Dofawa_Chars.Singleton.Instance.players[0];
            player.bonuspa += 1;
            player.bonuspm += 1;
            player.reach += 1;
            player.bonusatk += 500;
            player.bonuspv += 3500;
            player.xpDropFactor += 5;
            player.pv += 3500;
            player.atk += 500;
            player.actionPoint += 1;
            player.mouvmentPoint += 1;
        }
    }
}
