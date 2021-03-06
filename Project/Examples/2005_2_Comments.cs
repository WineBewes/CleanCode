using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace Examples
{
	/// <summary>
    /// Summary description for Code_2005_2_Comments.
	/// </summary>
    internal class Code_2005_2_Comments
	{
        public void AddGridToCollection(DBSNAPGridView sdg, DataGridNameCollection thispage_dgnc)
		{
			// is it already in the collection ?
			//if not, create a new datagridcouple and add it to the collection
			if (!IsDGNinDGNC(sdg.ID, thispage_dgnc)) 
			{
				DataGridControl dc = new DataGridControl();
				dc.GridName = sdg.ID;
				dc.ViewSource = sdg.ViewSource;
				dc.ActionTarget = sdg.ActionTarget;
				dc.SQLB.Distinct = sdg.Distinct;

				if (sdg.AllowPaging)
				{
					dc.SQLB.PageIndex = sdg.PageIndex;
					dc.SQLB.PageSize = sdg.PageSize;
				}
				else
				{
					dc.SQLB.PageSize = -1;
				}
				
				//setting the keyfields in the SQLB of the dc
				string[] keyfields = sdg.PrimaryKey.Split(',');
				foreach(string keyfield in keyfields)
				{
					string kf = keyfield.Trim();
					DataField dfield = new DataField();
					dfield.Name = kf;
					dfield.PKField = true;
					dc.SQLB.DataFields.Add(dfield);
				}
				
				dc.ParentControl = sdg.ParentControl;
				dc.ParentKeyExpression = sdg.ParentKeyExpression;

				//setting the parentkey fields as filters
				//getting the keys
				if (dc.ParentKeyExpression != null && dc.ParentKeyExpression != "")
				{
					string[] parentkeys = sdg.ParentKeyExpression.Split(',');

					foreach(string parentkey in parentkeys)
					{
						string[] keys = parentkey.Split('=');

						FilterField ffield = new FilterField();
						ffield.Name = keys[1].Trim();
						ffield.FilterType = snpFilterType.Equal;
//						ffield.Value = keys[0].Trim();
						ffield.ParentKey = true;

						dc.SQLB.FilterFields.Add(ffield);
					}
				}

				//putting in the datagrid columns as datafields

                DataControlFieldCollection cols = sdg.Columns;

                foreach (DataControlField col in cols)
				{
                    string dfieldname = string.Empty;

                    try
                    {
                        Type t = col.GetType();
                        dfieldname = (string)t.InvokeMember("DataField", System.Reflection.BindingFlags.GetProperty, null, col, new object[] { });
                    }
                    catch
                    { }

                    if (dfieldname != string.Empty || dfieldname.Trim() != string.Empty)
                    {

                        DataField dfield = new DataField();

                        dfield.Name = dfieldname.Trim();

                        dc.SQLB.DataFields.Add(dfield);
                    }
				}

				//setting the sortfields in the SQLB of the dc
				string[] sortfields = sdg.OrderBy.Split(',');

				foreach (string sortfield in sortfields)
				{
					string sf = sortfield.Trim();
					string[] sfe = sf.Split(' ');

					string sfe0 = sfe[0] == null || sfe[0] == "" ? "" : sfe[0].Trim();
				
					if (sfe0 != "")
					{
						SortField sfield = new SortField();
						sfield.Name = sfe0;

						string sfe1 = sfe[1] == null || sfe[1] == "" ? "" : sfe[1].Trim();

						if (sfe1 != "")
						{
							sfe1 = sfe1.ToUpper();
							if (sfe1 == "ASC") sfield.Order = snpOrder.ASC;
							else sfield.Order = snpOrder.DESC;
						}

						dc.SQLB.SortFields.Add(sfield);
					}
				}
				
					dc.GridPage = sdg.Page.ToString();
					dc.OnPage = true;
					thispage_dgnc.Add(dc);

				//also adding the parentdatagrid
				//this is necessary for 2 reasons :
				//-sorting the grids
				//-also, when there are no controls on the page

				if(dc.ParentControl != null && dc.ParentControl != "")
				{
					AddParentControl(sdg.Page, dc, thispage_dgnc);
				}
			}
		}


		private bool IsDGNinDGNC(string dgn, DataGridNameCollection thispage_dgnc)
		{
			bool found = false;

			foreach(DataGridControl dc in thispage_dgnc)
			{
				if(dc.GridName == dgn) 
				{
					found = true;
					break;
				}
			}

			return found;
		}


		private void AddParentControl(Page Page, DataGridControl dc, DataGridNameCollection thispage_dgnc)
		{
			string pc = dc.ParentControl;

			WebControl wc = (WebControl)Page.FindControl(pc);

			if (wc != null) //control is on page, so we can check the type
			{
				Type t = wc.GetType();

                DBSNAPGridView sdg = null; 

				if (t.ToString().EndsWith("DBSNAPDataGrid"))
				{
                    sdg = (DBSNAPGridView)Page.FindControl(pc);
					AddGridToCollection(sdg, thispage_dgnc);
					dc.ParentGrid = true;
				}
				else //it's a webcontrol, but not a datagrid
				{
					if (t.ToString().EndsWith("DBSNAPTextBox") || t.ToString().EndsWith("DBSNAPDropDownList"))
					{
						pc = (string)t.InvokeMember("DBSNAPGridName", System.Reflection.BindingFlags.GetProperty, null , wc, new object[] {});
					}

					if (pc != string.Empty && pc != null)
					{
                        sdg = (DBSNAPGridView)Page.FindControl(pc);

						if  (sdg != null) //grid on page
						{
                            sdg = (DBSNAPGridView)Page.FindControl(pc);
							AddGridToCollection(sdg, thispage_dgnc);						
						}
						else //grid not on page
						{
							if (!IsDGNinDGNC(pc, thispage_dgnc)) 
							{
								DataGridControl dcp = new DataGridControl();
								dcp.GridName = pc;
								dcp.GridPage = Page.ToString();
								dcp.OnPage = false;
								thispage_dgnc.Add(dcp);
							}
						}
					}
				}
			}
			else
			{
				//looking if it's a grid on another page
				DataGridNameCollection odgnc = (DataGridNameCollection)Page.Session["DGNC"];

				bool found = false;

				foreach(DataGridControl dc2 in odgnc)
				{
					if (dc2.GridName == pc) //it ' s a grid
					{
						found = true;
					}
				}

				if (found)
				{
					if (!IsDGNinDGNC(pc, thispage_dgnc)) 
					{
						DataGridControl dcp = new DataGridControl();
						dcp.GridName = pc;
						dcp.GridPage = Page.ToString();
						dcp.OnPage = false;
						thispage_dgnc.Add(dcp);
						dc.ParentGrid = true;
					}
				}
			}
		}
	}

    public class DataGridControl
    {
        public string GridName { get; set; }
        public string GridPage { get; set; }
        public bool OnPage { get; set; }
        public bool ParentGrid { get; set; }
        public string ParentControl { get; set; }
        public string ActionTarget { get; set; }
        public string ViewSource { get; set; }
        public SQLBuilder SQLB { get; set; }
        public string ParentKeyExpression { get; set; }

        //after refactoring
        public bool HasParentControl
        {
            get { return ParentControl != null && ParentControl != ""; }
        }
    }

    public class DataGridNameCollection : CollectionBase
    {
        public void Add(DataGridControl dgc)
        {

        }
    }

    public class DBSNAPGridView : Control
    {
        public string ActionTarget { get; set; }
        public string ViewSource { get; set; }
        public bool Distinct { get; set; }
        public bool AllowPaging { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string PrimaryKey { get; set; }
        public string ParentControl { get; set; }
        public string ParentKeyExpression { get; set; }
        public DataControlFieldCollection Columns { get; set; }
        public string OrderBy { get; set; }
    }
}