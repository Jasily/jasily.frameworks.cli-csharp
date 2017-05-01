﻿namespace Jasily.Frameworks.Cli.Converters
{
    internal class Int32Converter : StringConverter<int>
    {
        public override int Convert(string value)
        {
            return int.Parse(value);
        }
    }
}