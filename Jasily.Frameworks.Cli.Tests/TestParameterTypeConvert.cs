﻿using System;
using Jasily.Frameworks.Cli.Attributes.Parameters;
using Jasily.Frameworks.Cli.Tests.Olds;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jasily.Frameworks.Cli.Tests
{
    [TestClass]
    public class TestParameterTypeConvert : BaseTest
    {
        public class CommandClass
        {
            public bool Boolean(bool value) => value;

            public char Char(char value) => value;

            public sbyte SByte(sbyte value) => value;

            public byte Byte(byte value) => value;

            public short Int16(short value) => value;

            public ushort UInt16(ushort value) => value;

            public int Int32(int value) => value;

            public uint UInt32(uint value) => value;

            public long Int64(long value) => value;

            public ulong UInt64(ulong value) => value;

            public float Single(float value) => value;

            public double Double(double value) => value;

            public decimal Decimal(decimal value) => value;

            public DateTime DateTime(DateTime value) => value;

            public string String(string value) => value;

            public string[] Array(string[] value) => value;

            public string[] ParamsArray(params string[] value) => value;

            public int[] Int32Array(int[] value) => value;

            public int[] Int32ParamsArray(params int[] value) => value;

            public StringComparison Enum(StringComparison val) => val;
        }

        [TestMethod]
        public void TypeShouldConvertCorrect()
        {
            var executor = this.Fire(new CommandClass(), out var sb);
            Assert.AreEqual(5, executor.Execute(new[] { nameof(CommandClass.Int32), "5" }).Value);
            Assert.AreEqual((uint)5, executor.Execute(new[] { nameof(CommandClass.UInt32), "5" }).Value);
            Assert.AreEqual((long)5, executor.Execute(new[] { nameof(CommandClass.Int64), "5" }).Value);
            Assert.AreEqual((ulong)5, executor.Execute(new[] { nameof(CommandClass.UInt64), "5" }).Value);
            Assert.AreEqual((float)5, executor.Execute(new[] { nameof(CommandClass.Single), "5" }).Value);
            Assert.AreEqual((double)5, executor.Execute(new[] { nameof(CommandClass.Double), "5" }).Value);
            CollectionAssert.AreEqual(new[] { "5", "8" }, (string[]) executor.Execute(
                new[] { nameof(CommandClass.Array), "5", "8" }).Value);
            CollectionAssert.AreEqual(new[] { "5", "8" }, (string[]) executor.Execute(
                new[] { nameof(CommandClass.ParamsArray), "5", "8" }).Value);
            CollectionAssert.AreEqual(new[] { 5, 8 }, (int[])executor.Execute(
                new[] { nameof(CommandClass.Int32Array), "5", "8" }).Value);
            CollectionAssert.AreEqual(new[] { 5, 8 }, (int[])executor.Execute(
                new[] { nameof(CommandClass.Int32ParamsArray), "5", "8" }).Value);
            Assert.AreEqual(StringComparison.InvariantCultureIgnoreCase,
                executor.Execute(new[] { nameof(CommandClass.Enum), StringComparison.InvariantCultureIgnoreCase.ToString() }).Value);
        }

        [TestMethod]
        public void TypeConvertWrong()
        {
            var engine = this.Fire(new CommandClass(), out var sb);
            Assert.AreEqual(null, engine.Execute(new[] { nameof(CommandClass.Int32), "5.2" }).Value);
            //Assert.AreEqual((uint)5, engine.Execute(new[] { nameof(CommandClass.UInt32), "5" }));
            //Assert.AreEqual((long)5, engine.Execute(new[] { nameof(CommandClass.Int64), "5" }));
            //Assert.AreEqual((ulong)5, engine.Execute(new[] { nameof(CommandClass.UInt64), "5" }));
            //Assert.AreEqual((float)5, engine.Execute(new[] { nameof(CommandClass.Single), "5" }));
            //Assert.AreEqual((double)5, engine.Execute(new[] { nameof(CommandClass.Double), "5" }));
        }

        public class BooleanParamerterCommandClass
        {
            public bool Func(bool value) => value;

            public bool FuncA([BoolParameter(true, "x"), BoolParameter(false, "y")] bool value) => value;
        }

        [TestMethod]
        public void TestBooleanTypedParamerterConvert()
        {
            var executor = this.Fire(new BooleanParamerterCommandClass(), out var _);
            
            Assert.AreEqual(true, executor.Execute(new[] { nameof(BooleanParamerterCommandClass.Func), "true" }).Value);
            Assert.AreEqual(false, executor.Execute(new[] { nameof(BooleanParamerterCommandClass.Func), "false" }).Value);

            Assert.AreEqual(null, executor.Execute(new[] { nameof(BooleanParamerterCommandClass.Func), "x" }).Value);
            Assert.AreEqual(null, executor.Execute(new[] { nameof(BooleanParamerterCommandClass.Func), "y" }).Value);

            Assert.AreEqual(true, executor.Execute(new[] { nameof(BooleanParamerterCommandClass.FuncA), "x" }).Value);
            Assert.AreEqual(false, executor.Execute(new[] { nameof(BooleanParamerterCommandClass.FuncA), "y" }).Value);
        }
    }
}