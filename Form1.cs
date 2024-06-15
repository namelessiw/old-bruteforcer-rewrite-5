using System.Globalization;

namespace old_bruteforcer_rewrite_5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Player p;

        private void Form1_Load(object sender, EventArgs e)
        {
            NewPlayer();
            CreatePlayerRange();

            /*PlayerRange2.SetFloor(408);
            PlayerRange2.SetCeiling(363);
            PlayerRange2 p = new(362.1, 367.4, -3.6);
            List<PlayerRange2> ranges = PlayerRange2.CeilingCollision(p);
            MessageBox.Show(string.Join(",\n", ranges.Select(r => r.ToString())));*/
        }

        private void NewPlayer()
        {
            Player.SetFloorY(TxtFloorY.Text);
            p = new(double.Parse(TxtPlayerY.Text, CultureInfo.InvariantCulture), 0, 0, true, false, []);
            updateLabel();
        }

        private void updateLabel()
        {
            LblInfo.Text = "Info:\n" + p.ToString() + "\n" + p.GetStrat(false) + "\n" + p.GetMacro();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            NewPlayer();
        }

        private void BtnStep_Click(object sender, EventArgs e)
        {
            Input i = Input.None;

            if (ChkPress.Checked)
            {
                if (p.CanPress())
                {
                    i |= Input.Press;
                }
                else
                {
                    MessageBox.Show("Cannot press on this frame");
                    return;
                }
            }
            if (ChkRelease.Checked)
            {
                if (ChkPress.Checked || p.CanRelease())
                {
                    i |= Input.Release;
                }
                else
                {
                    MessageBox.Show("Cannot release on this frame");
                    return;
                }
            }

            p.Step(i);
            updateLabel();
        }

        private void BtnToStable_Click(object sender, EventArgs e)
        {
            while (!p.IsStable())
            {
                p.Step(Input.None);
                updateLabel();
            }
        }

        private void BtnDoStrat_Click(object sender, EventArgs e)
        {
            NewPlayer();
            try
            {
                p.DoStrat(TxtStrat.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            updateLabel();
        }

        PlayerRange2 pr;

        private void UpdateLabelRange(List<PlayerRange2> ranges)
        {
            LblInfo.Text = "Info:\n" + string.Join("\n", ranges.Select(r => r.ToString()));
        }

        private void CreatePlayerRange()
        {
            PlayerRange2.SetFloor(double.Parse(TxtFloorY.Text, CultureInfo.InvariantCulture));
            PlayerRange2.SetCeiling(double.Parse(TxtCeiling.Text, CultureInfo.InvariantCulture));
            pr = new(double.Parse(TxtYUpper.Text, CultureInfo.InvariantCulture), double.Parse(TxtYLower.Text, CultureInfo.InvariantCulture), double.Parse(TxtVSpeed.Text, CultureInfo.InvariantCulture));
        }

        private void BtnFloorCollision_Click(object sender, EventArgs e)
        {
            CreatePlayerRange();
            List<PlayerRange2> ranges = PlayerRange2.FloorCollision(pr);
            UpdateLabelRange(ranges);
        }

        private void BtnCeilingCollision_Click(object sender, EventArgs e)
        {
            CreatePlayerRange();
            List<PlayerRange2> ranges = PlayerRange2.CeilingCollision(pr);
            UpdateLabelRange(ranges);
        }
    }
}
