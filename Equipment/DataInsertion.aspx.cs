using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Equipment
{
    public partial class DataInsertion : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            // establish database connection
            string connectionString = "Data Source=DESKTOP-JJNQLSP;Initial Catalog=equipment;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // create SQL query
                string query = "INSERT INTO Book1 (FACTORYNAME, EquipementName, StatusInfo, INTERVAL_START, INTERVAL_END, Downtime_Duration, Standby, Engineering, SchDown, UnschDown, Day) " +
                               "VALUES (@FactoryName, @EquipmentName, @StatusInfo, @IntervalStart, @IntervalEnd, @DowntimeDuration, @Standby, @Engineer, @ScheduledDowntime, @UnScheduled, @Date)";
                SqlCommand command = new SqlCommand(query, connection);
                TimeSpan intervalStart = TimeSpan.Parse(txtintervalstart.Text);
                TimeSpan intervalEnd = TimeSpan.Parse(txtintervalend.Text);

                TimeSpan downtimeDuration = intervalEnd - intervalStart;

                double downtimeMinutes = downtimeDuration.TotalMinutes;
                // add parameters to the query
                command.Parameters.AddWithValue("@FactoryName", selectfactory.SelectedValue);
                command.Parameters.AddWithValue("@EquipmentName", txtequipment.Text);
                command.Parameters.AddWithValue("@StatusInfo", selectstatus.SelectedValue);
                command.Parameters.AddWithValue("@IntervalStart", txtintervalstart.Text);
                command.Parameters.AddWithValue("@IntervalEnd", txtintervalend.Text);
                command.Parameters.AddWithValue("@DowntimeDuration", downtimeMinutes);

                if (rbtnUnscheduled.Checked)
                {
                    command.Parameters.AddWithValue("@ScheduledDowntime", 0);
                    command.Parameters.AddWithValue("@UnScheduled", downtimeMinutes);
                    command.Parameters.AddWithValue("@Standby", 0);
                    command.Parameters.AddWithValue("@Engineer", 0);
                }
                else if (rbtnScheduled.Checked)
                {
                    command.Parameters.AddWithValue("@UnScheduled", 0);
                    command.Parameters.AddWithValue("@ScheduledDowntime", downtimeMinutes);
                    command.Parameters.AddWithValue("@Standby", 0);
                    command.Parameters.AddWithValue("@Engineer", 0);
                }
                else if (rbtnstandby.Checked)
                {
                    command.Parameters.AddWithValue("@UnScheduled", 0);
                    command.Parameters.AddWithValue("@ScheduledDowntime", 0);
                    command.Parameters.AddWithValue("@Standby", downtimeMinutes);
                    command.Parameters.AddWithValue("@Engineer", 0);
                }
                else
                {
                    command.Parameters.AddWithValue("@UnScheduled", 0);
                    command.Parameters.AddWithValue("@ScheduledDowntime", 0);
                    command.Parameters.AddWithValue("@Standby", 0);
                    command.Parameters.AddWithValue("@Engineer", downtimeMinutes);

                }
                // command.Parameters.AddWithValue("@ScheduledDowntime", txtscheduled.Text);
                //command.Parameters.AddWithValue("@UnScheduled", txtunscheduled.Text);
                command.Parameters.AddWithValue("@Date", txtdate.Text);

                // execute the query
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                // display success message or redirect to another page
                if (rowsAffected > 0)
                {
                    lblsuccess.Visible = true;
                }
                else
                {
                    lblerror.Visible = true;
                }
            }
        }
        protected void ResetButton_Click(object sender, EventArgs e)
        {
            // Clear all form fields
            txtequipment.Text = "";
            txtintervalstart.Text = "";
            txtintervalend.Text = "";
            txtdate.Text = "";
            selectfactory.SelectedIndex = 0;
            selectstatus.SelectedIndex = 0;
            rbtnUnscheduled.Checked = true;
            rbtnScheduled.Checked = false;
            rbtnstandby.Checked = false;
            rbtnengineer.Checked = false;
            valequipmentname.Visible = false;
            valintervalstart.Visible = false;
            valintervalend.Visible = false;
            valdate.Visible = false;
        }
    }
}