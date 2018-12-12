// <copyright file="ProjectTestBase.cs" company="Objectivity Bespoke Software Specialists">
// Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <license>
//     The MIT License (MIT)
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//     SOFTWARE.
// </license>
using global::NUnit.Framework;
using global::NUnit.Framework.Interfaces;
using System;
using System.IO;
using Pronet.Logger;
using Pronet.Driver;
using Pronet.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;


namespace Pronet
{
    /// <summary>
    /// The base class for all tests <see href="https://github.com/ObjectivityLtd/Test.Automation/wiki/ProjectTestBase-class">More details on wiki</see>
    /// </summary>
    public abstract class ProjectTestBase : TestBase
    {
        private static readonly DriverContext driverContext = new DriverContext();

        public ProjectTestBase() : base(driverContext)
        {

        }
        /// <summary>
        /// Gets or sets logger instance for driver
        /// </summary>
        public TestLogger LogTest
        {
            get
            {
                return this.DriverContext.LogTest;
            }

            set
            {
                this.DriverContext.LogTest = value;
            }
        }
       
        ///// <summary>
        ///// Before the class.
        ///// </summary>
        //[OneTimeSetUp]
        //public void BeforeClass()
        //{
        //    this.DriverContext.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        //    StartPerformanceMeasure();
        //    this.DriverContext.Start();
        //}

        ///// <summary>
        ///// After the class.
        ///// </summary>
        //[OneTimeTearDown]
        //public void AfterClass()
        //{
        //    StopPerfromanceMeasure();
        //    this.DriverContext.DeleteAllCookies();
        //    this.DriverContext.Stop();
        //}

        /// <summary>
        /// Before the test.
        /// </summary>
        [SetUp]
        public void BeforeTest()
        {
            if (BaseConfiguration.ShouldDestroyChrome)
            {
                DestroyAnyNonClosedBrowsers();
            }
            this.DriverContext.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            this.DriverContext.TestTitle = TestContext.CurrentContext.Test.Name;
            StartPerformanceMeasure();
            this.DriverContext.Start();
            this.LogTest.LogTestStarting(this.DriverContext);
            this.DriverContext.WindowMaximize();
        }

        /// <summary>
        /// After the test.
        /// </summary>
        [TearDown]
        public void AfterTest()
        {
            this.DriverContext.IsTestFailed = TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed || !this.DriverContext.VerifyMessages.Count.Equals(0);
            //var filePaths = this.SaveTestDetailsIfTestFailed(this.DriverContext);
            //this.SaveAttachmentsToTestContext(filePaths);
            this.LogTest.LogTestEnding(this.DriverContext);
            if (this.IsVerifyFailedAndClearMessages(this.DriverContext) && TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            {
                Assert.Fail("Check logs to see which verifies failed");
            }
            this.DriverContext.Stop();
            Thread.Sleep(2000);
        }

        /*private void SaveAttachmentsToTestContext(string[] filePaths)
        {
            if (filePaths != null)
            {
                foreach (var filePath in filePaths)
                {                   
                    AddTestAttachment(filePath);
                }
                this.LogTest.Info("Uploaded file [{0}] and {1} more to test context", filePaths[0], (filePaths.Length - 1).ToString());
            }
        } */      
    }
}
