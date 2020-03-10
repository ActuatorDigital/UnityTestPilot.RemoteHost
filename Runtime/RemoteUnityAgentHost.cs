using Object = UnityEngine.Object;
using AIR.UnityTestPilot.Queries;
using AIR.UnityTestPilot.Agents;
using System.Threading.Tasks;
using TachyonServerCore;
using UnityEngine;
using System.Linq;
using System;
using TITAN.Common.Config;

namespace AIR.UnityTestPilot.Remote {
    
    public class RemoteUnityAgentHost : IRemoteUnityDriver, IDisposable {
        
        private IUnityDriverAgent _nativeAgent; 
        private TachyonUnityHost _host;
        private Object _scheneObject;

        private bool _connected;
        public bool Started => _host.Started && _connected;
        public event Action<bool> OnConnectionChanged;

        public RemoteUnityAgentHost() {

            var container = new GameObject("RemoteUnityHostAgent");
            var host = container.AddComponent<TachyonUnityHost>();

            host.OnClientConnected += (c) => {
                OnConnectionChanged?.Invoke(true);
                _connected = true; 
            };
            host.OnClientDisconnected += (c) => {
                OnConnectionChanged?.Invoke(false);
                _connected = false; 
            };

            _nativeAgent = new NativeUnityDriverAgent();
            _scheneObject = container;
            _host = host;
        }

        public void StartHost() {
            _host.Initialize( new RemoteDriverSerializer(), this );
        }

        public Task<RemoteUiElement> Query(RemoteElementQuery query) {
            
            ElementQuery nativeQuery;
            switch (query.Format) {
                case QueryFormat.NamedQuery:
                    nativeQuery = new NamedElementQuery(query.Name);
                    break;
                case QueryFormat.TypedQuery:
                    nativeQuery = new TypedElementQuery(Type.GetType(query.TargetType));
                    break;
                default:
                    throw new ArgumentException("Element query format not known.");
            }

            var nativeResults = nativeQuery.Search();
            if (nativeResults != null) {
                var remoteResults = Enumerable.Select(
                    nativeResults, uie => 
                        new RemoteUiElement (
                            uie?.Name,
                            uie?.IsActive ?? false,
                            uie?.Text 
                        ));
            
                return Task.FromResult(remoteResults.FirstOrDefault());                
            } else {
              return  Task.FromResult<RemoteUiElement>(default);
            }
        }

        public void Shutdown(bool immedaite) {
            _nativeAgent.Shutdown();
        }

        public void SetTimeScale(float timeScale) {
            _nativeAgent.SetTimeScale(timeScale);
        }

        public void LeftClick(RemoteUiElement element) {
            var localElement = new NamedElementQuery(element.Name);
            var result = localElement.Search().FirstOrDefault();
            result?.LeftClick();
        }

        public void Dispose() {
            Object.Destroy(_scheneObject);
            // _host.Disconnect();
        }
    }
    
}