#region Using Directoves
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

using Infragistics.UltraChart.Shared.Styles;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Core.Layers;

using Microsoft.SqlServer.Server;
using Microsoft.SqlServer;
using Microsoft.CSharp;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;



#endregion

namespace GMonGr
{
  public partial class frmMain : Form
  {
    #region GlobalVariables
    //public GroundwaterMonitorDataSet gwMonDataSet;
    //public NeptuneDataSet neptuneDataSet;
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
      try
      {
        LoadGwMonGraphingData();

        DataTable gwMonDt = new DataTable();
        gwMonDt = groundwaterMonitorDataSet.GW_MONITORING;

        var qrySelectGwMonReadingDate =
          from g in gwMonDt.AsEnumerable()
          select g.Field<DateTime>("reading_date");
        DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
        DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
        txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
        clndrGwMonStart.Value = minReadingDate;
        clndrGwMonEnd.Value = maxReadingDate;
      }
      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running CheckRange: " + ex.Message, "Error Checking Date Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    ///// <summary>
    ///// Checks available date range for P1401 monitor
    ///// </summary>
    
    //private void CheckRangeP1401()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP1401();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P1401;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P1402 monitor
    ///// </summary>
    //private void CheckRangeP1402()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP1402();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P1402;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P4501 monitor
    ///// </summary>
    //private void CheckRangeP4501()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP4501();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P4501;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P4502 monitor
    ///// </summary>
    //private void CheckRangeP4502()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP4502();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P4502;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P4503 monitor
    ///// </summary>
    //private void CheckRangeP4503()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP4503();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P4503;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P4504 monitor
    ///// </summary>
    //private void CheckRangeP4504()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP4504();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P4504;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P4505 monitor
    ///// </summary>
    //private void CheckRangeP4505()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP4505();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P4505;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P5201 monitor
    ///// </summary>
    //private void CheckRangeP5201()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP5201();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P5201;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P5202 monitor
    ///// </summary>
    //private void CheckRangeP5202()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP5202();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P5202;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P5203 monitor
    ///// </summary>
    //private void CheckRangeP5203()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP5203();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P5203;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for P5204 monitor
    ///// </summary>
    //private void CheckRangeP5204()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableP5204();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.P5204;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for TGD01A monitor
    ///// </summary>
    //private void CheckRangeTGD01A()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableTGD1A();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.TGD1A;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for TGD01B monitor
    ///// </summary>
    //private void CheckRangeTGD01B()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableTGD1B();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.TGD1B;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for TGD02A monitor
    ///// </summary>
    //private void CheckRangeTGD02A()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableTGD2A();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.TGD2A;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for TGD02B monitor
    ///// </summary>
    //private void CheckRangeTGD02B()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableTGD2B();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.TGD2B;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for TGD03A monitor
    ///// </summary>
    //private void CheckRangeTGD03A()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableTGD3A();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.TGD3A;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    ///// <summary>
    ///// Checks available date range for TGD03B monitor
    ///// </summary>
    //private void CheckRangeTGD03B()
    //{
    //  gwMonDataSet = new GroundwaterMonitorDataSet();
    //  gwMonDataSet.InitTableTGD3B();
    //  DataTable gwMonDataTable = new DataTable();
    //  gwMonDataTable = gwMonDataSet.TGD3B;

    //  var qrySelectGwMonReadingDate =
    //    from g in gwMonDataTable.AsEnumerable()
    //    select g.Field<DateTime>("readingDate");
    //  DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
    //  DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
    //  txtGwMonDateRange.Text = "Begin: " + minReadingDate.ToString() + "\r\n" + "End: " + maxReadingDate.ToString();
    //  clndrGwMonStart.Value = minReadingDate;
    //  clndrGwMonEnd.Value = maxReadingDate;
    //}

