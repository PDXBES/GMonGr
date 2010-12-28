#region Using Directoves
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Core.Layers;
using System.Threading;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

#endregion

namespace GMonGr
{
  public partial class frmMain : Form
  {
    #region GlobalVariables
    public GroundwaterMonitorDataSet gwMonDataSet;
    #endregion

    public frmMain()
    {
      Thread thread = new Thread(DoSplash);
      thread.Start();
      this.Hide();
      InitializeComponent();
    }

    #region Methods
    /// <summary>
    /// 
    /// </summary>
    private static void DoSplash()
    {
      DoSplash(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private static void DoSplash(bool waitForClick)
    {
      string versionText = "x.x.x.x";
      string dateText = "1/1/1900";
      if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
      {
        Version v = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
        versionText = v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision;
        dateText = File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("MMMM dd yyyy");
      }
      else
      {
        System.Reflection.AssemblyName assemblyName =
          System.Reflection.Assembly.GetExecutingAssembly().GetName(false);
        int minorVersion = assemblyName.Version.Minor;
        int majorVersion = assemblyName.Version.Major;
        int build = assemblyName.Version.Build;
        int revision = assemblyName.Version.Revision;
        versionText = string.Format("{0}.{1}.{2}.{3}", majorVersion,minorVersion,build,revision);

        FileInfo fi = new FileInfo("GMonGr.exe");
        dateText = fi.CreationTime.Date.ToString("MMMM dd yyyy");
      }

      using (frmSplashScreen frmSp = new frmSplashScreen(versionText,dateText))
      {
        frmSp.ShowDialog(waitForClick);
      }

    }

    /// <summary>
    /// Activates ESRI map control and loads map layers for user to view available groundwater monitors
    /// </summary>
    private void LoadMapControl()
    {
      //TO-DO: obtain ESRI developer licences to enable ArcGIS controls
      //axMonitorMapControl.AddLayerFromFile("\\\\Oberon\\GRP117\\RGONZALEZ\\CoP\\BES\\ASM\\monitoring\\TaggartD\\lyr\\TaggartGWMonitors.lyr");
    }

    /// <summary>
    /// Checks and displays available date range for selected monitor
    /// Sets start and end calendars to min and max of date range by default
    /// Will be primary method for checking and displaying date range
    /// for normalized groundwater monitor data table
    /// </summary>
    private void CheckRange()
    {
      //gwMonDataSet = new GroundwaterMonitorDataSet();
      //gwMonDataSet.InitGroundwaterMonitorDataSet();
      //DataTable gwMonDataTable = new DataTable();
      //gwMonDataTable = gwMonDataTable.GW_MON_DATA;

      //var qrySelectGwMonReadingDate =
      //  from g in gwMonDataTable.AsEnumerable()
      //  where g.Field<string>("sensor_name") == cbxMonitorList.Value
      //  select g.Field<DateTime>("reading_date");
      //DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      //DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      //clndrGwMonStart.Value = minReadingDate;
      //clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P1401 monitor
    /// </summary>
    private void CheckRangeP1401()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP1401();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P1401;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P1402 monitor
    /// </summary>
    private void CheckRangeP1402()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP1402();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P1402;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P4501 monitor
    /// </summary>
    private void CheckRangeP4501()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4501();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P4501;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P4502 monitor
    /// </summary>
    private void CheckRangeP4502()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4502();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P4502;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P4503 monitor
    /// </summary>
    private void CheckRangeP4503()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4503();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P4503;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P4504 monitor
    /// </summary>
    private void CheckRangeP4504()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4504();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P4504;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P4505 monitor
    /// </summary>
    private void CheckRangeP4505()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4505();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P4505;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P5201 monitor
    /// </summary>
    private void CheckRangeP5201()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP5201();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P5201;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P5202 monitor
    /// </summary>
    private void CheckRangeP5202()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP5202();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P5202;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P5203 monitor
    /// </summary>
    private void CheckRangeP5203()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP5203();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P5203;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for P5204 monitor
    /// </summary>
    private void CheckRangeP5204()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP5204();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.P5204;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for TGD01A monitor
    /// </summary>
    private void CheckRangeTGD01A()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD01A();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.TGD01A;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for TGD01B monitor
    /// </summary>
    private void CheckRangeTGD01B()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD01B();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.TGD01B;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for TGD02A monitor
    /// </summary>
    private void CheckRangeTGD02A()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD02A();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.TGD02A;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for TGD02B monitor
    /// </summary>
    private void CheckRangeTGD02B()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD02B();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.TGD02B;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for TGD03A monitor
    /// </summary>
    private void CheckRangeTGD03A()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD03A();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.TGD03A;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Checks available date range for TGD03B monitor
    /// </summary>
    private void CheckRangeTGD03B()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD03B();
      DataTable gwMonDataTable = new DataTable();
      gwMonDataTable = gwMonDataSet.TGD03B;

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
      clndrGwMonStart.Value = minReadingDate;
      clndrGwMonEnd.Value = maxReadingDate;
    }

    /// <summary>
    /// Graphs selected/default date range for selected monitor
    /// Will be primary method for graphing from normalized groundwater monitor data table
    /// </summary>
    private void GraphGwMonData()
    {
      //gwMonDataSet = new GroundwaterMonitorDataSet();
      //gwMonDataSet.InitGroundwaterMonitorDataSet();
      //DataTable gwMonDataTable = new DataTable();
      //DateTime startDate = clndrGwMonStart.Value;
      //DateTime endDate = clndrGwMonEnd.Value;
      //string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      //string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      //gwMonDataTable = gwMonDataSet.GW_MON_DATA;
      //EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
      //  (from g in gwMonDataTable.AsEnumerable()
      //   where g.Field<DateTime>("reading_date") >= clndrGwMonStart.Value
      //   && g.Field<DateTime>("reading_date") <= clndrGwMonEnd.Value
      //   && g.Field<string>("sensor_name") == cbxMonitorList.Value
      //   select g);

      //Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      //foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      //{
      //  gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      //}

      //Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      //foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      //{
      //  groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      //}

      //Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      //foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      //{
      //  minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      //}

      //Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      //foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      //{
      //  maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      //}
      ////set chart properties
      //chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      //chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      //chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      //chartGwData.TitleLeft.Text = "Elevation (ft)";
      //chartGwData.TitleLeft.Visible = true;
      //chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      //chartGwData.TitleBottom.Text = "Monitor Reading Date";
      //chartGwData.TitleBottom.Visible = true;
      //chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      //chartGwData.Visible = true;
      //chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      //chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      //chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      //chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      ////set legend properties
      //chartGwData.Legend.Visible = true;
      //chartGwData.Legend.Location = LegendLocation.Right;
      //chartGwData.Legend.Margins.Left = 3;
      //chartGwData.Legend.Margins.Right = 3;
      //chartGwData.Legend.Margins.Top = 3;
      //chartGwData.Legend.Margins.Bottom = 3;
      //chartGwData.Legend.SpanPercentage = 7;
      //chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      //chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      //chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      //chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      //chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P1401 monitor
    /// </summary>
    private void GraphP1401()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP1401();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P1401;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      var qrySelectGwMonReadingDate =
        from g in gwMonDataTable.AsEnumerable()
        select g.Field<DateTime>("readingDate");
      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

      //if (minReadingDate <= clndrGwMonStart.Value || maxReadingDate>=clndrGwMonEnd.Value)
      //{
      //  throw new Exception("Selected begin date cannot be before and end date cannot be after specified date range for this monitor");
      //}

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P1402 monitor
    /// </summary>
    private void GraphP1402()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP1402();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P1402;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P4501 monitor
    /// </summary>
    private void GraphP4501()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4501();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P4501;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P4502 monitor
    /// </summary>
    private void GraphP4502()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4502();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P4502;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P4503 monitor
    /// </summary>
    private void GraphP4503()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4503();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P4503;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P4504 monitor
    /// </summary>
    private void GraphP4504()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4504();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P4504;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P4505 monitor
    /// </summary>
    private void GraphP4505()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP4505();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P4505;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P5201 monitor
    /// </summary>
    private void GraphP5201()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP5201();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P5201;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P5202 monitor
    /// </summary>
    private void GraphP5202()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP5202();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P5202;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P5203 monitor
    /// </summary>
    private void GraphP5203()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP5203();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P5203;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for P5204 monitor
    /// </summary>
    private void GraphP5204()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableP5204();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.P5204;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for TGD01A monitor
    /// </summary>
    private void GraphTGD01A()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD01A();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.TGD01A;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for TGD01B monitor
    /// </summary>
    private void GraphTGD01B()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD01B();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.TGD01B;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for TGD02A monitor
    /// </summary>
    private void GraphTGD02A()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD02A();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.TGD02A;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for TGD02B monitor
    /// </summary>
    private void GraphTGD02B()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD02B();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.TGD02B;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for TGD03A monitor
    /// </summary>
    private void GraphTGD03A()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD03A();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.TGD03A;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// Graphs selected/default date range for TGD03B monitor
    /// </summary>
    private void GraphTGD03B()
    {
      gwMonDataSet = new GroundwaterMonitorDataSet();
      gwMonDataSet.InitTableTGD03B();
      DataTable gwMonDataTable = new DataTable();
      DateTime startDate = clndrGwMonStart.Value;
      DateTime endDate = clndrGwMonEnd.Value;
      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

      gwMonDataTable = gwMonDataSet.TGD03B;
      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
        (from g in gwMonDataTable.AsEnumerable()
         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
         select g);

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
      {
        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
      {
        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
      {
        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "C", false));
      }

      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
      {
        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "C", false));
      }
      //set chart properties
      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
      chartGwData.TitleLeft.Text = "Elevation (ft)";
      chartGwData.TitleLeft.Visible = true;
      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
      chartGwData.TitleBottom.Text = "Monitor Reading Date";
      chartGwData.TitleBottom.Visible = true;
      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
      chartGwData.Visible = true;
      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

      //set legend properties
      chartGwData.Legend.Visible = true;
      chartGwData.Legend.Location = LegendLocation.Right;
      chartGwData.Legend.Margins.Left = 3;
      chartGwData.Legend.Margins.Right = 3;
      chartGwData.Legend.Margins.Top = 3;
      chartGwData.Legend.Margins.Bottom = 3;
      chartGwData.Legend.SpanPercentage = 7;
      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
      chartGwData.Refresh();
    }

    /// <summary>
    /// method for access tabControlMain tabs for switching
    /// </summary>
    private void LoadTab(string tabKey)
    {
      try
      {
        Cursor = Cursors.WaitCursor;
        this.tabControlMain.SelectedTab = this.tabControlMain.Tabs[tabKey];
      }
      catch
      {
        MessageBox.Show("Tab '" + tabKey + "' not found.");
      }
      finally
      {
        Cursor = Cursors.Default;
      }
    }

    /// <summary>
    /// Loads and copies update file to GroundwaterMonitorDataSet
    /// </summary>
    private void WriteGwMonUpdateFile(string fileName)
    {
      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException();
      }

      string gwMonUpdateFile = Properties.Settings.Default.GwMonUpdateFile;
      if (File.Exists(gwMonUpdateFile))
      {
        File.Delete(gwMonUpdateFile);
      }

      File.Copy(fileName, gwMonUpdateFile, true);
    }

