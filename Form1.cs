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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new();
            sw.Start();

            SearchParams.SolutionCondition = (SolutionCondition)CmbSolutionCondition.SelectedIndex;
            SearchParams.PlayerYUpper = TxtYUpper.Text;
            SearchParams.PlayerYLower = TxtYLower.Text;
            SearchParams.PlayerVSpeed = TxtVSpeed.Text;
            SearchParams.FloorY = TxtFloorY.Text;
            SearchParams.CeilingY = TxtCeilingY.Text;

            if (SearchParams.SolutionCondition == SolutionCondition.YPosition)
            {
                SearchParams.SolutionCondition = ChkSolutionYRange.Checked ? SolutionCondition.YRange : SolutionCondition.ExactY;
            }

            if (ChkSolutionYRange.Checked)
            {
                SearchParams.SolutionYUpper = TxtSolutionYUpper.Text;
                SearchParams.SolutionYLower = TxtSolutionYLower.Text;
            }
            else
            {
                SearchParams.SolutionYUpper = TxtSolutionYUpper.Text;
                SearchParams.SolutionYLower = TxtSolutionYUpper.Text;
            }

            if (ChkPlayerYRange.Checked)
            {
                List<PlayerRange> results = Search.SearchRange(ChkSinglejump.Checked, ChkDoublejump.Checked);

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
            else
            {
                SearchParams.PlayerYLower = TxtYUpper.Text;

                List<Player> results = Search.SearchExact(ChkSinglejump.Checked, ChkDoublejump.Checked);

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
        }

        private void ChkPlayerYRange_CheckedChanged(object sender, EventArgs e)
        {
            TxtYLower.Enabled = ChkPlayerYRange.Checked;
        }

        private void ChkSolutionYRange_CheckedChanged(object sender, EventArgs e)
        {
            TxtSolutionYLower.Enabled = ChkSolutionYRange.Checked;
        }
    }
}
