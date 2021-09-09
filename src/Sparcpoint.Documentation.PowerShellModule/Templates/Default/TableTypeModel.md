# [{{Identifier.SchemaString}}](./{{Identifier.SchemaString}}.md).{{Identifier.NameString}} *(User-Defined Table Type)*

{{{Description}}}

{{#HasCreateStatement}}
```SQL
{{{CreateStatement}}}
```
{{/HasCreateStatement}}

## Columns

| Name | DataType | Default Value | Description |
| ---- | -------- | ------------- | ----------- |
{{#Columns}}
| {{Name}} | {{DataType}} {{#IsNullable}}<b>?</b>{{/IsNullable}} | {{DefaultValue}} | {{{Description}}}
{{/Columns}}