    /// <summary>
    /// Invokes WriteGwMonUpdateFile method and runs QC prior to submitting updates
    /// </summary>
    private bool PrepareUpdateFile(string fileName)
    {
      WriteGwMonUpdateFile(fileName);
      LoadGwMonUpdateData();

      int qcCount = 0;

      //SetProgress = 0;

      gwMonDataSet.GwMonQc.Clear();
      
      qcCount += QCQueryTimeConflict(gwMonDataSet.GwMonQc);
      qcCount += QCQueryPiezoReadingError(gwMonDataSet.GwMonQc);

      return qcCount == 0;
    }

    #region DataUpdateQC
    /// <summary>
    /// Queries incoming update table against main data to make sure readings
    /// with duplicate timestamps are not appended
    /// </summary>
    private int QCQueryTimeConflict(DataTable qcDt)
    {   
      int qcCount = 0;
     
      var qryTimeRangeGwMon = 
        from u in gwMonDataSet.GwMonUpdater
        join g in gwMonDataSet.P1401
        on u.readingDate equals g.readingDate
        select new
        {
          readingDate = g.readingDate,
          errorCode = (string)GwMonErrors.ReadingDateConflict,
          errorDescription = g.readingDate + " already has reading for this date/time"
        };
          
      //TO-DO: update GWMonDataSet to include GWMonUpdate datatable
      foreach (var row in qryTimeRangeGwMon)
      {
        qcDt.Rows.Add(row.readingDate, row.errorCode, row.errorDescription);
        qcCount++;
      }
      return qcCount;
    }

