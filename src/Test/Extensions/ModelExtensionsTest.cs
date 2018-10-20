using System;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Extensions {
    [TestClass]
    public class ModelExtensionsTest {

        [TestMethod]
        public void CanCompareMemberwise() {
            var process = new ControllableProcess {
                ProcessId = 4711,
                Busy = false,
                ConfirmedAt = DateTime.Now,
                LaunchCommand = "StartMeUp.cmd",
                Title = "This is not a title"
            };
            var otherProcess = new ControllableProcess {
                ProcessId = 4712,
                Busy = false,
                ConfirmedAt = DateTime.Now,
                LaunchCommand = "StartMeUp.cmd",
                Title = "This is not a title"
            };
            Assert.IsTrue(process.MemberwiseEquals(process));
            Assert.IsFalse(process.MemberwiseEquals(otherProcess));
        }
    }
}
