﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Jasily.DependencyInjection.MethodInvoker;
using Jasily.Extensions.System.Collections.Generic;
using Jasily.Frameworks.Cli.Attributes;
using Jasily.Frameworks.Cli.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace Jasily.Frameworks.Cli.Commands
{
    internal sealed class PropertyCommand<T> : BaseCommand
    {
        private IInstanceMethodInvoker<T> _invoker;

        public PropertyCommand(TypeConfiguration<T>.PropertyConfiguration configuration) : base(configuration.Property.GetMethod, configuration)
        {
            this.ParameterConfigurations = new IParameterConfiguration[0].AsReadOnly();
        }

        public override IReadOnlyList<IParameterConfiguration> ParameterConfigurations { get; }

        public override object Invoke(object instance, IServiceProvider serviceProvider, OverrideArguments args)
        {
            if (this._invoker == null)
            {
                this._invoker = serviceProvider
                    .GetRequiredService<IMethodInvokerFactory<T>>()
                    .GetInstanceMethodInvoker((MethodInfo)this.Method);
            }

            return this._invoker.Invoke((T)instance, serviceProvider, args);
        }
    }
}
