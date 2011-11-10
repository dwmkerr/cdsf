﻿using CSDFServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace CSDFServer.Tests
{
    
    
    /// <summary>
    ///This is a test class for MetadataAggregatorTest and is intended
    ///to contain all MetadataAggregatorTest Unit Tests
    ///</summary>
  [TestClass()]
  public class MetadataAggregatorTest
  {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext
    {
      get
      {
        return testContextInstance;
      }
      set
      {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for AggregateMetadata
    ///</summary>
    [TestMethod()]
    public void AggregateMetadataTest()
    {
      //  Get metadata one.
      WebClient webClient = new WebClient();
      string metadata = webClient.DownloadString("http://localhost:1212/UsersDataService.svc/$metadata");

      EdmxFile f = new EdmxFile();
      f.Load(metadata);

      MetadataAggregator target = new MetadataAggregator(); // TODO: Initialize to an appropriate value
      target.AggregateMetadata(metadata);
      Assert.Inconclusive("A method that does not return a value cannot be verified.");
    }
  }
}