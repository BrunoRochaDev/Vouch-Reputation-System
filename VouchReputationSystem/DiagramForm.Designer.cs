
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
            this.ReputationReachLabel = new System.Windows.Forms.Label();
            this.ReputationReachSlider = new System.Windows.Forms.TrackBar();
            this.BrunoLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NetworkReachSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReputationReachSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // NetworkReachSlider
            // 
            this.NetworkReachSlider.Cursor = System.Windows.Forms.Cursors.Default;
            this.NetworkReachSlider.Location = new System.Drawing.Point(12, 35);
            this.NetworkReachSlider.Name = "NetworkReachSlider";
            this.NetworkReachSlider.Size = new System.Drawing.Size(151, 45);
            this.NetworkReachSlider.TabIndex = 0;
            this.NetworkReachSlider.Scroll += new System.EventHandler(this.NetworkReachSliderChange);
            // 
            // NetworkReachLabel
            // 
            this.NetworkReachLabel.AutoSize = true;
            this.NetworkReachLabel.Font = new System.Drawing.Font("Arial Narrow", 11F);
            this.NetworkReachLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(243)))));
            this.NetworkReachLabel.Location = new System.Drawing.Point(32, 12);
            this.NetworkReachLabel.Name = "NetworkReachLabel";
            this.NetworkReachLabel.Size = new System.Drawing.Size(106, 20);
            this.NetworkReachLabel.TabIndex = 1;
            this.NetworkReachLabel.Text = "Network Reach: []";
            this.NetworkReachLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReputationReachLabel
            // 
            this.ReputationReachLabel.AutoSize = true;
            this.ReputationReachLabel.Font = new System.Drawing.Font("Arial Narrow", 11F);
            this.ReputationReachLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(243)))));
            this.ReputationReachLabel.Location = new System.Drawing.Point(189, 12);
            this.ReputationReachLabel.Name = "ReputationReachLabel";
            this.ReputationReachLabel.Size = new System.Drawing.Size(121, 20);
            this.ReputationReachLabel.TabIndex = 3;
            this.ReputationReachLabel.Text = "Reputation Reach: []";
            this.ReputationReachLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReputationReachSlider
            // 
            this.ReputationReachSlider.Location = new System.Drawing.Point(169, 35);
            this.ReputationReachSlider.Name = "ReputationReachSlider";
            this.ReputationReachSlider.Size = new System.Drawing.Size(151, 45);
            this.ReputationReachSlider.TabIndex = 2;
            this.ReputationReachSlider.Scroll += new System.EventHandler(this.ReputationReachSliderChange);
            // 
            // BrunoLabel
            // 
            this.BrunoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BrunoLabel.AutoSize = true;
            this.BrunoLabel.Font = new System.Drawing.Font("Arial Narrow", 11F);
            this.BrunoLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(243)))));
            this.BrunoLabel.Location = new System.Drawing.Point(623, 421);
            this.BrunoLabel.Name = "BrunoLabel";
            this.BrunoLabel.Size = new System.Drawing.Size(165, 20);
            this.BrunoLabel.TabIndex = 4;
            this.BrunoLabel.Text = "github.com/BrunoRochaDev";
            this.BrunoLabel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // DiagramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(53)))), ((int)(((byte)(65)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BrunoLabel);
            this.Controls.Add(this.ReputationReachLabel);
            this.Controls.Add(this.ReputationReachSlider);
            this.Controls.Add(this.NetworkReachLabel);
            this.Controls.Add(this.NetworkReachSlider);
            this.Name = "DiagramForm";
            this.Text = "Vouch Reputation System";
            ((System.ComponentModel.ISupportInitialize)(this.NetworkReachSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReputationReachSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar NetworkReachSlider;
        private System.Windows.Forms.Label NetworkReachLabel;
        private System.Windows.Forms.Label ReputationReachLabel;
        private System.Windows.Forms.TrackBar ReputationReachSlider;
        private System.Windows.Forms.Label BrunoLabel;
    }
}

