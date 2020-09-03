using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BipolarityX {
    public partial class BipolarityGui : Form {

        public BipolarityGui() {
            InitializeComponent();
        }

        private readonly string _defPath = Application.StartupPath + "//Monaco//";

        private async void Main_Load(object sender, EventArgs e) {
            
            webBrowser1.Url = new Uri($"file:///{Directory.GetCurrentDirectory()}/Monaco/Monaco.html");
            await Task.Delay(500);
            if (!(webBrowser1.Document is null)) {
                webBrowser1.Document.InvokeScript("SetTheme", new object[] {
                    "Dark"
                    /*
                 There are 2 Themes Dark and Light
                */
                });
                AddBase();
                AddMath();
                AddGlobalNs();
                AddGlobalV();
                AddGlobalF();
                webBrowser1.Document.InvokeScript("SetText", new object[] {
                    "--- Join Our Discord Server https://discord.gg/bBRMt8f ---"
                });
            }

            {
                CheckAttached();
            }
            {
                listBox1.Items.Clear();
                PopulateListBox(listBox1, "./Scripts", "*.txt");
                PopulateListBox(listBox1, "./Scripts", "*.lua");
            }
            {
                TopMost = true;

                Opacity = 0.9;
            }
        }

        private void AddIntel(string label, string kind, string detail, string insertText) {
            webBrowser1.Document?.InvokeScript("AddIntellisense", new object[] {
                label,
                kind,
                detail,
                insertText
            });
        }

        private void AddGlobalF() {
            var array = File.ReadAllLines(_defPath + "//globalf.txt");
            foreach (var text in array) {
                var flag = text.Contains(":");
                AddIntel(text, "Function", text, flag ? text.Substring(1) : text);
            }
        }

        private void AddGlobalV() {
            foreach (var text in File.ReadLines(_defPath + "//globalv.txt")) {
                AddIntel(text, "Variable", text, text);
            }
        }

        private void AddGlobalNs() {
            foreach (var text in File.ReadLines(_defPath + "//globalns.txt")) {
                AddIntel(text, "Class", text, text);
            }
        }

        private void AddMath() {
            foreach (var text in File.ReadLines(_defPath + "//classfunc.txt")) {
                AddIntel(text, "Method", text, text);
            }
        }

        private void AddBase() {
            foreach (var text in File.ReadLines(_defPath + "//base.txt")) {
                AddIntel(text, "Keyword", text, text);
            }
        }

        private void button_launchExploit(object sender, EventArgs e) {
            //Module.LaunchExploit();
        }

        private void button_minimize(object sender, EventArgs e) {
            WindowState = FormWindowState.Minimized;
        }

        private void button_kill(object sender, EventArgs e) {
            Module.KillBipolarity();
        }

        private void button_execute(object sender, EventArgs e) {
        }

        private void button_openFile(object sender, EventArgs e) {
        }

        private void button_saveFile(object sender, EventArgs e) {
        }

        private void button_executeFile(object sender, EventArgs e) {
        }

        private void button_clear(object sender, EventArgs e) {
        }

        private void button_options(object sender, EventArgs e) {
        }

        private void button_scriptHub(object sender, EventArgs e) {
        }

        private void executeScriptToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex != -1) {
                // Module.ExecuteScript(File.ReadAllText("scripts\\" + this.listBox1.SelectedItem.ToString()));
            }
        }


        private void loadScriptIntoMonacoToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex == -1) return;
            webBrowser1.Document?.InvokeScript("SetText", new object[] {
                File.ReadAllText("Scripts\\" + listBox1.SelectedItem)
            });
        }

        private void refreshScriptListToolStripMenuItem_Click(object sender, EventArgs e) {
            listBox1.Items.Clear();
            PopulateListBox(listBox1, "./Scripts", "*.txt");
            PopulateListBox(listBox1, "./Scripts", "*.lua");
        }


        private static void CheckAttached() {
            /*if (Module.isAPIAttached()) {
                label2.ForeColor = Color.Lime;
                label2.Text = @"Attached";
            } else {
                label2.ForeColor = Color.Red;
                label2.Text = @"Not Attached";
            }*/
        }

        private void button_checkAttached(object sender, EventArgs e) {
            CheckAttached();
        }

        private static void PopulateListBox(ListBox lsb, string folder, string fileType) {
            var dinfo = new DirectoryInfo(folder);
            var files = dinfo.GetFiles(fileType);
            foreach (var file in files) {
                lsb.Items.Add(file.Name);
            }
        }
    }
}