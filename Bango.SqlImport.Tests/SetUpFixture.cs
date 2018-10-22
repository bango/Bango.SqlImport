using System;
using System.IO;
using NUnit.Framework;

namespace Bango.SqlImport.Tests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //NUnit 3 by default sets the current directory to User\AppData\
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
