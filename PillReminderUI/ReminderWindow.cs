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
using Microsoft.VisualBasic;


namespace PillReminderUI
{
    public partial class ReminderWindow : Form
    {
        BindingList<PillModel> medications = new BindingList<PillModel>();
        private Timer RefreshTimer = new Timer() { Enabled = true, Interval = 5000 }; 

        public ReminderWindow()
        {
            InitializeComponent();
            allPillsListBox.DataSource = medications;
            allPillsListBox.DisplayMember = "PillInfo";
            RefreshTimer.Tick += RefreshEvery5; 
            PopulateDummyData();
            
        }
        /*
         * TODO
         * 1) add database functionality for persistent pill list
         * 2) add remove pill button fucntionality for if a pill is no longer needed
         * 3) clean up UI
         * 4) add error handling
         * ****** clicking take pill when selecting from all pills list
         * ****** 
        */

        //Added ability to add a pill to the listbox of total pills on button click
        //note: if cancel is clicked after opening inputbox ends program
        //further, input currently MUST be in the form "name of pill, time to take" or else it crashes
        public void AddPillClicked(object source, EventArgs e)
        {
            addPill = (Button)source;
            
            string pillString = Interaction.InputBox("Type the pill you would like to add in the form 'name of pill,time to take' ", "Add Pill", "Pill Name");
            string pillStringName = pillString.Split(',')[0];
            string pillStringTime = pillString.Split(',')[1];
          
            PillModel testPill = new PillModel { PillName = pillStringName, TimeToTake = DateTime.Parse(pillStringTime) };
            medications.Add(testPill);
            
        }
        //Add ability to remove a pill if the pill is no longer needed
        public void RemovePillClicked(object source, EventArgs e)
        {
            removePill = (Button)source;
            //get selected item as a pillModel
            PillModel pillToBeRemoved = (PillReminderUI.PillModel)allPillsListBox.SelectedItem;
            //Use LINQ to find the one instance in medications of the pill to be removed
            PillModel removedPill = medications.Single(med => med.PillInfo == pillToBeRemoved.PillInfo);
            //Remove from medications bindinglist
            medications.Remove(removedPill);
            //placeholder string so we dont modify the collection
            string removeThisPill = "";
            //go through each item in pillstotake to find one instance
            //Must remove from toTake as well to maintain parity with allPills eg- avoid saying take this pill that you no longer need
            //NOTE: cant use LINQ becuase allPills is a listbox of pillmodels and toTake is a listbox of strings
            foreach(string item in pillsToTakeListBox.Items)
            {
                if(item == removedPill.PillInfo)
                {
                    removeThisPill = item;
                }
            }
            //remove from toTake listbox
            pillsToTakeListBox.Items.Remove(removeThisPill);
        }

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
