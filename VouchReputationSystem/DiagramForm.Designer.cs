
namespace VouchReputationSystem
{
    partial class DiagramForm
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
            this.NetworkReachSlider = new System.Windows.Forms.TrackBar();
            this.NetworkReachLabel = new System.Windows.Forms.Label();
            this.BrunoLabel = new System.Windows.Forms.Label();
            this.refreshButton = new System.Windows.Forms.Button();
            this.DefaultRep = new System.Windows.Forms.NumericUpDown();
            this.DefaultRepLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NetworkReachSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultRep)).BeginInit();
            this.SuspendLayout();
            // 
            // NetworkReachSlider
            // 
            this.NetworkReachSlider.Cursor = System.Windows.Forms.Cursors.Default;
            this.NetworkReachSlider.Location = new System.Drawing.Point(12, 35);
            this.NetworkReachSlider.Maximum = 6;
            this.NetworkReachSlider.Name = "NetworkReachSlider";
            this.NetworkReachSlider.Size = new System.Drawing.Size(151, 45);
            this.NetworkReachSlider.TabIndex = 0;
            this.NetworkReachSlider.Scroll += new System.EventHandler(this.NetworkReachSliderChange);
            // 
            // NetworkReachLabel
            // 
            this.NetworkReachLabel.AutoSize = true;
            this.NetworkReachLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.NetworkReachLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(243)))));
            this.NetworkReachLabel.Location = new System.Drawing.Point(32, 12);
            this.NetworkReachLabel.Name = "NetworkReachLabel";
            this.NetworkReachLabel.Size = new System.Drawing.Size(127, 18);
            this.NetworkReachLabel.TabIndex = 1;
            this.NetworkReachLabel.Text = "Network Reach: []";
            this.NetworkReachLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BrunoLabel
            // 
            this.BrunoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BrunoLabel.AutoSize = true;
            this.BrunoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.BrunoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(243)))));
            this.BrunoLabel.Location = new System.Drawing.Point(593, 423);
            this.BrunoLabel.Name = "BrunoLabel";
            this.BrunoLabel.Size = new System.Drawing.Size(195, 18);
            this.BrunoLabel.TabIndex = 4;
            this.BrunoLabel.Text = "github.com/BrunoRochaDev";
            this.BrunoLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(35, 122);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshNetwork);
            // 
            // DefaultRep
            // 
            this.DefaultRep.DecimalPlaces = 2;
            this.DefaultRep.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.DefaultRep.Location = new System.Drawing.Point(210, 35);
            this.DefaultRep.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DefaultRep.Name = "DefaultRep";
            this.DefaultRep.Size = new System.Drawing.Size(127, 20);
            this.DefaultRep.TabIndex = 6;
            this.DefaultRep.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.DefaultRep.ValueChanged += new System.EventHandler(this.DefaultRepChanged);
            // 
            // DefaultRepLabel
            // 
            this.DefaultRepLabel.AutoSize = true;
            this.DefaultRepLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.DefaultRepLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(243)))));
            this.DefaultRepLabel.Location = new System.Drawing.Point(191, 14);
            this.DefaultRepLabel.Name = "DefaultRepLabel";
            this.DefaultRepLabel.Size = new System.Drawing.Size(169, 18);
            this.DefaultRepLabel.TabIndex = 7;
            this.DefaultRepLabel.Text = "Default Node Reputation";
            this.DefaultRepLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DiagramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(53)))), ((int)(((byte)(65)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DefaultRepLabel);
            this.Controls.Add(this.DefaultRep);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.BrunoLabel);
            this.Controls.Add(this.NetworkReachLabel);
            this.Controls.Add(this.NetworkReachSlider);
            this.Name = "DiagramForm";
            this.Text = "Vouch Reputation System DEMO";
            ((System.ComponentModel.ISupportInitialize)(this.NetworkReachSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DefaultRep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar NetworkReachSlider;
        private System.Windows.Forms.Label NetworkReachLabel;
        private System.Windows.Forms.Label BrunoLabel;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.NumericUpDown DefaultRep;
        private System.Windows.Forms.Label DefaultRepLabel;
    }
}

