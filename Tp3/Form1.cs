﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace TP3
{
  public partial class tetris : Form
  {
    public tetris()
    {
      InitializeComponent();
    }

    #region Code fourni

    // Représentation visuelles du jeu en mémoire.
    PictureBox[,] toutesImagesVisuelles = null;

    /// <summary>
    /// Gestionnaire de l'événement se produisant lors du premier affichage 
    /// du formulaire principal.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void frmLoad(object sender, EventArgs e)
    {
      // Ne pas oublier de mettre en place les valeurs nécessaires à une partie.
      ExecuterTestsUnitaires();
      InitialiserSurfaceDeJeu(20, 10);
      InitialiserTableauDeJeu();
      //AfficherCarre();

      Timer descendreAuto = new Timer();
      descendreAuto.Interval = 1000;
      descendreAuto.Tick += new EventHandler(descendreBlockAuto_Tick);
    }

    private void InitialiserSurfaceDeJeu(int nbLignes, int nbCols)
    {
      // Création d'une surface de jeu 10 colonnes x 20 lignes
      toutesImagesVisuelles = new PictureBox[nbLignes, nbCols];
      tableauJeu.Controls.Clear();
      tableauJeu.ColumnCount = toutesImagesVisuelles.GetLength(1);
      tableauJeu.RowCount = toutesImagesVisuelles.GetLength(0);
      for (int i = 0; i < tableauJeu.RowCount; i++)
      {
        tableauJeu.RowStyles[i].Height = tableauJeu.Height / tableauJeu.RowCount;
        for (int j = 0; j < tableauJeu.ColumnCount; j++)
        {
          tableauJeu.ColumnStyles[j].Width = tableauJeu.Width / tableauJeu.ColumnCount;
          // Création dynamique des PictureBox qui contiendront les pièces de jeu
          PictureBox newPictureBox = new PictureBox();
          newPictureBox.Width = tableauJeu.Width / tableauJeu.ColumnCount;
          newPictureBox.Height = tableauJeu.Height / tableauJeu.RowCount;
          newPictureBox.BackColor = Color.Black;
          newPictureBox.Margin = new Padding(0, 0, 0, 0);
          newPictureBox.BorderStyle = BorderStyle.FixedSingle;
          newPictureBox.Dock = DockStyle.Fill;

          // Assignation de la représentation visuelle.
          toutesImagesVisuelles[i, j] = newPictureBox;
          // Ajout dynamique du PictureBox créé dans la grille de mise en forme.
          // A noter que l' "origine" du repère dans le tableau est en haut à gauche.
          tableauJeu.Controls.Add(newPictureBox, j, i);
        }
      }
    }
    #endregion





    #region Code à développer 
    enum TypeBloc { None, Gelé, Carré, Ligne, T, L, J, S, Z }
    Color[] toutesLesCouleurs = new Color[] { Color.Black, Color.Gray, Color.Magenta, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Turquoise };
    int nbColonnesJeu = 10;
    int nbLignesJeu = 20;
    TypeBloc[,] tableauDeJeu = null;
    int pointDepartY = 0;
    int pointDepartX = 4;
    int[] blocActifY = new int[4];
    int[] blocActifX = new int[4];
    bool peutBouger = false;
    
    Deplacement coup = new Deplacement();
    enum Deplacement { LEFT, RIGHT, DOWN, HORAIRE, ANTIHORAIRE, NOMOVE }

    //variables pour les statistiques
    int nbCarres = 0;
    int nbLignes = 0;
    int nbT = 0;
    int nbL = 0;
    int nbJ = 0;
    int nbS = 0;
    int nbZ = 0;

    //Rôle : Initialiser le tableau de jeu à zéro, c'est-à-dire rendre tous les blocs noirs.
    //Paramètre : Aucun
    //Retour : Aucun
    void InitialiserTableauDeJeu()
    {
      tableauDeJeu = new TypeBloc[nbLignesJeu, nbColonnesJeu];

      for (int i = 0; i < nbLignesJeu; i++)
      {
        for (int j = 0; j < nbColonnesJeu; j++)
        {
          tableauDeJeu[i, j] = TypeBloc.None;
        }
      }
    }
    //void AfficherCarre()
    //{

    //  for (int j = 2; j < 4; j++)
    //  {
    //    tableauDeJeu[blocActifY[1], blocActifX[j]] = TypeBloc.Carré;
    //    tableauDeJeu[blocActifY[0], blocActifX[j]] = TypeBloc.Carré;
    //    if (tableauDeJeu[blocActifY[0], blocActifX[j]] == TypeBloc.Carré && tableauDeJeu[blocActifY[1], blocActifX[j]] == TypeBloc.Carré)
    //    {         
    //        toutesImagesVisuelles[0, pointDepartX].BackColor = Color.Magenta;
    //      toutesImagesVisuelles[0, pointDepartX + 1].BackColor = Color.Magenta;
    //        toutesImagesVisuelles[1, pointDepartX].BackColor = Color.Magenta;
    //      toutesImagesVisuelles[1, pointDepartX + 1].BackColor = Color.Magenta;   
    //    }
    //  }
    //}
    bool VerifierPeutBouger()
    {
      peutBouger = true;
      if (coup == Deplacement.LEFT)
      {
        for (int i = 0; i < blocActifX.Length; i++)
        {
          if (pointDepartX + blocActifX[i] - 1 < 0)
          { peutBouger = false; }
          if (pointDepartX + blocActifY[i] + 1 > nbLignesJeu - 1)
          { peutBouger = false; }
          if (peutBouger == true)
          {
            if (tableauDeJeu[pointDepartY, pointDepartX + blocActifX[i]] == TypeBloc.None)
            { peutBouger = true; }
            else
            { peutBouger = false; }
          }

        }
      }
      else if (coup == Deplacement.RIGHT)
      {
        for (int i = 0; i < blocActifX.Length; i++)
        {
          if (pointDepartX + blocActifX[i] + 1 > nbColonnesJeu - 1)
          { peutBouger = false; }
          if (peutBouger == true)
          {
            if (tableauDeJeu[pointDepartY, pointDepartX + blocActifX[i]] == TypeBloc.None)
            { peutBouger = true; }
            else
            { peutBouger = false; }
          }

        }
      }
      else if (coup == Deplacement.DOWN)
      {
        for (int i = 0; i < blocActifY.Length; i++)
        {
          if (pointDepartY + blocActifY[i] >= nbLignesJeu - 1)
          { peutBouger = false; }
          if (peutBouger == true)
          {
            if (tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]] == TypeBloc.None)
            { peutBouger = true; }
            else
            { peutBouger = false; }
          }

        }
      }
      return peutBouger;
    }
    //Rôle : Déplacer le bloc carré vers la gauche.
    //Paramètre : aucun
    //Retour : Deplacement représentant le coup entré par le joueur(gauche).
    Deplacement DeplacerCarreGauche()
    {
      coup = Deplacement.LEFT;
      {
        if (coup == Deplacement.LEFT && peutBouger == true)
        {
          pointDepartX = pointDepartX - 1;
          for (int i = 0; i < blocActifX.Length; i++)
          {
            toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta;
          }
        }
      }

      return coup;
    }
    //Rôle : Déplacer le bloc carré vers la droite.
    //Paramètre : aucun
    //Retour : Deplacement représentant le coup entré par le joueur (droite).
    Deplacement DeplacerCarreDroite()
    {
      coup = Deplacement.RIGHT;
      if (coup == Deplacement.RIGHT && peutBouger == true)
      {
        pointDepartX = pointDepartX + 1;
        for (int i = 0; i < blocActifX.Length; i++)
        {
          toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta;
        }
      }
      return coup;
    }
    //Rôle : Déplacer le bloc carré vers le bas.
    //Paramètre : aucun
    //Retour : Deplacement représentant le coup entré par le joueur(bas).
    Deplacement DeplacerCarreBas()
    {
      coup = Deplacement.DOWN;
      if (coup == Deplacement.DOWN && peutBouger == true)
      {
        pointDepartY = pointDepartY + 1;
        for (int i = 0; i < blocActifY.Length; i++)
        {
          toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta;

        }
      }
      return coup;
    }
    //Rôle : Effectuer la rotation d'un bloc dans le sens horaire.
    //Paramètre : Aucun
    //Retour : Aucun
    void EffectuerRotationHoraire()
    {
      blocActifY[0] = blocActifX[0];
      blocActifY[1] = blocActifX[1];
      blocActifY[2] = blocActifX[2];
      blocActifY[3] = blocActifX[3];

      blocActifX[0] = -blocActifY[0];
      blocActifX[1] = -blocActifY[1];
      blocActifX[2] = -blocActifY[2];
      blocActifX[3] = -blocActifY[3];

      for (int i = 0; i < blocActifY.Length; i++)
      {
        for (int j = 0; j < blocActifX.Length; j++)
        {
          tableauDeJeu[blocActifY[i], blocActifX[i]] = TypeBloc.Carré;
        }
      }

    }
    //Rôle : Vérifier si un nouveau bloc peut être généré dans le tableau de jeu.
    //Paramètre : Aucun
    //Retour : Aucun
    bool VerifierSiFinPartie()
    {
      bool partieTerminee = false;

      if (tableauDeJeu[pointDepartY, pointDepartX] == TypeBloc.Gelé)
      {
        partieTerminee = true;
      }
      return partieTerminee;
    }
    //Rôle : Afficher une boîte de dialogue indiquant au joueur que la partie est terminée.
    //Paramètre : Aucun
    //Retour : Aucun
    void AfficherFinPartie()
    {
      bool partieTerminee = VerifierSiFinPartie();

      if (partieTerminee == true)
      {
        MessageBox.Show("Partie terminée", "La partie est terminée", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }
    
    bool estUneLigneComplete()
    {
      bool ligneComplete = false;
      for (int i = 0; i < tableauDeJeu.GetLength(0); i++)
      {
        for (int j = 0; j < tableauDeJeu.GetLength(1); j++)
        {          
          if (tableauDeJeu[i, j] != TypeBloc.Gelé)
          {
            ligneComplete = false;            
          }
          else
          { ligneComplete = true;
            tableauDeJeu[i, j] = TypeBloc.None;
          }
        }
      
      }
      return ligneComplete;
    }
    int RetirerLigneComplete()
    {
      int nbLignesEnleves = 0;
      bool ligneComplete = estUneLigneComplete();
      for (int i=0;i<nbLignesJeu;i++)
      { if(ligneComplete==true)
       { DecalerLigne();
          nbLignesEnleves++;
       }
      }
      return nbLignesEnleves;
    }
    //Rôle : Vérifier si une ligne peut être décalée, puis décaler celle-ci si elle peut l'être.
    //Paramètre : Aucun
    //Retour : Aucun
    void DecalerLigne()
    { 
      for (int i = nbLignesJeu-1; i > 0; i--)
      {
        for (int j = 0; j < nbColonnesJeu; j++)
        {
          if (tableauDeJeu[nbLignesJeu-2, j] == TypeBloc.None)
          {
            tableauDeJeu[nbLignesJeu - 2, j] = tableauDeJeu[pointDepartY, j];
          }
        }
      }
    }
    //Rôle : Recommencer la partie lorsqu'elle est terminée.
    //Paramètre : Aucun
    //Retour : Aucun
    void RecommencerPartie()
    {
      tableauDeJeu = new TypeBloc[nbLignesJeu, nbColonnesJeu];

      for (int i = 0; i < nbLignesJeu - 1; i++)
      {
        for (int j = 0; j < nbColonnesJeu - 1; j++)
        {
          tableauDeJeu[i, j] = TypeBloc.None;
        }
      }
      pointDepartY = 0;
      pointDepartX = 4;
      InitialiserSurfaceDeJeu(nbLignesJeu, nbColonnesJeu);
      AfficherJeu();
    }
    /// <summary>
    /// Faites ici les appels requis pour vos tests unitaires.
    /// </summary>
    void ExecuterTestsUnitaires()
    {
      ExecuterTestABC();
      // A compléter...
    }

    // A renommer et commenter!
    void ExecuterTestABC()
    {
      // Mise en place des données du test

      // Exécuter de la méthode à tester

      // Validation des résultats

      // Clean-up
    }
    private void tableauJeu_Paint(object sender, PaintEventArgs e)
    {

    }
    #endregion
    //Rôle : Afficher le tableau de jeu courant avec les blocs gelés et le bloc en court.
    //Paramètre : Aucun
    //Retour : Aucun
    void AfficherJeu()
    {
      blocActifY = new int[] { 0, 0, 1, 1 };
      blocActifX = new int[] { 0, 1, 0, 1 };
      for (int i = 0; i < tableauDeJeu.GetLength(0); i++)
      {
        for (int j = 0; j < tableauDeJeu.GetLength(1); j++)
        {
          if (tableauDeJeu[i, j] == TypeBloc.Gelé)
          {
            toutesImagesVisuelles[i, j].BackColor = Color.Gray;
          }
          else if (tableauDeJeu[i,j]==TypeBloc.None)
          {
            toutesImagesVisuelles[i, j].BackColor = Color.Black;
          }
        }
      }

      for (int i = 0; i < 4; i++)
      {
        toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta;
      }
      for (int i=0;i<4;i++)
      {
        if (pointDepartY+blocActifY[i] >= nbLignesJeu - 1)
        {
          pointDepartY = nbLignesJeu - 2;
          tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]] = TypeBloc.Gelé;
        }
      }
 
      {        
        for (int i = 0; i < 4; i++)
        {if (pointDepartY == nbLignesJeu - 2 || tableauDeJeu[pointDepartY+1 + blocActifY[i], pointDepartX + blocActifX[i]] == TypeBloc.Gelé)
          {
            tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]] = TypeBloc.Gelé;
            tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX +1 + blocActifX[i]] = TypeBloc.Gelé;
            pointDepartY = 0;
            pointDepartX = 4;      
            toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta;
          }
          bool resultat = estUneLigneComplete();
        }
        
      }
     
     }
    private void tetris_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == 'a')
      {
        coup = Deplacement.LEFT;
        bool Valider = VerifierPeutBouger();
        if (peutBouger == true)
        {
          DeplacerCarreGauche();
          AfficherJeu();
        }
      }
      else if (e.KeyChar == 'd')
      {
        coup = Deplacement.RIGHT;
        bool Valider = VerifierPeutBouger();
        if (peutBouger == true)
        {
          DeplacerCarreDroite();
          AfficherJeu();
        }

      }
      else if (e.KeyChar == 's')
      {
        coup = Deplacement.DOWN;
        bool Valider = VerifierPeutBouger();
        if (peutBouger == true)
        {
          DeplacerCarreBas();
          AfficherJeu();
        }

      }
      else if (e.KeyChar == 'w')
      {
        bool valider = VerifierPeutBouger();
        
      }
      AfficherJeu();
    }

    private void modifierParamètreToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FrmParametre aide = new FrmParametre();
      aide.ShowDialog();
    }

    private void descendreBlockAuto_Tick(object sender, EventArgs e)
    {
      if (tableauDeJeu[pointDepartY, pointDepartX] == TypeBloc.None)
      {
        bool valider = VerifierPeutBouger();
        if (valider == true)
        {
          descendreBlockAuto.Start();
          DeplacerCarreBas();
          AfficherJeu();
        }
        else if(valider==false)
        {
          descendreBlockAuto.Stop();
        }

      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      RecommencerPartie();
    }
  }

 }





