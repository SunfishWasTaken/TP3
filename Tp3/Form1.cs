using System;
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
      descendreBlockAuto.Start();
      
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
    enum Deplacement { LEFT, RIGHT, DOWN, HORAIRE, ANTIHORAIRE, NOMOVE}

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
    { peutBouger = false;
      if (coup == Deplacement.LEFT)
      {
        for (int i = 0; i < blocActifX.Length; i++)
        {
          if (tableauDeJeu[ pointDepartY, pointDepartX + blocActifX[i]] == TypeBloc.None)
          { peutBouger = true; }
          else
          { peutBouger = false; }
        }
      }
      else if (coup==Deplacement.RIGHT)
      {
      for(int i=0;i<blocActifX.Length;i++)
        {
          if (tableauDeJeu[pointDepartY, pointDepartX + blocActifX[i]] == TypeBloc.None)
          { peutBouger = true; }
          else
          { peutBouger = false; }
        }
      }
      else if (coup==Deplacement.DOWN)
      {
      for(int i=0;i<blocActifY.Length;i++)
       {
          if (tableauDeJeu[pointDepartY+blocActifY[i], pointDepartX + blocActifX[i]] == TypeBloc.None)
          { peutBouger = true; }
          else
          { peutBouger = false; }
        }
      }
      return peutBouger;
    }
    Deplacement DeplacerCarreGauche()
    {
      coup = Deplacement.LEFT;
      {
        if (coup == Deplacement.LEFT && peutBouger==true)
        {
          for (int i = 0; i < blocActifX.Length; i++)
          {
            if (tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX - blocActifX[i]] == TypeBloc.None)
            { toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX - blocActifX[i]].BackColor = Color.Magenta; }
          }
        }       
       }

      return coup;
    }
    void DeplacerCarreDroite()
    { coup = Deplacement.RIGHT;
      if (coup==Deplacement.RIGHT && peutBouger==true)
      { 
        for(int i=0;i<blocActifX.Length;i++)
        {if (tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]] == TypeBloc.None)
          { toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta; }
        }    
      }
    }
    void DeplacerCarreBas()
    { coup = Deplacement.DOWN;
      if (coup==Deplacement.DOWN && peutBouger==true)
      {
        for (int i = 0; i < blocActifY.Length; i++)
        {
          if (tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]] == TypeBloc.None)
          { toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta; }
        }
      }
    }

    void EffectuerRotationHoraire()
    {
      
    }
    void AfficherMessageBox()
    {
      
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
    void AfficherJeu()
    {
      blocActifY = new int[] { 0, 0, 1, 1 };
      blocActifX = new int[] { 0, 1, 0, 1 };
      for (int i=0;i<tableauDeJeu.GetLength(0);i++)
      {
        for (int j = 0; j < tableauDeJeu.GetLength(1); j++)
        { 
          if(tableauDeJeu[i,j] == TypeBloc.Gelé)
          {
            toutesImagesVisuelles[i, j].BackColor = Color.Gray;
          }
          else
          {
            toutesImagesVisuelles[i,j].BackColor = Color.Black;
          }
        }
      }

      for(int i=0;i<4;i++)
      {
        toutesImagesVisuelles[pointDepartY+blocActifY[i], pointDepartX+blocActifX[i]].BackColor = Color.Magenta;
      }
    }
    private void tetris_KeyPress(object sender, KeyPressEventArgs e)
    {
      if(e.KeyChar=='a')
      {
        AfficherJeu();
        bool Valider = VerifierPeutBouger();
        DeplacerCarreGauche();    
      }
      else if (e.KeyChar == 'd')
      {
        AfficherJeu();
        bool Valider = VerifierPeutBouger();
        DeplacerCarreDroite();        
      }
      else if (e.KeyChar == 's')
      {
        AfficherJeu();
        bool Valider = VerifierPeutBouger();
        DeplacerCarreBas();              
      }
<<<<<<< HEAD
     
=======
<<<<<<< HEAD
      else if(e.KeyChar == 'w')
      {
        
      }
=======
      AfficherJeu();
>>>>>>> e4fa77d21840490f236f09b30d7b04b855cf8179
>>>>>>> 5a840172364bf03ca172d4dfbb6b5fa1c585b46a
    }

    private void modifierParamètreToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FrmParametre aide = new FrmParametre();
      aide.ShowDialog();
    }

    private void descendreBlockAuto_Tick(object sender, EventArgs e)
    {
      DeplacerCarreBas();
=======
<<<<<<< HEAD
      DeplacerCarreBas();
      AfficherJeu();
      if(tableauDeJeu[pointDepartY + 2, pointDepartX] == TypeBloc.None)
      {
        DeplacerCarreBas();
      }
<<<<<<< HEAD

=======
>>>>>>> 1a1bef28db9e19e5f63f6becfd75dabbdff8b3e6
>>>>>>> e4fa77d21840490f236f09b30d7b04b855cf8179
>>>>>>> 5a840172364bf03ca172d4dfbb6b5fa1c585b46a
    }
  }








}
