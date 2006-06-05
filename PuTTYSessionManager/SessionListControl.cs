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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using uk.org.riseley.puttySessionManager.model;
using System.Collections;

namespace uk.org.riseley.puttySessionManager
{
    public partial class SessionListControl : SessionControl, uk.org.riseley.puttySessionManager.ISessionControl
    {
        
        public SessionListControl()
        {
            InitializeComponent();
            LoadSessions();
        }


        protected override void LoadSessions()
        {

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(Session.PUTTY_SESSIONS_REG_KEY);

            // Suppress repainting the ListBox until all the objects have been created.
            listBox1.BeginUpdate();

            // Clear out the current tree
            listBox1.Items.Clear();

            foreach (string keyName in rk.GetSubKeyNames())
            {
                RegistryKey sessKey = rk.OpenSubKey(keyName);

                String pempath = (String)sessKey.GetValue(Session.PUTTY_PSM_FOLDER_VALUE);

                Session s = new Session(keyName, pempath, false);

                listBox1.Items.Add(s);

                sessKey.Close();
            }
            rk.Close();

            // Sort the list
            listBox1.Sorted = true;

            // Select the first session
            if ( listBox1.Items.Count > 0 )
                listBox1.SelectedIndex = 0;

            // Begin repainting the TreeView.
            listBox1.EndUpdate();

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string sessionName = null;

            if ( sender is ListBox ) {
                Session s = (Session)((ListBox)sender).SelectedItem;
                sessionName = s.SessionDisplayText;
            }
            else if (sender is ToolStripMenuItem)
            {
                sessionName = ((ToolStripMenuItem)sender).Text;
            }

            if ( sessionName != null)
                OnLaunchSession(new SessionEventArgs(sessionName));
            else
                OnLaunchSession(new SessionEventArgs());
        }

        public override void getSessionMenuItems(ToolStripMenuItem parent)
        {
            parent.DropDownItems.Clear();
            
            IEnumerator ie = listBox1.Items.GetEnumerator();

            while (ie.MoveNext()) 
            {
                parent.DropDownItems.Add(new ToolStripMenuItem(((Session)ie.Current).SessionDisplayText, null, listBox1_DoubleClick));
            }
        }
    }
}