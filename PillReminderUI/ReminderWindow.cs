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
            //iterate through list of medications
            //for seems to be faster than foreach from http://codebetter.com/patricksmacchia/2008/11/19/an-easy-and-efficient-way-to-improve-net-code-performances/
            for (int i = 0; i < medications.Count; i++)
            {
            
                //compare the timeToTake to the current time 
                //compare the timeToTake to LastTaken
                //make sure no duplicates
                //IFF the timeToTake is or earlier than the current time AND the pill's LastTaken is before or equal the timeToTake, add pill to pillsToTake
                if (DateTime.Compare(medications[i].TimeToTake, DateTime.Now) <= 0 && (DateTime.Compare(medications[i].LastTaken, medications[i].TimeToTake) <= 0)
                    && !(pillsToTakeListBox.Items.Contains(medications[i].PillInfo)))
                {
                    //display medication at i position in pillsToTakeListBox
                    pillsToTakeListBox.Items.Add(medications[i].PillInfo);
                }
                
            }
        }

        /*
         * BONUS: Refresh every 5 seconds without having to click refresh button
         */
         public void RefreshEvery5(object sender, EventArgs e)
        {
            
            for (int i = 0; i < medications.Count; i++)
            {
                if (DateTime.Compare(medications[i].TimeToTake, DateTime.Now) <= 0 && (DateTime.Compare(medications[i].LastTaken, medications[i].TimeToTake) <= 0)
                    && !(pillsToTakeListBox.Items.Contains(medications[i].PillInfo)))
                {
                    //display medication at i position in pillsToTakeListBox
                    pillsToTakeListBox.Items.Add(medications[i].PillInfo);
                }

            }
        }

        //on click of take pill, remove from listbox and update time last taken so it doesnt appear in subsequent refreshes
        public void TakeClicked(object source, EventArgs e)
        {
            takePill = (Button)source;

            //get selected item as a string
            string takenPill = pillsToTakeListBox.SelectedItem.ToString();
            //iterate trhough medication list until finding the entry where pillInfo matches the selected pill
            //NOTE: this should have only ever have one match since even if you took the same pill twice in one day
            //(eg - one in morning one at night) the pillinfo will still be unique based on time
            for(int j = 0; j < medications.Count; j++)
            {
              if (medications[j].PillInfo == takenPill)
                {
                    //set the LastTaken time to the time when TakePill is clicked
                    medications[j].LastTaken = DateTime.Now;  
                }
            }
            //after updating the LastTaken of the pill, remove it from ListBox
            pillsToTakeListBox.Items.Remove(takenPill);
            
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
