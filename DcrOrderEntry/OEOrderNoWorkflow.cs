/** 
 * This file is part of the DcrOrderEntry project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
 * 
 * Delete old temp file
**/

using Activant.P21.Extensions.BusinessRule;
using DcrDataAccess;
using DcrDataAccess.Forms;
using DcrDataAccess.Models;
using System;

namespace DcrOrderEntry
{
    public class OEOrderNoWorkflow : RuleExt
    {
        /// <summary>
        /// Link order_no
        /// </summary>
        public override void ExecuteAsync()
        {
            SessionInfo info = GetSessionInfo();

            try
            {
                string order_no = GetDataFieldValue("order_no");

                if (GetErrorsCount() > 0)
                {
                    new ErrorForm(this.GetType().Name, info, GetErrors()).ShowDialog();
                    return;
                }

                using (OrderEntryService service = new OrderEntryService(info.Server, info.Db))
                {
                    service.DeleteFile(order_no);
                }
            }
            catch (Exception ex)
            {
                new ErrorForm(this.GetType().Name, info, ex.ToString()).ShowDialog();
            }
        }

        public override string GetDescription()
        {
            return this.GetType().Name;
        }

        public override string GetName()
        {
            return this.GetType().Name;
        }

        public override RuleResult Execute()
        {
            return base.Execute();
        }
    }
}
