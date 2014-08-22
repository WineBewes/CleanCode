using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace Examples
{
	/// <summary>
    /// Summary description for CleanCode_02_Comments.
	/// </summary>
    internal class CleanCode_02_Comments
	{
        /// <summary>
        /// Adds a DBSNAPGridView to a DatGridNameCollection
        /// </summary>
        /// <param name="sdg">DBSNAPGridView</param>
        /// <param name="thispage_dgnc">DataGridNameCollection of the Webpage</param>
        public void AddGridToCollection(DBSNAPGridView sdg, DataGridNameCollection thispage_dgnc)
		{
			if (IsNotGridViewAlreadyInGridNameCollection(sdg.ID, thispage_dgnc))
			{
			    DataGridControl dc = CreateNewDataGridControl(sdg);

				thispage_dgnc.Add(dc);

				if(dc.HasParentControl)
				{
					AddParentControl(sdg.Page, dc, thispage_dgnc);
                    //this is necessary for 2 reasons :
                    //-sorting the grids
                    //-also, when there are no controls on the page
                }
			}
		}

		private bool IsNotGridViewAlreadyInGridNameCollection(string dgn, DataGridNameCollection thispage_dgnc)
		{
			bool found = false;

			foreach (DataGridControl dc in thispage_dgnc)
			{
				if (dc.GridName == dgn)
				{
					found = true;
					break;
				}
			}

			return found;
		}

        //TODO : in een nieuwe klasse stoppen
        private DataGridControl CreateNewDataGridControl(DBSNAPGridView sdg)
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

			SetKeyFields(sdg, dc);

			SetParentKeyFieldsAsFilters(sdg, dc);

            SetDataFields(sdg, dc);

            SetSortFields(sdg, dc);

	        dc.GridPage = sdg.Page.ToString();
            dc.OnPage = true;

            return dc;
        }

		private void SetKeyFields(DBSNAPGridView sdg, DataGridControl dc)
		{
			string[] keyfields = sdg.PrimaryKey.Split(',');
			foreach (string keyfield in keyfields)
			{
				string kf = keyfield.Trim();
				DataField dfield = new DataField();
				dfield.Name = kf;
				dfield.PKField = true;
				dc.SQLB.DataFields.Add(dfield);
			}
		}

		private void SetParentKeyFieldsAsFilters(DBSNAPGridView sdg, DataGridControl dc)
		{
			dc.ParentControl = sdg.ParentControl;
			dc.ParentKeyExpression = sdg.ParentKeyExpression;

			string[] parentkeys = GetParentKeys(sdg, dc);

			foreach (string parentkey in parentkeys)
			{
				string[] keys = parentkey.Split('=');

				FilterField ffield = new FilterField();
				ffield.Name = keys[1].Trim();
				ffield.FilterType = snpFilterType.Equal;

				//SMELL : zwerfvuil
//						ffield.Value = keys[0].Trim(); 

				ffield.ParentKey = true;

				dc.SQLB.FilterFields.Add(ffield);
			}
		}

		private string[] GetParentKeys(DBSNAPGridView sdg, DataGridControl dc)
		{
			string[] parentkeys = new string[0];
			
			if (dc.ParentKeyExpression != null && dc.ParentKeyExpression != "")
			{
				parentkeys = sdg.ParentKeyExpression.Split(',');	
			}

			return parentkeys;
		}

		private void SetDataFields(DBSNAPGridView sdg, DataGridControl dc)
		{
			DataControlFieldCollection cols = sdg.Columns;

			foreach (DataControlField col in cols)
			{
				string dfieldname = string.Empty;

				try
				{
					Type t = col.GetType();
					dfieldname =
						(string)t.InvokeMember("DataField", System.Reflection.BindingFlags.GetProperty, null, col, new object[] { });
				}
				catch
				{
				}

				if (dfieldname != string.Empty || dfieldname.Trim() != string.Empty)
				{
					DataField dfield = new DataField();

					dfield.Name = dfieldname.Trim();

					dc.SQLB.DataFields.Add(dfield);
				}
			}
		}

		private void SetSortFields(DBSNAPGridView sdg, DataGridControl dc)
		{
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
							if (!IsNotGridViewAlreadyInGridNameCollection(pc, thispage_dgnc)) 
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
					if (!IsNotGridViewAlreadyInGridNameCollection(pc, thispage_dgnc)) 
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
}