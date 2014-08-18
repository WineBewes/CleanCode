using System;
using System.Collections;

namespace Examples
{
    public class CleanCode_05_UT_en_DI
    {
        private const string PhysicalAppPath = "xyz";
        private const string XsdName = "abc";

        public void SetFieldDataTypes(SQLBuilder sqlb)
        {
            //DEFECT : DI-violation - gebruik IXSDAAdapterFactory
            XSDAdapter xsda = new XSDAdapter(PhysicalAppPath);

            SetFilterFieldDataTypes(sqlb, xsda);

            SetDataFieldDataTypes(sqlb, xsda);
        }

        private void SetFilterFieldDataTypes(SQLBuilder sqlb, XSDAdapter xsda)
        {
            if (sqlb.FilterFields.Count > 0)
            {
                string[] datatypes = GetFilterFieldDataTypes(sqlb, xsda);

                SetFieldDataType(sqlb, datatypes, typeof (FilterField));
            }
        }

        private static string[] GetFilterFieldDataTypes(SQLBuilder sqlb, XSDAdapter xsda)
        {
            string[] fields = GetFilterFieldNames(sqlb);

            return xsda.GetDataTypes(XsdName, fields);
        }

        private static string[] GetFilterFieldNames(SQLBuilder sqlb)
        {
            ArrayList al_fields = new ArrayList();

            if (sqlb.DDLDataFilterField > 0)
            {
                DataField dfield = sqlb.DataFields.Item(sqlb.DDLDataFilterField - 1);

                al_fields.Add(dfield.Name);
            }
            else
            {
                foreach (FilterField ffield in sqlb.FilterFields)
                {
                    al_fields.Add(ffield.Name);
                }
            }

            return (string[]) al_fields.ToArray(typeof (string));
        }

        private void SetDataFieldDataTypes(SQLBuilder sqlb, XSDAdapter xsda)
        {
            if (MustSetDataFieldDataTypes(sqlb))
            {
                string[] datatypes = GetDataFieldDataTypes(sqlb, xsda);

                SetFieldDataType(sqlb, datatypes, typeof (DataField));
            }
        }

        private bool MustSetDataFieldDataTypes(SQLBuilder sqlb)
        {
            return sqlb.SQLtype != snpSQLtype.sqlSelect && sqlb.SQLtype != snpSQLtype.sqlDelete;
        }

        private static string[] GetDataFieldDataTypes(SQLBuilder sqlb, XSDAdapter xsda)
        {
            string[] fields = GetDataFieldNames(sqlb);

            return xsda.GetDataTypes(XsdName, fields);
        }

        private static string[] GetDataFieldNames(SQLBuilder sqlb)
        {
            var al_fields = new ArrayList();

            foreach (DataField dfield in sqlb.DataFields)
            {
                al_fields.Add(dfield.Name);
            }

            return (string[]) al_fields.ToArray(typeof (string));
        }

        private static void SetFieldDataType(SQLBuilder sqlb, string[] datatypes, Type t)
        {
            int u = datatypes.GetUpperBound(0);

            for (int i = 0; i <= u; i++)
            {
                Field field = null;

                if (t == typeof (FilterField))
                {
                    field = sqlb.FilterFields.Item(i);
                }

                if (t == typeof (DataField))
                {
                    field = sqlb.DataFields.Item(i);
                }

                field.DataType = datatypes[i];
            }
        }
    }

    //SMELL : geen Interface of virtual members
    public class SQLBuilder
    {
        public snpSQLtype SQLtype { get; set; }
        public FilterFields FilterFields { get; set; }
        public DataFields DataFields { get; set; }
        public SortFields SortFields { get; set; }
        public int DDLDataFilterField { get; set; }
        public bool Distinct { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    //SMELL : geen Interface of virtual members
    public class XSDAdapter
    {
        public XSDAdapter(string path)
        {

        }

        public string[] GetDataTypes(string a, string[] fields)
        {
            return new string[0];
        }
    }
}

