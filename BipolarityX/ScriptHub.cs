using System;
using System.IO;
using System.Windows.Forms;
using EasyExploits;

namespace BipolarityX {
    public partial class ScriptHub : Form {
        
        private static readonly Module Module = new Module();

        public ScriptHub() {
            InitializeComponent();
        }

        private void button_execute(object sender, EventArgs e) {
            Module.ExecuteScript(File.ReadAllText("./Bin/ScriptHub//" + listBox1.SelectedItem));
        }

        private void ScriptHub_Load(object sender, EventArgs e) {
            listBox1.Items.Clear();
            Utils.PopulateListBox(listBox1, "./Bin/ScriptHub", "*.txt");
            Utils.PopulateListBox(listBox1, "./Bin/ScriptHub", "*.lua");
            TopMost = true;
            Opacity = 1;
        }

        private void button_hide(object sender, EventArgs e) {
            Hide();
        }
    }
}