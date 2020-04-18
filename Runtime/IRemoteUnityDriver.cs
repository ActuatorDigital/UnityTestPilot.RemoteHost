// Copyright (c) AIR Pty Ltd. All rights reserved.

using System;
using System.Text;
using System.Threading.Tasks;
using TachyonCommon;

namespace AIR.UnityTestPilotRemote.Common
{

    public enum QueryFormat
    {
        /// <summary>
        /// Query which converts to By.Name call in TestPilot Native.
        /// </summary>
        NamedQuery = 0,

        /// <summary>
        /// Query which converts to By.Type call in TestPilot Native.
        /// </summary>
        TypedQuery = 1,
    }

    [GenerateBindings]
    public interface IRemoteUnityDriver
    {
        Task<RemoteUiElement> Query(RemoteElementQuery query);
        void Shutdown(bool immediate);
        void SetTimeScale(float timeScale);
        void LeftClick(RemoteUiElement element);
    }

    public interface ISerializableAgentMessage
    {
        byte[] Serialize();
        void Deserialize(byte[] objBytes);
    }

    public struct RemoteUiElement : ISerializableAgentMessage
    {
        public string Name;
        public bool IsActive;
        public string Text;

        public RemoteUiElement(
            string name,
            bool isActive,
            string text
        )
        {
            Name = name;
            IsActive = isActive;
            Text = text;
        }

        public byte[] Serialize()
        {
            var elementStr = string.Join(
                "|",
                Name,
                IsActive ? "Active" : "Inactive",
                Text);
            return Encoding.ASCII.GetBytes(elementStr);
        }

        public void Deserialize(byte[] objBytes)
        {
            var elementStr = Encoding.ASCII.GetString(objBytes);
            var elementParts = elementStr.Split('|');
            Name = elementParts[0];
            IsActive = elementParts[1] == "Active";
            Text = elementParts[2];
        }
    }

    public struct RemoteElementQuery : ISerializableAgentMessage
    {
        public QueryFormat Format;
        public string Name;
        public string TargetType;

        public RemoteElementQuery(
            QueryFormat format,
            string name,
            string targetType
        ) {
            Format = format;
            Name = name;
            TargetType = targetType;
        }

        public byte[] Serialize()
        {
            var queryString = string.Join(
                "|",
                Format.ToString(),
                Name,
                TargetType
            );

            return Encoding.ASCII.GetBytes(queryString);
        }

        public void Deserialize(byte[] objBytes)
        {
            var queryString = Encoding.ASCII.GetString(objBytes);
            var elementParts = queryString.Split('|');
            Format = (QueryFormat)Enum.Parse(typeof(QueryFormat), elementParts[0]);
            Name = elementParts[1];
            TargetType = elementParts[2];
        }
    }
}