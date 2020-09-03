using System.Diagnostics;

namespace BipolarityX {
    
    public static class Module {
        
        public static void KillBipolarity() {
            foreach (var process in Process.GetProcesses()) {
                if (process.ProcessName == "BipolarityX") {
                    process.Kill();
                }
            }
        }
    }
}