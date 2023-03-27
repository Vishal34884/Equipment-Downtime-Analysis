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
            //if (!IsPostBack)
            //{

            //    BindData();
            //}
        }

        //private void BindData()
        //{

        //    string connectionString = "Data Source=DESKTOP-JJNQLSP;Initial Catalog=equipment;Integrated Security=True";
        //    string query = "GetAllData";
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter adapter = new SqlDataAdapter(command);
        //    DataSet dataset = new DataSet();
        //    adapter.Fill(dataset);
        //    //parameter for specific equipment
        //    if (!string.IsNullOrEmpty(EquipmentList.SelectedValue))
        //    {
        //        command.Parameters.AddWithValue("@EquipmentName", EquipmentList.SelectedValue);
        //    }
        //    GridData.DataSource = dataset.Tables[0];
        //    GridData.DataBind();

        //    // Clear any existing series from the chart
        //    downtimechart.Series.Clear();

        //    // Create a new series for each unique status info value in the dataset
        //    var statusInfos = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("StatusInfo")).Distinct();
        //    foreach (var statusInfo in statusInfos)
        //    {
        //        // Create a new series for this status info and set its properties
        //        Series series = new Series();
        //        series.Name = statusInfo;
        //        series.ChartType = SeriesChartType.StackedColumn;
        //        if (statusInfo == "Up")
        //        {
        //            series.Color = Color.SeaGreen;
        //        }
        //        else
        //        {
        //            series.Color = Color.OrangeRed;
        //        }

        //        // Get all the unique equipment names and dates in the dataset
        //        var equipmentNames = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("EquipementName")).Distinct();
        //        var dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("Day")).Distinct();

        //        // Loop through all the possible combinations of equipment names and dates
        //        foreach (var equipmentName in equipmentNames)
        //        {
        //            foreach (var date in dates)
        //            {
        //                // Check if there is data for this combination in the database
        //                var rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && row.Field<string>("Day") == date && row.Field<string>("StatusInfo") == statusInfo);
        //                if (rows.Count() == 0)
        //                {
        //                    // If there is no data, create a new data point with a downtime duration of zero
        //                    DataPoint dataPoint = new DataPoint();
        //                    dataPoint.YValues = new double[] { 0 };
        //                    dataPoint.AxisLabel = date + "-" + equipmentName;
        //                    series.Points.Add(dataPoint);
        //                }
        //                else
        //                {
        //                    // If there is data, loop through the rows and add them as data points to the series
        //                    foreach (DataRow row in rows)
        //                    {
        //                        int downtimeDurationInMinutes = Convert.ToInt32(row["Downtime_Duration"]);
        //                        double downtimeDurationInHours = (double)downtimeDurationInMinutes / 60.0;
        //                        string downtimeDurationInHoursStr = downtimeDurationInHours.ToString("0.00");
        //                        DataPoint dataPoint = new DataPoint();
        //                        dataPoint.YValues = new double[] { downtimeDurationInHours };
        //                        dataPoint.AxisLabel = date + "-" + equipmentName;
        //                        dataPoint.ToolTip = "Status: " + statusInfo + "  " + "Downtime: " + downtimeDurationInMinutes + " minutes";
        //                        dataPoint.Label = downtimeDurationInHoursStr;
        //                        series.Points.Add(dataPoint);
        //                    }
        //                }
        //            }
        //        }
        //        // Add the series to the chart
        //        downtimechart.Series.Add(series);
        //    }

        //    downtimechart.ChartAreas[0].AxisX.Title = "Date and Equipment Name";
        //    downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours";
        //    downtimechart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
        //    downtimechart.Series[0].Label = "#VALY{0}";
        //    downtimechart.ChartAreas[0].AxisX.Interval = 1;
        //    downtimechart.DataBind();
        //}
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
            if (SelectDateList.SelectedValue == "0")
            {
                //BindData();
                GridData.Visible = false;
                lblwarning.Visible = true;
                return;
            }
            else
            {
                GridData.Visible = true;
                lblwarning.Visible = false;
            }


            switch (SelectDateList.SelectedValue)
            {
                case "date":
                    query = "EquipmentDowntimeByDate";
                    downtimechart.ChartAreas[0].AxisX.Title = "Date and Equipment Name";
                    downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours";
                    break;
                case "month":
                    query = "EquipmentDowntimeByMont";
                    downtimechart.ChartAreas[0].AxisX.Title = "Month and Equipment Name";
                    downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours";
                    break;
                case "year":
                    query = "EquipmentDowntimeByYear";
                    downtimechart.ChartAreas[0].AxisX.Title = "Year and Equipment Name";
                    downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours";
                    break;
                default:
                    break;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;

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
                if (!string.IsNullOrEmpty(EquipmentList.SelectedValue))
                {
                    downtimechart.Titles.Add("Downtime Report for " + EquipmentList.SelectedItem.Text);
                }
                else
                {
                    downtimechart.Titles.Add("Downtime Report for All Equipments");
                }
                switch (ChartTypeDropDownList.SelectedValue)
                {

                    case "Pie":
                        downtimechart.Series["Downtime"].ChartType = SeriesChartType.Pie;
                        downtimechart.Series["Downtime"].Font = new Font("Verdana", 8, FontStyle.Bold);
                        downtimechart.Series["Downtime"].LabelForeColor = Color.Black;
                        downtimechart.Series["Downtime"]["PieLabelStyle"] = "Outside";

                        // Set the tooltip text for each data point
                        foreach (DataPoint point in downtimechart.Series["Downtime"].Points)
                        {
                            point.ToolTip = "TotalDowntime: " + point.YValues[0].ToString();
                        }

                        if (SelectDateList.SelectedValue == "date")
                        {
                            downtimechart.Series[0].XValueMember = "Date";
                            downtimechart.Series[0].YValueMembers = "TotalDowntime";
                            downtimechart.Series[0].LegendText = "#AXISLABEL - #VALY{0}";
                            downtimechart.Series["Downtime"].Label = "#VALY{0}";

                        }
                        else if (SelectDateList.SelectedValue == "month")
                        {
                            downtimechart.Series[0].XValueMember = "Month";
                            downtimechart.Series[0].YValueMembers = "TotalDowntime";
                            downtimechart.Series[0].LegendText = "#AXISLABEL - #VALY{0}";
                            downtimechart.Series["Downtime"].Label = "#VALY{0}";
                        }
                        else if (SelectDateList.SelectedValue == "year")
                        {
                            downtimechart.Series[0].XValueMember = "Year";
                            downtimechart.Series[0].YValueMembers = "TotalDowntime";
                            downtimechart.Series[0].LegendText = "#VALX{0} - #VALY{0}";
                            downtimechart.Series["Downtime"].Label = "#VALY{0}";
                        }
                        break;

                    case "Column":
                        downtimechart.Series[0].ChartType = SeriesChartType.Column;
                        downtimechart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                        downtimechart.ChartAreas[0].AxisX.Interval = 1;
                        downtimechart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                        // Clear any existing series from the chart
                        downtimechart.Series.Clear();

                        // Create a new series for each unique status info value in the dataset
                        var statusInfos = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("StatusInfo")).Distinct();
                        foreach (var statusInfo in statusInfos)
                        {
                            // Creating new series for status info 
                            Series series = new Series();
                            series.Name = statusInfo;
                            series.ChartType = SeriesChartType.StackedColumn;
                            if (statusInfo == "Up")
                            {
                                series.Color = Color.SeaGreen;
                            }
                            else
                            {
                                series.Color = Color.OrangeRed;
                            }

                            // Getting all unique equipment name and date in the dataset
                            var equipmentNames = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("EquipementName")).Distinct();
                           

                            var dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("Date")).Distinct();

                            if (SelectDateList.SelectedValue == "date")
                            {
                                dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("Date")).Distinct();
                            }
                            else if (SelectDateList.SelectedValue == "month")
                            {
                                dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("Month")).Distinct();
                            }
                            else
                            {
                                dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<int>("Year").ToString()).Distinct();

                            }
                            foreach (var equipmentName in equipmentNames)
                            {
                                foreach (var date in dates)
                                {
                                    var rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && row.Field<string>("Date") == date && row.Field<string>("StatusInfo") == statusInfo);
                                    if (SelectDateList.SelectedValue == "date")
                                    {
                                         rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && row.Field<string>("Date") == date && row.Field<string>("StatusInfo") == statusInfo);
                                    }
                                    else if (SelectDateList.SelectedValue == "month")
                                    {
                                        rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && row.Field<string>("Month") == date && row.Field<string>("StatusInfo") == statusInfo);
                                    }
                                    else
                                    {
                                        rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && Convert.ToInt32(row["Year"]) == Convert.ToInt32(date) && row.Field<string>("StatusInfo") == statusInfo);

                                    }
                                    if (rows.Count() == 0)
                                    {
                                        // If there is no data, create a new data point with a downtime duration of zero
                                        DataPoint dataPoint = new DataPoint();
                                        dataPoint.YValues = new double[] { 0 };
                                        dataPoint.AxisLabel = date + " - " + equipmentName;
                                        series.Points.Add(dataPoint);
                                    }
                                    else
                                    {
                                        // If there is data, loop through the rows and add them as data points to the series
                                        foreach (DataRow row in rows)
                                        {
                                            int downtimeDurationInMinutes = Convert.ToInt32(row["TotalDowntime"]);
                                            double downtimeDurationInHours = (double)downtimeDurationInMinutes / 60.0;
                                            string downtimeDurationInHoursStr = downtimeDurationInHours.ToString("0.00", CultureInfo.InvariantCulture);

                                            DataPoint dataPoint = new DataPoint();
                                            dataPoint.YValues = new double[] { downtimeDurationInHours };

                                             if (SelectDateList.SelectedValue == "date")
                                            {
                                                dataPoint.AxisLabel = row["Date"] + "-" + row["EquipementName"];
                                            }
                                            else if (SelectDateList.SelectedValue == "month")
                                            {
                                                dataPoint.AxisLabel = row["Month"] + "-" + row["EquipementName"];
                                            }
                                            else
                                            {
                                                dataPoint.AxisLabel = row["Year"].ToString() + "-" + row["EquipementName"];

                                            }
                                            dataPoint.ToolTip = "Status: " + statusInfo + "  " + "Downtime: " + downtimeDurationInMinutes + " minutes";
                                            dataPoint.Label = downtimeDurationInHoursStr;
                                            series.Points.Add(dataPoint);

                                        }
                                    }
                                }
                            }
                           

                            // Add the series to the chart
                            downtimechart.Series.Add(series);
                        }

                        break;

                    
                    case "Line":
                        downtimechart.Series[0].ChartType = SeriesChartType.Line;
                        downtimechart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
                        downtimechart.ChartAreas[0].AxisX.Interval = 1;
                        downtimechart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;

                        downtimechart.Series.Clear();
                        
                        var statusInfos1 = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("StatusInfo")).Distinct();
                        foreach (var statusInfo in statusInfos1)
                        {
                            //New series
                            Series series = new Series();
                            series.Name = statusInfo;
                            series.ChartType = SeriesChartType.Line;
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
                                double downtimeDurationInHours = (double)downtimeDurationInMinutes / 60.0;
                                string downtimeDurationInHoursStr = downtimeDurationInHours.ToString("0.00");
                                DataPoint dataPoint = new DataPoint();
                                dataPoint.YValues = new double[] { downtimeDurationInHours };
                                if (SelectDateList.SelectedValue == "date")
                                {
                                    dataPoint.AxisLabel = row["Date"] + "-" + row["EquipementName"];
                                }
                                else if (SelectDateList.SelectedValue == "month")
                                {
                                    dataPoint.AxisLabel = row["Month"] + "-" + row["EquipementName"];
                                }
                                else
                                {
                                    dataPoint.AxisLabel = row["Year"] + "-" + row["EquipementName"];
                                }
                               
                                dataPoint.ToolTip = "Status: " + statusInfo + "  " + "Downtime: " + downtimeDurationInMinutes + " minutes";
                                dataPoint.Label = downtimeDurationInHoursStr;
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


        