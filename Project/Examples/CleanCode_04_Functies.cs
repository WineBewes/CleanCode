using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    public class CleanCode_04_Functies
    {
        private const string PhysicalAppPath = "xyz";
        private const string XsdName = "abc";

        public void SetFieldDataTypes(SQLBuilder sqlb)
        {
            XSDAdapter xsda = new XSDAdapter(PhysicalAppPath);

            SetFilterFieldDataTypes(sqlb, xsda);

            SetDataFieldDataTypes(sqlb, xsda);
        }

        private void SetFilterFieldDataTypes(SQLBuilder sqlb, XSDAdapter xsda)
        {
            if (sqlb.FilterFields.Count > 0)
            {
                //SMELL : "fields" en "datatypes" 
                //kunnen samengebracht worden in 1 nieuwe functie :
                //GetFilterFieldDatatypes
                //dat onmiddellijk string[] datatypes teruggeeft
                string[] fields = GetFilterFieldNames(sqlb);

                string[] datatypes = xsda.GetDataTypes(XsdName, fields);
                //end SMELL

                SetFilterFieldsDataType(sqlb, datatypes);
            }            
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

            return (string[])al_fields.ToArray(typeof(string));
        }

        private static void SetFilterFieldsDataType(SQLBuilder sqlb, string[] datatypes)
        {
            //SMELL : bijna DRY-violation met SetDataFieldsDataType
            //misschien in 1 functie "SetFieldDataType" verenigen ?
            int u = datatypes.GetUpperBound(0);

            for (int i = 0; i <= u; i++)
            {
                FilterField ffield = sqlb.FilterFields.Item(i);
                ffield.DataType = datatypes[i];
            }
        }
        
        private void SetDataFieldDataTypes(SQLBuilder sqlb, XSDAdapter xsda)
        {
			//SMELL : functie met duidelijk naam ipv. comment
			
			//SMELL : level of abstraction
			
			//only get datafield types when not sql select or sql delete            
			if (sqlb.SQLtype != snpSQLtype.sqlSelect && sqlb.SQLtype != snpSQLtype.sqlDelete)
			{
                //SMELL : "fields" en "datatypes" 
                //kunnen samengebracht worden in 1 nieuwe functie :
                //GetFilterFieldDatatypes
                //dat onmiddellijk string[] datatypes teruggeeft
                string[] fields = GetDataFieldNames(sqlb);

                string[] datatypes = xsda.GetDataTypes(XsdName, fields);
                //end SMELL

                SetDataFieldsDataType(sqlb, datatypes);
            }            
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
    
        private static void SetDataFieldsDataType(SQLBuilder sqlb, string[] datatypes)
        {
            //SMELL : bijna DRY-violation met SetFilterFieldsDataType
            //misschien in 1 functie "SetFieldDataType" verenigen ?
            int u = datatypes.GetUpperBound(0);

            for (int i = 0; i <= u; i++)
            {
                DataField dfield = sqlb.DataFields.Item(i);
                dfield.DataType = datatypes[i];
            }
        }
    }
}

