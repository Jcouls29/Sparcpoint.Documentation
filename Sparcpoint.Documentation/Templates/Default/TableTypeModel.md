# [{{Identifier.SchemaString}}](./{{Identifier.SchemaString}}.md).{{Identifier.NameString}} *(User-Defined Table Type)*

{{{Description}}}

{{#HasCreateStatement}}
```SQL
{{{CreateStatement}}}
```
{{/HasCreateStatement}}

## Columns

| Name | DataType | Nullable? | Default Value | Description |
| ---- | -------- | --------- | ------------- | ----------- |
{{#Columns}}
| {{Name}} | {{DataType}} | {{#IsNullable}}Y{{/IsNullable}} | {{DefaultValue}} | {{{Description}}}
{{/Columns}}
