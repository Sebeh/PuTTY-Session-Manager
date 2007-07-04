/* 
 * Copyright (C) 2006 David Riseley 
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using uk.org.riseley.puttySessionManager.form;

namespace uk.org.riseley.puttySessionManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Upgrade settings from a previous release
            if (Properties.Settings.Default.UpgradeRequired == true)
            {                
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            PsmApplicationContext appContext = new PsmApplicationContext();

            Application.Run(appContext);           
        }
    }

    class PsmApplicationContext : ApplicationContext
    {
        private SessionManagerForm smf;

        public PsmApplicationContext()
        {
            // Instantiate the SessionManagerForm           
            smf = new SessionManagerForm();

            // Register it as the main form
            this.MainForm = smf;

            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);

            if (Properties.Settings.Default.MinimizeOnStart == false)
            {
                smf.Show();
            }
            
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }

}