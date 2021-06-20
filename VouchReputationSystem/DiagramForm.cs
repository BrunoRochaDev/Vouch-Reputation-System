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
            InitializeComponent();

            network = _network;
            _random = new Random();

            DrawDiagram();
            SetupUI();
        }

        private void DrawDiagram()
        {
            _diagram = new Diagram(network.allNodes, network.observerNode);
            Invalidate();
        }

        private void SetupUI()
        {
            //NetworkReachSlider
            NetworkReachSlider.Value = network.networkReach;
            NetworkReachSliderChange(NetworkReachSlider, null);

            //Default Node Rep
            DefaultRep.Value = (decimal)network.defaultNodeReputation;
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

        private void RefreshNetwork(object sender, EventArgs e)
        {
            network.RefreshNetwork();
            DrawDiagram();
        }

        private void DefaultRepChanged(object sender, EventArgs e)
        {
            NumericUpDown _NumericUpDown = sender as NumericUpDown;
            network.defaultNodeReputation = (float)_NumericUpDown.Value;
        }
    }
}