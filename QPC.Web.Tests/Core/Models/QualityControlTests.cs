using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QPC.Core.Models;
using QPC.Core.DTOs;

namespace QPC.Web.Tests.Core.Models
{
    [TestClass]
    public class QualityControlTests
    {
        private Mock<QualityControl> _qualityControl;

        [TestInitialize]
        public void TestInitialize()
        {
            //Fields Initialization;
            _qualityControl = new Mock<QualityControl>();
        }
    }
}
