using System;
using System.Collections;

namespace Examples
{
    public class CleanCode_06_Resultaat
    {
        private const string PhysicalAppPath = "xyz";
        private const string XsdName = "abc";
        private readonly IXSDAdapterFactory xsdAdapterFactory;

        public CleanCode_06_Resultaat(IXSDAdapterFactory xsdAdapterFactory)
        {
            this.xsdAdapterFactory = xsdAdapterFactory;
        }

        /// <summary>
        /// Sets the datatype on the Filter- en DataFields of the SQLBuilder
        /// </summary>
        /// <param name="sqlb">SQLBuilder</param>
        public void SetFieldDataTypes(SQLBuilder sqlb)
        {
            XSDAdapter xsda = xsdAdapterFactory.Create(PhysicalAppPath);

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

        private static void SetDataFieldDataTypes(SQLBuilder sqlb, XSDAdapter xsda)
        {
            if (MustSetDataFieldDataTypes(sqlb))
            {
                string[] datatypes = GetDataFieldDataTypes(sqlb, xsda);

                SetFieldDataType(sqlb, datatypes, typeof (DataField));
            }
        }

        private static bool MustSetDataFieldDataTypes(SQLBuilder sqlb)
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

    public interface IXSDAdapterFactory
    {
        XSDAdapter Create(string path);
    }

    public class XSDAdapterFactory : IXSDAdapterFactory
    {
        public XSDAdapter Create(string path)
        {
            return new XSDAdapter(path);
        }
    }
}

