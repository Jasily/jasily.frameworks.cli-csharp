﻿using System;
using System.Threading.Tasks;
using Jasily.Frameworks.Cli.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests.Olds
{
    [TestClass]
    public class TestAsyncCommandClass1 : BaseTest
    {
        [CommandClass]
        public class CommandClass
        {
            [CommandMethod]
            public int Number(int value)
            {
                return value;
            }

            [CommandMethod]
            public int Select(int val1, int val2, int val3)
            {
                return val3;
            }

            public string Usage(
                int val1,
                string val2,
                [CommandParameter(Names = new [] { "valx" })]
                double val3 = 15,
                params string[] val4)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// just <see cref="CommandClass"/> with <see cref="CommandClassAttribute"/> attribute
        /// </summary>
        [CommandClass]
        public class CommandClass2 : CommandClass
        {

        }

        public class AsyncCommandClass1
        {
            public async Task<CommandClass> GetCommandClass()
            {
                await Task.Run(() => { });
                return new CommandClass();
            }

            public async Task<CommandClass> GetNullCommandClass()
            {
                await Task.Run(() => { });
                return null;
            }

            public async Task<CommandClass2> GetCommandClass2()
            {
                await Task.Run(() => { });
                return new CommandClass2();
            }

            public async Task<CommandClass2> GetCommandClass2_Null()
            {
                await Task.Run(() => { });
                return null;
            }
        }

        [TestMethod]
        public void Test()
        {
            foreach (var item in this.Fire<AsyncCommandClass1>())
            {
                Assert.AreEqual(1, item.Execute(new[] {
                    nameof(AsyncCommandClass1.GetCommandClass2),
                    nameof(CommandClass2.Number),
                    "1" }).Value);
                Assert.AreEqual(455, item.Execute(new[] {
                    nameof(AsyncCommandClass1.GetCommandClass2),
                    nameof(CommandClass2.Select),
                    "1", "2", "455" }).Value);
            }
        }
    }
}
