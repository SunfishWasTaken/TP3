using System;
using System.Drawing;
using System.Windows.Forms;

namespace TP3
{
  public partial class tetris : Form
  {
    public tetris( )
    {
      InitializeComponent( );
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
    private void frmLoad( object sender, EventArgs e )
    {
      // Ne pas oublier de mettre en place les valeurs nécessaires à une partie.
      ExecuterTestsUnitaires();
      InitialiserSurfaceDeJeu(20,10);
      InitialiserTableauDeJeu();
      InitialiserCarreDeMouvement();
      AfficherCarre();
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
    enum TypeBloc { None, Gelé, Carré, Ligne, T, L, J, S, Z}
    int nbColonnesJeu = 10;
    int nbLignesJeu = 20;
    TypeBloc[,] tableauDeJeu = null;
    int[] blocActifY = new int[4] { 3, 4, 5, 6 };
    int[] blocActifX = new int[4] { 0, 1, 2, 3 };
    enum Deplacement { LEFT, RIGHT, DOWN, NOMOVE}

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
    void AfficherCarre()
    {
      for (int i = 0; i < 2; i++)
      {
        for (int j = 4; j < 6; j++)
        {
          tableauDeJeu[i, j] = TypeBloc.Carré;
          if (tableauDeJeu[i, j] == TypeBloc.Carré)
          {
            toutesImagesVisuelles[i, j].BackColor = Color.Magenta;
          }
        }
      }
    }
    Deplacement SaisirCoupJoueur()
    {
      ConsoleKeyInfo coup = Console.ReadKey();
      if(coup.Key == ConsoleKey.LeftArrow)
      {
        return Deplacement.LEFT;
      }
      if (coup.Key == ConsoleKey.RightArrow)
      {
        return Deplacement.RIGHT;
      }
      if (coup.Key == ConsoleKey.DownArrow)
      {
        return Deplacement.DOWN;
      }
      else
      {
        return Deplacement.NOMOVE;
      }
    }
    void InitialiserCarreDeMouvement()
    {
      for (int i = 0; i < 4; i++)
      {
        for (int j = 3; j < 7; j++)
        {
          toutesImagesVisuelles[i, j].BackColor = Color.White;
        }
      }
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
  }








}
