/** 
 * This file is part of the DcrOrderEntry project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
 * 
 * Run UnitQtyValidator Rule
**/

using Activant.P21.Extensions.BusinessRule;
using DcrDataAccess;
using DcrDataAccess.Forms;
using DcrDataAccess.Models;
using DcrDataAccess.Models.OrderEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DcrOrderEntry
{
    public class DispositionValidator : RuleExt
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

            SessionInfo info = GetSessionInfo();

            try
            {
                OEUnitQtyValidator qtyVal = new OEUnitQtyValidator();
                qtyVal.Data = this.Data;
                qtyVal.Execute();
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
