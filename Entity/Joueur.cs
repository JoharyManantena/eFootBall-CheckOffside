using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Point = OpenCvSharp.Point;

namespace FootBall.Entity
{
    internal class Joueur
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public List<Point> Position { get; set; }
        public Equipe Equipe { get; set; }
        public string reponse { get; set; }
        public Boolean isGoal { get; set; }

        public Joueur() { }
        public Joueur(int id, string numero, List<Point> position, Equipe equipe)
        {
            Id = id;
            Numero = numero;
            Position = position;
            Equipe = equipe;
        }


        public static List<Joueur> DetectJoueurs(Mat mask, Equipe equipe)
        {
            List<Joueur> joueurs = new List<Joueur>();
            Cv2.FindContours(mask, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            foreach (var contour in contours)
            {
                
                Moments moments = Cv2.Moments(contour);
                int x = (int)(moments.M10 / moments.M00);
                int y = (int)(moments.M01 / moments.M00);
                Point centre = new Point(x, y);

               
                Joueur joueur = new Joueur
                {
                    Id = joueurs.Count + 1,
                    Numero = $"J{joueurs.Count + 1}",
                    Position = contour.ToList(),
                    Equipe = equipe 
                };

                joueurs.Add(joueur);
            }

            return joueurs;
        }
    }
}
