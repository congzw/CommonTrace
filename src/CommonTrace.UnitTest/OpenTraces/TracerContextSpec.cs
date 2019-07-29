using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTrace.OpenTraces
{
    [TestClass]
    public class TracerContextSpec
    {
        [TestMethod]
        public void GetCurrent_ArgsNull_Should_Return_Default()
        {
            var tracer = TracerContext.GetCurrent(null);
            tracer.ShouldNotNull();
            var tracer2 = TracerContext.GetCurrent(null);
            tracer2.ShouldSame(tracer);
        }

        [TestMethod]
        public void GetCurrent_ArgsSame_Should_Ok()
        {
            var tracer = TracerContext.GetCurrent("A");
            tracer.ShouldNotNull();
            var tracer2 = TracerContext.GetCurrent("a");
            tracer2.ShouldSame(tracer);
        }

        [TestMethod]
        public void GetCurrent_ArgsDiff_Should_Ok()
        {
            var tracer = TracerContext.GetCurrent("A");
            tracer.ShouldNotNull();
            var tracer2 = TracerContext.GetCurrent("B");
            tracer2.ShouldNotSame(tracer);
        }
    }
}