    /// <summary>
    /// Queries incoming update table to make sure valid monitor readings are present
    /// </summary>
    private int QCQueryPiezoReadingError(DataTable qcDt)
    {
      int qcCount = 0;

      //TO-DO: update GroundwaterMonitorDataSet to include GwMonUpdate datatable
      var qryPiezoError =
        from u in gwMonDataSet.GwMonUpdater
        where u.readingHertz == 0
        select new
        {
          readingDate = u.readingDate,
          errorCode = (string)GwMonErrors.PiezoReadingError,
          errorDescription = u.readingDate + " has piezo reading error"
        };

      foreach (var row in qryPiezoError)
      {
        qcDt.Rows.Add(row.readingDate, row.errorCode, row.errorDescription);
        qcCount++;
      }
      return qcCount;
    }
    #endregion

    /// <summary>
    /// Resets the UI to the initial state for beginning the Update process
    /// </summary>
    private void RestartUpdate()
    {
      tabControlMain.SelectedTab=tabControlMain.Tabs[0];
      dgvDataUpdate.Visible = false;
      btnSubmitUpdates.Enabled = false;
      txtUploadFilePath.Clear();
    }

    private void LoadGwMonUpdateData()
    {
      GroundwaterMonitorDataSetTableAdapters.GwMonUpdaterTableAdapter gwMonUpdaterTA;
      gwMonUpdaterTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.GwMonUpdaterTableAdapter();
      gwMonUpdaterTA.Fill(gwMonDataSet.GwMonUpdater);

      GroundwaterMonitorDataSetTableAdapters.P1401TableAdapter p1401TA;
      p1401TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P1401TableAdapter();
      p1401TA.Fill(gwMonDataSet.P1401);
    }
    #endregion

