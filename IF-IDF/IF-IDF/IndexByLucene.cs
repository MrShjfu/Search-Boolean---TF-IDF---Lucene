using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Version = Lucene.Net.Util.Version;


namespace IF_IDF
{
    class IndexByLucene
    {
        public DataTable Datatable;

        public IndexByLucene(DataTable data)
        {
            this.Datatable = data;
        }
        public RAMDirectory InitIndex(DataTable dt)
        {
            var directory = new RAMDirectory();
            using (Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30))
            using (var writer = new IndexWriter(directory, analyzer, new IndexWriter.MaxFieldLength(10000)))
            {
                foreach (DataRow row in dt.Rows)
                {
                    var document = new Document();
                    document.Add(new Field("Title", (string)row[$"Title"], Field.Store.YES, index: Field.Index.ANALYZED));
                    document.Add(new Field("Content", (string)row["Content"], Field.Store.YES, index: Field.Index.ANALYZED));
                    writer.AddDocument(document);
                }
                writer.Optimize();
                writer.Flush(true, true, true);
            }
            return directory;
        }

        private DataTable Sample
        {
            get
            {
                var columnNames = Datatable.Columns.Cast<DataColumn>()
                    .Select(x => x.ColumnName)
                    .ToList();
                columnNames.Add("Score");
                var dt = new DataTable();
                foreach (var variable in columnNames)
                {
                    dt.Columns.Add(variable);
                }
                return dt;
            }
        }

        public DataTable Search(string text)
        {
            var table = Sample.Clone();
            var index = InitIndex(Datatable);
            using (var reader = IndexReader.Open(index, true))
            using (var searcher = new IndexSearcher(reader))
            {
                using (Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30))
                {
                    var query = new QueryParser(Version.LUCENE_30, "Content", analyzer) { AllowLeadingWildcard = true };
                    var querySearch = query.Parse(text);
                    var collec = TopScoreDocCollector.Create(10, true);
                    searcher.Search(querySearch, collec);
                    var matches = collec.TopDocs().ScoreDocs;
                    foreach (var item in matches)
                    {
                        var id = item.Doc;
                        var doc = searcher.Doc(id);
                        var row = table.NewRow();
                        row["Title"] = doc.GetField("Title").StringValue;
                        row["Score"] = item.Score.ToString(CultureInfo.InvariantCulture);
                        table.Rows.Add(row);
                    }
                }
            }
            return table;
        }
    }
}
