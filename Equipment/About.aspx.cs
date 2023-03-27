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

namespace Equipment
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{

            //    BindData();
            //}
        }
        

        protected void SelectDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectDateList.SelectedValue == "6")
            {
                lblStartDate.Visible = true;
                lblEndDate.Visible = true;
                txtStartDate.Visible = true;
                txtEndDate.Visible = true;
            }
            else
            {
                lblStartDate.Visible = false;
                lblEndDate.Visible = false;
                txtStartDate.Visible = false;
                txtEndDate.Visible = false;
            }
        }

        //Display All Data
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
        //    downtimechart.Series.Clear();
        //    var statusInfos = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("StatusInfo")).Distinct();
        //    foreach (var statusInfo in statusInfos)
        //    {
        //       //New series
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


        //    downtimechart.ChartAreas[0].AxisX.Title = "Equipment Name";
        //    downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours";
        //    downtimechart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
           
        //    downtimechart.ChartAreas[0].AxisX.Interval = 1;
        //    downtimechart.DataBind();
        //}
        protected void ExportButton_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=EquipmentDowntimeData.xls");
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
            string query = "GetData";
            if (SelectDateList.SelectedValue == "0")
            {
                //BindData();
                GridData.Visible= false;
                lblwarning.Visible = true;
                return;
            }
            else
            {
                GridData.Visible = true;
                lblwarning.Visible = false;
            }
            //  select date range
            DateTime startDate;
            DateTime endDate = DateTime.Now;

            switch (SelectDateList.SelectedValue)
            {
                case "1": // 1 day
                    startDate = endDate.AddDays(-1);
                    break;
                case "7": // 7 days
                    startDate = endDate.AddDays(-7);
                    break;
                case "30": // 30 days
                    startDate = endDate.AddDays(-30);
                    break;
                case "90": // Quarterly
                    startDate = endDate.AddMonths(-3);
                    break;
                case "365": // 1 year
                    startDate = endDate.AddYears(-1);
                    break;
                case "6": // Custom
                    startDate = DateTime.ParseExact(txtStartDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    endDate = DateTime.ParseExact(txtEndDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    break;
                default:
                    startDate = DateTime.MinValue;
                    break;
            }
            if (startDate > endDate)
            {
                lbldaterange.Visible = true;
            }
            else
            {
                lbldaterange.Visible= false;
            }
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
           
            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);
            //parameter for specific equipment
            if (!string.IsNullOrEmpty(EquipmentList.SelectedValue))
            {
                command.Parameters.AddWithValue("@EquipmentName", EquipmentList.SelectedValue);
            }
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);


            // No data in the selected date range
            if (dataset.Tables[0].Rows.Count == 0)
            {
                DataTable dataTable = dataset.Tables[0].Clone();
                dataTable.Rows.Add(dataTable.NewRow()); // add a dummy row
                GridData.AutoGenerateColumns = true;
                GridData.DataSource = dataTable;
                GridData.DataBind();
                downtimechart.Series.Clear();
                downtimechart.Titles.Clear();
                Series series = new Series("No data");
                series.Points.AddXY(DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy"), 0); // add yesterday's date
                series.Points.AddXY(DateTime.Today.ToString("dd/MM/yyyy"), 0); // add today's date
                downtimechart.Series.Add(series);
                downtimechart.Series[0].XValueType = ChartValueType.Date;
                downtimechart.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy";
                downtimechart.Series[0].XValueMember = "Day";
                downtimechart.Series[0].YValueMembers = "Downtime_Duration";
                downtimechart.ChartAreas[0].AxisX.Title = "Date and Equipment Name";
                downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours";
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
                        downtimechart.Series[0].ChartType = SeriesChartType.Pie;
                        downtimechart.Series[0].LegendText = "#AXISLABEL";
                        dataTable.Rows.Add("No Data", 0);
                        break;
                    case "Column":
                        downtimechart.Series[0].ChartType = SeriesChartType.Column;

                        break;
                    case "Line":
                        downtimechart.Series[0].ChartType = SeriesChartType.Line;
                        break;
                }

            }
            else
            {
                // Display data in grid and chart
                GridData.DataSource = dataset.Tables[0];
                GridData.DataBind();
                downtimechart.DataSource = dataset.Tables[0];
                downtimechart.DataBind();
                // Configure chart
                downtimechart.Series[0].XValueType = ChartValueType.Date;
                downtimechart.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy";
                downtimechart.Series[0].XValueMember = "Day";
                downtimechart.Series[0].YValueMembers = "Downtime_Duration";
                downtimechart.ChartAreas[0].AxisX.Title = "Date and Equipment";
                downtimechart.ChartAreas[0].AxisY.Title = "Downtime Duration in Hours";
                downtimechart.Series[0].Label = "#VALY{0}";


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
                        downtimechart.Series[0].ChartType = SeriesChartType.Pie;
                        downtimechart.Series[0].LegendText = "#AXISLABEL- #VALY{0}";
                        downtimechart.Series[0].Label = "#VALY{0}";
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
                            var dates = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("Day")).Distinct();

                            foreach (var equipmentName in equipmentNames)
                            {
                                foreach (var date in dates)
                                {
                                    var rows = dataset.Tables[0].AsEnumerable().Where(row => row.Field<string>("EquipementName") == equipmentName && row.Field<string>("Day") == date && row.Field<string>("StatusInfo") == statusInfo);
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
                                            int downtimeDurationInMinutes = Convert.ToInt32(row["Downtime_Duration"]);
                                            double downtimeDurationInHours = (double)downtimeDurationInMinutes / 60.0;
                                            string downtimeDurationInHoursStr = downtimeDurationInHours.ToString("0.00");
                                            DataPoint dataPoint = new DataPoint();
                                            dataPoint.YValues = new double[] { downtimeDurationInHours };
                                            dataPoint.AxisLabel = date + "-" + equipmentName;
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
                        // Create a new series for each unique StatusInfo in the dataset
                        var statusInfos1 = dataset.Tables[0].AsEnumerable().Select(row => row.Field<string>("StatusInfo")).Distinct();
                        foreach (var statusInfo in statusInfos1)
                        {
                            // Create a new series and set its properties
                            Series series = new Series();
                            series.Name = statusInfo;
                            series.ChartType = SeriesChartType.Line;
                            // Set the color of the series based on the value of the StatusInfo column
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
                            // Loop through the rows and add them as data points to the series
                            foreach (DataRow row in rows)
                            {
                                // Get the values from the row
                                int downtimeDurationInMinutes = Convert.ToInt32(row["Downtime_Duration"]);
                                double downtimeDurationInHours = (double)downtimeDurationInMinutes / 60.0;
                                string downtimeDurationInHoursStr = downtimeDurationInHours.ToString("0.00");
                                DataPoint dataPoint = new DataPoint();
                                dataPoint.YValues = new double[] { downtimeDurationInHours };
                                dataPoint.AxisLabel = row["Day"] + "-" + row["EquipementName"];
                                dataPoint.ToolTip = "Status: " + statusInfo + "  " + "Downtime: " + downtimeDurationInMinutes + " minutes";
                                dataPoint.Label = downtimeDurationInHoursStr;
                                series.Points.Add(dataPoint);
                            }
                            // Add the series to the chart
                            downtimechart.Series.Add(series);
                        }
                        break;
                }

            }


        }


    }
}   