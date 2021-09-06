# Table: {{Identifier}}
#### Schema: [{{Identifier.Schema}}](.\{{Identifier.Schema}}.md)

{{{Description}}}

{{#HasCreateStatement}}
```SQL
{{{CreateStatement}}}
```
{{/HasCreateStatement}}

## Columns

| Name | DataType | Nullable? | Primary Key? | Default Value | Description |
| ---- | -------- | --------- | ------------ | ------------- | ----------- |
{{#Columns}}
| {{Name}} | {{DataType}} | {{IsNullable}} | {{IsPrimaryKey}} | {{DefaultValue}} | {{{Description}}}
{{/Columns}}

{{#HasIndices}}
## Indices
{{#Indices}}
### {{Identifier}}
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
### {{Identifier}}
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
### {{Name}}
Target: [{{TargetTable.Identifier}}](.\{{TargetTable.Identifier}}.md)

| Local | Description | Foreign | Description |
| ----- | ----------- | ------- | ----------- |
{{#ColumnMapping}}
| {{Local.Name}} | {{Local.Description}} | {{Foreign.Name}} | {{Foreign.Description}} |
{{/ColumnMapping}}
{{/ForeignKeys}}
{{/HasForeignKeys}}