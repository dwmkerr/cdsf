using CompositeDataServiceFramework.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CompositeDataServiceFramework.Server.Tests
{


  /// <summary>
  /// This is a test class for WcfDataServiceMetadataLoaderTest and is intended
  /// to contain all WcfDataServiceMetadataLoaderTest Unit Tests
  /// </summary>
  [TestClass()]
  public class WcfDataServiceMetadataLoaderTest
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
    ///A test for LoadMetadata
    ///</summary>
    [TestMethod()]
    public void LoadMetadataTest()
    {
      WcfDataServiceMetadataLoader target = new WcfDataServiceMetadataLoader();
      Uri serviceUri = new Uri("http://localhost:53282/UsersDataService.svc");
      target.LoadMetadata(serviceUri);

      //  We must have entities etc.
      var entityContainers = target.EntityContainers;
      var entitySets = target.EntitySets;
      var entityTypes = target.EntityTypes;
      
      Assert.Inconclusive("Verify the correctness of this test method.");
    }
  }
}
