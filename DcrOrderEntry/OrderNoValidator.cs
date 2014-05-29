/** 
 * This file is part of the DcrOrderEntry project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
 * 
 * Delete old temp file if found then re-gen new temp file
**/

using Activant.P21.Extensions.BusinessRule;
using DcrDataAccess;
using System;
using System.Windows.Forms;

namespace DcrOrderEntry
{
    public class OrderNoValidator : RuleExt
    {
        /// <summary>
        /// Link order_no
        /// </summary>
        /// <returns></returns>
        public override RuleResult Execute()
        {
            GetBasicInfo();
            
            RuleResult result = new RuleResult { Success = true };

            try
            {
                string order_no = GetDataFieldValue("order_no");

                if (GetErrorCount() > 0)
                {
                    MessageBox.Show(GetErrors(), "Error");
                    return result;
                }

                using (OrderEntryService service = new OrderEntryService(Server, Db))
                {
                    var items = service.GetOeLineItems(order_no);

                    if (items.Count > 0)
                        service.SaveLinesFile(order_no, items);
                    else
                        service.DeleteFile(order_no);
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
