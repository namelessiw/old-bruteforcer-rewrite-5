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

            List<(double upper, double lower)> ranges = PlayerRange.test(400.1, 408.3, 408, 3.6);
            MessageBox.Show(string.Join(",\n", ranges.Select(r => $"[{r.upper}, {r.lower}]")));
        }

        private void NewPlayer()
        {
            Player.SetFloorY(TxtFloorY.Text);
            p = new(double.Parse(TxtPlayerY.Text, CultureInfo.InvariantCulture), 0, 0, true, false, []);
            updateLabel();
        }

        private void updateLabel()
        {
            LblStats.Text = p.ToString() + "\n" + p.GetStrat(false) + "\n" + p.GetMacro();
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
    }
}
