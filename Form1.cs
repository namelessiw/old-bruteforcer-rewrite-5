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
            while (!p.IsStable())
            {
                p.Step(p.CanJump() ? Input.Press : Input.None);
                strats += p.GetStrat(oneframeConvention) + "\n";
            }
            MessageBox.Show(strats);
        }
    }
}
