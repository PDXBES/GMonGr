namespace GMonGr
{
  partial class frmSplashScreen
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
      this.components = new System.ComponentModel.Container();
      Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
      this.lblVersionInfo = new Infragistics.Win.Misc.UltraLabel();
      this.tmrSplashScreen = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // lblVersionInfo
      // 
      appearance1.BackColor = System.Drawing.Color.Transparent;
      this.lblVersionInfo.Appearance = appearance1;
      this.lblVersionInfo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblVersionInfo.Location = new System.Drawing.Point(178, 237);
      this.lblVersionInfo.Name = "lblVersionInfo";
      this.lblVersionInfo.Size = new System.Drawing.Size(172, 23);
      this.lblVersionInfo.TabIndex = 0;
      this.lblVersionInfo.Text = "Version X.X, January 1st, 2000";
      // 
      // tmrSplashScreen
      // 
      this.tmrSplashScreen.Tick += new System.EventHandler(this.tmrSplashScreen_Tick);
      // 
      // frmSplashScreen
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = global::GMonGr.Properties.Resources.GMonGrSplash;
      this.ClientSize = new System.Drawing.Size(354, 258);
      this.ControlBox = false;
      this.Controls.Add(this.lblVersionInfo);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmSplashScreen";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SplashScreen";
      this.TopMost = true;
      this.Load += new System.EventHandler(this.frmSplashScreen_Load);
      this.Click += new System.EventHandler(this.frmSplashScreen_Click);
      this.ResumeLayout(false);

    }

    #endregion

    private Infragistics.Win.Misc.UltraLabel lblVersionInfo;
    private System.Windows.Forms.Timer tmrSplashScreen;
  }
}