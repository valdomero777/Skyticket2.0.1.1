/*
Printer++ Virtual Printer Processor
Copyright (C) 2012 - Printer++

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
using Microsoft.Win32;
using System;

namespace Skyticket
{
    public static class AutorunHelper
    {
        public static void AddToStartup()
        {
            try
            {
                LogHelper.Log("Adding Skyticket to Startup");
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                        ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                //rk.SetValue("Skyticket", "C:\\Program Files (x86)\\Skyticket\\Skyticket.exe");
                rk.SetValue("Skyticket", "C:\\Program Files\\Skyticket\\Skyticket.exe");
            }
            catch (Exception)
            {
            }
        }

        public static void RemoveFromStartup()
        {
            try
            {
                LogHelper.Log("removing Skyticket from Startup");
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                        ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rk.DeleteValue("Skyticket");
            }
            catch (Exception)
            {
            }
        }

    }
}
