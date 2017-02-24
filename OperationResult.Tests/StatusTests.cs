using Microsoft.VisualStudio.TestTools.UnitTesting;
using static OperationResult.Helpers;

namespace OperationResult.Tests
{
    [TestClass]
    public class StatusTests
    {
        private Status GetStatus(int arg)
        {
            if (arg == 1)
            {
                return Ok();
            }
            return Error();
        }

        [TestMethod]
        public void TestStatusWithoutError()
        {
            var res1 = GetStatus(1);

            Assert.IsTrue(res1);
            Assert.IsTrue(res1.IsSuccsess);
            Assert.IsFalse(res1.IsError);
            
            var res2 = GetStatus(2);

            Assert.IsFalse(res2);
            Assert.IsFalse(res2.IsSuccsess);
            Assert.IsTrue(res2.IsError);
        }

        private Status<string> GetStatusOrError(int arg)
        {
            if (arg == 1)
            {
                return Ok();
            }
            return Error("Invalid Operation");
        }

        [TestMethod]
        public void TestStatusWithError()
        {
            var res1 = GetStatusOrError(1);

            Assert.IsTrue(res1);
            Assert.IsTrue(res1.IsSuccsess);
            Assert.IsFalse(res1.IsError);
            Assert.IsNull(res1.Error);
            
            var res2 = GetStatusOrError(2);

            Assert.IsFalse(res2);
            Assert.IsFalse(res2.IsSuccsess);
            Assert.IsTrue(res2.IsError);
            Assert.AreEqual(res2.Error, "Invalid Operation");
        }
    }
}
