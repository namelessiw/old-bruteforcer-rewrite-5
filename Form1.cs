namespace old_bruteforcer_rewrite_5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // TODO: remove
        {
            Player p = new Player(407, 0, 0, true, false, []);
            bool oneframeConvention = true;
            string strats = p.GetStrat(oneframeConvention) + "\n";
            p.Step(Input.Press);
            strats += p.GetStrat(oneframeConvention) + "\n";
            while (p.Step(p.CanPress() ? Input.Press : Input.None))
            {
                strats = p.GetStrat(oneframeConvention) + "\n";
            }
            MessageBox.Show(strats + p.GetMacro());
        }

        Player p = new(407, 0, 0, true, false, []);

        private void Form1_Load(object sender, EventArgs e)
        {
            updateLabel();
        }

        private void updateLabel()
        {
            LblStats.Text = p.ToString() + "\n" + p.GetStrat(false) + "\n" + p.GetMacro();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            p = new Player(407, 0, 0, true, false, []);
            updateLabel();
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
    }
}
