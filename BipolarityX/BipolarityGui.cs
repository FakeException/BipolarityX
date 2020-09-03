using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyExploits;

namespace BipolarityX {
    public partial class BipolarityGui : Form {
        public BipolarityGui() {
            InitializeComponent();
        }

        private static readonly Module Module = new Module();
        private readonly string _defPath = Application.StartupPath + "//Monaco//";

        private async void Main_Load(object sender, EventArgs e) {
            try {
                var registryKey = Registry.CurrentUser.OpenSubKey(
                    "SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
                var friendlyName = AppDomain.CurrentDomain.FriendlyName;
                var flag2 = registryKey != null && registryKey.GetValue(friendlyName) == null;
                if (flag2) {
                    registryKey.SetValue(friendlyName, 11001, RegistryValueKind.DWord);
                }
            }
            catch (Exception) {
                // ignored
            }

            webBrowser1.Url = new Uri($"file:///{Directory.GetCurrentDirectory()}/Monaco/Monaco.html");
            await Task.Delay(500);
            if (!(webBrowser1.Document is null)) {
                webBrowser1.Document.InvokeScript("SetTheme", new object[] {
                    "Dark"
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
            Module.LaunchExploit();
        }

        private void button_minimize(object sender, EventArgs e) {
            WindowState = FormWindowState.Minimized;
        }

        private void button_kill(object sender, EventArgs e) {
            Utils.KillBipolarity();
        }

        private void button_execute(object sender, EventArgs e) {
            var text = webBrowser1.Document;
            const string scriptName = "GetText";
            var args = new object[0];
            if (text is null) return;
            var obj = text.InvokeScript(scriptName, args);
            var script = obj.ToString();
            if (webBrowser1.Text.Contains("loadstring(game:HttpGet((")) {
                var num = script.IndexOf("('", StringComparison.Ordinal) + 2;
                var length = script.IndexOf("')", StringComparison.Ordinal) - num;
                var webRequest = WebRequest.Create(script.Substring(num, length));
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                var httpWebResponse = (HttpWebResponse) webRequest.GetResponse();
                Console.WriteLine(httpWebResponse.StatusDescription);
                var loadstring =
                    new StreamReader(httpWebResponse.GetResponseStream() ?? throw new InvalidOperationException())
                        .ReadToEnd();

                Module.ExecuteScript(loadstring);
            }
            else {
                Module.ExecuteScript(script);
            }

            if (webBrowser1.Text.Contains("loadstring(game:GetObjects((")) {
                var num = script.IndexOf("('", StringComparison.Ordinal) + 2;
                var length = script.IndexOf("')", StringComparison.Ordinal) - num;
                var webRequest = WebRequest.Create(script.Substring(num, length));
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                var httpWebResponse = (HttpWebResponse) webRequest.GetResponse();
                Console.WriteLine(httpWebResponse.StatusDescription);
                var str3 = new StreamReader(
                    httpWebResponse.GetResponseStream() ?? throw new InvalidOperationException()).ReadToEnd();

                Module.ExecuteScript(str3);
            }

            if (!webBrowser1.Text.Contains("game:GetObjects((")) return;
            {
                var num = script.IndexOf("('", StringComparison.Ordinal) + 2;
                var length = script.IndexOf("')", StringComparison.Ordinal) - num;
                var webRequest = WebRequest.Create(script.Substring(num, length));
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                var httpWebResponse = (HttpWebResponse) webRequest.GetResponse();
                Console.WriteLine(httpWebResponse.StatusDescription);
                var reader =
                    new StreamReader(httpWebResponse.GetResponseStream() ?? throw new InvalidOperationException())
                        .ReadToEnd();
                Module.ExecuteScript(reader);
            }
        }

        private void button_openFile(object sender, EventArgs e) {
            var openFileDialog = new OpenFileDialog {
                Filter = @"Lua File (*.lua)|*.lua|Text File (*.txt)|*.txt",
                FilterIndex = 2,
                Title = @"Open a script",
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK) {
                return;
            }

            try {
                var webBrowser1Document = webBrowser1.Document;
                if (webBrowser1Document is null) return;
                webBrowser1Document.InvokeScript("SetText", new object[] {
                    ""
                });
                var stream = openFileDialog.OpenFile();
                using (stream) {
                    webBrowser1Document.InvokeScript("SetText", new object[] {
                        File.ReadAllText(openFileDialog.FileName)
                    });
                }
            }
            catch {
                // ignored
            }
        }

        private void button_saveFile(object sender, EventArgs e) {
            var saveFileDialog = new SaveFileDialog {
                Filter = @"Lua File (*.lua)|*.lua|Text File (*.txt)|*.txt", Title = @"Save Script"
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) {
                return;
            }

            try {
                var document = webBrowser1.Document;
                const string scriptName = "GetText";
                var array = new object[0];
                var args = array;
                if (document is null) return;
                var value = document.InvokeScript(scriptName, args).ToString();
                var streamWriter = new StreamWriter(File.Create(saveFileDialog.FileName));
                streamWriter.Write(value);
                streamWriter.Dispose();
            } catch {
                // ignored
            }
        }

        private void button_executeFile(object sender, EventArgs e) {
            var ef = new OpenFileDialog {
                Filter = @"Txt Files (*.txt)|*.txt|Lua Files (*.lua)|*.lua|All Files (*.*)|*.*"
            };
            if (ef.ShowDialog() == DialogResult.OK) {
                Module.ExecuteScript(File.ReadAllText(ef.FileName));
            }
        }

        private void button_clear(object sender, EventArgs e) {
            var webBrowser1Document = webBrowser1.Document;
            webBrowser1Document?.InvokeScript("SetText", new object[] {
                ""
            });
        }

        private void button_scriptHub(object sender, EventArgs e) {
        }

        private void executeScriptToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex != -1) {
                Module.ExecuteScript(File.ReadAllText("scripts\\" + listBox1.SelectedItem));
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

        private void CheckAttached() {
            if (Utils.IsApiAttached()) {
                label2.ForeColor = Color.Lime;
                label2.Text = @"Attached";
            } else {
                label2.ForeColor = Color.Red;
                label2.Text = @"Not Attached";
            }
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