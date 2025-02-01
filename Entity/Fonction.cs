using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FootBall.Entity
{
    internal class Fonction
    {
        public static Joueur AQuiLeBallon(List<Joueur> joueurs, Ballon ballon)
        {
            if (ballon == null || joueurs.Count == 0)
                return null; // Aucun joueur ou ballon présent

            // Calculer le centre du ballon
            Point centreBallon = CalculerCentre(ballon.Position);

            Joueur joueurLePlusProche = null;
            Point pointLePlusProche = new Point();
            double distanceMin = double.MaxValue;

            foreach (var joueur in joueurs)
            {
                foreach (var point in joueur.Position)
                {
                    // Calculer la distance entre le point du joueur et le centre du ballon
                    double distance = CalculerDistance(centreBallon, point);

                    if (distance < distanceMin)
                    {
                        distanceMin = distance;
                        joueurLePlusProche = joueur;
                        pointLePlusProche = point;
                    }
                }
            }

            return joueurLePlusProche;
        }

        private static Point CalculerCentre(List<Point> points)
        {
            // Utiliser les moments pour calculer le centre du ballon
            Moments moments = Cv2.Moments(points);
            int x = (int)(moments.M10 / moments.M00);
            int y = (int)(moments.M01 / moments.M00);
            return new Point(x, y);
        }

        private static double CalculerDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }


        public static void AttribuerDirectionAuxEquipes(List<Joueur> joueursRouges, List<Joueur> joueursBleus)
        {
            // Trouver le joueur rouge le plus haut (plus petit Y) et le plus bas (plus grand Y)
            Joueur joueurRougePlusHaut = joueursRouges
                .OrderBy(joueur => joueur.Position.Min(point => point.Y))
                .FirstOrDefault();
            Joueur joueurRougePlusBas = joueursRouges
                .OrderByDescending(joueur => joueur.Position.Max(point => point.Y))
                .FirstOrDefault();

            // Trouver le joueur bleu le plus haut (plus petit Y) et le plus bas (plus grand Y)
            Joueur joueurBleuPlusHaut = joueursBleus
                .OrderBy(joueur => joueur.Position.Min(point => point.Y))
                .FirstOrDefault();
            Joueur joueurBleuPlusBas = joueursBleus
                .OrderByDescending(joueur => joueur.Position.Max(point => point.Y))
                .FirstOrDefault();

            // Déterminer les directions
            if (joueurRougePlusHaut.Position.Min(point => point.Y) < joueurBleuPlusHaut.Position.Min(point => point.Y) &&
                joueurRougePlusBas.Position.Max(point => point.Y) < joueurBleuPlusBas.Position.Max(point => point.Y))
            {
                // Les rouges sont plus haut, donc ils marquent en bas
                joueursRouges.ForEach(joueur => joueur.Equipe.Direction = "Bas");
                joueursBleus.ForEach(joueur => joueur.Equipe.Direction = "Haut");
                joueurRougePlusHaut.isGoal = true;
                joueurBleuPlusBas.isGoal = true;
            }
            else if (joueurRougePlusHaut.Position.Min(point => point.Y) > joueurBleuPlusHaut.Position.Min(point => point.Y) &&
                     joueurRougePlusBas.Position.Max(point => point.Y) > joueurBleuPlusBas.Position.Max(point => point.Y))
            {
                // Les rouges sont plus bas, donc ils marquent en haut
                joueursRouges.ForEach(joueur => joueur.Equipe.Direction = "Haut");
                joueursBleus.ForEach(joueur => joueur.Equipe.Direction = "Bas");
                joueurRougePlusBas.isGoal = true;
                joueurBleuPlusHaut.isGoal = true;
            }
        }


        public static List<Joueur> DetectHorsJeu(List<Joueur> tousLesJoueurs, Joueur joueurBallon)
        {
            List<Joueur> valiny = new List<Joueur>();

            // Obtenir la direction de l'équipe qui possède le ballon
            string directionEquipe = joueurBallon.Equipe.Direction;

            // Position Y du ballon
            int positionYBallon = joueurBallon.Position.Min(point => point.Y);

            // Identifier les joueurs adverses (exclure les gardiens de but)
            var defenseursAdverses = tousLesJoueurs
                .Where(joueur => joueur.Equipe != joueurBallon.Equipe && !joueur.isGoal)
                .ToList();

            // Trouver le défenseur adverse le plus bas et le plus haut
            int positionYDefenseurPlusBas = defenseursAdverses.Max(joueur => joueur.Position.Max(point => point.Y));
            int positionYDefenseurPlusHaut = defenseursAdverses.Min(joueur => joueur.Position.Min(point => point.Y));

            // Trouver le défenseur adverse le plus bas
            Joueur defenseurPlusBas = defenseursAdverses
                .OrderByDescending(joueur => joueur.Position.Max(point => point.Y))
                .FirstOrDefault();

            // Trouver le défenseur adverse le plus haut
            Joueur defenseurPlusHaut = defenseursAdverses
                .OrderBy(joueur => joueur.Position.Min(point => point.Y))
                .FirstOrDefault();


            // Détection des joueurs hors-jeu
            foreach (var joueur in tousLesJoueurs)
            {
                if (joueur.Equipe == joueurBallon.Equipe && !joueur.isGoal)
                {
                    if (joueur == joueurBallon)
                    {
                        // Cas spécifique : Joueur en possession du ballon
                        if ((directionEquipe == "Haut" && joueur.Position.Min(point => point.Y) < positionYDefenseurPlusHaut) ||
                            (directionEquipe == "Bas" && joueur.Position.Max(point => point.Y) > positionYDefenseurPlusBas))
                        {
                            joueur.reponse = "AVEC BALLON MAIS HORS JEU";
                        }
                        else
                        {
                            joueur.reponse = "AVEC BALLON";
                        }
                    }
                    else
                    {
                        // Joueur sans ballon de l'équipe
                        bool horsJeu = (directionEquipe == "Haut" && joueur.Position.Min(point => point.Y) < positionYDefenseurPlusHaut) ||
                                       (directionEquipe == "Bas" && joueur.Position.Max(point => point.Y) > positionYDefenseurPlusBas);

                        if (horsJeu)
                        {
                            joueur.reponse = "HORS JEU";
                        }
                        else
                        {
                            bool okPosition = (directionEquipe == "Haut" && joueur.Position.Min(point => point.Y) < positionYBallon) ||
                                              (directionEquipe == "Bas" && joueur.Position.Max(point => point.Y) > positionYBallon);

                            joueur.reponse = okPosition ? "OK" : null;
                        }
                    }
                }
                else if (joueur.Equipe != joueurBallon.Equipe)
                {
                    // Joueur de l'équipe adverse marquant la ligne hors-jeu
                    if ((directionEquipe == "Haut" && joueur == defenseurPlusHaut) ||
                        (directionEquipe == "Bas" && joueur == defenseurPlusBas))
                    {
                        joueur.reponse = "LIGNE HORS JEU";
                    }
                }
                else
                {
                    // Cas par défaut (par exemple gardiens)
                    joueur.reponse = null;
                }

                if (joueur.reponse != null)
                {
                    valiny.Add(joueur);
                }
            }


            return valiny;
        }


        public static bool DetectButOuNonBut(List<Joueur> joueursImportant, Mat imagesApres, Scalar scalarcouleur)
        {

            // Trouver le joueur avec le ballon dans la liste des joueurs importants
            Joueur joueurAvecBallon = joueursImportant.FirstOrDefault(j => j.reponse == "AVEC BALLON");
            if (joueurAvecBallon == null)
            {
                Joueur joueurPositionne = joueursImportant.FirstOrDefault(j => j.reponse == "AVEC BALLON MAIS HORS JEU");
                if (joueurPositionne != null)
                {
                    MessageBox.Show("POSITION HORS JEUX !!!");
                    return false;
                }
                else
                {
                    MessageBox.Show("Aucun joueur avec le ballon dans l'image Avant, donc pas de but possible");
                    return false;
                }
            }

            // Détecter la position du but dans l'image
            Rect butAdverse = DetecterPositionBut(imagesApres,joueurAvecBallon.Equipe.Direction, scalarcouleur);
            if (butAdverse == new Rect(0, 0, 0, 0))
            {
                MessageBox.Show("Aucun but détecté");
                return false;
            }

            // Détecter le ballon dans l'image
            Mat mask = new Mat();
            Cv2.InRange(imagesApres, scalarcouleur, new Scalar(50, 50, 50), mask);
            Ballon ballon = Ballon.DetectBallonMaj(mask, "Noir");

            if (ballon == null || ballon.Position == null || ballon.Position.Count == 0)
            {
                MessageBox.Show("Pas de ballon détecté");
                return false;
            }

            bool ballonDansBut = true;
            foreach (var point in ballon.Position)
            {
                
                if (!(point.X >= butAdverse.X &&
                      point.X <= butAdverse.X + butAdverse.Width &&
                      point.Y >= butAdverse.Y &&
                      point.Y <= butAdverse.Y + butAdverse.Height))
                {
                    ballonDansBut = false;
                    break;
                }
            }

            if (ballonDansBut)
            {
                Equipe equipe = joueurAvecBallon.Equipe;
                equipe.score = equipe.score + 1;
                equipe.UpdateEquipe();
                MessageBox.Show($"Buttttt !!!! Pour {joueurAvecBallon.Equipe.Name} , Score actuelle {equipe.score}");
            }
            else
            {
                MessageBox.Show($"Rate pour {joueurAvecBallon.Equipe.Name}  ");
            }
            return ballonDansBut;
        }


        public static Rect DetecterPositionBut(Mat image, string directionEquipe, Scalar couleurBut)
        {
            // Convertir l'image en niveaux de gris
            Mat gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

            // Appliquer un seuillage pour détecter les zones correspondant à la couleur cible
            Mat thresh = new Mat();
            double seuilInferieur = couleurBut.Val0 - 30; // Ajuster selon la tolérance
            double seuilSuperieur = couleurBut.Val0 + 30;
            Cv2.InRange(gray, new Scalar(seuilInferieur), new Scalar(seuilSuperieur), thresh);

            // Appliquer une transformation de Canny pour détecter les contours
            Mat edges = new Mat();
            Cv2.Canny(thresh, edges, 50, 150);

            // Détecter les lignes avec la transformée de Hough
            LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Math.PI / 180, 50, 50, 10);

            List<LineSegmentPoint> verticalLines = new List<LineSegmentPoint>();

            // Filtrer les lignes verticales
            foreach (var line in lines)
            {
                // Calculer l'angle de la ligne
                double angle = Math.Atan2(line.P2.Y - line.P1.Y, line.P2.X - line.P1.X) * 180 / Math.PI;

                // Filtrer les lignes verticales (angle proche de 90)
                if (Math.Abs(angle - 90) < 10 || Math.Abs(angle + 90) < 10)
                {
                    verticalLines.Add(line);
                }
            }

            // Déterminer les limites de la zone de recherche selon la direction de l'équipe
            int minY = 0, maxY = image.Rows;

            if (directionEquipe == "Haut")
            {
                maxY = 160; // Limite supérieure pour la direction "Haut"
            }
            else if (directionEquipe == "Bas")
            {
                minY = 300; // Limite inférieure pour la direction "Bas"
            }

            // Filtrer les lignes verticales dans la zone spécifiée
            List<LineSegmentPoint> filteredVerticalLines = verticalLines.Where(line =>
            {
                return (line.P1.Y >= minY && line.P1.Y <= maxY) && (line.P2.Y >= minY && line.P2.Y <= maxY);
            }).ToList();

            // Trouver les lignes verticales les plus pertinentes (les plus hautes pour "Haut" ou les plus basses pour "Bas")
            if (filteredVerticalLines.Count >= 2)
            {
                // Trouver les deux lignes les plus pertinentes selon la direction
                filteredVerticalLines = filteredVerticalLines.OrderBy(line =>
                {
                    return directionEquipe == "Haut" ? line.P1.Y : -line.P1.Y; // Trier par Y pour "Haut" ou "Bas"
                }).Take(2).ToList();

                // Créer le rectangle englobant à partir des deux lignes verticales
                int minX = Math.Min(filteredVerticalLines[0].P1.X, filteredVerticalLines[1].P1.X);
                int maxX = Math.Max(filteredVerticalLines[0].P2.X, filteredVerticalLines[1].P2.X);
                int minYFinal = Math.Min(filteredVerticalLines[0].P1.Y, filteredVerticalLines[1].P1.Y);
                int maxYFinal = Math.Max(filteredVerticalLines[0].P2.Y, filteredVerticalLines[1].P2.Y);


                if ((maxYFinal - minYFinal) < 0 && (maxX - minX) > 0)
                {
                    return new Rect(minX, maxYFinal, maxX - minX, minYFinal - maxYFinal);
                }
                else if ((maxYFinal - minYFinal) > 0 && (maxX - minX) < 0)
                {
                    return new Rect(maxX, minYFinal, minX - maxX, maxYFinal - minYFinal); 
                }
                else if ((maxYFinal - minYFinal) < 0 && (maxX - minX) < 0)
                {
                    return new Rect(maxX, maxYFinal, minX - maxX, minYFinal - maxYFinal); 
                }
                else
                {
                    return new Rect(minX, minYFinal, maxX - minX, maxYFinal - minYFinal);
                }

            }
            else
            {
                // Si moins de deux lignes verticales ont été détectées
                MessageBox.Show("Lignes verticales insuffisantes pour former un rectangle.");
                return new Rect(0, 0, 0, 0); // Aucun rectangle détecté
            }
        }


    }
}
