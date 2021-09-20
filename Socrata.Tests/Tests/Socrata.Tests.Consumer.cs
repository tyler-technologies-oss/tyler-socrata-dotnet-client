using NUnit.Framework;
using System;
using Socrata.SODA;
using System.Collections.Generic;

namespace Socrata
{
    [TestFixture]
    public class ConsumerTests
    {
        /******************/
        /* Consumer Tests */
        /******************/
        SocrataClient socrataClient = new SocrataClient(new Uri("https://peter.demo.socrata.com"), Environment.GetEnvironmentVariable("SODA_USERNAME"), Environment.GetEnvironmentVariable("SODA_PASSWORD"));
        string datasetFixture = "tzmz-8bnb";

        [Test]
        public void TestFetchRows()
        {
            Resource resource = socrataClient.GetResource(datasetFixture);
            Rows rows = resource.Rows();
            List<TestJson> result = rows.Fetch<TestJson>(1000, 0);
            Assert.AreEqual(result.Count, 1000);
        }

        [Test]
        public void TestFetchAllRows()
        {
            Resource resource = socrataClient.GetResource(datasetFixture);
            Rows rows = resource.Rows();
            List<TestJson> result = rows.FetchAll<TestJson>();
            Assert.IsTrue(result.Count > 1000);
        }

        [Test]
        public void TestRowCount()
        {
            Resource resource = socrataClient.GetResource(datasetFixture);
            Rows rows = resource.Rows();
            long result = rows.Count();
            Assert.IsTrue(result > 3000);
        }

        [Test]
        public void TestPagination()
        {
            Resource resource = socrataClient.GetResource(datasetFixture);
            Rows rows = resource.Rows();
            long total = rows.Count();
            long limit = 1000;
            long offset = 0;
            while(offset < total)
            {
                rows.Fetch<Dictionary<string, object>>(limit, offset);
                offset += limit;
            }
            Assert.AreEqual(offset, 4000);
        }
    }
}
