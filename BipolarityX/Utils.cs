using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BipolarityX {
    public static class Utils {
        public static void KillBipolarity() {
            foreach (var process in Process.GetProcesses()) {
                if (process.ProcessName == "BipolarityX") {
                    process.Kill();
                }
            }
        }
        
        public static bool IsApiAttached() {
            return NamedPipeExist("ocybedam");
        }
        
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool WaitNamedPipe(string name, int timeout);

        private static bool NamedPipeExist(string pipeName) {
            bool result;
            try {
                if (!WaitNamedPipe(Path.GetFullPath($"\\\\.\\pipe\\{pipeName}"), 0)) {
                    var lastWin32Error = Marshal.GetLastWin32Error();
                    switch (lastWin32Error) {
                        case 0:
                            return false;
                        case 2:
                            return false;
                    }
                }

                result = true;
            } catch (Exception) {
                result = false;
            }

            return result;
        }
        
        public static void PopulateListBox(ListBox lsb, string folder, string fileType) {
            var dinfo = new DirectoryInfo(folder);
            var files = dinfo.GetFiles(fileType);
            foreach (var file in files) {
                lsb.Items.Add(file.Name);
            }
        }
    }
}