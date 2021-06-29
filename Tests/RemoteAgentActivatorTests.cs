using AIR.UnityTestPilotRemote.Host;
using NUnit.Framework;

namespace RemoteUnityDriverTests {
    
    [TestFixture]
    public class RemoteAgentActivatorTests 
    {

        [TearDown]
        public void TearDown() {
            RemoteAgentActivator.Deactivate();
        }

        [Test]
        public void Activate_NoEnvironmentArgs_InactiveAgent() {
            
            // Act
            var agent = RemoteAgentActivator.TryActivate();
            
            // Assert
            Assert.IsNull(agent);
            
        }

        [Test]
        public void RemoteAgent_ArgumentsProvided_ActivatesAgent(
            [Values( 
                new[]{RemoteAgentActivator.ACTIVATION_ARGUMENT_ARG}, 
                new[]{RemoteAgentActivator.VERBOSE_ACTIVATION_ARGUMENT_ARG})] 
            string[] args
        ) {
            
            // Act
            var agent = RemoteAgentActivator.TryActivate(args);
            
            // Assert
            Assert.IsNotNull(agent);
            
        }
        
        [Test]
        public void RemoteAgent_IrreleventArgsProvided_DoesNotActivateAgent(
            [Values( 
                new[]{"-batchmode"}, 
                new[]{"-projectPath"}, 
                new[]{"-projectPath", "-batchmode"})] 
            string[] args
        ) {
            
            // Act
            var agent = RemoteAgentActivator.TryActivate(args);
            
            // Assert
            Assert.IsNull(agent);
            
        }

        [Test]
        public void RemoteAgent_ActivatedTwice_OnlyActivatesOnce() {
            // Arrange
            var firstActivatedAgent = RemoteAgentActivator
                .TryActivate(new[]{RemoteAgentActivator.ACTIVATION_ARGUMENT_ARG});

            // Act
            var secondActivatedAgent = RemoteAgentActivator
                .TryActivate(new[] {RemoteAgentActivator.VERBOSE_ACTIVATION_ARGUMENT_ARG});
            
            // Assert
            Assert.AreEqual(
                firstActivatedAgent.GetHashCode(),
                secondActivatedAgent.GetHashCode());

        }
    }
   
}