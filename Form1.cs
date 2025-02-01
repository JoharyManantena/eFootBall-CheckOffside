using FootBall.Entity;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Point = OpenCvSharp.Point;
using OpenCvSharp.Extensions;
using Image = System.Drawing.Image;



namespace FootBall
{
    public partial class FootBall : Form
    {
        private string importedImagePath = null;
        private string importedImagePathApres = null;
        private Equipe EquipeRouge = Equipe.GetEquipe("Rouge");
        private Equipe EquipeBlue = Equipe.GetEquipe("Blue");

        public FootBall()
        {
            InitializeComponent();
            if (EquipeRouge != null)
            {
                scoreEquipeRouge.Text = EquipeRouge.score.ToString();
            }
            else
            {
                MessageBox.Show(
                    "L'équipe Rouge n'a pas été trouvée dans la base de données.",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            if (EquipeBlue != null)
            {
                scoreEquipeBleu.Text = EquipeBlue.score.ToString();
            }
            else
            {
                MessageBox.Show(
                    "L'équipe Bleu n'a pas été trouvée dans la base de données.",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnImportation_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Sélectionnez une image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;
                        Image importedImage = Image.FromFile(filePath);
                        this.importedImagePath = filePath;

                        pictureBox1.Image = importedImage;
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'importation de l'image : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSanner(object sender, EventArgs e)
        {
            // Vérifier si les chemins des images sont définis
            if (string.IsNullOrEmpty(this.importedImagePath) || string.IsNullOrEmpty(this.importedImagePathApres))
            {
                MessageBox.Show("L'une ou les deux images n'ont pas été sélectionnées !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Charger l'image avant
            string imagePath = this.importedImagePath;
            Mat image = Cv2.ImRead(imagePath);

            // Charger l'image apres
            string imagePathApres = this.importedImagePathApres; 
            Mat imageApres = Cv2.ImRead(imagePathApres);

            // Vérifiez si les deux images sont valides
            if (image.Empty() || imageApres.Empty())
            {
                MessageBox.Show("L'une ou les deux images n'ont pas pu être chargées !");
                return;
            }


            // Pour l'image Avant ------------------------------------------------------------------------------------------------
                
                // Convertir en espace de couleur HSV
                Mat hsvImage = new Mat();

                // Traiter l'image ici
                List<Joueur> response = traitementImage(image, hsvImage);

                // Annoter l'image avec les IDs des joueurs et du ballon
                Mat imageAnnotée = AnnoterImage(image, response);

                // Convertir l'image annotée (Mat) en Bitmap
                Bitmap bitmap = BitmapConverter.ToBitmap(imageAnnotée);

                // Afficher l'image dans un PictureBox
                pictureBox1.Image = bitmap;

            //Fin Image Avant ----------------------------------------------------------------------------------------------------

            // Scalar possible
            Scalar scalarnoir = new Scalar(0, 0, 0);       // Noir (BGR)
            Scalar scalarblanc = new Scalar(255, 255, 255); // Blanc (BGR)
            Scalar scalarGris = new Scalar(128, 128, 128);  // Gris (BGR)


            // Pour l'image Apres ------------------------------------------------------------------------------------------------
               
                Joueur joueurAvecBallon = response.FirstOrDefault(j => j.reponse == "AVEC BALLON");
                if (joueurAvecBallon == null)
                {
                    joueurAvecBallon = response.FirstOrDefault(j => j.reponse == "AVEC BALLON MAIS HORS JEU");
                }

                // Convertir en espace de couleur HSV
                Mat hsvImageApres = new Mat();
                
                // Traiter le but ou non but
                Fonction.DetectButOuNonBut(response, imageApres,scalarnoir);

                

            //Fin Image Apres ----------------------------------------------------------------------------------------------------

            // Changer score dans les information
            scoreEquipeRouge.Text = EquipeRouge.score.ToString();
            scoreEquipeBleu.Text = EquipeBlue.score.ToString();

            // Sauvegarder l'image annotée
            // Cv2.ImWrite("image_annotée.jpg", imageAnnotée);

        }


        private Mat AnnoterImage(Mat image, List<Joueur> tousLesJoueurs)
        {
            foreach (var joueur in tousLesJoueurs)
            {
                if (joueur.Position.Count > 0)
                {

                    Moments moments = Cv2.Moments(joueur.Position);
                    if (moments.M00 == 0) continue;
                    int x = (int)(moments.M10 / moments.M00);
                    int y = (int)(moments.M01 / moments.M00);

                    Scalar couleurTexte;


                    switch (joueur.reponse)
                    {
                        case "HORS JEU":
                            couleurTexte = Scalar.Yellow;
                            Cv2.PutText(image, "Hors Jeu", new Point(x, y), HersheyFonts.HersheySimplex, 0.7, couleurTexte, 2);
                            break;

                        case "OK":
                            couleurTexte = Scalar.Green;
                            Cv2.PutText(image, "Valid", new Point(x, y), HersheyFonts.HersheySimplex, 0.7, couleurTexte, 2);
                            break;

                        case "AVEC BALLON":
                            couleurTexte = Scalar.Black;
                            Cv2.PutText(image, "Pass", new Point(x, y), HersheyFonts.HersheySimplex, 0.7, couleurTexte, 2);
                            break;

                        case "LIGNE HORS JEU":
                            int yLigne = DeterminerPositionLigneHorsJeu(joueur);
                            Mat overlay = image.Clone();
                            Scalar couleurZone = new Scalar(0, 0, 255, 128); // Bleu clair semi-transparent

                            if (joueur.Equipe.Direction == "Haut")
                            {
                                Cv2.Rectangle(overlay, new Point(0, yLigne), new Point(image.Width, image.Height), couleurZone, -1);
                            }
                            else if (joueur.Equipe.Direction == "Bas")
                            {
                                Cv2.Rectangle(overlay, new Point(0, 0), new Point(image.Width, yLigne), couleurZone, -1);
                            }
                            Cv2.AddWeighted(overlay, 0.5, image, 0.5, 0, image);
                            Scalar couleurLigne = Scalar.Yellow;
                            Cv2.Line(image, new Point(0, yLigne), new Point(image.Width, yLigne), couleurLigne, 2);
                            break;
                    }
                }
            }

            // ALEA 1
            foreach (var passeur in tousLesJoueurs.Where(j => j.reponse == "AVEC BALLON"))
            {
                Moments momentsPasseur = Cv2.Moments(passeur.Position);
                if (momentsPasseur.M00 == 0) continue;
                Point centrePasseur = new Point((int)(momentsPasseur.M10 / momentsPasseur.M00), (int)(momentsPasseur.M01 / momentsPasseur.M00));

                foreach (var valid in tousLesJoueurs.Where(j => j.reponse == "OK"))
                {
                    Moments momentsValid = Cv2.Moments(valid.Position);
                    if (momentsValid.M00 == 0) continue;
                    Point centreValid = new Point((int)(momentsValid.M10 / momentsValid.M00), (int)(momentsValid.M01 / momentsValid.M00));

                    Scalar couleurFleche = Scalar.Black;
                    int epaisseur = 2;
                    Cv2.ArrowedLine(image, centrePasseur, centreValid, couleurFleche, epaisseur, LineTypes.Link8, 0, 0.2);
                }
            }

            return image;

        }


        /// <summary>
        /// Détermine la position Y de la ligne de hors-jeu en fonction de la direction de l'équipe.
        /// </summary>
        private int DeterminerPositionLigneHorsJeu(Joueur joueur)
        {
            if (joueur.Equipe.Direction == "Haut")
            {
                return joueur.Position.Max(point => point.Y);
            }
            else if (joueur.Equipe.Direction == "Bas")
            {
                return joueur.Position.Min(point => point.Y);
            }
            else
            {
                Moments moments = Cv2.Moments(joueur.Position);
                return (int)(moments.M01 / moments.M00);
            }
        }

        private void btnimportationApres_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Sélectionnez une image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = openFileDialog.FileName;
                        Image importedImage = Image.FromFile(filePath);
                        this.importedImagePathApres = filePath;

                        pictureBox2.Image = importedImage;
                        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'importation de l'image : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    
        private List<Joueur> traitementImage(Mat image,Mat hsvImage)
        {
            Cv2.CvtColor(image, hsvImage, ColorConversionCodes.BGR2HSV);

            // Détection des joueurs rouges
            Mat rougeMask1 = new Mat();
            Mat rougeMask2 = new Mat();
            Cv2.InRange(hsvImage, new Scalar(0, 120, 70), new Scalar(10, 255, 255), rougeMask1); // Rouge clair
            Cv2.InRange(hsvImage, new Scalar(170, 120, 70), new Scalar(180, 255, 255), rougeMask2); // Rouge foncé
            Mat rougeMask = rougeMask1 | rougeMask2;
            List<Joueur> joueursRouges = Joueur.DetectJoueurs(rougeMask, this.EquipeRouge);

            // Détection des joueurs bleus
            Mat bleuMask = new Mat();
            Cv2.InRange(hsvImage, new Scalar(100, 120, 70), new Scalar(140, 255, 255), bleuMask);
            List<Joueur> joueursBleus = Joueur.DetectJoueurs(bleuMask, this.EquipeBlue);

            // Détection du ballon noir
            Mat noirMask = new Mat();
            Cv2.InRange(hsvImage, new Scalar(0, 0, 0), new Scalar(180, 255, 50), noirMask);
            Ballon ballonNoir = Ballon.DetectBallonMaj(noirMask, "Noir");

            // Listes des tous les Joueurs
            List<Joueur> tousLesJoueurs = joueursRouges.Concat(joueursBleus).ToList();

            // Afficher les Direction des equipes
            Fonction.AttribuerDirectionAuxEquipes(joueursRouges, joueursBleus);

            // Afficher le ballon noir
            Joueur joueurBallon = null;
            if (ballonNoir != null)
            {
                joueurBallon = Fonction.AQuiLeBallon(tousLesJoueurs, ballonNoir);
            }
            else
            {
                MessageBox.Show("Aucun ballon détecté !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            List<Joueur> response = Fonction.DetectHorsJeu(tousLesJoueurs, joueurBallon);
            return response;
        }

        private void scoreEquipeRouge_Click(object sender, EventArgs e)
        {

        }
    }
}
