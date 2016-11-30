namespace TP3
{
  partial class FrmParametre
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.choixNbColonnes = new System.Windows.Forms.NumericUpDown();
      this.choixNbLignes = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.choixNbColonnes)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.choixNbLignes)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 29);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(66, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "nbColonnes:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 55);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(53, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "nbLignes:";
      // 
      // choixNbColonnes
      // 
      this.choixNbColonnes.Location = new System.Drawing.Point(84, 29);
      this.choixNbColonnes.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
      this.choixNbColonnes.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.choixNbColonnes.Name = "choixNbColonnes";
      this.choixNbColonnes.Size = new System.Drawing.Size(37, 20);
      this.choixNbColonnes.TabIndex = 2;
      this.choixNbColonnes.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
      // 
      // choixNbLignes
      // 
      this.choixNbLignes.Location = new System.Drawing.Point(84, 55);
      this.choixNbLignes.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
      this.choixNbLignes.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.choixNbLignes.Name = "choixNbLignes";
      this.choixNbLignes.Size = new System.Drawing.Size(37, 20);
      this.choixNbLignes.TabIndex = 3;
      this.choixNbLignes.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
      // 
      // FrmParametre
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(352, 261);
      this.Controls.Add(this.choixNbLignes);
      this.Controls.Add(this.choixNbColonnes);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "FrmParametre";
      this.Text = "FrmParametre";
      ((System.ComponentModel.ISupportInitialize)(this.choixNbColonnes)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.choixNbLignes)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.NumericUpDown choixNbColonnes;
    private System.Windows.Forms.NumericUpDown choixNbLignes;
  }
}