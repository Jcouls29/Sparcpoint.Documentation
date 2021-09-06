# Schema: {{Identifier}}
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
| [{{Identifier}}](.\{{Identifier}}.md) | {{{Description}}} |
{{/Tables}}
{{/HasTables}}

{{#HasTableTypes}}
## Table Types
| Name | Description |
| ---- | ----------- |
{{#TableTypes}}
| [{{Identifier}}](.\{{Identifier}}.md) | {{Description}} |
{{/TableTypes}}
{{/HasTableTypes}}