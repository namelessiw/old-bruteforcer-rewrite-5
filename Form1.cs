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
            string strats = p.GetStrat() + "\n";
            p.Step(Input.Press);
            strats += p.GetStrat() + "\n";
            while (!p.IsStable())
            {
                p.Step(p.CanJump() ? Input.Press : Input.None);
                strats += p.GetStrat() + "\n";
            }
            MessageBox.Show(strats);
        }
    }
}
