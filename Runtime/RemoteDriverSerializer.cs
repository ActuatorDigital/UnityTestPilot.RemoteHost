using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TachyonCommon;

namespace AIR.UnityTestPilot.Remote {
    public class RemoteDriverSerializer : ISerializer {
        
        public byte[] SerializeObject<T>(T obj) {
            if (obj is ISerializableAgentMessage serializable)
                return serializable.Serialize();

            throw new ArgumentException(obj.GetType().Name + 
                " does not implement " + typeof(ISerializableAgentMessage).Name);
        }

        public object DeserializeObject(byte[] objBytes, Type type) {
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