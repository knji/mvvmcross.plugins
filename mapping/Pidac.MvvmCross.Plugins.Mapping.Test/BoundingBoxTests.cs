using System;
using System.Linq;
using NUnit.Framework;
using Pidac.MvvmCross.Plugins.Mapping.Geometries;

namespace Pidac.MvvmCross.Plugins.Mapping.Test
{
    [TestFixture]
    public class BoundingBoxTests
    {
        [Test]
        public void TestUnion()
        {
            var bb = new BoundingBox();
            var bb1 = new BoundingBox(-10, 10, 10, 10);
            var union = bb1.Union(bb);
            Assert.That(union.Equals(bb));
        }

        [Test]
        public void TestCreationByFeatures()
        {
            var features = new []
                {
                    new Feature(new Point(-15, -25)), 
                    new Feature(new Point(-15, 10)), 
                    new Feature(new Point(20, 10)), 
                    new Feature(new Point(20, -25))
                };
            
            var bb = new BoundingBox(features.Select( f => f.Geometry).ToArray());
            Console.WriteLine(bb.ToString());
            Assert.That(bb.Equals(new BoundingBox(-15, -25, 20, 10)));
        }


        [Test]
        public void TestBufferBy()
        {
            var features = new[]
                {
                    new Feature(new Point(-15, -25)), 
                    new Feature(new Point(-15, 10)), 
                    new Feature(new Point(20, 10)), 
                    new Feature(new Point(20, -25))
                };

            var bb = new BoundingBox(features.Select(f => f.Geometry).ToArray()).BufferBy(2);
            Assert.That(bb.Equals(new BoundingBox(-17, -27, 22, 12)));
        }
    }
}
