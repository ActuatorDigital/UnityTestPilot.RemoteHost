// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using AIR.UnityTestPilotRemote.Host;
using TachyonCommon;

namespace AIR.UnityTestPilotRemote.Common
{
    public class RemoteDriverSerializer : ISerializer
    {
        public byte[] SerializeObject<T>(T obj)
        {
            if (obj is ISerializableAgentMessage serializable)
                return serializable.Serialize();

            throw new ArgumentException(obj.GetType().Name +
                                        " does not implement " + typeof(ISerializableAgentMessage).Name);
        }

        public object DeserializeObject(byte[] objBytes, Type type)
        {
            var typedObj = Activator.CreateInstance(type);

            if (typedObj is ISerializableAgentMessage obj) {
                obj.Deserialize(objBytes);
                return obj;
            }

            throw new ArgumentException(type.Name +
                                        " does not implement " + typeof(ISerializableAgentMessage).Name);
        }
    }
}