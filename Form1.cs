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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private double Parse(string s) => double.Parse(s, CultureInfo.InvariantCulture);

        private void BtnSearchExact_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new();
            sw.Start();

            double y = Parse(TxtPlayerY.Text);
            double vspeed = Parse(TxtVSpeed.Text);

            List<Player> results = Search.SearchExact(TxtFloorY.Text, TxtCeiling.Text, y, vspeed, ChkSinglejump.Checked, ChkDoublejump.Checked);

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

            List<PlayerRange> results = Search.SearchRange(TxtFloorY.Text, TxtCeiling.Text, yUpper, yLower, vspeed, ChkSinglejump.Checked, ChkDoublejump.Checked);

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
