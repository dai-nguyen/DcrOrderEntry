using DcrDataAccess;
using DcrDataAccess.Forms;
using DcrDataAccess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DcrOrderEntryTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SessionInfo info = new SessionInfo("sql_server", "p21_db", "john.doe");

            ErrorForm f = new ErrorForm("Order No Validator", info , "order_no not found");
            f.ShowDialog();

            Console.ReadKey();
        }
    }
}
