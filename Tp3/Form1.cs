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
      AfficherCarre();

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
    void AfficherCarre()
    {
      blocActifY = new int[] { 0, 0, 1, 1 };
      blocActifX = new int[] { 0, 1, 0, 1 };
      for (int j = 2; j < 4; j++)
      {
        tableauDeJeu[blocActifY[1], blocActifX[j]] = TypeBloc.Carré;
        tableauDeJeu[blocActifY[0], blocActifX[j]] = TypeBloc.Carré;
        if (tableauDeJeu[blocActifY[0], blocActifX[j]] == TypeBloc.Carré && tableauDeJeu[blocActifY[1], blocActifX[j]] == TypeBloc.Carré)
        {         
            toutesImagesVisuelles[0, pointDepartX].BackColor = Color.Magenta;
          toutesImagesVisuelles[0, pointDepartX + 1].BackColor = Color.Magenta;
            toutesImagesVisuelles[1, pointDepartX].BackColor = Color.Magenta;
          toutesImagesVisuelles[1, pointDepartX + 1].BackColor = Color.Magenta;   
        }
      }
    }
    void DeplacerCarreGauche()
    {
      if (pointDepartX > 0)
      {

        pointDepartX = pointDepartX - 1;
        toutesImagesVisuelles[pointDepartY, pointDepartX].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY, pointDepartX+1].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX+1].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY, pointDepartX + 2].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX + 2].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY, pointDepartX + 3].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX + 3].BackColor = Color.Black;
      }
      else if(pointDepartX==0)
      {
        toutesImagesVisuelles[pointDepartY, pointDepartX].BackColor = Color.Magenta;
      }
    }
    void DeplacerCarreDroite()
    {
      if (pointDepartX < nbColonnesJeu - 1)
      {
        pointDepartX = pointDepartX + 1;
        toutesImagesVisuelles[pointDepartY, pointDepartX].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY, pointDepartX + 1].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX + 1].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY, pointDepartX - 1].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX - 1].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY, pointDepartX - 2].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX - 2].BackColor = Color.Black;
      }
    }
    void DeplacerCarreBas()
    {
      if (pointDepartY < nbLignesJeu - 1)
      {
        pointDepartY = pointDepartY+1;
        toutesImagesVisuelles[pointDepartY, pointDepartX].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY, pointDepartX+1].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY + 1, pointDepartX+1].BackColor = Color.Magenta;
        toutesImagesVisuelles[pointDepartY - 1, pointDepartX].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY - 1, pointDepartX + 1].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY, pointDepartX - 1].BackColor = Color.Black;
        toutesImagesVisuelles[pointDepartY - 1, pointDepartX - 1].BackColor = Color.Black;
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

    private void tetris_KeyPress(object sender, KeyPressEventArgs e)
    {
      if(e.KeyChar=='a')
      {
        DeplacerCarreGauche();
      }
      else if (e.KeyChar == 'd')
      {
        DeplacerCarreDroite();
      }
      else if (e.KeyChar == 's')
      {
        DeplacerCarreBas();
      }
    }

    private void modifierParamètreToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FrmParametre aide = new FrmParametre();
      aide.ShowDialog();
    }

    private void descendreBlockAuto_Tick(object sender, EventArgs e)
    {
      DeplacerCarreBas();
    }
  }








}
