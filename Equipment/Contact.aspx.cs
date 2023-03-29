using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Globalization;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.DynamicData;

namespace Equipment
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
      
        }
        // Export data
        protected void ExportButton_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=OverallDowntimeData.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridData.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }


        //Display data based on specific criteria after clicking run button
        protected void btnRun_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=DESKTOP-JJNQLSP;Initial Catalog=equipment;Integrated Security=True";
            string query = "";
            //If duration selected value is select then display empty grid and warining message
            if (SelectDateList.SelectedValue == "0")
            {
                GridData.Visible = false; //Hiding the grid data
                lblwarning.Visible = true; // Displaying the warining Message
                return;
            }
            else
            {
                GridData.Visible = true; //Displaying the grid data
                lblwarning.Visible = false; //Hiding the warining Message
            }

            //  select Time period
            switch (SelectDateList.SelectedValue)
            {
                case "date": //Date
                    query = "EquipmentDowntimeByDate"; //Stored procedure for summarize the data by date
                    downtimechart.ChartAreas[0].AxisX.Title = "Date and Equipment Name"; //X Axis Title
                    downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours"; //Y Axis Title
                    break;
                case "month": // Month
                    query = "EquipmentDowntimeByMont";  //Stored procedure for summarize the data by Month
                    downtimechart.ChartAreas[0].AxisX.Title = "Month and Equipment Name"; //X Axis Title
                    downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours"; //Y Axis Title
                    break;
                case "year": // Year
                    query = "EquipmentDowntimeByYear";  //Stored procedure for summarize the data by Year
                    downtimechart.ChartAreas[0].AxisX.Title = "Year and Equipment Name"; //X Axis Title
                    downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours"; //Y Axis Title
                    break;
                default:
                    break;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                //parameter for specific equipment
                if (!string.IsNullOrEmpty(EquipmentList.SelectedValue))
                {
                    command.Parameters.AddWithValue("@EquipmentName", EquipmentList.SelectedValue);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);

                // Display data in grid
                GridData.DataSource = dataset.Tables[0];
                GridData.DataBind();

                // Bind data to chart
                downtimechart.DataSource = dataset.Tables[0];
                //chart title based on selected Equipment
                if (!string.IsNullOrEmpty(EquipmentList.SelectedValue))
                {
                    downtimechart.Titles.Add("Downtime Report for " + EquipmentList.SelectedItem.Text);
                }
                else
                {
                    downtimechart.Titles.Add("Downtime Report for All Equipments");
                }
                //Chart Selection
                switch (ChartTypeDropDownList.SelectedValue)
                {

                    case "Pie": // Pie Chart
                        downtimechart.Series["Downtime"].ChartType = SeriesChartType.Pie; //Chart Type
                        downtimechart.Series["Downtime"].Font = new Font("Verdana", 8, FontStyle.Bold); // Label Font style
                        downtimechart.Series["Downtime"].LabelForeColor = Color.Black; // Label Color
                        downtimechart.Series["Downtime"]["PieLabelStyle"] = "Outside";

                        // Set the tooltip text for each data point
                        foreach (DataPoint point in downtimechart.Series["Downtime"].Points)
                        {
                            point.ToolTip = "TotalDowntime: " + point.YValues[0].ToString();
                        }
                        // Chart Value Members Based on the dropdown selection
                        if (SelectDateList.SelectedValue == "date") // By date
                        {
                            downtimechart.Series[0].XValueMember = "Date"; //X Axis Value
                            downtimechart.Series[0].YValueMembers = "TotalDowntime"; //Y Axis Value
                            downtimechart.Series[0].LegendText = "#AXISLABEL - #VALY{0}";//Legend
                            downtimechart.Series["Downtime"].Label = "#VALY{0}"; // Label

                        }
                        else if (SelectDateList.SelectedValue == "month")// By Month
                        {
                            downtimechart.Series[0].XValueMember = "Month"; //X Axis Value
                            downtimechart.Series[0].YValueMembers = "TotalDowntime"; //Y Axis Value
                            downtimechart.Series[0].LegendText = "#AXISLABEL - #VALY{0}"; //Legend
                            downtimechart.Series["Downtime"].Label = "#VALY{0}"; // Label
                        }
                        else if (SelectDateList.SelectedValue == "year")// By Year
                        {
                            downtimechart.Series[0].XValueMember = "Year"; //X Axis Value
                            downtimechart.Series[0].YValueMembers = "TotalDowntime"; //Y Axis Value
                            downtimechart.Series[0].LegendText = "#VALX{0} - #VALY{0}"; //Legend
                            downtimechart.Series["Downtime"].Label = "#VALY{0}"; // Label
                        }
                        break;

                    case "Column": // Stacked Column Chart
                        downtimechart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                        downtimechart.ChartAreas[0].AxisX.Interval = 1;
                        downtimechart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                        // Clear any existing series from the chart
                        downtimechart.Series.Clear();

                        // Create a new series for each unique status info
                        var statusInfos = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("StatusInfo")).Distinct();
                        foreach (var statusInfo in statusInfos)
                        {
                            // Creating new series for status info 
                            Series series = new Series();
                            series.Name = statusInfo;
                            series.ChartType = SeriesChartType.StackedColumn;  // Chart Type
                            if (statusInfo == "Up")
                            {
                                series.Color = Color.SeaGreen;
                            }
                            else
                            {
                                series.Color = Color.OrangeRed;
                            }

                            // Getting all unique equipment name and date
                            var equipmentNames = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("EquipementName")).Distinct();
                            var dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("Date")).Distinct();
                            // Assiging the values for dates variable based on time period selection
                            if (SelectDateList.SelectedValue == "date") //Date
                            {
                                dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("Date")).Distinct();
                            }
                            else if (SelectDateList.SelectedValue == "month") //Month
                            {
                                dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("Month")).Distinct();
                            }
                            else // Year
                            {
                                dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<int>("Year").ToString()).Distinct();

                            }
                            // Getting all unique equipment name and date
                            foreach (var equipmentName in equipmentNames)
                            {
                                foreach (var date in dates)
                                {
                                    var rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && row.Field<string>("Date") == date && row.Field<string>("StatusInfo") == statusInfo);
                                    // Assigning values to rows variable based on time period selection
                                    if (SelectDateList.SelectedValue == "date") //Date
                                    {
                                         rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && row.Field<string>("Date") == date && row.Field<string>("StatusInfo") == statusInfo);
                                    }
                                    else if (SelectDateList.SelectedValue == "month") //Month
                                    {
                                        rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && row.Field<string>("Month") == date && row.Field<string>("StatusInfo") == statusInfo);
                                    }
                                    else // Year
                                    {
                                        rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && Convert.ToInt32(row["Year"]) == Convert.ToInt32(date) && row.Field<string>("StatusInfo") == statusInfo);

                                    }
                                    // If there is no data, create a new data point with a downtime duration of zero
                                    if (rows.Count() == 0)
                                    { 
                                        DataPoint dataPoint = new DataPoint();
                                        dataPoint.YValues = new double[] { 0 };
                                        dataPoint.AxisLabel = date + " - " + equipmentName;
                                        series.Points.Add(dataPoint);
                                    }
                                    else
                                    // If there is data, add them as data points to the series
                                    {
                                        foreach (DataRow row in rows)
                                        {
                                            int downtimeDurationInMinutes = Convert.ToInt32(row["TotalDowntime"]);
                                            // Converting downtime from minutes to hours
                                            double downtimeDurationInHours = (double)downtimeDurationInMinutes / 60.0;
                                            string downtimeDurationInHoursStr = downtimeDurationInHours.ToString("0.00", CultureInfo.InvariantCulture);

                                            DataPoint dataPoint = new DataPoint();
                                            dataPoint.YValues = new double[] { downtimeDurationInHours };
                                            //X Axis Label based on time period selection
                                             if (SelectDateList.SelectedValue == "date") //Date
                                            {
                                                dataPoint.AxisLabel = row["Date"] + "-" + row["EquipementName"];//X Axis Label
                                            }
                                            else if (SelectDateList.SelectedValue == "month")//Month
                                            {
                                                dataPoint.AxisLabel = row["Month"] + "-" + row["EquipementName"];//X Axis Label
                                            }
                                            else //Year
                                            {
                                                dataPoint.AxisLabel = row["Year"].ToString() + "-" + row["EquipementName"];//X Axis Label

                                            }
                                            dataPoint.ToolTip = "Status: " + statusInfo + "  " + "Downtime: " + downtimeDurationInMinutes + " minutes";
                                            dataPoint.Label = downtimeDurationInHoursStr; //Label
                                            series.Points.Add(dataPoint);

                                        }
                                    }
                                }
                            }
                            // Add the series to the chart
                            downtimechart.Series.Add(series);
                        }

                        break;

                    
                    case "Line": // Line Chart
                        downtimechart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                        downtimechart.ChartAreas[0].AxisX.Interval = 1;
                        downtimechart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                        // CLear existing series
                        downtimechart.Series.Clear();
                        // Create a new series for each unique status info
                        var statusInfos1 = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("StatusInfo")).Distinct();
                        foreach (var statusInfo in statusInfos1)
                        {
                            // Creating new series for status info 
                            Series series = new Series();
                            series.Name = statusInfo;
                            series.ChartType = SeriesChartType.Line; // Chart Type
                            // Set the color of the series based on  StatusInfo 
                            if (statusInfo == "Up")
                            {
                                series.Color = Color.SeaGreen;
                            }
                            else
                            {
                                series.Color = Color.OrangeRed;
                            }
                            // Get all the rows with the current StatusInfo
                            var rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("StatusInfo") == statusInfo);
                            foreach (DataRow row in rows)
                            {
                                // Get the values from the row
                                int downtimeDurationInMinutes = Convert.ToInt32(row["TotalDowntime"]);
                                // Converting downtime from minutes to hours
                                double downtimeDurationInHours = (double)downtimeDurationInMinutes / 60.0;
                                string downtimeDurationInHoursStr = downtimeDurationInHours.ToString("0.00");
                                DataPoint dataPoint = new DataPoint();
                                dataPoint.YValues = new double[] { downtimeDurationInHours }; // Y Axis datapoint
                                //X Axis Label based on time period selection
                                if (SelectDateList.SelectedValue == "date") //Date
                                {
                                    dataPoint.AxisLabel = row["Date"] + "-" + row["EquipementName"]; //X Axis Label
                                }
                                else if (SelectDateList.SelectedValue == "month") // Month
                                {
                                    dataPoint.AxisLabel = row["Month"] + "-" + row["EquipementName"]; //X Axis Label
                                }
                                else// Year
                                {
                                    dataPoint.AxisLabel = row["Year"] + "-" + row["EquipementName"]; //X Axis Label
                                }
                               
                                dataPoint.ToolTip = "Status: " + statusInfo + "  " + "Downtime: " + downtimeDurationInMinutes + " minutes";
                                dataPoint.Label = downtimeDurationInHoursStr; //Label
                                series.Points.Add(dataPoint);
                            }
                            // Add the series to the chart
                            downtimechart.Series.Add(series);
                        }
                        break;

                }




                connection.Close();

            }
        }
    }
}


        