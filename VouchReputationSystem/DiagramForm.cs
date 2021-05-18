using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VouchReputationSystem.Classes;
using VouchReputationSystem.Classes.Diagram;

namespace VouchReputationSystem
{
    public partial class DiagramForm : Form
    {
        Diagram _diagram;
        Random _random;

        public DiagramForm(Network _network)
        {
            InitializeComponent();
            _diagram = new Diagram(_network.allNodes);
            _random = new Random();
            SetupUI(_network);
        }

        private void SetupUI(Network _network)
        {
            NetworkReachSlider.Value = _network.networkReach;
            NetworkReachSliderChange(NetworkReachSlider, null);

            ReputationReachSlider.Value = _network.reputationReach;
            ReputationReachSliderChange(ReputationReachSlider, null);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // draw with anti-aliasing and a 12 pixel border
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            _diagram.Draw(e.Graphics, Rectangle.FromLTRB(12, 12, ClientSize.Width - 12, ClientSize.Height - 12));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // redraw on resize
            Invalidate();
        }

        private void NetworkReachSliderChange(object sender, EventArgs e)
        {
            TrackBar _TrackBar = sender as TrackBar;
            NetworkReachLabel.Text = "Network Reach: " + _TrackBar.Value;
        }

        private void ReputationReachSliderChange(object sender, EventArgs e)
        {
            TrackBar _TrackBar = sender as TrackBar;
            ReputationReachLabel.Text = "Reputation Reach: " + _TrackBar.Value;
        }
    }
}