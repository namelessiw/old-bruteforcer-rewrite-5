using System.Diagnostics;
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
            //NewPlayer();
            //CreatePlayerRange();
        }

        private void test()
        {
            /*Stopwatch sw = new();
            sw.Start();

            List<PlayerRange> results = Search.SearchRange();

            sw.Stop();

            LblInfo.Text = "Info:\n" + $"{results.Count} results in {sw.Elapsed}";

            results.Sort(new Comparison<PlayerRange>((a, b) => a.Frame == b.Frame ? 0 : a.Frame - b.Frame));

            LstResults.Items.Clear();

            int elements = Math.Min(results.Count, 10000);
            foreach (PlayerRange range in results[..elements])
            {
                LstResults.Items.Add(range);
            }*/
        }

        private void NewPlayer()
        {
            /*Player.SetFloorY(TxtFloorY.Text);
            p = new(double.Parse(TxtPlayerY.Text, CultureInfo.InvariantCulture), 0, 0, true, true, false, []);
            updateLabel();*/
        }

        private void updateLabel()
        {
            //LblInfo.Text = "Info:\n" + p.ToString() + "\n" + p.GetStrat(false) + "\n" + p.GetMacro();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            //NewPlayer();
        }

        private void BtnStep_Click(object sender, EventArgs e)
        {
            /*Input i = Input.None;

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
            updateLabel();*/
        }

        private void BtnToStable_Click(object sender, EventArgs e)
        {
            /*while (!p.IsStable())
            {
                p.Step(Input.None);
                updateLabel();
            }*/
        }

        private void BtnDoStrat_Click(object sender, EventArgs e)
        {
            /*NewPlayer();
            try
            {
                p.DoStrat(TxtStrat.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            updateLabel();*/
        }

        PlayerRange pr;

        private void UpdateLabelRange(List<PlayerRange> ranges)
        {
            //LblInfo.Text = "Info:\n" + string.Join("\n", ranges.Select(r => r.ToString()));
        }

        private void CreatePlayerRange()
        {
            /*PlayerRange.SetFloor(double.Parse(TxtFloorY.Text, CultureInfo.InvariantCulture));
            PlayerRange.SetCeiling(double.Parse(TxtCeiling.Text, CultureInfo.InvariantCulture));
            pr = new(double.Parse(TxtYUpper.Text, CultureInfo.InvariantCulture), double.Parse(TxtYLower.Text, CultureInfo.InvariantCulture), double.Parse(TxtVSpeed.Text, CultureInfo.InvariantCulture));*/
        }

        private void BtnFloorCollision_Click(object sender, EventArgs e)
        {
            //CreatePlayerRange();
            /*List<PlayerRange> ranges = PlayerRange.FloorCollision(pr);
            UpdateLabelRange(ranges);*/
        }

        private void BtnCeilingCollision_Click(object sender, EventArgs e)
        {
            //CreatePlayerRange();
            /*List<PlayerRange> ranges = PlayerRange.CeilingCollision(pr);
            UpdateLabelRange(ranges);*/
        }

        private double Parse(string s) => double.Parse(s, CultureInfo.InvariantCulture);

        private void BtnSearchExact_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new();
            sw.Start();

            double y = Parse(TxtPlayerY.Text);
            double vspeed = Parse(TxtVSpeed.Text);

            List<Player> results = Search.SearchExact(TxtFloorY.Text, TxtCeiling.Text, y, vspeed, true, true);

            sw.Stop();

            LblInfo.Text = "Info:\n" + $"{results.Count} results in {sw.Elapsed}";

            results.Sort(new Comparison<Player>((a, b) => a.Frame == b.Frame ? 0 : a.Frame - b.Frame));

            LstResults.Items.Clear();

            int elements = Math.Min(results.Count, 10000);
            foreach (Player player in results[..elements])
            {
                LstResults.Items.Add(player);
            }
        }

        private void BtnSearchRange_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new();
            sw.Start();

            double yUpper = Parse(TxtYUpper.Text);
            double yLower = Parse(TxtYLower.Text);
            double vspeed = Parse(TxtVSpeed.Text);

            List<PlayerRange> results = Search.SearchRange(TxtFloorY.Text, TxtCeiling.Text, yUpper, yLower, vspeed, true, true);

            sw.Stop();

            LblInfo.Text = "Info:\n" + $"{results.Count} results in {sw.Elapsed}";

            results.Sort(new Comparison<PlayerRange>((a, b) => a.Frame == b.Frame ? 0 : a.Frame - b.Frame));

            LstResults.Items.Clear();

            int elements = Math.Min(results.Count, 10000);
            foreach (PlayerRange range in results[..elements])
            {
                LstResults.Items.Add(range);
            }
        }
    }
}
