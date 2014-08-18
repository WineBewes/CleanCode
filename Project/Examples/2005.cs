using System;
using System.Collections;

namespace Examples
{
    //versie C# : 2.0
    public class Code2005
    {
        private string _physappath = "xyz";
        private string _xsdn = "abc";
    
        public void GetFieldDataTypes(SQLBuilder sqlb)
        {
            XSDAdapter xsda = new XSDAdapter(_physappath);

            ArrayList al_fields = new ArrayList();

            //getting datatypes for filterfields

            if (sqlb.FilterFields.Count > 0)
            {
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

                string[] fields = (string[])al_fields.ToArray(typeof(string));

                string[] datatypes = xsda.GetDataTypes(_xsdn, fields);

                int u = datatypes.GetUpperBound(0);

                for (int i = 0; i <= u; i++)
                {
                    FilterField ffield = sqlb.FilterFields.Item(i);
                    ffield.DataType = datatypes[i];
                }
            }


            //only get datafield types when not sql select or sql delete
            if (sqlb.SQLtype != snpSQLtype.sqlSelect && sqlb.SQLtype != snpSQLtype.sqlDelete)
            {
                al_fields = new ArrayList();

                foreach (DataField dfield in sqlb.DataFields)
                {
                    al_fields.Add(dfield.Name);
                }

                string[] fields = (string[])al_fields.ToArray(typeof(string));

                string[] datatypes = xsda.GetDataTypes(_xsdn, fields);

                int u = datatypes.GetUpperBound(0);

                for (int i = 0; i <= u; i++)
                {
                    DataField dfield = sqlb.DataFields.Item(i);
                    dfield.DataType = datatypes[i];
                }
            }
        }
    }
    
    public enum snpSQLtype
    {
        sqlSelect,
        sqlDelete,
        sqlCreate,
        sqlUpdate
    }

    public class FilterFields : CollectionBase
    {
        public FilterField Item(int index)
        {
            throw new NotImplementedException();
        }
        public void Add(FilterField filterField)
        {

        }
    }

    public class SortFields : CollectionBase
    {
        public SortField Item(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(SortField sortField)
        {

        }
    }

    public class DataFields : CollectionBase
    {
        public DataField Item(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(DataField dataField)
        {
            
        }
    }

    public enum snpFilterType
    {
        Equal
    }

    public enum snpOrder
    {
        ASC,
        DESC
    }

    public class FilterField : Field
    {
        public snpFilterType FilterType { get; set; }
        public bool ParentKey { get; set; }
    }

    public class DataField : Field
    {
        public bool PKField { get; set; }
        
    }

    public class SortField : Field
    {

    }

    public abstract class Field
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public snpOrder Order { get; set; }
    }

}
