using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("AIR.UnityTestPilot.Remote.Tests")]
namespace AIR.UnityTestPilot.Remote { 
    
    public static class RemoteAgentActivator {

        internal const string VERBOSE_ACTIVATION_ARGUMENT_ARG = "-testAgent";
        internal const string ACTIVATION_ARGUMENT_ARG = "-ta";
        static RemoteUnityAgentHost _singleton;

        public static RemoteUnityAgentHost TryActivate(string[] args = null) {

            if (_singleton != null) 
                return _singleton;

            if (args == null)
                args = System.Environment.GetCommandLineArgs();

            for (int i = 0; i < args.Length; i++) {
                if (args[i] == ACTIVATION_ARGUMENT_ARG || args[i] == VERBOSE_ACTIVATION_ARGUMENT_ARG) {
                    _singleton = new RemoteUnityAgentHost();
                    _singleton.StartHost();
                    return _singleton;
                }
            }

            // No activation argument provided.
            return null;
        }

        public static void Deactivate() {
            _singleton.Shutdown(false);
            _singleton = null;
        }

    }
}