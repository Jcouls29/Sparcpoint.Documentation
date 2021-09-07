# {{Identifier}} *(Schema)*
{{{Description}}}

{{#HasCreateStatement}}
```SQL
{{{CreateStatement}}}
```
{{/HasCreateStatement}}

{{#HasTables}}
## Tables

| Name | Description |
| ---- | ----------- |
{{#Tables}}
| [{{Identifier}}](./{{Identifier}}.md) | {{Description}} |
{{/Tables}}
{{/HasTables}}

{{#HasTableTypes}}
## Table Types
| Name | Description |
| ---- | ----------- |
{{#TableTypes}}
| [{{Identifier}}](./{{Identifier}}.md) | {{Description}} |
{{/TableTypes}}
{{/HasTableTypes}}

{{#HasViews}}
## Views
| Name | Description |
| ---- | ----------- |
{{#Views}}
| [{{Identifier}}](./{{Identifier}}.md) | {{Description}} |
{{/Views}}
{{/HasViews}}

{{#HasStoredProcedures}}
## Stored Procedures
| Name | Description |
| ---- | ----------- |
{{#StoredProcedures}}
| [{{Identifier}}](./{{Identifier}}.md) | {{Description}} |
{{/StoredProcedures}}
{{/HasStoredProcedures}}

{{#HasSequences}}
## Sequences
| Name | Description |
| ---- | ----------- |
{{#Sequences}}
| [{{Identifier}}](./{{Identifier}}.md) | {{Description}} |
{{/Sequences}}
{{/HasSequences}}

{{#HasFunctions}}
## Functions
| Name | Description |
| ---- | ----------- |
{{#Functions}}
| [{{Identifier}}](./{{Identifier}}.md) | {{Description}} |
{{/Functions}}
{{/HasFunctions}}