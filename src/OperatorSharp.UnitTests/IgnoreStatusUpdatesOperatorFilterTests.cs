﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OperatorSharp.Filters;
using OperatorSharp.UnitTests.Examples;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OperatorSharp.UnitTests
{
    [TestClass]
    public class IgnoreStatusUpdatesOperatorFilterTests
    {
        [TestMethod]
        public void WhenGenerationMatches_FilterReturnsFalse()
        {
            var filter = new IgnoreStatusUpdatesOperatorFilter<ExampleResource, ExampleStatus>();
            var message = new ExampleResource() { Metadata = new k8s.Models.V1ObjectMeta(generation: 1), Status = new ExampleStatus() { ObservedGeneration = 1 } };

            var result = filter.Execute(k8s.WatchEventType.Added, message);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WhenGenerationDoesntMatch_FilterReturnsTrue()
        {
            var filter = new IgnoreStatusUpdatesOperatorFilter<ExampleResource, ExampleStatus>();
            var message = new ExampleResource() { Metadata = new k8s.Models.V1ObjectMeta(generation: 2), Status = new ExampleStatus() { ObservedGeneration = 1 } };

            var result = filter.Execute(k8s.WatchEventType.Added, message);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenStatusHasntBeenInitialized_FilterReturnsTrue()
        {
            var filter = new IgnoreStatusUpdatesOperatorFilter<ExampleResource, ExampleStatus>();
            var message = new ExampleResource() { Metadata = new k8s.Models.V1ObjectMeta(generation: 2) };

            var result = filter.Execute(k8s.WatchEventType.Added, message);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenObservedGenerationIsNull_FilterReturnsTrue()
        {
            var filter = new IgnoreStatusUpdatesOperatorFilter<ExampleResource, ExampleStatus>();
            var message = new ExampleResource() { Metadata = new k8s.Models.V1ObjectMeta(generation: 2), Status = new ExampleStatus() };

            var result = filter.Execute(k8s.WatchEventType.Added, message);

            Assert.IsTrue(result);
        }
    }
}
