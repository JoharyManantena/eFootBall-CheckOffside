using OpenCvSharp;
using System.Collections.Generic;
using System.Linq;
using Point = OpenCvSharp.Point;

namespace FootBall.Entity
{
    internal class Ballon
    {
        public int Id { get; set; }
        public string Couleur { get; set; }  
        public List<Point> Position { get; set; }

        public Ballon() { }

        public static Ballon DetectBallon(Mat mask, string couleur)
        {
            Cv2.FindContours(mask, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length == 0)
                return null;

            // Prendre le premier contour valide
            Point[] contour = contours[0]; // Récupère le premier contour trouvé

            // Créer un objet Ballon avec les informations
            Ballon ballon = new Ballon
            {
                Id = 1,
                Couleur = couleur,
                Position = contour.ToList()
            };

            return ballon;
        }

        public static Ballon DetectBallonMaj(Mat mask, string couleur)
        {
            // Trouver les contours des objets dans le masque
            Cv2.FindContours(mask, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length == 0)
                return null; // Aucun ballon détecté

            // Parcourir les contours pour trouver un contour valide correspondant à la couleur
            foreach (var contour in contours)
            {
                // Calculer le centre du contour (moments)
                Moments moments = Cv2.Moments(contour);
                if (moments.M00 == 0)
                    continue; // Éviter la division par zéro

                int centerX = (int)(moments.M10 / moments.M00);
                int centerY = (int)(moments.M01 / moments.M00);

                if (centerX >= 0 && centerX < mask.Width && centerY >= 0 && centerY < mask.Height)
                {
                    byte pixelValue = mask.At<byte>(centerY, centerX);
                    if (pixelValue > 0)
                    {
                       
                        Ballon ballon = new Ballon
                        {
                            Id = 1,
                            Couleur = couleur,
                            Position = contour.ToList()
                        };

                        return ballon;
                    }
                }
            }

            return null;
        }


    }
}
