using System.Collections.Generic;
using System.Linq;

namespace Sparcpoint.Documentation.Sql
{
    public class SchemaModel : SqlModelBase
    {
        public IList<TableModel> Tables { get; } = new List<TableModel>();
        public bool HasTables => Tables?.Any() ?? false;

        public IList<TableTypeModel> TableTypes { get; } = new List<TableTypeModel>();
        public bool HasTableTypes => TableTypes?.Any() ?? false;

        public IList<SequenceModel> Sequences { get; } = new List<SequenceModel>();
        public bool HasSequences => Sequences?.Any() ?? false;

        public IList<ViewModel> Views { get; } = new List<ViewModel>();
        public bool HasViews => Views?.Any() ?? false;

        public IList<StoredProcedureModel> StoredProcedures { get; } = new List<StoredProcedureModel>();
        public bool HasStoredProcedures => StoredProcedures?.Any() ?? false;
    }
}
