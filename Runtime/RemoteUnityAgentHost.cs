// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using AIR.UnityTestPilot.Agents;
using AIR.UnityTestPilot.Queries;
using AIR.UnityTestPilotRemote.Common;
using TachyonServerCore;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AIR.UnityTestPilotRemote.Host
{
    public class RemoteUnityAgentHost : IRemoteUnityDriver, IDisposable
    {
        private IUnityDriverAgent _nativeAgent;
        private TachyonUnityHost _host;
        private Object _scheneObject;
        private bool _connected;

        public RemoteUnityAgentHost()
        {
            var container = new GameObject("RemoteUnityHostAgent");
            var host = container.AddComponent<TachyonUnityHost>();

            host.OnClientConnected += (c) => {
                OnConnectionChanged?.Invoke(true);
                _connected = true; };
            host.OnClientDisconnected += (c) => {
                OnConnectionChanged?.Invoke(false);
                _connected = false; };

            _nativeAgent = new NativeUnityDriverAgent();
            _scheneObject = container;
            _host = host;
        }

        public event Action<bool> OnConnectionChanged;
        public bool Started => _host.Started && _connected;

        public void StartHost()
        {
            _host.Initialize(new RemoteDriverSerializer(), this);
        }

        public Task<RemoteUiElement> Query(RemoteElementQuery query)
        {
            ElementQuery nativeQuery;
            switch (query.Format)
            {
            case QueryFormat.NamedQuery:
                nativeQuery = new NamedElementQueryNative(query.Name);
                break;
            case QueryFormat.TypedQuery:
                var queryType = Type.GetType(query.TargetType);
                nativeQuery = string.IsNullOrEmpty(query.Name)
                    ? new TypedElementQueryNative(queryType)
                    : new TypedElementQueryNative(queryType, query.Name);
                break;
            case QueryFormat.PathQuery:
                nativeQuery = new PathElementQueryNative(query.Name);
                break;
            default:
                throw new ArgumentException("Element query format not known.");
            }

            var nativeResults = nativeQuery.Search();
            if (nativeResults != null) {
                var remoteResults = Enumerable.Select(
                    nativeResults, uie =>
                        new RemoteUiElement {
                            Name = uie?.Name,
                            IsActive = uie?.IsActive ?? false,
                            Text = uie?.Text,
                            XPos = uie.LocalPosition.X,
                            YPos = uie.LocalPosition.Y,
                            ZPos = uie.LocalPosition.Z,
                            XRot = uie.EulerRotation.X,
                            YRot = uie.EulerRotation.Y,
                            ZRot = uie.EulerRotation.Z,
                        });

                return Task.FromResult(remoteResults.FirstOrDefault());
            } else {
                return Task.FromResult<RemoteUiElement>(default);
            }
        }

        public void Shutdown(bool immedaite)
        {
            _nativeAgent.Shutdown();
        }

        public void SetTimeScale(float timeScale)
        {
            _nativeAgent.SetTimeScale(timeScale);
        }

        public void LeftClick(RemoteUiElement element)
        {
            var localElement = new NamedElementQueryNative(element.Name);
            var result = localElement.Search().FirstOrDefault();
            result?.LeftClick();
        }

        public void LeftClickDown(RemoteUiElement element)
        {
            var localElement = new NamedElementQueryNative(element.Name);
            var result = localElement.Search().FirstOrDefault();
            result?.LeftClickDown();
        }

        public void LeftClickUp(RemoteUiElement element)
        {
            var localElement = new NamedElementQueryNative(element.Name);
            var result = localElement.Search().FirstOrDefault();
            result?.LeftClickUp();
        }

        public void Dispose()
        {
            Object.Destroy(_scheneObject);
        }

    }
}