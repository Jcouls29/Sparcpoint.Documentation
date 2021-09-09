# [{{Identifier.SchemaString}}](./{{Identifier.SchemaString}}.md).{{Identifier.NameString}} *(Table)*

{{{Description}}}

{{#HasCreateStatement}}
```SQL
{{{CreateStatement}}}
```
{{/HasCreateStatement}}

## Columns

| PK | Name | DataType | Default Value | Description |
| -- | ---- | -------- | ------------- | ----------- |
{{#Columns}}
| {{#IsPrimaryKey}}<img src="https://github.githubassets.com/images/icons/emoji/unicode/1f511.png?v" height=16 />{{/IsPrimaryKey}} | {{Name}} | {{DataType}} {{#IsNullable}}<b>?</b>{{/IsNullable}} | {{DefaultValue}} | {{{Description}}}
{{/Columns}}

{{#HasIndices}}
## Indices
{{#Indices}}
### `{{Identifier}}`
{{{Description}}}

{{#HasCreateStatement}}
```SQL
{{{CreateStatement}}}
```
{{/HasCreateStatement}}
{{/Indices}}
{{/HasIndices}}

{{#HasUniqueIndices}}
## Unique Indices
{{#UniqueIndices}}
### `{{Identifier}}`
{{{Description}}}

{{#HasCreateStatement}}
```SQL
{{{CreateStatement}}}
```
{{/HasCreateStatement}}
{{/UniqueIndices}}
{{/HasUniqueIndices}}

{{#HasForeignKeys}}
## Foreign Keys
{{#ForeignKeys}}
### `{{Name}}`
Target: [{{TargetTable.Identifier}}](./{{TargetTable.Identifier}}.md)

{{{Description}}}

| Local | Description | Foreign | Description |
| ----- | ----------- | ------- | ----------- |
{{#ColumnMapping}}
| {{Local.Name}} | {{Local.Description}} | {{Foreign.Name}} | {{Foreign.Description}} |
{{/ColumnMapping}}
{{/ForeignKeys}}
{{/HasForeignKeys}}

{{#HasTriggers}}
## Triggers
{{#Triggers}}
### `{{Identifier}}`
{{{Description}}}

{{#HasCreateStatement}}
```SQL
{{{CreateStatement}}}
```
{{/HasCreateStatement}}

{{/Triggers}}
{{/HasTriggers}}