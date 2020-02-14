﻿using collNotes.Data.Models;
using System.Collections.Generic;
using Xunit;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public class TripPropertyValidator
    {
        [Fact]
        public void Trip_ValidateProperties()
        {
            // setup
            List<string> propertyNames = new List<string>()
            {
                "TripID",
                "PrimaryCollector",
                "AdditionalCollectors",
                "CollectionDate",
                "TripNumber",
                "TripName"
            };
            // act
            var isValid = ModelClassValidator.ClassValidator(typeof(Trip), propertyNames);
            // assert
            Assert.True(isValid);
        }
    }
}