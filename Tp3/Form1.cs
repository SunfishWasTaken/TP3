using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace TP3
/*Jeu de tetris, dans lequel le joueur peut faire bouger des blocs, suivant les règles générales du jeu classique Tetris.
 * 
 * 
 * Auteurs: Anthony Custeau et Antoine Roy-Lachance
 * */
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
    //enum contenant tous les états possibles du tableau de jeu
    enum TypeBloc { None, Gelé, Carré, Ligne, T, L, J, S, Z }
    //Tableau contenant toutes les couleurs possibles du jeu
    Color[] toutesLesCouleurs = new Color[] { Color.Black, Color.Gray, Color.Magenta, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.LightBlue, Color.Turquoise };
    //Entier représentant le nombre de colonnes du jeu
    int nbColonnesJeu = 10;
    //Entier représentant le nombre de lignes du jeu
    int nbLignesJeu = 20;
    //Tableau 2D représentant le tableau de jeu
    TypeBloc[,] tableauDeJeu = null;
    //Variable servant de point de départ en Y
    int pointDepartY = 0;
    //Variable servant de point de départ en X
    int pointDepartX = 4;
    //Tableau d'entiers contenant les positions en Y du bloc actif
    int[] blocActifY = new int[4];
    //Tableau d'entiers contenant les positions en X du bloc actif
    int[] blocActifX = new int[4];
    //Variable booléene servant à déterminer si un bloc peut bouger
    bool peutBouger = false;
    //Score actif du joueur
    int scoreActif = 0;
    
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

    //ARoy-Lachance
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
    //ACusteau
    //Rôle : Trouver aléatoirement une nouvelle pièce à générer
    //Paramètre : Aucun
    //Retour : Le type de bloc à générer, choisi aléatoirement.
    TypeBloc TrouverNouvellePieceAGenerer()
    {
      TypeBloc blocActif = TypeBloc.None;
      int pieceActive = 0;
      Random rnd = new Random();
      pieceActive = rnd.Next(2, 9);

      if (pieceActive == 2)
      {
        blocActif = TypeBloc.Carré;
      }
      if (pieceActive == 3)
      {
        blocActif = TypeBloc.Ligne;
      }
      if (pieceActive == 4)
      {
        blocActif = TypeBloc.T;
      }
      if (pieceActive == 5)
      {
        blocActif = TypeBloc.L;
      }
      if (pieceActive == 6)
      {
        blocActif = TypeBloc.J;
      }
      if (pieceActive == 7)
      {
        blocActif = TypeBloc.S;
      }
      if (pieceActive == 8)
      {
        blocActif = TypeBloc.Z;
      }
      return blocActif;
    }
    //ACusteau
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
    
    //A.Roy-Lachance
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
    //A.Roy-Lachance

    //ACusteau
    //Rôle : Déplacer le bloc carré vers la gauche.
    //Paramètre : aucun
    //Retour : Deplacement représentant le coup entré par le joueur(gauche).
    Deplacement DeplacerCarreGauche()
    {
      coup = Deplacement.LEFT;
      {
        //S'assure que le bloc peut bouger
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
    //ACusteau
    ///Rôle : Déplacer le bloc carré vers la droite.
    ///Paramètre : aucun
    ///Retour : Deplacement représentant le coup entré par le joueur (droite).
    Deplacement DeplacerCarreDroite()
    {
      coup = Deplacement.RIGHT;
      //S'assure que le bloc peut bouger
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
    //ACusteau
    ///Rôle : Déplacer le bloc carré vers le bas.
    ///Paramètre : aucun
    ///Retour : Deplacement représentant le coup entré par le joueur(bas).
    Deplacement DeplacerCarreBas()
    {
      coup = Deplacement.DOWN;
      //S'assure que le bloc peut bouger
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
    //ACusteau
    ///Rôle : Effectuer la rotation d'un bloc dans le sens horaire.
    ///Paramètre : Aucun
    ///Retour : Aucun
    void EffectuerRotationHoraire()
    {
      int[] temporaire = new int[4];

      for (int i = 0; i < temporaire.Length; i++)
      {
        temporaire[i] = blocActifX[i];
      }
      //Effectue la rotation
      for (int j = 0; j < temporaire.Length; j++)
      {
        blocActifX[j] = -blocActifY[j];
        blocActifY[j] = temporaire[j];
      }
    }
    //ACusteau
    ///Rôle : Vérifier si un nouveau bloc peut être généré dans le tableau de jeu.
    ///Paramètre : Aucun
    ///Retour : Booléen indiquant si la partie est terminée (true) ou non (false).
    bool VerifierSiFinPartie()
    {
      bool partieTerminee = false;

      //Vérifie si le point de départ est occupé par un bloc gelé, ce qui cause la fin de la partie.
      if (tableauDeJeu[pointDepartY, pointDepartX] == TypeBloc.Gelé)
      {
        partieTerminee = true;
      }
      return partieTerminee;
    }
    //ACusteau
    ///Rôle : Afficher une boîte de dialogue indiquant au joueur que la partie est terminée.
    ///Paramètre : Aucun
    ///Retour : Aucun
    void AfficherFinPartie()
    {
        MessageBox.Show("Partie terminée", "La partie est terminée", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    /// Rôle: Fonction qui verifie si une ligne est complète. Si c'est le cas, la fonction effacera les blocs de cette ligne
    /// 
    /// Paramètre:Aucun paramètre
    /// Retour: retourne un entier représentant le nombre de lignes complètées par le joueur
   
    //A.Roy-Lachance
     int estUneLigneComplete()
    {
      int nbLigneCompleter = 0;
      int nbGeler = 0;
      bool ligneComplete = false;
      for (int i = 0; i < tableauDeJeu.GetLength(0); i++)
      {
        nbGeler = 0;
        for (int j = 0; j < tableauDeJeu.GetLength(1); j++)
        {          
          if (tableauDeJeu[i, j] == TypeBloc.Gelé)
          {
            nbGeler++;
            if (nbGeler == nbColonnesJeu)
            {
              nbLigneCompleter++;
              ligneComplete = true;
              for (int compteur = 0; compteur < nbColonnesJeu; compteur++)
              { tableauDeJeu[i, compteur] = TypeBloc.None; }
              if (nbLigneCompleter >= 1)
              {
                for (int g = nbLignesJeu-1; g > 0; g--)
                {
                  for (int h = 0; h < nbColonnesJeu; h++)
                  {
                    tableauDeJeu[g, h] = tableauDeJeu[g - nbLigneCompleter, h];
                  }
                }
              }
             }      
          }
        }
      
      }
      scoreActif = nbLigneCompleter * 100;
      return nbLigneCompleter;
    }
    //A.Roy-Lachance

 
    //Acusteau
    ///Rôle : Recommencer la partie lorsqu'elle est terminée.
    ///Paramètre : Aucun
    ///Retour : Aucun
    void RecommencerPartie()
    {
      tableauDeJeu = new TypeBloc[nbLignesJeu, nbColonnesJeu];

      //Réinitialise le tableau de jeu
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
    //ACusteau
    ///Rôle: Fonction qui appelle les tests unitaires à exécuter
    ///Aucun Paramètre
    ///Aucun retour
    void ExecuterTestsUnitaires()
    {
      ExecuterTestUneLigne();
      ExecuterTestDeuxLigne();
      ExecuterTestTroisLigne();
      ExecuterTestPartieTerminee();
      ExecuterTestPartieNonTerminee();
      ExecuterTestRotationCentre();
      ExecuterTestRotationDroite();
      ExecuterTestRotationGauche();
      ExecuterTestRotationEntoure();
      //ExecuterTestABC();
    }

    //TypeBloc[,] methodeTest(TypeBloc[,] tableauDeJeu)
    // { 
    //int resultat = estUneLigneComplete();
    //  return tableauDeJeu;
    //  }


    //ACusteau
    //Rôle : Tester la rotation d'un bloc au centre du tableau.
    //Paramètres : Aucun
    //Retour : Aucun
    void ExecuterTestRotationCentre()
    {
      int pointDepartYTest = pointDepartY;
      int pointDepartXTest = pointDepartX;
      pointDepartYTest = 5;
      pointDepartXTest = 4;
      int[] blocActifYTest = new int[4];
      int[] blocActifXTest = new int[4];

      EffectuerRotationHoraire();

      for (int i = 0; i < blocActifX.Length; i++)
      {
        blocActifYTest[i] = blocActifY[i];
        blocActifXTest[i] = blocActifX[i];
      }

      for (int j = 0; j < blocActifX.Length; j++)
      {
        Debug.Assert(blocActifYTest[j] == -blocActifX[j], "Erreur lors de la rotation d'un bloc au centre");
        Debug.Assert(blocActifXTest[j] == blocActifY[j], "Erreur lors de la rotation d'un bloc au centre");
      }
    }
    //ACusteau
    ///Rôle : Tester la rotation d'un bloc à droite du tableau.
    ///Paramètres : Aucun
    ///Retour : Aucun
    void ExecuterTestRotationGauche()
    {
      int pointDepartYTest = pointDepartY;
      int pointDepartXTest = pointDepartX;
      pointDepartYTest = 5;
      pointDepartXTest = 0;
      int[] blocActifYTest = new int[4];
      int[] blocActifXTest = new int[4];

      EffectuerRotationHoraire();

      for (int i = 0; i < blocActifX.Length; i++)
      {
        blocActifYTest[i] = blocActifY[i];
        blocActifXTest[i] = blocActifX[i];
      }

      for (int j = 0; j < blocActifX.Length; j++)
      {
        Debug.Assert(blocActifYTest[j] == -blocActifX[j], "Erreur lors de la rotation d'un bloc a gauche");
        Debug.Assert(blocActifXTest[j] == blocActifY[j], "Erreur lors de la rotation d'un bloc a gauche");
      }
    }
    //ACusteau
    ///Rôle : Tester la rotation d'un bloc à droite du tableau.
    ///Paramètres : Aucun
    ///Retour : Aucun
    void ExecuterTestRotationDroite()
    {
      int pointDepartYTest = pointDepartY;
      int pointDepartXTest = pointDepartX;
      pointDepartYTest = 5;
      pointDepartXTest = nbColonnesJeu - 1;
      int[] blocActifYTest = new int[4];
      int[] blocActifXTest = new int[4];

      EffectuerRotationHoraire();

      for (int i = 0; i < blocActifX.Length; i++)
      {
        blocActifYTest[i] = blocActifY[i];
        blocActifXTest[i] = blocActifX[i];
      }

      for (int j = 0; j < blocActifX.Length; j++)
      {
        Debug.Assert(blocActifYTest[j] == -blocActifX[j], "Erreur lors de la rotation d'un bloc a droite");
        Debug.Assert(blocActifXTest[j] == blocActifY[j], "Erreur lors de la rotation d'un bloc a droite");
      }
    }
    //ACusteau
    ///Rôle : S'assurer qu'un bloc qui est entouré d'autres bloc ne fera pas de rotation
    ///Paramètres : Aucun
    ///Retour : Aucun
    void ExecuterTestRotationEntoure()
    {
      int pointDepartYTest = pointDepartY;
      int pointDepartXTest = pointDepartX;
      pointDepartYTest = 5;
      pointDepartXTest = 0;
      int[] blocActifYTest = new int[4];
      int[] blocActifXTest = new int[4];

      EffectuerRotationHoraire();

      for (int i = 0; i < blocActifX.Length; i++)
      {
        blocActifYTest[i] = blocActifY[i];
        blocActifXTest[i] = blocActifX[i];
      }

      for (int j = 0; j < blocActifX.Length; j++)
      {
        Debug.Assert(blocActifYTest[j] == blocActifYTest[j], "Erreur lors de la rotation d'un bloc qui ne peut faire de rotation");
        Debug.Assert(blocActifXTest[j] == blocActifXTest[j], "Erreur lors de la rotation d'un bloc qui ne peut faire de rotation");
      }
    }
    //ACusteau
    ///Rôle : Tester si la partie est terminée.
    ///Paramètres : Aucun
    ///Retour : Aucun
    void ExecuterTestPartieTerminee()
    {
      //bool partieTerminee = VerifierSiFinPartie();
     // tableauDeJeu[pointDepartY, pointDepartX] = TypeBloc.Gelé;

      //Debug.Assert(partieTerminee == true, "Erreur lors de la vérification de la partie terminée");
    }
    //ACusteau
    ///Rôle : Tester si la partie n'est pas terminée.
    ///Paramètre : Aucun
    ///Retour : Aucun
    void ExecuterTestPartieNonTerminee()
    {
      //bool partieTerminee = VerifierSiFinPartie();
      //tableauDeJeu[pointDepartY, pointDepartX] = TypeBloc.Gelé;

      //Debug.Assert(partieTerminee == false, "Erreur lors de la vérification de la partie non terminée");
    }
    // ARoy-Lachance
    ///Rôle: Fonction permettant de tester si une ligne est enlever correctement lorsqu'elle est complète
  /// Aucun paramètre
  /// Aucun retour
    void ExecuterTestUneLigne()
    {
      // Mise en place des données du test
     // TypeBloc[,] tableauTest = new TypeBloc[1, nbColonnesJeu];
      //for(int i=0;i<nbColonnesJeu;i++)
     // { tableauTest[0, i] = TypeBloc.Gelé; }

       //Exécuter de la méthode à tester
     // tableauTest = methodeTest(tableauTest);
       //Validation des résultats
     //for (int i = 0; i < nbColonnesJeu; i++)
     //{ Debug.Assert(tableauTest[0,i]==TypeBloc.None); }
     
       //Clean-up
    }
    //ARoy-Lachance
    ///Rôle: Fonction permettant de tester si les lignes sont enlevées correctement lorsqu'il y en a deux de complétées en même temps
    ///Aucun Paramètre
    ///Aucun retour
    void ExecuterTestDeuxLigne()
    {
     // TypeBloc[,] tableauTest = new TypeBloc[2, nbColonnesJeu - 1];
     // for (int i = 0; i < 2; i++)
   //   {
    //    for (int j = 0; j < nbColonnesJeu; j++)
    //    { tableauTest[i, j] = TypeBloc.Gelé; }
   //   }
      // Exécuter de la méthode à tester
   //   tableauTest = methodeTest(tableauTest);
      // Validation des résultats

  //    for (int j = 0; j < nbColonnesJeu; j++)
   //   {
    //    Debug.Assert(tableauTest[0, j] == TypeBloc.None);
    //    Debug.Assert(tableauDeJeu[1, j] == TypeBloc.None);
   //   }
    }
    //ARoy-Lachance
    ///Rôle:Fonction permettant de tester si les lignes sont enlevées correctement lorsqu'il y en a trois de complétées en même temps
    ///Aucun Paramètre
    ///Aucun retour
    void ExecuterTestTroisLigne()
    {
      // TypeBloc[,] tableauTest = new TypeBloc[2, nbColonnesJeu - 1];
      // for (int i = 0; i < 3; i++)
      //   {
      //    for (int j = 0; j < nbColonnesJeu; j++)
      //    { tableauTest[i, j] = TypeBloc.Gelé; }
      //   }
      // Exécuter de la méthode à tester
      //   tableauTest = methodeTest(tableauTest);
      // Validation des résultats

      //    for (int j = 0; j < nbColonnesJeu; j++)
      //   {
      //    Debug.Assert(tableauTest[0, j] == TypeBloc.None);
      //    Debug.Assert(tableauDeJeu[1, j] == TypeBloc.None);
      //Debug.Assert(tableauDeJeu[2,j]==TypeBloc.None);
      //   }
    }
    //ARoy-Lachance
    private void tableauJeu_Paint(object sender, PaintEventArgs e)
    {

    }
    #endregion
    ///Rôle : Afficher le tableau de jeu courant avec les blocs gelés et le bloc en court.
    ///Paramètre : Aucun
    ///Retour : Aucun
    //A.Roy-Lachance
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
            else if (tableauDeJeu[i, j] == TypeBloc.None)
            {
              toutesImagesVisuelles[i, j].BackColor = Color.Black;
            }
          }
        }

        for (int i = 0; i < 4; i++)
        {
          toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta;
        }
        for (int i = 0; i < 4; i++)
        {
          if (pointDepartY + blocActifY[i] >= nbLignesJeu - 1)
          {
            pointDepartY = nbLignesJeu - 2;
            tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]] = TypeBloc.Gelé;
          }
        }


        for (int i = 0; i < 4; i++)
        {
          if (pointDepartY == nbLignesJeu - 2 || tableauDeJeu[pointDepartY + 1 + blocActifY[i], pointDepartX + blocActifX[i]] == TypeBloc.Gelé)
          {
            tableauDeJeu[pointDepartY, pointDepartX + blocActifX[i]] = TypeBloc.Gelé;
            tableauDeJeu[pointDepartY, pointDepartX + 1 + blocActifX[i]] = TypeBloc.Gelé;
            tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]] = TypeBloc.Gelé;
            tableauDeJeu[pointDepartY + blocActifY[i], pointDepartX + 1 + blocActifX[i]] = TypeBloc.Gelé;
            pointDepartY = 0;
            pointDepartX = 4;
            toutesImagesVisuelles[pointDepartY + blocActifY[i], pointDepartX + blocActifX[i]].BackColor = Color.Magenta;
          }
          int resultat = estUneLigneComplete();
          if (resultat > 1)
          { score.Text += scoreActif; }
          if (resultat > 0)
          { score.Text = "Score: 100"; }
        }

     }
     //A.Roy-Lachance

     //ACusteau
     ///Rôle : Recevoir les touches entrées par le joueur pour déplacer les blocs
     ///Paramètres :
     ///Retour : Aucun
    private void tetris_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == 'a')
      {
        bool partieTerminee = VerifierSiFinPartie();  

        if(partieTerminee)
        {
          AfficherFinPartie();
          RecommencerPartie();
        }
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
        bool partieTerminee = VerifierSiFinPartie();

        if (partieTerminee)
        {
          AfficherFinPartie();
          RecommencerPartie();
        }
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
        bool partieTerminee = VerifierSiFinPartie();

        if (partieTerminee)
        {
          AfficherFinPartie();
          RecommencerPartie();
        }
        coup = Deplacement.DOWN;
        bool Valider = VerifierPeutBouger();


        if (peutBouger == true)
        {
          DeplacerCarreBas();
          AfficherJeu();
        }


      }
      else if (e.KeyChar == 'e')
      {
        bool valider = VerifierPeutBouger();
        coup = Deplacement.HORAIRE;
        if(peutBouger == true)
        {
          EffectuerRotationHoraire();
          AfficherJeu();
        }
        
      }
      
    }
    //Acusteau


    //A.Roy-Lachance
    private void modifierParamètreToolStripMenuItem_Click(object sender, EventArgs e)
    { decimal nbColonnesChoisit = 0;
      decimal nbLignesChoisit = 0;
      Form2 aide = new Form2();
      aide.ShowDialog();
      aide.SpecifierInfo( nbColonnesChoisit, nbLignesChoisit);
      aide.Show();
    }
    //A.Roy-Lachance

    //ACusteau
    ///Rôle : S'occuper de faire descendre le bloc automatiquement à toutes les secondes.
    ///Paramètres :
    ///Retour : Aucun
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
    //ACusteau
    ///Rôle : Permettre au joueur de recommencer la partie.
    ///Paramètres :
    ///Retour : Aucun
    private void button1_Click(object sender, EventArgs e)
    {
      RecommencerPartie();
    }
  }

 }





