# tyler-socrata-net-sdk
[![build](https://github.com/tyler-technologies-oss/tyler-socrata-dotnet-client/actions/workflows/dotnet-core.yml/badge.svg)](https://github.com/tyler-technologies-oss/tyler-socrata-dotnet-client/actions/workflows/dotnet-core.yml)

.NET Socrata SDK for reading and writing to Socrata APIs

SocrataClient
-----
```c#
// Create the client
ISocrataClient client = new SocrataClient(new Uri("https://{domain}.data.socrata.com"), "apikey", "apisecret");

// Optionally validate the client connection
bool IsValidConnection = client.ValidateConnection();
```

Socrata Resources
-----
```c#
// Get a resource by its ID*
Resource dataset = client.GetResource("abcd-1234");
```
***Note***
[Dataset IDs](https://support.socrata.com/hc/en-us/articles/202950258-What-is-a-Dataset-UID-or-a-Dataset-4x4-)

Working with Resources
----

### Data Manipulation

#### Socrata Data Types and You

| Socrata Data Type | Rough SQL Mapping | Example |
| ----------------- | ----------------- | ------- |
| Text              | varchar, text     | "Text" |
| Number            | int, float, double | 1, 3.14 |
| Date              | convert(date, 426) | "2020-01-01" |
| Timestamp         | convert(datetime, 426) | "2020-01-01T13:42:10" |
| Boolean           | boolean, bit      | 1, true |
| Null | null | null |

***Notes:*** 
- The escape character for text values is `\` 
- Dates must be passed in as string
- Datetimes will not be converted to specific timezones, so be sure all the dates are in the correct/same timezone when they are exported
- Decimals are demarcated with a `.`
- All of these will be converted to JSON when sent to Socrata, so when in doubt, consult the 

#### Geospatial Data

While the Socrata platform does support geocoding, it is not supported via this SDK currently. Any geospatial data will need to converted into [Well-Known Text (WKT)](https://en.wikipedia.org/wiki/Well-known_text_representation_of_geometry) with WGS84 (ESPG 4326) Latitude and Longitude values. 

In general, a decimal precision of 5 is sufficient.

| Socrata Data Type | Example |
| ----------------- | ------- |
| Point             | "POINT (37.192 -168.223)" |
| MultiPoint        | "MULTIPOINT ((37.192 -168.223),(42.1110 -172.9339))"  |
| LineString        | "LINESTRING (30 10, 10 30, 40 40)"  |
| MultiLineString   | "MULTILINESTRING ((10 10, 20 20, 10 40),(40 40, 30 30, 40 20, 30 10))"  |
| Polygon           | "POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10),(20 30, 35 35, 30 20, 20 30))"  |
| MultiPolygon      | "MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))"  |


Direct Row Manipulation
---

#### Insert
```c#
// Insert Functionality
List<Dictionary<string, string>> newRecords = new List<Dictionary<string, string>> 
{
  new Dictionary<string, string>{{"api_field", "nextvalue"},{"other_field", "test"}}
};
Result result = dataset.Rows().Insert(newRecords);

// Check if it was successful
Assert.IsFalse(Result.IsError);
// Check the results
Assert.AreEqual(newRecords.Length, result.Inserted);
```
##### Update
```c#
// Update Functionality
List<Dictionary<string, object>> recordsToUpdate = new List<Dictionary<string, object>> 
{
  new Dictionary<string, object>{{"row_identifier", 1},{"column_that_changed", "newvalue"}}
};
Result result = dataset.Rows().Update(recordsToUpdate);

// Check if it was successful
Assert.IsFalse(Result.IsError);
// Check the results
Assert.AreEqual(recordsToUpdate.Length, result.Updated);
```
##### Delete
```c#
// Delete Functionality
List<Dictionary<string, string>> recordsToDelete = new List<Dictionary<string, string>> 
{
  new Dictionary<string, string>{{"row_identifier", "ID-1"}}
};
Result result = dataset.Rows().Delete(recordsToDelete);

// Check if it was successful
Assert.IsFalse(Result.IsError);
// Check the results
Assert.AreEqual(recordsToDelete.Length, result.Deleted);
```

Dataset Management API
---
[Full Documentation](https://socratapublishing.docs.apiary.io/#introduction/examples)

### Revision Types
Replace: A full replace of the dataset
Update: A upsert of the contents based off the row identifier
Delete: A delete of the contents based off the row identifier

### File Types
| file extension | content type |
| --- | --- |
| CSV | text/csv |
| TSV | text/tsv |
| KML | google-kml |
| ZIP (shapefile) | application/octet-stream |
| BLOB | application/octet-stream |

### Create a Revision
```c#
  Resource resource = socrataClient.GetResource("tzmz-8bnb");
  Revision revision = resource.OpenRevision(RevisionType.REPLACE);
  Source source = revision.CreateUploadSource("test-csv.csv");
```

### Using an Import Config

### Small (in memory) file uploads
```c#
  string filepath = @"C:\Path\To\MyFile.csv";
  string csv = System.IO.File.ReadAllText(filepath);
  source.AddBytesToSource(csv);
```

### Large (Streaming) file uploads
```c#
  ByteSink sink = source.StreamingSource(ContentType.CSV);
  //Open the stream and read the file.
  sink.FileSink(@"C:\Path\To\MyFile.csv");
```

### Handling error rows
```c#
  if(source.HasErrorRows())
  {
    string errorFile = "C:\Path\To\ErrorRows.csv";
    int numberOfErrors = source.NumberOfErrors();
    source.ExportErrorRows(errorFile);
    // Throw an exception
    throw new Exception($"{numberOfErrors} Error(s) were found in the upload");
  }
```

### Dataset Metadata Only Revisions
```c#
  Resource resource = socrataClient.GetResource("tzmz-8bnb");
  Revision revision = resource.OpenRevision(RevisionType.REPLACE);
  revision.CreateViewSource();
  revision.SetDescription("New description for the <b>resource</b>");
  revision.RenameResource("New Name");
```

### Apply the revision
```c#
  source.AwaitCompletion(status => Console.WriteLine(status));
  revision.Apply();
  revision.AwaitCompletion(status => Console.WriteLine(status));
```

Reading Data
---
You can also use the Rows() to access records in the dataset
```c#
/*
Example custom class
[DataContract]
class MyResource
{
  [DataMember(Name = "column_field")]
  public string ColumnField { get; internal set; }
}
*/
Resource resource = socrataClient.GetResource("tzmz-8bnb");
Rows rows = resource.Rows();
List<MyResource> allRecords = rows.FetchAll<MyResource>();
long limit = 1000;
long offset = 0;
List<MyResource> someRecords = rows.Fetch<MyResource>(limit, offset);

// Conversely, you can just use a Map<string, object>
List<Map<string, object>> allRecords = rows.FetchAll<Map<string, object>>();
```

### Pagination
You might have a really big dataset, which will require paginating over the result set.

```c#
Resource resource = socrataClient.GetResource("tzmz-8bnb");
Rows rows = resource.Rows();
// Get the total number of records
long total = rows.Count();
long limit = 1000;
long offset = 0;
while(offset < total)
{
  List<Map<string, object>> allRecords = rows.Fetch<Map<string, object>>(limit, offset);
  offset += limit;
}
```

Metadata
---

### Accessing Resource Metadata
```c#
Resource dataset = client.GetResource("abcd-1234");
string resourceName = dataset.metadata.Name;
```

Running Tests
----

```bash
dotnet test
```

Or a specific test:
```bash
dotnet test --filter {testname}
```
