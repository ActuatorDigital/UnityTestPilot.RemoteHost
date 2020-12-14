using System;
using System.Threading.Tasks;
using AIR.UnityTestPilotRemote.Common;
using TachyonCommon;
using TachyonServerCore;
using TachyonServerRPC;

namespace GeneratedBindings
{
    [HostBindingAttribute(typeof(IRemoteUnityDriver))]
    public class IRemoteUnityDriverHostBinding
    {
        IRemoteUnityDriver _service;
        HostCore _connection;

        public void Bind(HostCore connection, IRemoteUnityDriver service, EndPointMap endPoints)
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
            map.AddAskEndpoint<RemoteElementQuery, RemoteUiElement>((c, m) => Query(m).Result, "Query");
            map.AddSendEndpoint<bool>((c, m) => Shutdown(m), "Shutdown");
            map.AddSendEndpoint<Single>((c, m) => SetTimeScale(m), "SetTimeScale");
            map.AddSendEndpoint<RemoteUiElement>((c, m) => LeftClick(m), "LeftClick");
            map.AddSendEndpoint<RemoteUiElement>((c, m) => LeftClickDown(m), "LeftClickDown");
            map.AddSendEndpoint<RemoteUiElement>((c, m) => LeftClickUp(m), "LeftClickUp");
        }

        Task<AIR.UnityTestPilotRemote.Common.RemoteUiElement> Query(RemoteElementQuery query)
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

        void LeftClickDown(RemoteUiElement element)
        {
            _service.LeftClickDown(element);
        }

        void LeftClickUp(RemoteUiElement element)
        {
            _service.LeftClickUp(element);
        }
    }
}