    /// <summary>
    /// Graphs selected/default date range for selected monitor
    /// Will be primary method for graphing from normalized groundwater monitor data table
    /// </summary>
    private void GraphGwMonData()
    {
      try
      {
        //chartGwData.ChartType = ChartType.Composite;

        //ChartArea chartGwDataArea = new ChartArea();
        //chartGwData.CompositeChart.ChartAreas.Add(chartGwDataArea);

        ////X-axis settings
        //AxisItem axisX = new AxisItem();
        //axisX.OrientationType = AxisNumber.X_Axis;
        //axisX.DataType = AxisDataType.String;
        //axisX.SetLabelAxisType = SetLabelAxisType.GroupBySeries;
        //axisX.Labels.ItemFormatString = "<ITEM_LABEL>";
        //axisX.Labels.Orientation = TextOrientation.VerticalLeftFacing;

        ////Y-axis settings
        //AxisItem axisY = new AxisItem();
        //axisY.OrientationType = AxisNumber.Y_Axis;
        //axisY.DataType = AxisDataType.Numeric;
        //axisY.Labels.ItemFormatString = "<DATA_VALUE:0.#>";

        //chartGwDataArea.Axes.Add(axisX);
        //chartGwDataArea.Axes.Add(axisY);

        //Add data series to series collection
        Dictionary<string, NumericTimeSeries> numTimeSeriesDict = GetNumericTimeSeriesCollection();

        NumericTimeSeries grndElSeries = new NumericTimeSeries();
        NumericTimeSeries maxGwElSeries = new NumericTimeSeries();
        NumericTimeSeries gwElSeries = new NumericTimeSeries();
        NumericTimeSeries minGwElSeries = new NumericTimeSeries();

        grndElSeries = numTimeSeriesDict["grndElSeries"];
        maxGwElSeries = numTimeSeriesDict["maxGwElSeries"];
        gwElSeries = numTimeSeriesDict["gwElSeries"];
        minGwElSeries = numTimeSeriesDict["minGwElSeries"];

        grndElSeries.Label = "Ground Elev. (ft)";
        maxGwElSeries.Label = "Max GW Elev. (ft)";
        gwElSeries.Label = "GW Elev. (ft)";
        minGwElSeries.Label = "Min GW Elev. (ft)";

        //chartGwData.CompositeChart.Series.Add(gwElSeries);

        //set chart properties
        //chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
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
        chartGwData.Legend.Location = LegendLocation.Bottom;
        chartGwData.Legend.Margins.Left = 3;
        chartGwData.Legend.Margins.Right = 3;
        chartGwData.Legend.Margins.Top = 3;
        chartGwData.Legend.Margins.Bottom = 3;
        chartGwData.Legend.SpanPercentage = 15;
        chartGwData.Legend.ChartComponent.Series.Add(grndElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(maxGwElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(gwElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(minGwElSeries);
        chartGwData.Refresh();

        chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + clndrGwMonStart.Value.ToShortDateString() + " - " + clndrGwMonEnd.Value.ToShortDateString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
        //chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
        //chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
        //chartGwData.TitleLeft.Text = "Elevation (ft)";
        //chartGwData.TitleLeft.Visible = true;
        //chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
        //chartGwData.TitleBottom.Text = "Monitor Reading Date";
        //chartGwData.TitleBottom.Visible = true;

        ////Add a chart layer
        //ChartLayerAppearance chartGwDataChrtLyr = new ChartLayerAppearance();
        //chartGwDataChrtLyr.ChartType = ChartType.LineChart;
        //chartGwDataChrtLyr.ChartArea = chartGwDataArea;
        //chartGwDataChrtLyr.AxisX = axisX;
        //chartGwDataChrtLyr.AxisY = axisY;
        //chartGwDataChrtLyr.Series.Add(gwElSeries);
        //chartGwData.CompositeChart.ChartLayers.Add(chartGwDataChrtLyr);

        ////Add a legend
        //CompositeLegend chartGwDataLgd = new CompositeLegend();
        //chartGwDataLgd.ChartLayers.Add(chartGwDataChrtLyr);
        //chartGwDataLgd.Bounds = new Rectangle(0,90,100,10);
        //chartGwDataLgd.BoundsMeasureType = MeasureType.Percentage;
        //chartGwDataLgd.PE.ElementType = PaintElementType.Gradient;
        //chartGwDataLgd.PE.FillGradientStyle = GradientStyle.ForwardDiagonal;
        //chartGwDataLgd.PE.Fill = Color.CornflowerBlue;
        //chartGwDataLgd.PE.FillStopColor = Color.Transparent;
        //chartGwDataLgd.Border.CornerRadius = 10;
        //chartGwDataLgd.Border.Thickness = 0;

        //chartGwData.CompositeChart.Legends.Add(chartGwDataLgd);
        //chartGwData.Visible = true;     
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running GraphGwMonData: " + ex.Message, "Error Graphing Time Series", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    /// <summary>
    /// Graphs selected/default date range for P1401 monitor
    /// </summary>
    private void GraphP1401(frmMain frm)
    {
      try
      {
        //chartGwData.ChartType = ChartType.Composite;

        //ChartArea chartGwDataArea = new ChartArea();
        //chartGwData.CompositeChart.ChartAreas.Add(chartGwDataArea);

        ////X-axis settings
        //AxisItem axisX = new AxisItem();
        //axisX.OrientationType = AxisNumber.X_Axis;
        //axisX.DataType = AxisDataType.String;
        //axisX.SetLabelAxisType = SetLabelAxisType.GroupBySeries;
        //axisX.Labels.ItemFormatString = "<ITEM_LABEL>";
        //axisX.Labels.Orientation = TextOrientation.VerticalLeftFacing;

        ////Y-axis settings
        //AxisItem axisY = new AxisItem();
        //axisY.OrientationType = AxisNumber.Y_Axis;
        //axisY.DataType = AxisDataType.Numeric;
        //axisY.Labels.ItemFormatString = "<DATA_VALUE:0.#>";

        //chartGwDataArea.Axes.Add(axisX);
        //chartGwDataArea.Axes.Add(axisY);

        //Add data series to series collection
        Dictionary<string, NumericTimeSeries> numTimeSeriesDict = frm.GetP1401NumericTimeSeriesCollection();

        NumericTimeSeries grndElSeries = new NumericTimeSeries();
        NumericTimeSeries maxGwElSeries = new NumericTimeSeries();
        NumericTimeSeries gwElSeries = new NumericTimeSeries();
        NumericTimeSeries minGwElSeries = new NumericTimeSeries();

        grndElSeries = numTimeSeriesDict["grndElSeries"];
        maxGwElSeries = numTimeSeriesDict["maxGwElSeries"];
        gwElSeries = numTimeSeriesDict["gwElSeries"];
        minGwElSeries = numTimeSeriesDict["minGwElSeries"];

        grndElSeries.Label = "Ground Elev. (ft)";
        maxGwElSeries.Label = "Max GW Elev. (ft)";
        gwElSeries.Label = "GW Elev. (ft)";
        minGwElSeries.Label = "Min GW Elev. (ft)";

        //chartGwData.CompositeChart.Series.Add(gwElSeries);

        //set chart properties
        //chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
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
        chartGwData.Legend.Location = LegendLocation.Bottom;
        chartGwData.Legend.Margins.Left = 3;
        chartGwData.Legend.Margins.Right = 3;
        chartGwData.Legend.Margins.Top = 3;
        chartGwData.Legend.Margins.Bottom = 3;
        chartGwData.Legend.SpanPercentage = 15;
        chartGwData.Legend.ChartComponent.Series.Add(grndElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(maxGwElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(gwElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(minGwElSeries);
        chartGwData.Refresh();

        chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + clndrGwMonStart.Value.ToShortDateString() + " - " + clndrGwMonEnd.Value.ToShortDateString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
        //chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
        //chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
        //chartGwData.TitleLeft.Text = "Elevation (ft)";
        //chartGwData.TitleLeft.Visible = true;
        //chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
        //chartGwData.TitleBottom.Text = "Monitor Reading Date";
        //chartGwData.TitleBottom.Visible = true;

        ////Add a chart layer
        //ChartLayerAppearance chartGwDataChrtLyr = new ChartLayerAppearance();
        //chartGwDataChrtLyr.ChartType = ChartType.LineChart;
        //chartGwDataChrtLyr.ChartArea = chartGwDataArea;
        //chartGwDataChrtLyr.AxisX = axisX;
        //chartGwDataChrtLyr.AxisY = axisY;
        //chartGwDataChrtLyr.Series.Add(gwElSeries);
        //chartGwData.CompositeChart.ChartLayers.Add(chartGwDataChrtLyr);

        ////Add a legend
        //CompositeLegend chartGwDataLgd = new CompositeLegend();
        //chartGwDataLgd.ChartLayers.Add(chartGwDataChrtLyr);
        //chartGwDataLgd.Bounds = new Rectangle(0,90,100,10);
        //chartGwDataLgd.BoundsMeasureType = MeasureType.Percentage;
        //chartGwDataLgd.PE.ElementType = PaintElementType.Gradient;
        //chartGwDataLgd.PE.FillGradientStyle = GradientStyle.ForwardDiagonal;
        //chartGwDataLgd.PE.Fill = Color.CornflowerBlue;
        //chartGwDataLgd.PE.FillStopColor = Color.Transparent;
        //chartGwDataLgd.Border.CornerRadius = 10;
        //chartGwDataLgd.Border.Thickness = 0;

        //chartGwData.CompositeChart.Legends.Add(chartGwDataLgd);
        //chartGwData.Visible = true;     
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running GraphP1401 function: " + ex.Message, "Graphing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }

    /// <summary>
    /// Graphs selected/default date range for P1402 monitor
    /// </summary>
//    private void GraphP1402()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP1402();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P1402;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();
      
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P4501 monitor
//    /// </summary>
//    private void GraphP4501()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP4501();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P4501;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P4502 monitor
//    /// </summary>
//    private void GraphP4502()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP4502();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P4502;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P4503 monitor
//    /// </summary>
//    private void GraphP4503()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP4503();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P4503;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P4504 monitor
//    /// </summary>
//    private void GraphP4504()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP4504();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P4504;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P4505 monitor
//    /// </summary>
//    private void GraphP4505()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP4505();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P4505;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P5201 monitor
//    /// </summary>
//    private void GraphP5201()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP5201();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P5201;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P5202 monitor
//    /// </summary>
//    private void GraphP5202()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP5202();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P5202;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P5203 monitor
//    /// </summary>
//    private void GraphP5203()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP5203();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P5203;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for P5204 monitor
//    /// </summary>
//    private void GraphP5204()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableP5204();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.P5204;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for TGD01A monitor
//    /// </summary>
//    private void GraphTGD1A()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableTGD1A();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.TGD1A;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for TGD01B monitor
//    /// </summary>
//    private void GraphTGD1B()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableTGD1B();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.TGD1B;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for TGD02A monitor
//    /// </summary>
//    private void GraphTGD2A()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableTGD2A();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.TGD2A;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//#endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for TGD02B monitor
//    /// </summary>
//    private void GraphTGD2B()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableTGD2B();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.TGD2B;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for TGD03A monitor
//    /// </summary>
//    private void GraphTGD3A()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableTGD3A();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.TGD3A;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

//    /// <summary>
//    /// Graphs selected/default date range for TGD03B monitor
//    /// </summary>
//    private void GraphTGD3B()
//    {
//      gwMonDataSet = new GroundwaterMonitorDataSet();
//      gwMonDataSet.InitTableTGD3B();
//      DataTable gwMonDataTable = new DataTable();
//      DateTime startDate = clndrGwMonStart.Value;
//      DateTime endDate = clndrGwMonEnd.Value;
//      string startDateStr = clndrGwMonStart.Value.ToShortDateString();
//      string endDateStr = clndrGwMonEnd.Value.ToShortDateString();

//      gwMonDataTable = gwMonDataSet.TGD3B;
//      EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
//        (from g in gwMonDataTable.AsEnumerable()
//         where g.Field<DateTime>("readingDate") >= clndrGwMonStart.Value && g.Field<DateTime>("readingDate") <= clndrGwMonEnd.Value
//         select g);

//      var qrySelectGwMonReadingDate =
//        from g in gwMonDataTable.AsEnumerable()
//        select g.Field<DateTime>("readingDate");
//      DateTime minReadingDate = qrySelectGwMonReadingDate.Min();
//      DateTime maxReadingDate = qrySelectGwMonReadingDate.Max();

//      #region ErrorHandling
//      //Throw exception if user has specified a date out of range
//      if (minReadingDate > clndrGwMonStart.Value || maxReadingDate < clndrGwMonEnd.Value)
//      {
//        throw new Exception("Selected begin date cannot be before and end date cannot be after date range for this monitor");
//      }

//      //Throw an exception if the user has chosen an end date preceding a start date
//      if (clndrGwMonEnd.Value < clndrGwMonStart.Value)
//      {
//        throw new Exception("Calendar begin date must occur BEFORE the calendar end date. Please choose a begin date that precedes an end date.");
//      }
//      #endregion

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries gwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow gwElevDr in qrySelectGwMonRecords)
//      {
//        gwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(gwElevDr.ItemArray[2].ToString()), System.Double.Parse(gwElevDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElevDr.ItemArray[2]), false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries groundElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow groundElevDr in qrySelectGwMonRecords)
//      {
//        groundElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(groundElevDr.ItemArray[2].ToString()), System.Double.Parse(groundElevDr.ItemArray[11].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries minGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow minGwElevDr in qrySelectGwMonRecords)
//      {
//        minGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(minGwElevDr.ItemArray[2].ToString()), System.Double.Parse(minGwElevDr.ItemArray[9].ToString()), "", false));
//      }

//      Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries maxGwElevSeries = new Infragistics.UltraChart.Resources.Appearance.NumericTimeSeries();
//      foreach (DataRow maxGwElevDr in qrySelectGwMonRecords)
//      {
//        maxGwElevSeries.Points.Add(new Infragistics.UltraChart.Resources.Appearance.NumericTimeDataPoint(System.DateTime.Parse(maxGwElevDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElevDr.ItemArray[10].ToString()), "", false));
//      }
//      //set chart properties
//      chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + startDateStr.ToString() + " - " + endDateStr.ToString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
//      chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
//      chartGwData.TitleLeft.Text = "Elevation (ft)";
//      chartGwData.TitleLeft.Visible = true;
//      chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
//      chartGwData.TitleBottom.Text = "Monitor Reading Date";
//      chartGwData.TitleBottom.Visible = true;
//      chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//      chartGwData.Visible = true;
//      chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
//      chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
//      chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

//      //set legend properties
//      chartGwData.Legend.Visible = true;
//      chartGwData.Legend.Location = LegendLocation.Right;
//      chartGwData.Legend.Margins.Left = 3;
//      chartGwData.Legend.Margins.Right = 3;
//      chartGwData.Legend.Margins.Top = 3;
//      chartGwData.Legend.Margins.Bottom = 3;
//      chartGwData.Legend.SpanPercentage = 7;
//      chartGwData.Legend.ChartComponent.Series.Add(gwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(maxGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(minGwElevSeries);
//      chartGwData.Legend.ChartComponent.Series.Add(groundElevSeries);
//      chartGwData.Refresh();
//    }

    /// <summary>
    /// Main method for accessing GetGwDataCollection method to pass acquired Dictionary of NumericTimeSeries to
    /// the main graphing method, GraphGwMonData
    /// TO-DO: implementation of GetNumericTimeSeriesCollection method complete, waiting to completion of implementation of GroundwaterMonitorDataSet
    /// </summary>
    private Dictionary<string, NumericTimeSeries> GetNumericTimeSeriesCollection()
    {     
      Dictionary<string, NumericTimeSeries> numTimeSeriesDict = new Dictionary<string, NumericTimeSeries>();

      try
      {
        Dictionary<string, DataTable> gwDataTableDict = GetGwDataCollection();
        NumericTimeSeries grndElSeries = new NumericTimeSeries();
        NumericTimeSeries maxGwElSeries = new NumericTimeSeries();
        NumericTimeSeries gwElSeries = new NumericTimeSeries();
        NumericTimeSeries minGwElSeries = new NumericTimeSeries();

        DataTable gwDt = gwDataTableDict["gwDataDt"];
        EnumerableRowCollection gwDataEnumRowColl = gwDt.AsEnumerable();

        foreach (DataRow grndElDr in gwDataEnumRowColl)
        {
          grndElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(grndElDr.ItemArray[5].ToString()), System.Double.Parse(grndElDr.ItemArray[13].ToString()),"",false));
        }

        foreach (DataRow maxGwElDr in gwDataEnumRowColl)
        {
          maxGwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(maxGwElDr.ItemArray[5].ToString()), System.Double.Parse(maxGwElDr.ItemArray[11].ToString()), "", false));
        }
        
        foreach (DataRow gwElDr in gwDataEnumRowColl)
        {
          gwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(gwElDr.ItemArray[5].ToString()), System.Double.Parse(gwElDr.ItemArray[14].ToString()), String.Format("{0:M/d/yyyy}", gwElDr.ItemArray[5]), false));
        }

        foreach (DataRow minGwElDr in gwDataEnumRowColl)
        {
          minGwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(minGwElDr.ItemArray[5].ToString()), System.Double.Parse(minGwElDr.ItemArray[15].ToString()),"",false)); 
        }

        numTimeSeriesDict.Add("grndElSeries", grndElSeries);
        numTimeSeriesDict.Add("maxGwElSeries", maxGwElSeries);
        numTimeSeriesDict.Add("gwElSeries", gwElSeries);
        numTimeSeriesDict.Add("minGwElSeries", minGwElSeries);
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running GetNumericTimeSeriesCollection: " + ex.Message, "Error Loading Numeric Time Series", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return numTimeSeriesDict;
    }

    /// <summary>
    /// Method testing GetP1401GwDataCollection method to pass acquired Dictionary of NumericTimeSeries to
    /// the test graphing method, GraphP1401GwMonData
    /// TO-DO: implementation of GetP1401NumericTimeSeriesCollection method complete, waiting to completion of implementation of GroundwaterMonitorDataSet
    /// to fully implement the main GetGwDataCollection method
    /// </summary>
    private Dictionary<string, NumericTimeSeries> GetP1401NumericTimeSeriesCollection()
    {
      Dictionary<string, NumericTimeSeries> numTimeSeriesDict = new Dictionary<string, NumericTimeSeries>();

      try
      {
        Dictionary<string, DataTable> gwDataTableDict = GetP1401GwDataCollection();
        NumericTimeSeries grndElSeries = new NumericTimeSeries();
        NumericTimeSeries maxGwElSeries = new NumericTimeSeries();
        NumericTimeSeries gwElSeries = new NumericTimeSeries();
        NumericTimeSeries minGwElSeries = new NumericTimeSeries();

        DataTable gwDt = gwDataTableDict["gwDataDt"];
        EnumerableRowCollection gwDataEnumRowColl = gwDt.AsEnumerable();

        foreach (DataRow grndElDr in gwDataEnumRowColl)
        {
          grndElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(grndElDr.ItemArray[2].ToString()), System.Double.Parse(grndElDr.ItemArray[9].ToString()), "", false));
        }

        foreach (DataRow maxGwElDr in gwDataEnumRowColl)
        {
          maxGwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(maxGwElDr.ItemArray[2].ToString()), System.Double.Parse(maxGwElDr.ItemArray[10].ToString()), "", false));
        }

