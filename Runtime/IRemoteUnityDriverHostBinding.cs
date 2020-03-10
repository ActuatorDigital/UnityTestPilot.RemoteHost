using AIR.UnityTestPilot.Remote;
using AIR.UnityTestPilot.Remote;
using TachyonServerCore;
using TachyonServerRPC;
using System;
using System.Threading.Tasks;
using TachyonCommon;
namespace GeneratedBindings
{
    [HostBindingAttribute(typeof(IRemoteUnityDriver))]
    public class IRemoteUnityDriverHostBinding
    {
        IRemoteUnityDriver _service;
        HostCore _connection;
        public void Bind(HostCore connection, IRemoteUnityDriver service, EndPointMap endPoints )
        {
            _connection = connection;
            _service = service;
            BindClientCallbacks();
            BindHostMethodEndPoints(endPoints);
        }

        void BindClientCallbacks()
        {
        }

        void BindHostMethodEndPoints(EndPointMap map)
        {
            map.AddAskEndpoint<RemoteElementQuery,RemoteUiElement>((c,m) => Query(m).Result, "Query");
            map.AddSendEndpoint<bool>((c,m) => Shutdown(m), "Shutdown");
            map.AddSendEndpoint<Single>((c,m) => SetTimeScale(m), "SetTimeScale");
            map.AddSendEndpoint<RemoteUiElement>((c,m) => LeftClick(m), "LeftClick");
        }

        Task<AIR.UnityTestPilot.Remote.RemoteUiElement> Query(RemoteElementQuery query)
        {
            return _service.Query(query);
        }

        void Shutdown(Boolean immediate)
        {
            _service.Shutdown(immediate);
        }

        void SetTimeScale(Single timeScale)
        {
            _service.SetTimeScale(timeScale);
        }

        void LeftClick(RemoteUiElement element)
        {
            _service.LeftClick(element);
        }

    }

}

