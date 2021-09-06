using NUnit.Framework;
using Sparcpoint.Documentation.Sql;
using Sparcpoint.Documentation.Stubble;

namespace Sparcpoint.Documentation.UnitTests;

public class FrameworkAssertions_Stubble
{
    private StubbleTemplateProcessor _Processor;

    [SetUp]
    public void Setup()
    {
        _Processor = new StubbleTemplateProcessor();
    }

    [Test]
    public async Task StubbleListsRender()
    {
        const string TEMPLATE = @"
# TABLE {{Identifier}}
{{{Description}}}

## COLUMNS
| Name | Description | DataType |
| ---- | ----------- | -------- |
{{#Columns}}
| {{Name}} | {{{Description}}} | {{DataType}} |
{{/Columns}}
        ";

        var model = new TableModel(new SchemaModel { })
        {
            Identifier = new SqlIdentifier("Users", "dbo"),
            Description = "Holds all users within the system."
        };

        model.Columns = new[]
        {
            new TableColumnModel(model)
            {
                Name = "UserId",
                Description = "The primary key",
                DataType = "INT"
            },
            new TableColumnModel(model)
            {
                Name = "Name",
                Description = "The name of the user.",
                DataType = "VARCHAR (64)"
            }
        };

        string output = await _Processor.ProcessAsync(new Abstractions.Template { Value = TEMPLATE }, model);
        Assert.AreEqual(@"
# TABLE [dbo].[Users]
Holds all users within the system.

## COLUMNS
| Name | Description | DataType |
| ---- | ----------- | -------- |
| UserId | The primary key | INT |
| Name | The name of the user. | VARCHAR (64) |
        ", output);
    }
}