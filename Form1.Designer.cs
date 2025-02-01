namespace FootBall
{
    partial class FootBall
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnImportation = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnScanner = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnimportationApres = new System.Windows.Forms.Button();
            this.scoreEquipeRouge = new System.Windows.Forms.Label();
            this.scoreEquipeBleu = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImportation
            // 
            this.btnImportation.Location = new System.Drawing.Point(677, 319);
            this.btnImportation.Name = "btnImportation";
            this.btnImportation.Size = new System.Drawing.Size(151, 34);
            this.btnImportation.TabIndex = 0;
            this.btnImportation.Text = "Import 1";
            this.btnImportation.UseVisualStyleBackColor = true;
            this.btnImportation.Click += new System.EventHandler(this.btnImportation_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(42, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(612, 764);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnScanner
            // 
            this.btnScanner.Location = new System.Drawing.Point(677, 428);
            this.btnScanner.Name = "btnScanner";
            this.btnScanner.Size = new System.Drawing.Size(151, 33);
            this.btnScanner.TabIndex = 2;
            this.btnScanner.Text = "Scan";
            this.btnScanner.UseVisualStyleBackColor = true;
            this.btnScanner.Click += new System.EventHandler(this.btnSanner);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(846, 26);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(613, 764);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // btnimportationApres
            // 
            this.btnimportationApres.Location = new System.Drawing.Point(677, 375);
            this.btnimportationApres.Name = "btnimportationApres";
            this.btnimportationApres.Size = new System.Drawing.Size(151, 34);
            this.btnimportationApres.TabIndex = 5;
            this.btnimportationApres.Text = "Import 2";
            this.btnimportationApres.UseVisualStyleBackColor = true;
            this.btnimportationApres.Click += new System.EventHandler(this.btnimportationApres_Click);
            // 
            // scoreEquipeRouge
            // 
            this.scoreEquipeRouge.AutoSize = true;
            this.scoreEquipeRouge.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreEquipeRouge.Location = new System.Drawing.Point(119, 26);
            this.scoreEquipeRouge.Name = "scoreEquipeRouge";
            this.scoreEquipeRouge.Size = new System.Drawing.Size(32, 24);
            this.scoreEquipeRouge.TabIndex = 9;
            this.scoreEquipeRouge.Text = "00";
            this.scoreEquipeRouge.Click += new System.EventHandler(this.scoreEquipeRouge_Click);
            // 
            // scoreEquipeBleu
            // 
            this.scoreEquipeBleu.AutoSize = true;
            this.scoreEquipeBleu.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreEquipeBleu.Location = new System.Drawing.Point(119, 64);
            this.scoreEquipeBleu.Name = "scoreEquipeBleu";
            this.scoreEquipeBleu.Size = new System.Drawing.Size(32, 24);
            this.scoreEquipeBleu.TabIndex = 13;
            this.scoreEquipeBleu.Text = "00";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.scoreEquipeBleu);
            this.panel1.Controls.Add(this.scoreEquipeRouge);
            this.panel1.Location = new System.Drawing.Point(660, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(180, 117);
            this.panel1.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "ROUGE";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 24);
            this.label2.TabIndex = 15;
            this.label2.Text = "BLEU";
            // 
            // FootBall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 822);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnimportationApres);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnScanner);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnImportation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(1500, 1500);
            this.MinimumSize = new System.Drawing.Size(850, 725);
            this.Name = "FootBall";
            this.Text = "FootBall";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImportation;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnScanner;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnimportationApres;
        private System.Windows.Forms.Label scoreEquipeRouge;
        private System.Windows.Forms.Label scoreEquipeBleu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

