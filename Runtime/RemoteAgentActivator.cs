// Copyright (c) AIR Pty Ltd. All rights reserved.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AIR.UnityTestPilot.Remote.Tests")]
namespace AIR.UnityTestPilotRemote.Host
{
    public static class RemoteAgentActivator
    {
        public const string VERBOSE_ACTIVATION_ARGUMENT_ARG = "-testAgent";
        public const string ACTIVATION_ARGUMENT_ARG = "-ta";
        private static RemoteUnityAgentHost _singleton;

        public static RemoteUnityAgentHost TryActivate(string[] args = null)
        {
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

        public static void Deactivate()
        {
            if (_singleton != null) {
                _singleton.Shutdown(false);
                _singleton = null;
            }
        }
    }
}