﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TechTalk.SpecFlow.Configuration
{
    /// <summary>
    /// Represents the subset of runtime configuration that might be interesing for the tooling
    /// </summary>
    public class RuntimeConfigurationForGenerator
    {
        public CultureInfo BindingCulture { get; set; }

        private readonly List<string> additionalStepAssemblies = new List<string>();

        public IEnumerable<string> AdditionalStepAssemblies
        {
            get
            {
                return additionalStepAssemblies;
            }
        }

        public RuntimeConfigurationForGenerator()
        {
            BindingCulture = null;
        }

        internal void LoadConfiguration(ConfigurationSectionHandler configSection)
        {
            if (configSection == null) throw new ArgumentNullException("configSection");

            if (IsSpecified(configSection.BindingCulture))
            {
                this.BindingCulture = CultureInfo.GetCultureInfo(configSection.BindingCulture.Name);
            }

            foreach (var element in configSection.StepAssemblies)
            {
                var stepAssembly = ((StepAssemblyConfigElement)element).Assembly;
                this.additionalStepAssemblies.Add(stepAssembly);
            }
        }

        private bool IsSpecified(ConfigurationElement configurationElement)
        {
            return configurationElement != null && configurationElement.ElementInformation.IsPresent;
        }

        #region Equality

        public bool Equals(RuntimeConfigurationForGenerator other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.additionalStepAssemblies.SequenceEqual(additionalStepAssemblies) && Equals(other.BindingCulture, BindingCulture);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (RuntimeConfigurationForGenerator)) return false;
            return Equals((RuntimeConfigurationForGenerator) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((additionalStepAssemblies.Count.GetHashCode())*397) ^ (BindingCulture != null ? BindingCulture.GetHashCode() : 0);
            }
        }
        #endregion
    }
}