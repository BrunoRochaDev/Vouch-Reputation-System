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
        Network network;
        Diagram _diagram;
        Random _random;

        public DiagramForm(Network _network)
        {
            network = _network;
            InitializeComponent();
            _diagram = new Diagram(network.allNodes, _network.observerNode);
            _random = new Random();
            SetupUI();
        }

        private void SetupUI()
        {
            NetworkReachSlider.Value = network.networkReach;
            NetworkReachSliderChange(NetworkReachSlider, null);

            ReputationReachSlider.Value = network.reputationReach;
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
            network.networkReach = _TrackBar.Value;
        }

        private void ReputationReachSliderChange(object sender, EventArgs e)
        {
            TrackBar _TrackBar = sender as TrackBar;
            ReputationReachLabel.Text = "Reputation Reach: " + _TrackBar.Value;
            network.reputationReach = _TrackBar.Value;
        }
    }
}