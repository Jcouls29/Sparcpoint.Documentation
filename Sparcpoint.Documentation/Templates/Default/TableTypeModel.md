# User Defined Table Type: {{Identifier}}
#### Schema: [{{Identifier.Schema}}](.\{{Identifier.Schema}}.md)

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
| {{Name}} | {{DataType}} | {{IsNullable}} | {{DefaultValue}} | {{{Description}}}
{{/Columns}}
