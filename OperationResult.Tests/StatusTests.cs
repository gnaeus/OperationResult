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
            Assert.IsTrue(res1.IsSuccess);
            Assert.IsFalse(res1.IsError);
            
            var res2 = GetStatus(2);

            Assert.IsFalse(res2);
            Assert.IsFalse(res2.IsSuccess);
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
            Assert.IsTrue(res1.IsSuccess);
            Assert.IsFalse(res1.IsError);
            Assert.IsNull(res1.Error);
            
            var res2 = GetStatusOrError(2);

            Assert.IsFalse(res2);
            Assert.IsFalse(res2.IsSuccess);
            Assert.IsTrue(res2.IsError);
            Assert.AreEqual(res2.Error, "Invalid Operation");
        }

        private Status<string, int> GetStatusOrMultipleErrors(int arg)
        {
            if (arg == 1)
            {
                return Ok();
            }
            if (arg == 2)
            {
                return Error(404);
            }
            return Error("Invalid Operation");
        }

        [TestMethod]
        public void TestStatusWithMultipleErrors()
        {
            var res1 = GetStatusOrMultipleErrors(1);

            Assert.IsTrue(res1);
            Assert.IsTrue(res1.IsSuccess);
            Assert.IsFalse(res1.IsError);
            Assert.IsNull(res1.Error);

            var res2 = GetStatusOrMultipleErrors(2);

            Assert.IsFalse(res2);
            Assert.IsFalse(res2.IsSuccess);
            Assert.IsTrue(res2.IsError);
            Assert.IsTrue(res2.HasError<int>());
            Assert.AreEqual(res2.Error, 404);
            Assert.AreEqual(res2.GetError<int>(), 404);

            var res3 = GetStatusOrMultipleErrors(3);

            Assert.IsFalse(res3);
            Assert.IsFalse(res3.IsSuccess);
            Assert.IsTrue(res3.IsError);
            Assert.IsTrue(res3.HasError<string>());
            Assert.AreEqual(res3.Error, "Invalid Operation");
            Assert.AreEqual(res3.GetError<string>(), "Invalid Operation");
        }
    }
}
