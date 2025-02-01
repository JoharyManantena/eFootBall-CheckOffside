using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootBall.Connexion
{
    internal class SeConnecter
    {
        static string serveur = "localhost";
        static string database = "football";
        static string utilisateur = "postgres";
        static string mdp = "joh";

        public static string dataSource()
        {
            string settings = $"Host={serveur};Database={database};Username={utilisateur};Password={mdp}";
            return settings;
        }
    }
}
