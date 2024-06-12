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
            p.Step(Input.Press);
            while (!p.IsStable())
            {
                p.Step(p.CanJump() ? Input.Press : Input.None);
                MessageBox.Show(p.ToString());
            }
        }
    }
}
