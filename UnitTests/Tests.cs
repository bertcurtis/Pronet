using System.Threading;
using System;
using Xunit;
using Common.Helpers;

namespace UnitTests
{
    public class Tests
    {
        [Fact]
        public void TestPerformanceHelper()
        {
            PerformanceHelper.Instance = new PerformanceHelper();
            PerformanceHelper.Instance.StartMeasure();
            Thread.Sleep(3000);
            PerformanceHelper.Instance.StopMeasure();
            Assert.True(PerformanceHelper.Instance.GetLoadTimeList[0].Duration > 3000);
        }
    }
}
