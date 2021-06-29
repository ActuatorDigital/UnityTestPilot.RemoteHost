using System.Collections;
using System.Net.Sockets;
using AIR.UnityTestPilotRemote.Common;
using AIR.UnityTestPilotRemote.Host;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace RemoteUnityDriverTests {
    [TestFixture]
    public class RemoteUnityHostAgentTests {
        private const int DEFAULT_TIMESCALE = 1;
            
        private RemoteUnityAgentHost _agent;
        private GameObject _rootGO;

        [SetUp]
        public void SetUp() {
            
            var rootGO = new GameObject("rootGo");
            var canvasGO = new GameObject("Canvas", typeof(Canvas));
            canvasGO.transform.SetParent(rootGO.transform);
            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.referencePixelsPerUnit = 100;
            GameObject eventSystemGO = new GameObject("EventSystem",
                typeof(EventSystem));
            eventSystemGO.transform.SetParent(rootGO.transform);
            GameObject testButtonGO = new GameObject("TestButton", 
                typeof(RectTransform), typeof(Button));
            testButtonGO.transform.SetParent(canvasGO.transform);

            _agent = new RemoteUnityAgentHost();
            _rootGO = rootGO;
        }

        [TearDown]
        public void TearDown() {
            _agent.Dispose();
            Object.DestroyImmediate(_rootGO);
            Time.timeScale = DEFAULT_TIMESCALE;
        }

        [UnityTest]
        public IEnumerator StartHost_WithinTimeout_StartsServer() {

            // Arrange
            const int TIMEOUT = 4;
            float startTime = Time.time;

            // Act
            try {
                _agent.StartHost();
            } catch (SocketException se) {
                if (se.Message.Contains("Access denied")) {
                    Assert.Inconclusive("Test suite not running with socket permissions.");
                }
            }

            while (Time.time < startTime + TIMEOUT && !_agent.Started)
                yield return null;

            // Assert
            Assert.IsTrue(_agent.Started);

        }

        [Test]
        public void Query_WithSceneObjects_TaskResultHasSceneObjects() {
            
            // Arrange
            var remoteQuery = new RemoteElementQuery(
                QueryFormat.NamedQuery,
                "TestButton",
                "Button");

            // Act
            var foundElements = _agent.Query(remoteQuery).Result;

            // Assert
            Assert.IsNotNull(foundElements);

        }

        [Test]
        public void SetTimeScale_TimeScaleDefault_ChangesTimeScale() {
            // Arrange
            const int ACCELERATED_TIMESCALE = 10;
            Time.timeScale = DEFAULT_TIMESCALE;

            // Act
            _agent.SetTimeScale(ACCELERATED_TIMESCALE);
            
            // Assert
            Assert.AreEqual(Time.timeScale, ACCELERATED_TIMESCALE);
            Assert.AreNotEqual(Time.timeScale, DEFAULT_TIMESCALE);
            
        }

    }
}