        foreach (DataRow gwElDr in gwDataEnumRowColl)
        {
          gwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(gwElDr.ItemArray[2].ToString()), System.Double.Parse(gwElDr.ItemArray[8].ToString()), String.Format("{0:M/d/yyyy}", gwElDr.ItemArray[2]), false));
        }

        foreach (DataRow minGwElDr in gwDataEnumRowColl)
        {
          minGwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(minGwElDr.ItemArray[2].ToString()), System.Double.Parse(minGwElDr.ItemArray[11].ToString()), "", false));
        }

        numTimeSeriesDict.Add("grndElSeries", grndElSeries);
        numTimeSeriesDict.Add("maxGwElSeries", maxGwElSeries);
        numTimeSeriesDict.Add("gwElSeries", gwElSeries);
        numTimeSeriesDict.Add("minGwElSeries", minGwElSeries);
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running GetNumericTimeSeriesCollection: " + ex.Message, "Error Loading Numeric Time Series", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return numTimeSeriesDict;
    }

    /// <summary>
    /// Gets main groundwater data collection for returning to the GetNumericTimeSeries method
    /// </summary>
    private Dictionary<string, DataTable> GetGwDataCollection()
    {
      Dictionary<string, DataTable> dataTableDict = new Dictionary<string, DataTable>();

      try
      {
        LoadGwMonGraphingData();

        double grndElFt = 0;
        double maxElFt = 0;
        double minElFt = 0;
        
        DataTable grndElDt = new DataTable();
        DataTable gwDataDt = new DataTable();
        DataView gwDataDv = new DataView();
        
        DateTime startDate = new DateTime();
        startDate = SetCalendarStartSafe();
        DateTime endDate = new DateTime();
        endDate = SetCalendarEndSafe();

        gwDataDt = groundwaterMonitorDataSet.GW_MONITORING;
        grndElDt = groundwaterMonitorDataSet.MONITOR_LOCATIONS;

        EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
          (from g in gwDataDt.AsEnumerable()
           where g.Field<DateTime>("reading_date") >= startDate && g.Field<DateTime>("reading_date") <= endDate
           select g);
        
        EnumerableRowCollection<DataRow> qrySelectGrndEl =
          (from m in grndElDt.AsEnumerable()
           where m.Field<double>("toc_elev_ft") != null
           select m);

        grndElFt = qrySelectGrndEl.Max(q => q.Field<double>("toc_elev_ft"));
        maxElFt = qrySelectGwMonRecords.Max(q => q.Field<double>("gw_elev_ft"));
        minElFt = qrySelectGwMonRecords.Min(q => q.Field<double>("gw_elev_ft"));

        gwDataDv = qrySelectGwMonRecords.AsDataView();
        gwDataDt = gwDataDv.ToTable();

        DataColumn grndElevCol = new DataColumn();
        grndElevCol.DataType = System.Type.GetType("System.Double");
        grndElevCol.DefaultValue = grndElFt;
        gwDataDt.Columns.Add(grndElevCol);
        DataColumn maxGwElevCol = new DataColumn();
        maxGwElevCol.DataType = System.Type.GetType("System.Double");
        maxGwElevCol.DefaultValue = maxElFt;
        gwDataDt.Columns.Add(maxGwElevCol);
        DataColumn minGwElevCol = new DataColumn();
        minGwElevCol.DataType = System.Type.GetType("System.Double");
        minGwElevCol.DefaultValue = minElFt;
        gwDataDt.Columns.Add(minGwElevCol);

        dataTableDict.Add("gwDataDt", gwDataDt);
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running GetGwDataCollection: " + ex.Message,"Error Loading Gw Data Collection",MessageBoxButtons.OK,MessageBoxIcon.Error);
      }
      return dataTableDict;
    }

    /// <summary>
    /// Gets test groundwater data collection for returning to the GetP1401NumericTimeSeries method
    /// </summary>
    private Dictionary<string, DataTable> GetP1401GwDataCollection()
    {
      Dictionary<string, DataTable> dataTableDict = new Dictionary<string, DataTable>();
      try
      {
        //LoadP1401GwGraphingData();

        double grndElFt = 0;
        double maxElFt = 0;
        double minElFt = 0;

        DataTable grndElDt = new DataTable();
        DataTable gwDataDt = new DataTable();
        DataView gwDataDv = new DataView();

        DateTime startDate = new DateTime();
        startDate = SetCalendarStartSafe();
        DateTime endDate = new DateTime();
        endDate = SetCalendarEndSafe();

        gwDataDt = groundwaterMonitorDataSet.P1401;
        grndElDt = groundwaterMonitorDataSet.MONITOR_LOCATIONS;

        EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
          (from g in gwDataDt.AsEnumerable()
           where g.Field<DateTime>("readingDate") >= startDate && g.Field<DateTime>("readingDate") <= endDate
           select g);

        EnumerableRowCollection<DataRow> qrySelectGrndEl =
          (from m in grndElDt.AsEnumerable()
           where m.Field<double>("toc_elev_ft") != null
           select m);

        grndElFt = qrySelectGrndEl.Max(q => q.Field<double>("toc_elev_ft"));
        maxElFt = qrySelectGwMonRecords.Max(q => q.Field<double>("gwElevation"));
        minElFt = qrySelectGwMonRecords.Min(q => q.Field<double>("gwElevation"));

        gwDataDv = qrySelectGwMonRecords.AsDataView();
        gwDataDt = gwDataDv.ToTable();

        foreach (DataRow dr in gwDataDt.AsEnumerable())
        {
          //gwDataDt.Columns.Add("groundElevation");
          gwDataDt.Columns["groundElevation"].Equals(grndElFt);
          //gwDataDt.Columns.Add("maxGwElev");
          gwDataDt.Columns["gwMax"].Equals(maxElFt);
          //gwDataDt.Columns.Add("minGwElev");
          gwDataDt.Columns["gwMin"].Equals(minElFt);
        }
        dataTableDict.Add("gwDataDt", gwDataDt);
      }
      catch (Exception ex)
      {
        MessageBox.Show("Error running GetP1401GwDataCollection: " + ex.Message, "Error Loading Gw Data Collection", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      return dataTableDict;
      //GroundwaterMonitorDataSet gwMonDataSet;
      //gwMonDataSet = new GroundwaterMonitorDataSet();
      //gwMonDataSet.InitTableP1401();

      //DataTable gwMonDataTable = new DataTable();
      //DataView gwMonDataView = new DataView();
      
      //DateTime startDate = new DateTime();      
      //startDate = frm.SetCalendarStartSafe();
      //DateTime endDate = new DateTime();
      //endDate = frm.SetCalendarEndSafe();
      
      //string startDateStr = "";
      //startDateStr = frm.SetCalendarStartStringSafe();
      //string endDateStr = "";
      //endDateStr = frm.SetCalendarEndStringSafe();

      //gwMonDataTable = gwMonDataSet.P1401;
      //EnumerableRowCollection<DataRow> qrySelectGwMonRecords =
      //  (from g in gwMonDataTable.AsEnumerable()
      //   where g.Field<DateTime>("readingDate") >= startDate && g.Field<DateTime>("readingDate") <= endDate
      //   select g);
      //gwMonDataView = qrySelectGwMonRecords.AsDataView();
      //gwMonDataTable = gwMonDataView.ToTable();
      //return gwMonDataTable;
    }

    /// <summary>
    /// TO-DO: fill in summary
    /// TO-DO: continue implementation
    /// </summary>
    public DateTime SetCalendarStartSafe()
    {    
      DateTime startDateSafe;
      startDateSafe = clndrGwMonStart.Value;
      return startDateSafe;
    }

    /// <summary>
    /// TO-DO: fill in summary
    /// TO-DO: continue implementation
    /// </summary>
    public DateTime SetCalendarEndSafe()
    {
      DateTime endDateSafe;
      endDateSafe = clndrGwMonEnd.Value;
      return endDateSafe;
    }

    /// <summary>
    /// TO-DO: fill in summary
    /// TO-DO: continue implementation
    /// </summary>
    public string SetCalendarStartStringSafe()
    {
      string startDateStringSafe = "";
      startDateStringSafe = clndrGwMonStart.Value.ToShortDateString();
      return startDateStringSafe;
    }

    /// <summary>
    /// TO-DO: fill in summary
    /// TO-DO: continue implementation
    /// </summary>
    public string SetCalendarEndStringSafe()
    {
      string endDateStringSafe = "";
      endDateStringSafe = clndrGwMonEnd.Value.ToShortDateString();
      return endDateStringSafe;
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
    /// Invokes WriteGwMonUpdateFile method and runs QC prior to submitting updates
    /// </summary>
    private bool PrepareUpdateFile(string fileName)
    {
      int qcCount = 0;
      try
      {
        WriteGwMonUpdateFile(fileName);
        LoadGwMonUpdateData();

        SetProgress = 0;

        groundwaterMonitorDataSet.GwMonQc.Clear();
        qcCount += QCQueryTimeConflict(groundwaterMonitorDataSet.GwMonQc);
        qcCount += QCQueryPiezoReadingError(groundwaterMonitorDataSet.GwMonQc);
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running PrepareUpdateFile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetProgress = 0;
      }

      return qcCount == 0;
    }
    
    /// <summary>
    /// Loads and copies user update file
    /// </summary>
    private void WriteGwMonUpdateFile(string fileName)
    {
      try
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
      catch (Exception ex)
      {
        MessageBox.Show("Error running WriteGwMonUpdateFile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
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
        from u in groundwaterMonitorDataSet.GwMonUpdater
        join g in groundwaterMonitorDataSet.GW_MONITORING
        on u.readingDate equals g.reading_date
        select new
        {
          readingDate = g.reading_date,
          errorCode = (string)GwMonErrors.ReadingDateConflict,
          errorDescription = g.reading_date + " already has reading for this date/time"
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
        from u in groundwaterMonitorDataSet.GwMonUpdater
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

    /// <summary>
    /// 
    /// </summary>
    private void UpdateGwMonDataConnection()
    {
      Microsoft.Data.ConnectionUI.DataConnectionDialog dataConnectionDialog =
        new Microsoft.Data.ConnectionUI.DataConnectionDialog();
      Microsoft.Data.ConnectionUI.DataSource.AddStandardDataSources(dataConnectionDialog);

      //TO-DO; Detect whether Master Data is SQL or Access (Jet) and set SelectedDataSource accordingly
      string gwMonDataConnectionString = Properties.Settings.Default.GwMonitoringConnectionString;
      dataConnectionDialog.SelectedDataSource = Microsoft.Data.ConnectionUI.DataSource.SqlDataSource;
      dataConnectionDialog.SelectedDataProvider = Microsoft.Data.ConnectionUI.DataProvider.SqlDataProvider;
      dataConnectionDialog.ConnectionString = gwMonDataConnectionString;

      if (Microsoft.Data.ConnectionUI.DataConnectionDialog.Show(dataConnectionDialog) != DialogResult.OK)
      {
        return;
      }

      Properties.Settings.Default.SetGwMonConnectionString = dataConnectionDialog.ConnectionString;
      Properties.Settings.Default.Save();
      statusBarMain.Panels["gwMonDataConnection"].Text = "Monitor Data: " + ConnectionStringSummary(Properties.Settings.Default.GwMonitoringConnectionString);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    private string ConnectionStringSummary(string connectionString)
    {
      System.Data.Common.DbConnectionStringBuilder csb;
      csb = new System.Data.Common.DbConnectionStringBuilder();
      csb.ConnectionString = connectionString;
      string summary = csb["data source"].ToString();
      return summary;
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateGwMonTables()
    {
      int editSessionId = UpdateEditSession();

      try
      {
        LoadGwMonUpdateData();
        foreach (GroundwaterMonitorDataSet.GwMonUpdaterRow gwMonUpdaterRow in groundwaterMonitorDataSet.GwMonUpdater)
        {
          DateTime readingDate = gwMonUpdaterRow.readingDate;
          //UpdateGWMon();
        }
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running UpdateGwMonTables: " + ex.Message, "Error Updating Gw Monitor Table", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private int UpdateEditSession()
    {
      try
      {
        //GroundwaterMonitorDataSet.SESSIONRow sessionRow =
        //gwMonDataSet.SESSION.AddSESSIONRow(DateTime.Now,Environment.UserName);
      
        //return sessionRow.edit_id;
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running UpdateEditSession: " + ex.Message, "Error Updating Session Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      
      return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="editSessionId"></param>
    /// <param name="gwMonUpdaterRow"></param>
    private void UpdateGwMon(int editSessionId, GroundwaterMonitorDataSet.GwMonUpdaterRow gwMonUpdaterRow)
    {
      DateTime readingDate;
      Double readingHertz;
      Double headPsi;
      Double tempCelsius;

      readingDate = (DateTime)gwMonUpdaterRow.readingDate;
      readingHertz = (Double)gwMonUpdaterRow.readingHertz;
      headPsi = (Double)gwMonUpdaterRow.headPsi;
      tempCelsius = (Double)gwMonUpdaterRow.tempCelsius;

      try
      {
        //gwMonDataSet.GW_MONITORING.AddGW_MONITORINGRow(gwMonDataSet.SESSION.FindByedit_id(editSessionId),
        //  DateTime.Now, Environment.UserName, gwMonUpdaterRow.readingDate, 
        //  gwMonUpdaterRow.readingHertz, gwMonUpdaterRow.headPsi, 
        //  gwMonUpdaterRow.tempCelsius, true);
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running UpdateGwMon: " + ex.Message, "Error Running Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="status"></param>
    private void SetStatus(string status)
    {
      this.statusBarMain.Panels["status"].Text = status;
    }

    /// <summary>
    /// 
    /// </summary>
    private int SetProgress
    {
      get
      {
        return this.statusBarMain.Panels["progressBar"].ProgressBarInfo.Value;
      }
      set
      {
        this.statusBarMain.Panels["progressBar"].ProgressBarInfo.Value = value;
      }
    }
  
    /// <summary>
    /// 
    /// </summary>
    private void LoadGwMonGraphingData()
    {
      SetStatus("Loading groundwater monitor graphing data...");
      try
      {
        string sensorName = cbxMonitorList.Value.ToString();

        List<string> sensorNameList = new List<string>();
        sensorNameList.Add(sensorName);

        string[] sensNameArr = sensorNameList.ToArray();

        IEnumerable<string> sensorname = sensNameArr;

        GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter gwMonDataTA;
        gwMonDataTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter();
        gwMonDataTA.FillGwMonDataBySensorName(groundwaterMonitorDataSet.GW_MONITORING, sensorname);

        GroundwaterMonitorDataSetTableAdapters.MONITOR_LOCATIONSTableAdapter monLocationsTA;
        monLocationsTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.MONITOR_LOCATIONSTableAdapter();
        monLocationsTA.FillMonLocationBySensorName(groundwaterMonitorDataSet.MONITOR_LOCATIONS, sensorname);
        SetStatus("Nothing");
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running LoadGwMonGraphingData: " + ex.Message, "Error Loading Graphing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        SetStatus("Loading complete");
      }
    }

    /// <summary>
    /// 
    /// </summary>
    //private void LoadP1401GwGraphingData()
    //{
    //  try
    //  {
    //    string sensorName = cbxMonitorList.Value.ToString();
    //    GroundwaterMonitorDataSetTableAdapters.P1401TableAdapter p1401TA;
    //    p1401TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P1401TableAdapter();
    //    p1401TA.Fill(groundwaterMonitorDataSet.P1401);

    //    GroundwaterMonitorDataSetTableAdapters.MONITOR_LOCATIONSTableAdapter monLocationsTA;
    //    monLocationsTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.MONITOR_LOCATIONSTableAdapter();
    //    monLocationsTA.FillMonLocationBySensorName(groundwaterMonitorDataSet.MONITOR_LOCATIONS, sensorname);
    //  }

    //  catch (Exception ex)
    //  {
    //    MessageBox.Show("Error running LoadP1401GwGraphingData: " + ex.Message, "Error Loading P1401 Graphing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //  }
    //}
    
    /// <summary>
    /// 
    /// </summary>
    private void LoadGwMonUpdateData()
    {
      SetStatus("Loading groundwater monitor update data...");
      try
      {
        GroundwaterMonitorDataSetTableAdapters.GwMonUpdaterTableAdapter gwMonUpdaterTA;
        gwMonUpdaterTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.GwMonUpdaterTableAdapter();
        gwMonUpdaterTA.Fill(groundwaterMonitorDataSet.GwMonUpdater);

        //IEnumerable<string> sensorid = from g in groundwaterMonitorDataSet.GwMonUpdater select g.sensorId;

        //GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter gwMonTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter();
        //gwMonTA.FillBySensorId(groundwaterMonitorDataSet.GW_MONITORING, sensorid);
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error running LoadGwMonUpdateData: " + ex.Message, "Error Loading Update Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        SetStatus("Loading complete");
      }
      
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateMonitorList()
    {
      SqlConnection sqlCon = new SqlConnection(Properties.Settings.Default.GwMonitoringConnectionString);
      string sqlStr = "DELETE FROM [MonitorList]";
      SqlCommand sqlCmd = new SqlCommand(sqlStr,sqlCon);
      sqlCon.Open();
      sqlCmd.ExecuteNonQuery();
      sqlCon.Close();
    }

    private void ResetMonitorList()
    {
      SqlConnection sqlCon = new SqlConnection(Properties.Settings.Default.GwMonitoringConnectionString);
      string sqlStr = "INSERT INTO [MonitorList] " +
        "SELECT sensor_name FROM [MONITOR_LOCATIONS]";
      SqlCommand sqlCmd = new SqlCommand(sqlStr, sqlCon);
      sqlCon.Open();
      sqlCmd.ExecuteNonQuery();
      sqlCon.Close();
    }
    
    #endregion

    #region Events
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void frmMain_Load(object sender, EventArgs e)
    {
      // TODO: This line of code loads data into the 'groundwaterMonitorDataSet.MonitorList' table. You can move, or remove it, as needed.
      this.monitorListTableAdapter.Fill(this.groundwaterMonitorDataSet.MonitorList);
      SetStatus("Updating monitor list...");
      UpdateMonitorList();
      ResetMonitorList();
      SetStatus("Monitor list updated");
      // TODO: This line of code loads data into the 'ProjectDataSet.GwMonUpdater' table. You can move, or remove it, as needed.
      groundwaterMonitorDataSet.EnforceConstraints = false;
      
      chartGwData.Visible = false;
      //LoadMapControl();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void expBarMain_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
    {
      if(e.Item.Key.Equals("dataConnection"))
      {
        UpdateGwMonDataConnection();
      }
      else
      {
        LoadTab(e.Item.Key);
      }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnLoadGraph_Click(object sender, EventArgs e)
    {
      try
      {
        //clear existing data series
        chartGwData.Series.Clear();
        chartGwData.Visible = false;
        //object gwMonSelection = cbxMonitorList.Value;
        GraphGwMonData();
        //switch (gwMonSelection.ToString())
        //{
        //  case "P-14-01":
        //    GraphP1401(this);
        //    break;
        //  //case "P-14-02":
        //  //  GraphP1402();
        //  //  break;
        //  //case "P-45-01":
        //  //  GraphP4501();
        //  //  break;
        //  //case "P-45-02":
        //  //  GraphP4502();
        //  //  break;
        //  //case "P-45-03":
        //  //  GraphP4503();
        //  //  break;
        //  //case "P-45-04":
        //  //  GraphP4504();
        //  //  break;
        //  //case "P-45-05":
        //  //  GraphP4505();
        //  //  break;
        //  //case "P-52-01":
        //  //  GraphP5201();
        //  //  break;
        //  //case "P-52-02":
        //  //  GraphP5202();
        //  //  break;
        //  //case "P-52-03":
        //  //  GraphP5203();
        //  //  break;
        //  //case "P-52-04":
        //  //  GraphP5204();
        //  //  break;
        //  //case "TGD-01A":
        //  //  GraphTGD1A();
        //  //  break;
        //  //case "TGD-01B":
        //  //  GraphTGD1B();
        //  //  break;
        //  //case "TGD-02A":
        //  //  GraphTGD2A();
        //  //  break;
        //  //case "TGD-02B":
        //  //  GraphTGD2B();
        //  //  break;
        //  //case "TGD-03A":
        //  //  GraphTGD3A();
        //  //  break;
        //  //case "TGD-03B":
        //  //  GraphTGD3B();
        //    //break;
        //  default:
        //    MessageBox.Show("Invalid selection. Please select a monitor from the drop-down box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    break;
        //}
      }

      catch (Exception ex)
      {
        MessageBox.Show("Error loading chart: " + ex.Message, "Chart Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnClearChart_Click(object sender, EventArgs e)
    {
      chartGwData.Series.Clear();
      chartGwData.Visible = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnLoadUpdateFile_Click(object sender, EventArgs e)
    {
      SetStatus("Loading update file...");

      try
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
      
      catch (Exception ex)
      {
        MessageBox.Show("Error loading update file: " + ex.Message,"File Loading Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }    
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSubmitUpdates_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      
      SetStatus("Submitting updates...");
      
      if (!PrepareUpdateFile(txtUploadFilePath.Text))
      {
        tabControlMain.Tabs[2].Visible = true;
        LoadTab("updateFileErrors");
        return;
      }

      try
      {
        
        //TO-DO: implement UpdateGwMonData()
        //UpdateGwMonData();
        if (groundwaterMonitorDataSet.HasErrors)
        {
          groundwaterMonitorDataSet.RejectChanges();
          MessageBox.Show("Errors were found in the update table. No changes committed.", "Changes Rejected",MessageBoxButtons.OK,MessageBoxIcon.Error);
          return;
        }
        //TODO implement SaveGwMonData()
        //SaveGwMonData();
        SetStatus("Updates submitted");
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
        SetStatus("Ready");
        Cursor.Current = Cursors.Default;
        RestartUpdate();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancelUpdates_Click(object sender, EventArgs e)
    {
      RestartUpdate();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        case "dataConnection":
          UpdateGwMonDataConnection();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbxMonitorList_ValueChanged(object sender, EventArgs e)
    {
      if (cbxMonitorList.Value == null)
      {
        return;
      }
      txtGwMonDateRange.Text = "";
      //object gwMonSelection = cbxMonitorList.Value;
      CheckRange();
      //switch (gwMonSelection.ToString())
      //{
      //  case "P-14-01":
          //CheckRangeP1401();
      //    break;
      //  case "P-14-02":
      //    CheckRangeP1402();
      //    break;
      //  case "P-45-01":
      //    CheckRangeP4501();
      //    break;
      //  case "P-45-02":
      //    CheckRangeP4502();
      //    break;
      //  case "P-45-03":
      //    CheckRangeP4503();
      //    break;
      //  case "P-45-04":
      //    CheckRangeP4504();
      //    break;
      //  case "P-45-05":
      //    CheckRangeP4505();
      //    break;
      //  case "P-52-01":
      //    CheckRangeP5201();
      //    break;
      //  case "P-52-02":
      //    CheckRangeP5202();
      //    break;
      //  case "P-52-03":
      //    CheckRangeP5203();
      //    break;
      //  case "P-52-04":
      //    CheckRangeP5204();
      //    break;
      //  case "TGD-01A":
      //    CheckRangeTGD01A();
      //    break;
      //  case "TGD-01B":
      //    CheckRangeTGD01B();
      //    break;
      //  case "TGD-02A":
      //    CheckRangeTGD02A();
      //    break;
      //  case "TGD-02B":
      //    CheckRangeTGD02B();
      //    break;
      //  case "TGD-03A":
      //    CheckRangeTGD03A();
      //    break;
      //  case "TGD-03B":
      //    CheckRangeTGD03B();
      //    break;
      //  default:
      //    MessageBox.Show("Invalid selection. Please select a monitor from the drop-down box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //    break;
      //}
    }
    #endregion
  }
}

public static class GwMonErrors
{
  public const string ReadingDateConflict = "Conflicting Reading Date";
  public const string PiezoReadingError = "No Piezometer Reading";
}
