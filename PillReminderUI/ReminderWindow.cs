/*
 * NOTE: Unless otherwise stated, code is given started code from Tim Correy at https://www.iamtimcorey.com/courses/c-weekly-challenges/lectures/8464895
 *
 * David Slusher, 7/13/19
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PillReminderUI
{
    public partial class ReminderWindow : Form
    {
        BindingList<PillModel> medications = new BindingList<PillModel>();
        private Timer RefreshTimer = new Timer() { Enabled = true, Interval = 5000 };  /*my code*/

        public ReminderWindow()
        {
            InitializeComponent();
            allPillsListBox.DataSource = medications;
            allPillsListBox.DisplayMember = "PillInfo";
            RefreshTimer.Tick += RefreshEvery5; /*my code*/
            PopulateDummyData();
            
        }
        //Begin my code

        
        // on click of Refresh, update list of pills to take by time
        public void RefreshClicked(object source, EventArgs e)
        {
            refreshPillsToTake = (Button)source;

            //Using LINQ get all instances where you need to take a pill
            //Take a pill if timeToTake is or before the current time, the lastTaken time is or before the timeToTake, and the pill isnt already in the listbox
            IEnumerable<PillModel> getMeds = medications.Where(med => DateTime.Compare(med.TimeToTake, DateTime.Now) <= 0 && (DateTime.Compare(med.LastTaken, med.TimeToTake) <= 0)
                    && !(pillsToTakeListBox.Items.Contains(med.PillInfo)));
            //go through adding the pills to the listbox
            foreach (PillModel pill in getMeds)
            {
                pillsToTakeListBox.Items.Add(pill.PillInfo);
            }

            
        }

        /*
         * BONUS: Refresh every 5 seconds without having to click refresh button
         */
         public void RefreshEvery5(object sender, EventArgs e)
        {
            //event is raised every five seconds and is the same as if clicked refresh button
            IEnumerable<PillModel> getMeds = medications.Where(med => DateTime.Compare(med.TimeToTake, DateTime.Now) <= 0 && (DateTime.Compare(med.LastTaken, med.TimeToTake) <= 0)
                    && !(pillsToTakeListBox.Items.Contains(med.PillInfo)));
            foreach (PillModel pill in getMeds)
            {
                pillsToTakeListBox.Items.Add(pill.PillInfo);
            }
            
        }

        //on click of take pill, remove from listbox and update time last taken so it doesnt appear in subsequent refreshes
        public void TakeClicked(object source, EventArgs e)
        {
            takePill = (Button)source;

            //get selected item as a string
            string pillString = pillsToTakeListBox.SelectedItem.ToString();
            //Using LINQ find the single instance of the selected pill in the list of medications
            PillModel takenPill = medications.Single(med => med.PillInfo == pillString);
            //update the LastTaken to the current time
            takenPill.LastTaken = DateTime.Now;
            //Remove from pills to take since no longer need to take said pill
            pillsToTakeListBox.Items.Remove(pillString);
            
        }
        //end my code

            private void PopulateDummyData()
        {
            medications.Add(new PillModel { PillName = "The white one", TimeToTake = DateTime.ParseExact("0:15 am", "h:mm tt", null, System.Globalization.DateTimeStyles.AssumeLocal) });
            medications.Add(new PillModel { PillName = "The big one", TimeToTake = DateTime.Parse("8:00 am") });
            medications.Add(new PillModel { PillName = "The red ones", TimeToTake = DateTime.Parse("11:45 pm") });
            medications.Add(new PillModel { PillName = "The oval one", TimeToTake = DateTime.Parse("0:27 am") });
            medications.Add(new PillModel { PillName = "The round ones", TimeToTake = DateTime.Parse("6:15 pm") });
        }

    }
}
