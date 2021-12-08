/**
 * Auteur : Leandro Antunes & Anthony Marcacci
 * Date : 21-22
 * Projet : Dofawa Monogame
 * Details : Program.cs, Auto-généré par Monogame. Peut causer un crash si les drivers ne sont pas mis a jour
 * Version : v1
 */
using System;

namespace Dofawa_MG
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
