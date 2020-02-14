﻿using collNotes.Data.Models;
using System.Collections.Generic;
using Xunit;

namespace collNotes.UnitTests.Data.Models.PropertyValidators
{
    public class SettingPropertyValidator
    {
        [Fact]
        public void Setting_ValidateProperties()
        {
            // setup
            List<string> propertyNames = new List<string>()
            {
                "SettingID",
                "SettingName",
                "SettingValue",
                "LastSaved"
            };
            // act
            var isValid = ModelClassValidator.ClassValidator(typeof(Setting), propertyNames);
            // assert
            Assert.True(isValid);
        }
    }
}