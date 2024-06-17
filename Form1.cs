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
            NewPlayer();
            CreatePlayerRange();

            test();
            
            /*MessageBox.Show(p.ToString());
            for (int i = 0; i < inputs.Count; i++)
            {
                int size = ranges.Count;
                for (int j = 0; j < size; j++)
                {
                    List<PlayerRange> newRanges = ranges[j].Step(inputs[i]);
                    foreach (PlayerRange newRange in newRanges)
                    {
                        ranges.Add(newRange);
                    }

                    MessageBox.Show($"Input: {inputs[i]}\nRange: {j}\n" + ranges[j].ToString());
                }
            }

            while (ranges.Count > 0)
            {
                int size = ranges.Count;
                for (int j = 0; j < size; j++)
                {
                    List<PlayerRange> newRanges = ranges[j].Step(Input.None);
                    foreach (PlayerRange newRange in newRanges)
                    {
                        ranges.Add(newRange);
                    }

                    MessageBox.Show($"To STable:\nRange: {j}\n" + ranges[j].ToString());

                    if (ranges[j].IsStable())
                    {
                        PlayerRange stable = ranges[j].SplitOffStable();
                        if (ranges[j] == stable)
                        {
                            ranges.RemoveAt(j);
                            size--;
                            j--;
                        }
                    }
                }
            }*/

            /*PlayerRange2.SetFloor(408);
            PlayerRange2.SetCeiling(363);
            PlayerRange2 p = new(362.1, 367.4, -3.6);
            List<PlayerRange2> ranges = PlayerRange2.CeilingCollision(p);
            MessageBox.Show(string.Join(",\n", ranges.Select(r => r.ToString())));*/
        }

        private void test()
        {
            Stopwatch sw = new();
            sw.Start();

            Stack<PlayerRange> activeRanges = new([new(406.5, 406.5, 0, true, true)]);

            List<PlayerRange> results = [], ranges;

            while (activeRanges.Count > 0)
            {
                PlayerRange p = activeRanges.Peek(), temp;

                if (p.IsStable())
                {
                    PlayerRange stable = p.SplitOffStable();

                    if (p == stable)
                    {
                        activeRanges.Pop();
                        results.Add(p);
                        continue;
                    }
                    else
                    {
                        results.Add(stable);
                    }
                }

                if (p.CanPress())
                {
                    temp = p.Copy();
                    ranges = temp.Step(Input.Press);
                    foreach (PlayerRange range in ranges)
                    {
                        activeRanges.Push(range);
                    }
                    activeRanges.Push(temp);

                    temp = p.Copy();
                    ranges = temp.Step(Input.Press | Input.Release);
                    foreach (PlayerRange range in ranges)
                    {
                        activeRanges.Push(range);
                    }
                    activeRanges.Push(temp);
                }

                if (p.CanRelease())
                {
                    temp = p.Copy();
                    ranges = temp.Step(Input.Release);
                    foreach (PlayerRange range in ranges)
                    {
                        activeRanges.Push(range);
                    }
                    activeRanges.Push(temp);
                }

                ranges = p.Step(Input.None);
                foreach (PlayerRange range in ranges)
                {
                    activeRanges.Push(range);
                }
            }

            sw.Stop();

            LblInfo.Text = "Info:\n" + $"{results.Count} results in {sw.Elapsed}";

            results.Sort(new Comparison<PlayerRange>((a, b) => a.Frame == b.Frame ? 0 : a.Frame - b.Frame));

            LstResults.Items.Clear();

            int elements = Math.Min(results.Count, 10000);
            foreach (PlayerRange range in results[..elements])
            {
                LstResults.Items.Add(range);
            }



            /*List<PlayerRange> ranges = [new(406.5, double.BitDecrement(407.5), 0, true)];

            List<Input> inputs = [Input.Press, Input.None, Input.None, Input.None, Input.None, Input.Release, Input.None, Input.None, Input.None, Input.None, Input.None, Input.Press, Input.None, 
                Input.None, Input.None, Input.None, Input.Release, Input.None,];

            MessageBox.Show(ranges[0].ToString());

            for (int i = 0; true; i++)
            {
                int size = ranges.Count;

                // next input
                for (int j = 0; j < size; j++)
                {
                    PlayerRange p = ranges[j];
                    List<PlayerRange> newRanges = p.Step(i >= inputs.Count ? Input.None : inputs[i]);

                    foreach (PlayerRange newRange in newRanges)
                    {
                        ranges.Add(newRange);
                    }
                }

                // split off stable
                for (int j = 0; j < size; j++)
                {
                    PlayerRange p = ranges[j];
                    if (p.IsStable())
                    {
                        PlayerRange stable = p.SplitOffStable();
                        if (p != stable)
                        {
                            ranges.Add(stable);
                        }
                    }
                }

                // print
                string strat = ranges[0].GetStrat(false), macro = ranges[0].GetMacro();
                MessageBox.Show(strat + "\n" + macro + "\n" + ranges[0].GetStrat(false) + "\n" + string.Join("\n", ranges.Select(r => r.ToString())));

                // all stable?
                if (ranges.All(r => r.IsStable()))
                {
                    break;
                }
            }*/
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

        PlayerRange pr;

        private void UpdateLabelRange(List<PlayerRange> ranges)
        {
            LblInfo.Text = "Info:\n" + string.Join("\n", ranges.Select(r => r.ToString()));
        }

        private void CreatePlayerRange()
        {
            /*PlayerRange.SetFloor(double.Parse(TxtFloorY.Text, CultureInfo.InvariantCulture));
            PlayerRange.SetCeiling(double.Parse(TxtCeiling.Text, CultureInfo.InvariantCulture));
            pr = new(double.Parse(TxtYUpper.Text, CultureInfo.InvariantCulture), double.Parse(TxtYLower.Text, CultureInfo.InvariantCulture), double.Parse(TxtVSpeed.Text, CultureInfo.InvariantCulture));*/
        }

        private void BtnFloorCollision_Click(object sender, EventArgs e)
        {
            CreatePlayerRange();
            /*List<PlayerRange> ranges = PlayerRange.FloorCollision(pr);
            UpdateLabelRange(ranges);*/
        }

        private void BtnCeilingCollision_Click(object sender, EventArgs e)
        {
            CreatePlayerRange();
            /*List<PlayerRange> ranges = PlayerRange.CeilingCollision(pr);
            UpdateLabelRange(ranges);*/
        }
    }
}
