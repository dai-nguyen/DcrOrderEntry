/** 
 * This file is part of the DcrOrderEntry project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
 * 
 * Delete old temp file if found then re-gen new temp file
**/

using Activant.P21.Extensions.BusinessRule;
using DcrDataAccess;
using DcrDataAccess.Forms;
using DcrDataAccess.Models;
using System;
using System.Windows.Forms;

namespace DcrOrderEntry
{
    public class OEOrderNoValidator : RuleExt
    {
        /// <summary>
        /// Link order_no
        /// </summary>
        /// <returns></returns>
        public override RuleResult Execute()
        {                        
            RuleResult result = new RuleResult { Success = true };

            SessionInfo info = GetSessionInfo();

            try
            {
                string order_no = GetDataFieldValue("order_no");

                if (GetErrorsCount() > 0)
                {
                    new ErrorForm(this.GetType().Name, info, GetErrors()).ShowDialog();
                    return result;
                }

                using (OrderEntryService service = new OrderEntryService(info.Server, info.Db))
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
                new ErrorForm(this.GetType().Name, info, ex.ToString()).ShowDialog();
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
