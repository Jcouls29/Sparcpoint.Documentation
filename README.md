Sparcpoint.Documentation (sp-docs)
==================================

CLI tool for compiling (building) documentation for various types of files and projects.

# Installation
The `sp-docs` tool can be installed via `dotnet tool install` using the following command:

```bash
dotnet tool install --global sp-docs
```

Once installed, usage is simply calling

```bash
sp-docs --help
```

# Supported Documentation Types
## SQL Server (`build-sql`)
--------------------------------
Converts a set of SQL Server scripts (*.sql) to linked markdown files (by default).

> **NOTE**: Only Microsoft SQL Server 15.0 is currently supported by the system.

The input `path` parameter is the root directory to search for `.sql` files and attempt to compile.

The output `output` parameter is the directory to store all rendered files. If the directory does not exist, it will be created automatically.

If no other parameters are supplied, the `default` template is used to render the SQL files. However, by supplying a path to the `--template` parameter, you can specify your own templates.

If an index page is not required, utilize the `--no-index-page` parameter to ignore this output.

Warnings will show for all statements and constraints that are not currently supported by the system.

### Templates
-------------
Templates are simply a set of flat text files of each model type allowed in the compilation process.

For instance, the default template is a set of markdown files that utilize `Stubble (Mustache)` template rendering.

> **Note**: Only `Stubble` (Mustache) template syntax is currently supported

| Filename | Description |
| -------- | ----------- |
| Index.md | The primary index file that can be used as the jumping off point for the remainder of the schema |
| SchemaModel.md | Describes a schema and includes links to all associated schema objects (tables, types, views, etc.) |
| TableModel.md | Describes a table and includes all columns, constraints, and foreign keys |
| ViewModel.md | Describes a view |
| StoredProcedureModel.md | Describes a Stored Procedure |
| FunctionModel.md | Describes a user-defined function |
| TableTypeModel.md | Describes a user-defined table type |
| DataTypeModel.md | Describes a user-defined data type |
| SequenceModel.md | Describes a sequence |

> **NOTE**: Not all types are currently supported and the current types only support the `CREATE` syntax. `ALTER` syntax is not currently supported.

> **FUTURE**: Describe the available data on the models to provide insight for developers creating their own templates. For now, see the model sources: [./Sparcpoint.Documentation.Sql/Models/](./Sparcpoint.Documentation.Sql/Models/)

### Usage
-------------
```bash
build-sql
  Build the SQL Commands into Markdown

Usage:
  sp-docs [options] build-sql <path> <output>

Arguments:
  <path>    Path to source directory or .dbproj
  <output>  Path to output directory

Options:
  -t, --template <template>  Path to the template directory [default: .\Templates\Default]
  --no-index-page            Do not generate an index page
  -v, --verbose              Show detail output logging
  -?, -h, --help             Show help and usage information
```

#### Example
```bash
sp-docs --verbose build-sql "C:\SqlProject\Scripts" "C:\Temp\Output"
```