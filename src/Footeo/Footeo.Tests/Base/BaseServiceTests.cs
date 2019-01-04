namespace Footeo.Tests.Base
{
    using AutoMapper;

    using Footeo.Web.Infrastructure.Mapping;

    using NUnit.Framework;

    [TestFixture]
    public class BaseServiceTests
    {
        [SetUp]
        public void TestInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
        }

        [TearDown]
        public void TestCleanUp() { }
    }
}