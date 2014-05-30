/** 
 * This file is part of the DcrOrderEntry project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
 * 
 * Load data from temp file then update/delete item accordingly
 * then save back to temp file
**/

using Activant.P21.Extensions.BusinessRule;
using DcrDataAccess;
using DcrDataAccess.Models;
using DcrDataAccess.Models.OrderEntry;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DcrOrderEntry
{
    public class UnitQtyValidator : RuleExt
    {
        /// <summary>
        /// Link    unit_quantity
        ///         order_no
        ///         oe_line_uid
        ///         inv_mast_uid
        ///         unit_price
        ///         disposition
        /// </summary>
        /// <returns></returns>
        public override RuleResult Execute()
        {            
            RuleResult result = new RuleResult { Success = true };

            try
            {
                string unit_quantity = GetDataFieldValue("unit_quantity");
                string order_no = GetDataFieldValue("order_no");                
                string oe_line_uid = GetDataFieldValue("oe_line_uid");
                string inv_mast_uid = GetDataFieldValue("inv_mast_uid");
                string unit_price = GetDataFieldValue("unit_price");
                string disposition = GetDataFieldValue("disposition");

                if (GetErrorsCount() > 0)
                {
                    MessageBox.Show(GetErrors(), "Error");
                    return result;
                }

                SessionInfo info = GetSessionInfo();

                OeLineItem item = new OeLineItem(oe_line_uid,
                    inv_mast_uid,
                    unit_quantity,
                    unit_price,
                    disposition);
                                
                using (OrderEntryService service = new OrderEntryService(info.Server, info.Db))
                {
                    var items = service.LoadLinesFile(order_no);

                    var found = items.FirstOrDefault(t => t.oe_line_uid == item.oe_line_uid);

                    if (found != null)
                    {
                        if (item.unit_quantity <= 0 || item.disposition == "C")
                            items.Remove(found);    // Canceled Item
                        else
                            found.Copy(item);   // Update Item
                    }
                    else if (item.unit_quantity > 0 && item.disposition != "C")
                        items.Add(item);    // Add new Item
                    
                    service.SaveLinesFile(order_no, items);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

            return result;
        }

        public override string GetDescription()
        {
            return this.GetType().Name;
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }
    }
}
