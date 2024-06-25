using System.Diagnostics;
using System.Globalization;
using System.Numerics;

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
            CmbSolutionCondition.SelectedIndex = 0;
        }

        private void BtnSearchExact_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new();
            sw.Start();

            SearchParams.PlayerYLower = TxtPlayerY.Text;
            SearchParams.PlayerYUpper = TxtPlayerY.Text;
            SearchParams.PlayerVSpeed = TxtVSpeed.Text;
            SearchParams.FloorY = TxtFloorY.Text;
            SearchParams.CeilingY = TxtCeilingY.Text;

            List<Player> results = Search.SearchExact(ChkSinglejump.Checked, ChkDoublejump.Checked, (SolutionCondition)CmbSolutionCondition.SelectedIndex);

            sw.Stop();

            LblInfo.Text = "Info:\n" + $"{results.Count} results in {sw.Elapsed}";

            results.Sort(new Comparison<Player>((a, b) => a.Frame == b.Frame ? 0 : a.Frame - b.Frame));

            LstResults.Items.Clear();

            int elements = Math.Min(results.Count, 10000);
            foreach (Player player in results[..elements])
            {
                LstResults.Items.Add($"({player.Frame}) {player.GetStrat(Chk1fConvention.Checked)} {player}");
            }
        }

        private void BtnSearchRange_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new();
            sw.Start();

            SearchParams.PlayerYLower = TxtYLower.Text;
            SearchParams.PlayerYUpper = TxtYUpper.Text;
            SearchParams.PlayerVSpeed = TxtVSpeed.Text;
            SearchParams.FloorY = TxtFloorY.Text;
            SearchParams.CeilingY = TxtCeilingY.Text;

            List<PlayerRange> results = Search.SearchRange(ChkSinglejump.Checked, ChkDoublejump.Checked, (SolutionCondition)CmbSolutionCondition.SelectedIndex);

            sw.Stop();

            LblInfo.Text = "Info:\n" + $"{results.Count} results in {sw.Elapsed}";

            results.Sort(new Comparison<PlayerRange>((a, b) => a.Frame == b.Frame ? 0 : a.Frame - b.Frame));

            LstResults.Items.Clear();

            int elements = Math.Min(results.Count, 10000);
            foreach (PlayerRange range in results[..elements])
            {
                LstResults.Items.Add($"({range.Frame}) {range.GetStrat(Chk1fConvention.Checked)} {range}");
            }
        }
    }
}