    #region Events
    private void frmMain_Load(object sender, EventArgs e)
    {
      // TODO: This line of code loads data into the 'groundwaterMonitorDataSet.MONITOR_LOCATIONS' table. You can move, or remove it, as needed.
      this.mONITOR_LOCATIONSTableAdapter.Fill(this.groundwaterMonitorDataSet.MONITOR_LOCATIONS);
      chartGwData.Visible = false;
      //LoadMapControl();
    }
    
    private void btnLoadGraph_Click(object sender, EventArgs e)
    {
      try
      {
        //clear existing data series
        chartGwData.Series.Clear();
        chartGwData.Visible = false;
        object gwMonSelection = cbxMonitorList.Value;
        switch (gwMonSelection.ToString())
        {
          case "P-14-01":
            GraphP1401();
            break;
          case "P-14-02":
            GraphP1402();
            break;
          case "P-45-01":
            GraphP4501();
            break;
          case "P-45-02":
            GraphP4502();
            break;
          case "P-45-03":
            GraphP4503();
            break;
          case "P-45-04":
            GraphP4504();
            break;
          case "P-45-05":
            GraphP4505();
            break;
          case "P-52-01":
            GraphP5201();
            break;
          case "P-52-02":
            GraphP5202();
            break;
          case "P-52-03":
            GraphP5203();
            break;
          case "P-52-04":
            GraphP5204();
            break;
          case "TGD-01A":
            GraphTGD01A();
            break;
          case "TGD-01B":
            GraphTGD01B();
            break;
          case "TGD-02A":
            GraphTGD02A();
            break;
          case "TGD-02B":
            GraphTGD02B();
            break;
          case "TGD-03A":
            GraphTGD03A();
            break;
          case "TGD-03B":
            GraphTGD03B();
            break;
          default:
            MessageBox.Show("Invalid selection. Please select a monitor from the drop-down box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            break;
        }
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error loading chart: " + ex.Message, "Chart Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        chartGwData.Visible = true;
      }
    }

    private void btnClearChart_Click(object sender, EventArgs e)
    {
      chartGwData.Series.Clear();
      chartGwData.Visible = false;
    }

    private void btnLoadUpdateFile_Click(object sender, EventArgs e)
    {
      //Open file dialog handling
      ofdMain.DefaultExt = "*.csv";
      ofdMain.FileName = "*.csv";
      ofdMain.Filter = "csv files|*.csv|txt files|*.txt";
      DialogResult ofdMainResult = ofdMain.ShowDialog();

      //Check to make sure the OFD dialog result is "OK"
      if (ofdMainResult != DialogResult.OK)
      {
        return;
      }

      Cursor = Cursors.WaitCursor;
      btnSubmitUpdates.Enabled = false;
      txtUploadFilePath.Text = ofdMain.FileName;

      if (!PrepareUpdateFile(txtUploadFilePath.Text))
      {
        tabControlMain.Tabs[2].Visible = true;
        LoadTab("updateFileErrors");
        return;
      }

      Cursor = Cursors.Default;
      btnSubmitUpdates.Enabled = true;
      dgvDataUpdate.Visible = true;
    }
    
    private void btnSubmitUpdates_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      //SetStatus("Submitting");
      
      if (!PrepareUpdateFile(txtUploadFilePath.Text))
      {
        tabControlMain.Tabs[2].Visible = true;
        LoadTab("updateFileErrors");
        return;
      }

      try
      {
        //UpdateGwMonData();
        if (gwMonDataSet.HasErrors)
        {
          gwMonDataSet.RejectChanges();
          MessageBox.Show("Errors were found in the update table. No changes committed.");
          return;
        }
        //SaveGwMonData();
        MessageBox.Show("All updates to the groundwater monitoring system have completed successfully." + "\n"
        + "To review changes from this edit session, return to the main page, and click on the 'Load Update History' button to load the desired edit session.",
        "GMonGr: Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "GMonGr: Exception Thrown", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        //SetStatus("Ready");
        Cursor.Current = Cursors.Default;
        RestartUpdate();
      }
    }

    private void btnCancelUpdates_Click(object sender, EventArgs e)
    {
      RestartUpdate();
    }

    private void toolBarManagerMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
    {
      switch (e.Tool.SharedProps.Category)
      {
        case "loadTab":
          LoadTab(e.Tool.Key);
          return;
        default:
          break;
      }
      switch (e.Tool.Key)
      {
        case "exitProgram":
          Application.Exit();
          return;
        default:
          MessageBox.Show("Tool '" + e.Tool.Key + "' not implemented");
          return;
      }
    }

    private void cbxMonitorList_ValueChanged(object sender, EventArgs e)
    {
      txtGwMonDateRange.Text = "";
      object gwMonSelection = cbxMonitorList.Value;
      switch (gwMonSelection.ToString())
      {
        case "P-14-01":
          CheckRangeP1401();
          break;
        case "P-14-02":
          CheckRangeP1402();
          break;
        case "P-45-01":
          CheckRangeP4501();
          break;
        case "P-45-02":
          CheckRangeP4502();
          break;
        case "P-45-03":
          CheckRangeP4503();
          break;
        case "P-45-04":
          CheckRangeP4504();
          break;
        case "P-45-05":
          CheckRangeP4505();
          break;
        case "P-52-01":
          CheckRangeP5201();
          break;
        case "P-52-02":
          CheckRangeP5202();
          break;
        case "P-52-03":
          CheckRangeP5203();
          break;
        case "P-52-04":
          CheckRangeP5204();
          break;
        case "TGD-01A":
          CheckRangeTGD01A();
          break;
        case "TGD-01B":
          CheckRangeTGD01B();
          break;
        case "TGD-02A":
          CheckRangeTGD02A();
          break;
        case "TGD-02B":
          CheckRangeTGD02B();
          break;
        case "TGD-03A":
          CheckRangeTGD03A();
          break;
        case "TGD-03B":
          CheckRangeTGD03B();
          break;
        default:
          MessageBox.Show("Invalid selection. Please select a monitor from the drop-down box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          break;
      }
    }
   
    #endregion
  }
}

public static class GwMonErrors
{
  public const string ReadingDateConflict = "Conflicting Reading Date";
  public const string PiezoReadingError = "No Piezometer Reading";
}
