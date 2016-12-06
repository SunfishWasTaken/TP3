using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP3
{
  public partial class Form2 : Form
  {
    public Form2()
    {
      InitializeComponent();
    }

    private void choixNbColonnes_ValueChanged(object sender, EventArgs e)
    {
      decimal nbColonneChoisit = choixNbColonnes.Value;
    }

    private void choixNbLignes_ValueChanged(object sender, EventArgs e)
    {
      decimal nbLignesChoisit = choixNbLignes.Value;
    }
    public void SpecifierInfo(decimal nbColonneChoisit, decimal nbLignesChoisit)
    {
      nbColonneChoisit = choixNbColonnes.Value;
      nbLignesChoisit = choixNbLignes.Value;
    }
  }
}
