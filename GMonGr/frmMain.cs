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
using Microsoft.Office.Interop.Excel;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Assemblies;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace GMonGr
{
  public partial class frmMain : Form
  {
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
    /// Deletes all records from the MonitorList table in the GWMONITORING DB
    /// prepares for update of monitor list for populating cbxMonitorList, which
    /// is bound to the MonitorList table during FormLoad
    /// </summary>
    private void UpdateMonitorList()
    {
      SqlConnection sqlCon = new SqlConnection();
      SqlCommand sqlCmd = new SqlCommand();
      string sqlStr;

      try
      {
        SetStatus("Updating monitor list");
        sqlCon.ConnectionString = Properties.Settings.Default.GwMonitoringConnectionString.ToString();
        sqlStr = "DELETE FROM [MonitorList]";
        sqlCmd.CommandText = sqlStr.ToString();
        sqlCmd.Connection = sqlCon;
        sqlCon.Open();
        sqlCmd.ExecuteNonQuery();
        sqlCon.Close();
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running UpdateMonitorList: " + ex.Message, "Data Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Inserts all monitors to MonitorList table in GWMONITORING DB; this is the table
    /// that the cbxMonitorList control gets bound to during FormLoad
    /// </summary>
    private void ResetMonitorList()
    {
      SqlConnection sqlCon = new SqlConnection();
      SqlCommand sqlCmd = new SqlCommand();
      string sqlStr;

      try
      {
        SetStatus("Resetting monitor list...");
        sqlCon.ConnectionString = Properties.Settings.Default.GwMonitoringConnectionString.ToString();
        sqlStr = "INSERT INTO [MonitorList] " +
        "SELECT sensor_name FROM [MONITOR_LOCATIONS]";
        sqlCmd.CommandText = sqlStr.ToString();
        sqlCmd.Connection = sqlCon;
        sqlCon.Open();
        sqlCmd.ExecuteNonQuery();
        sqlCon.Close();
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running ResetMonitorList: " + ex.Message, "Data Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Sets status bar label during various events and methods
    /// </summary>
    /// <param name="status"></param>
    private void SetStatus(string status)
    {
      this.statusBarMain.Panels["status"].Text = status;
    }

    /// <summary>
    /// Sets progress bar's progress level for various events and methods
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
    /// method for access tabControlMain tabs for switching
    /// </summary>
    private void LoadTab(string tabKey)
    {
      try
      {
        Cursor = Cursors.WaitCursor;
        if (tabControlMain.Tabs[tabKey].Equals("flagData"))
        {
          tabControlMain.Tabs["flagData"].Visible = true;
          tabControlMain.SelectedTab = tabControlMain.Tabs[tabKey];
        }
        if (tabControlMain.Tabs[tabKey].Equals("updateHistory"))
        {
          tabControlMain.Tabs["updateHistory"].Visible = true;
          tabControlMain.SelectedTab = tabControlMain.Tabs[tabKey];
        }
        else
        {
          //tabControlMain.Tabs["flagData"].Visible = false;
          //tabControlMain.Tabs["updateHistory"].Visible = false;
          tabControlMain.SelectedTab = tabControlMain.Tabs[tabKey];
        }
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
    /// Resets the UI to the initial state for beginning the Update process
    /// </summary>
    private void RestartUpdate()
    {
      try
      {
        SetStatus("Restarting form");
        tabControlMain.SelectedTab = tabControlMain.Tabs[0];
        dgvDataUpdates.Visible = false;
        btnSubmitUpdates.Enabled = false;
        txtUploadFilePath.Clear();
        cbxMonitorList.Value = null;
        cbxUpdateMonitorList.Value = null;
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running RestartUpdate: " + ex.Message, "Form Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Obtains connection string info for display in status bar
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    private string ConnectionStringSummary(string connectionString)
    {
      string summary = "";
      try
      {
        System.Data.Common.DbConnectionStringBuilder csb;
        csb = new System.Data.Common.DbConnectionStringBuilder();
        csb.ConnectionString = connectionString;
        summary = csb["data source"].ToString();
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running ConnectionStringSummary: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
      return summary;
    }

    /// <summary>
    /// Opens connection string builder dialog window for viewing and changing connection string settings
    /// </summary>
    private void UpdateGwMonDataConnection()
    {
      try
      {
        SetStatus("Running connection update");
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

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running UpdateGwMonDataConnection: " + ex.Message, "Data Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
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
        SetStatus("Checking date range...");
        LoadGwMonGraphingData();

        System.Data.DataTable gwMonDt = new System.Data.DataTable();
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
      finally
      {
        SetStatus("Ready");
      }
    }
    
    /// <summary>
    /// Graphs selected/default date range for selected monitor
    /// Will be primary method for graphing from normalized groundwater monitor data table
    /// </summary>
    private void GraphGwMonData()
    {
      try
      {
        SetStatus("Graphing groundwater monitor data...");
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
  
        //set legend properties
        grndElSeries.Label = "Ground Elev. (ft)";
        maxGwElSeries.Label = "Max GW Elev. (ft)";
        gwElSeries.Label = "GW Elev. (ft)";
        minGwElSeries.Label = "Min GW Elev. (ft)";
        chartGwData.Legend.Location = LegendLocation.Bottom;
        chartGwData.Legend.Margins.Left = 3;
        chartGwData.Legend.Margins.Right = 3;
        chartGwData.Legend.Margins.Top = 3;
        chartGwData.Legend.Margins.Bottom = 3;
        chartGwData.Legend.SpanPercentage = 7;
        chartGwData.Legend.ChartComponent.Series.Add(grndElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(maxGwElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(gwElSeries);
        chartGwData.Legend.ChartComponent.Series.Add(minGwElSeries);
        chartGwData.Legend.BorderColor = Color.Black;
        chartGwData.Legend.Visible = true;
        
        //set chart properties
        chartGwData.Axis.X.Labels.SeriesLabels.Orientation = TextOrientation.Horizontal;
        chartGwData.Axis.X.Labels.SeriesLabels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
        chartGwData.Axis.X.Labels.Layout.Behavior = AxisLabelLayoutBehaviors.None;
        chartGwData.Axis.X.Labels.SeriesLabels.Visible = true;

        chartGwData.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
        chartGwData.TitleTop.Text = "Groundwater Monitor Time Series " + clndrGwMonStart.Value.ToShortDateString() + " - " + clndrGwMonEnd.Value.ToShortDateString() + " " + cbxMonitorList.Value.ToString() + " Piezometer";
        chartGwData.TitleLeft.HorizontalAlign = StringAlignment.Center;
        chartGwData.TitleLeft.Orientation = TextOrientation.VerticalLeftFacing;
        chartGwData.TitleLeft.Text = "Elevation (ft)";
        chartGwData.TitleLeft.Visible = true;
        chartGwData.TitleBottom.HorizontalAlign = StringAlignment.Center;
        chartGwData.TitleBottom.VerticalAlign = StringAlignment.Center;
        chartGwData.TitleBottom.Orientation = TextOrientation.Horizontal;
        chartGwData.TitleBottom.Text = "Monitor Reading Date";
        chartGwData.TitleBottom.OrientationAngle = 45;
        chartGwData.TitleBottom.Visible = true;
        chartGwData.TitleBottom.Visible = true;

        chartGwData.Axis.X.TickmarkStyle = AxisTickStyle.Smart;
        chartGwData.Axis.Y.TickmarkStyle = AxisTickStyle.Smart;
        chartGwData.Axis.Y.TickmarkIntervalType = AxisIntervalType.Days;
        chartGwData.Axis.Y.TickmarkInterval = 1;

        chartGwData.Visible = true;
        chartGwData.Refresh();        
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running GraphGwMonData: " + ex.Message, "Error Graphing Time Series", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        SetStatus("Ready");
      }
    }

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
        SetStatus("Loading numeric time series collection");
        Dictionary<string, System.Data.DataTable> gwDataTableDict = GetGwDataCollection();
        NumericTimeSeries grndElSeries = new NumericTimeSeries();
        NumericTimeSeries maxGwElSeries = new NumericTimeSeries();
        NumericTimeSeries gwElSeries = new NumericTimeSeries();
        NumericTimeSeries minGwElSeries = new NumericTimeSeries();

        System.Data.DataTable gwDt = gwDataTableDict["gwDataDt"];
        EnumerableRowCollection gwDataEnumRowColl = gwDt.AsEnumerable();

        foreach (DataRow grndElDr in gwDataEnumRowColl)
        {
          grndElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(grndElDr.ItemArray[5].ToString()), System.Double.Parse(grndElDr.ItemArray[14].ToString()), "", false));
        }

        foreach (DataRow maxGwElDr in gwDataEnumRowColl)
        {
          maxGwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(maxGwElDr.ItemArray[5].ToString()), System.Double.Parse(maxGwElDr.ItemArray[15].ToString()), "", false));
        }

        foreach (DataRow gwElDr in gwDataEnumRowColl)
        {
          gwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(gwElDr.ItemArray[5].ToString()), System.Double.Parse(gwElDr.ItemArray[11].ToString()), String.Format("{0:M/d/yyyy}", gwElDr.ItemArray[5]), false));
        }

        foreach (DataRow minGwElDr in gwDataEnumRowColl)
        {
          minGwElSeries.Points.Add(new NumericTimeDataPoint(System.DateTime.Parse(minGwElDr.ItemArray[5].ToString()), System.Double.Parse(minGwElDr.ItemArray[16].ToString()), "", false));
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

      finally
      {
        SetStatus("Ready");
      }
      return numTimeSeriesDict;
    }

    /// <summary>
    /// Gets main groundwater data collection for returning to the GetNumericTimeSeries method
    /// </summary>
    private Dictionary<string, System.Data.DataTable> GetGwDataCollection()
    {
      Dictionary<string, System.Data.DataTable> dataTableDict = new Dictionary<string, System.Data.DataTable>();

      try
      {
        SetStatus("Loading groundwater data collection");
        LoadGwMonGraphingData();
        
        double grndElFt = 0;
        double maxElFt = 0;
        double minElFt = 0;
        
        System.Data.DataTable grndElDt = new System.Data.DataTable();
        System.Data.DataTable gwDataDt = new System.Data.DataTable();
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
    /// sets calendar start date for graphing methods
    /// </summary>
    public DateTime SetCalendarStartSafe()
    {    
      DateTime startDateSafe;
      startDateSafe = clndrGwMonStart.Value;
      return startDateSafe;
    }

    /// <summary>
    /// sets calendar end date for graphing methods
    /// </summary>
    public DateTime SetCalendarEndSafe()
    {
      DateTime endDateSafe;
      endDateSafe = clndrGwMonEnd.Value;
      return endDateSafe;
    }

    /// <summary>
    /// sets calendar start date string for graphing methods
    /// </summary>
    public string SetCalendarStartStringSafe()
    {
      string startDateStringSafe = "";
      startDateStringSafe = clndrGwMonStart.Value.ToShortDateString();
      return startDateStringSafe;
    }

    /// <summary>
    /// sets calendar end date string for graphing methods
    /// </summary>
    public string SetCalendarEndStringSafe()
    {
      string endDateStringSafe = "";
      endDateStringSafe = clndrGwMonEnd.Value.ToShortDateString();
      return endDateStringSafe;
    }
    
    /// <summary>
    /// Invokes WriteGwMonUpdateFile method and runs QC prior to submitting updates
    /// </summary>
    private bool PrepareUpdateFile(string fileName)
    {
      int qcCount = 0;
      try
      {
        SetProgress = 0;
        SetStatus("Preparing update file");
        WriteGwMonUpdateFile(fileName);
        LoadGwMonUpdateData();

        groundwaterMonitorDataSet.GwMonQc.Clear();
        qcCount += QCQueryTimeConflict(groundwaterMonitorDataSet.GwMonQc);
        SetProgress = 50;
        qcCount += QCQueryPiezoReadingError(groundwaterMonitorDataSet.GwMonQc);
        SetProgress = 100;
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running PrepareUpdateFile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetProgress = 0;
        SetStatus("Ready");
      }
      return qcCount == 0;
    }

    #region DataUpdateQC
    /// <summary>
    /// Queries incoming update table against main data to make sure readings
    /// with duplicate timestamps are not appended
    /// </summary>
    private int QCQueryTimeConflict(System.Data.DataTable qcDt)
    {
      int qcCount = 0;
      try
      {
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

        //Debug message
        //MessageBox.Show("Leaving QC 1-TimeRange");

        //TO-DO: update GWMonDataSet to include GWMonUpdate datatable
        foreach (var row in qryTimeRangeGwMon)
        {
          qcDt.Rows.Add(row.readingDate, row.errorCode, row.errorDescription);
          qcCount++;
        }
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running QCQueryTimeConflict: " + ex.Message, "QC Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
      return qcCount;
    }

    /// <summary>
    /// Queries incoming update table to make sure valid monitor readings are present
    /// </summary>
    private int QCQueryPiezoReadingError(System.Data.DataTable qcDt)
    {
      int qcCount = 0;
      try
      {
        SetStatus("Running QC query");
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

        //Debug message
        //MessageBox.Show("Leaving QC2-PiezoError");

        foreach (var row in qryPiezoError)
        {
          qcDt.Rows.Add(row.readingDate, row.errorCode, row.errorDescription);
          qcCount++;
        }
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running QCQueryPiezoReadingError: " + ex.Message, "QC Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
      return qcCount;
    }
    #endregion
    
    /// <summary>
    /// Loads and copies user update file
    /// </summary>
    private void WriteGwMonUpdateFile(string fileName)
    {
      try
      {
        SetStatus("Writing groundwater monitor update file");
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
        SetStatus("Error encountered");
        MessageBox.Show("Error running WriteGwMonUpdateFile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Loads data from GWMONITORING DB for use in graphing functions GW_MONITORING and MONITOR_LOCATIONS tables
    /// </summary>
    private void LoadGwMonGraphingData()
    {
      string sensorName;
      List<string> sensorNameList = new List<string>();
      string[] sensNameArr;
      IEnumerable<string> sensorNameEnum;

      try
      {
        SetStatus("Loading groundwater monitor graphing data...");
        sensorName = cbxMonitorList.Value.ToString();
        sensorNameList.Add(sensorName);
        sensNameArr = sensorNameList.ToArray();
        sensorNameEnum = sensNameArr;

        GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter gwMonDataTA;
        gwMonDataTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter();
        gwMonDataTA.FillGwMonDataBySensorName(groundwaterMonitorDataSet.GW_MONITORING, sensorNameEnum);

        GroundwaterMonitorDataSetTableAdapters.MONITOR_LOCATIONSTableAdapter monLocationsTA;
        monLocationsTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.MONITOR_LOCATIONSTableAdapter();
        monLocationsTA.FillMonLocationBySensorName(groundwaterMonitorDataSet.MONITOR_LOCATIONS, sensorNameEnum);
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running LoadGwMonGraphingData: " + ex.Message, "Error Loading Graphing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Loads groundwater monitor data from GWMONITORING DB for use in update processes, including GW_MONITORING and SESSION tables
    /// </summary>
    private void LoadGwMonUpdateData()
    {
      try
      {
        SetStatus("Loading groundwater monitor update data...");

        GroundwaterMonitorDataSetTableAdapters.GwMonUpdaterTableAdapter gwMonUpdaterTA;
        gwMonUpdaterTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.GwMonUpdaterTableAdapter();
        gwMonUpdaterTA.Fill(groundwaterMonitorDataSet.GwMonUpdater);

        GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter gwMonTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter();
        gwMonTA.Fill(groundwaterMonitorDataSet.GW_MONITORING);

        GroundwaterMonitorDataSetTableAdapters.SESSIONTableAdapter sessionTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.SESSIONTableAdapter();
        sessionTA.Fill(groundwaterMonitorDataSet.SESSION);
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running LoadGwMonUpdateData: " + ex.Message, "Error Loading Update Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Loads data from GWMONITORING DB for use in data updates, including SESSION table
    /// </summary>
    private void LoadGwMonSessionData()
    {
      try
      {
        SetStatus("Loading groundwater monitor session data...");
        GroundwaterMonitorDataSetTableAdapters.SESSIONTableAdapter gwMonSessionTA;
        gwMonSessionTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.SESSIONTableAdapter();
        gwMonSessionTA.Fill(groundwaterMonitorDataSet.SESSION);
      }

      catch (Exception ex)
      {
        SetStatus("Error Encountered");
        MessageBox.Show("Error running LoadGwMonSessionData: " + ex.Message, "Error Loading Session Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Loads groundwater monitor locations table, MONITOR_LOCATIONS
    /// </summary>
    private void LoadGwMonLocationsData()
    {
      try
      {
        SetStatus("Loading groundwater monitor locations data...");
        GroundwaterMonitorDataSetTableAdapters.MONITOR_LOCATIONSTableAdapter gwMonLocationsTA;
        gwMonLocationsTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.MONITOR_LOCATIONSTableAdapter();
        gwMonLocationsTA.Fill(groundwaterMonitorDataSet.MONITOR_LOCATIONS);
      }

      catch (Exception ex)
      {
        SetStatus("Error Encountered");
        MessageBox.Show("Error running LoadGwMonLocationsData: " + ex.Message, "Error Loading Monitor Location Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Obtains the current edit_id value and increments to next edit_id value for use in data update of SESSION table
    /// </summary>
    /// <returns></returns>
    private int GetSessionEditId()
    {
      int maxEditId;
      int sessionEditId = 0;
      System.Data.DataTable sessionEditDt;

      try
      {
        LoadGwMonSessionData();
        sessionEditDt = groundwaterMonitorDataSet.SESSION;

        EnumerableRowCollection<DataRow> qrySelectCurrentSessionEditId =
          (from s in sessionEditDt.AsEnumerable()
           select s);

        if (qrySelectCurrentSessionEditId.Count() > 0)
        {
          maxEditId = qrySelectCurrentSessionEditId.Max(q => q.Field<int>("edit_id"));
          sessionEditId = maxEditId + 1;
        }

        else
        {
          sessionEditId = 1;
        }
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered...");
        MessageBox.Show("Error running GetSessionEditId: " + ex.Message, "Error Getting Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
      return sessionEditId;
    }
    
    /// <summary>
    /// Runs update of table that will get appended to GW_MONITORING; final step before updates are committted
    /// </summary>
    /// <param name="editSessionId"></param>
    /// <param name="gwMonUpdaterRow"></param>
    private void UpdateGwMon(int editSessionId, string sensorName, GroundwaterMonitorDataSet.GwMonUpdaterRow gwMonUpdaterRow)
    {  
      try
      {
        SetStatus("Running groundwater monitor update...");

        DateTime readingDate;
        Double readingHertz;
        Double headPsi;
        Double headFt;
        Double tempCelsius;
        Double gwDepthFt;
        Double gwElevFt;
        
        readingDate = (DateTime)gwMonUpdaterRow.readingDate;
        readingHertz = (Double)gwMonUpdaterRow.readingHertz;
        headPsi = (Double)gwMonUpdaterRow.headPsi;
        headFt = (Double)gwMonUpdaterRow.headFt;
        tempCelsius = (Double)gwMonUpdaterRow.tempCelsius;
        gwDepthFt = (Double)gwMonUpdaterRow.gwDepthFt;
        gwElevFt = (Double)gwMonUpdaterRow.gwElevFt;

        groundwaterMonitorDataSet.GW_MONITORING.AddGW_MONITORINGRow(
        editSessionId,
        DateTime.Now,
        (Environment.UserDomainName + "\\" + Environment.UserName),
        sensorName,
        readingDate,
        readingHertz,
        headPsi,
        headFt,
        tempCelsius,
        gwDepthFt,
        gwElevFt,
        "+"
        );
        
        return;
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running UpdateGwMon: " + ex.Message, "Error Running Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Updates GwMonUpdater table with required field values for input to main data table, GW_MONITORING
    /// </summary>
    private void UpdateGwMonTables()
    {
      int editSessionId = GetSessionEditId();
      string sensorName = "";
      string f2ScaleStr = "";
      Double readingHertz;
      Double headPsi;
      Double headFt;
      Double f2Scale;
      Double calFactPsiA;
      Double calFactPsiB;
      Double calFactPsiC;
      Double calFactHeadA;
      Double calFactHeadB;
      Double calFactHeadC;
      Double measureDownFt;
      Double sensorDepthFt;
      Double tocElevFt;
      Double gwDepthFt;
      Double gwElevFt;
        
      try
      {
        SetStatus("Updating groundwater monitor tables...");

        LoadGwMonLocationsData();
        
        sensorName = cbxUpdateMonitorList.Value.ToString();
       
        var qryMonLocations =
          (from l in groundwaterMonitorDataSet.MONITOR_LOCATIONS
          where l.sensor_name == sensorName
          select new
          {
            MeasureDownFt = l.measure_down_ft,
            SensorDepthFt = l.sensor_depth_ft,
            TocElevFt = l.toc_elev_ft,
            CalFactPsiA = l.cal_fact_psi_a,
            CalFactPsiB = l.cal_fact_psi_b,
            CalFactPsiC = l.cal_fact_psi_c,
            CalFactHeadA = l.cal_fact_head_ft_a,
            CalFactHeadB = l.cal_fact_head_ft_b,
            CalFactHeadC = l.cal_fact_head_ft_c,
            F2Scale = l.f2_scale
          }).First();

        measureDownFt = qryMonLocations.MeasureDownFt;
        sensorDepthFt = qryMonLocations.SensorDepthFt;
        tocElevFt = qryMonLocations.TocElevFt;
        calFactPsiA = qryMonLocations.CalFactPsiA;
        calFactPsiB = qryMonLocations.CalFactPsiB;
        calFactPsiC = qryMonLocations.CalFactPsiC;
        calFactHeadA = qryMonLocations.CalFactHeadA;
        calFactHeadB = qryMonLocations.CalFactHeadB;
        calFactHeadC = qryMonLocations.CalFactHeadC;
        f2ScaleStr = qryMonLocations.F2Scale;

        if (f2ScaleStr == "T")
        {
          foreach (GroundwaterMonitorDataSet.GwMonUpdaterRow gwMonUpdaterRow in groundwaterMonitorDataSet.GwMonUpdater)
          {
            readingHertz = gwMonUpdaterRow.Field<Double>("readingHertz");
            f2Scale = ((Math.Pow((readingHertz / 1000),2)) * 1000);
            headPsi = ((Math.Pow(f2Scale, 2)) * (calFactPsiA)) + (f2Scale * calFactPsiB) + calFactPsiC;
            headFt = ((Math.Pow(f2Scale, 2)) * (calFactHeadA)) + (f2Scale * calFactHeadB) + calFactHeadC;
            gwDepthFt = -(headFt + sensorDepthFt + measureDownFt);
            gwElevFt = tocElevFt - (-(headFt + sensorDepthFt + measureDownFt));
            
            gwMonUpdaterRow.SetField<Double>("headPsi", headPsi);
            gwMonUpdaterRow.SetField<Double>("headFt", headFt);
            gwMonUpdaterRow.SetField<Double>("gwDepthFt", gwDepthFt);
            gwMonUpdaterRow.SetField<Double>("gwElevFt", gwElevFt);
            gwMonUpdaterRow.SetField<Double>("f2Scale", f2Scale);

            UpdateGwMon(editSessionId, sensorName, gwMonUpdaterRow);
          }
        }
        else
        {
          foreach (GroundwaterMonitorDataSet.GwMonUpdaterRow gwMonUpdaterRow in groundwaterMonitorDataSet.GwMonUpdater)
          {
            readingHertz = gwMonUpdaterRow.Field<Double>("readingHertz");
            headPsi = ((Math.Pow(readingHertz, 2)) * (calFactPsiA)) + (readingHertz * calFactPsiB) + calFactPsiC;
            headFt = ((Math.Pow(readingHertz, 2)) * (calFactHeadA)) + (readingHertz * calFactHeadB) + calFactHeadC;
            gwDepthFt = -(headFt + sensorDepthFt + measureDownFt);
            gwElevFt = tocElevFt - (-(headFt + sensorDepthFt + measureDownFt));
            
            gwMonUpdaterRow.SetField<Double>("headPsi", headPsi);
            gwMonUpdaterRow.SetField<Double>("headFt", headFt);
            gwMonUpdaterRow.SetField<Double>("gwDepthFt", gwDepthFt);
            gwMonUpdaterRow.SetField<Double>("gwElevFt", gwElevFt);
            
            UpdateGwMon(editSessionId, sensorName, gwMonUpdaterRow);
          }
        }
        
        groundwaterMonitorDataSet.SESSION.AddSESSIONRow
          (DateTime.Now,
           (Environment.UserDomainName + "\\" + Environment.UserName),
           sensorName);

        return;
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running UpdateGwMonTables: " + ex.Message, "Error Updating Gw Monitor Table", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }     
    }

    /// <summary>
    /// Updates edit_id value for the SESSION table data update
    /// </summary>
    /// <returns>
    /// editId
    /// </returns>
    private int UpdateEditSession()
    {
      int editId = GetSessionEditId();
      GroundwaterMonitorDataSet.SESSIONRow sessionRow =
        groundwaterMonitorDataSet.SESSION.AddSESSIONRow
        (DateTime.Now, Environment.UserDomainName+ "\\" +Environment.UserName,"");
      
      return sessionRow.edit_id;
    }

    /// <summary>
    /// Commits user update changes 
    /// </summary>
    private void SaveGwMonData()
    {
      try
      {
        SetStatus("Saving groundwater monitor data");
        GroundwaterMonitorDataSet changedGroundwaterMonitorDataSet = (GroundwaterMonitorDataSet)groundwaterMonitorDataSet.GetChanges();

        GroundwaterMonitorDataSetTableAdapters.GwMonUpdaterTableAdapter gwMonUpdaterTA;
        gwMonUpdaterTA = new GroundwaterMonitorDataSetTableAdapters.GwMonUpdaterTableAdapter();
   
        GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter gwMonTA = new GroundwaterMonitorDataSetTableAdapters.GW_MONITORINGTableAdapter();
        gwMonTA.Update(changedGroundwaterMonitorDataSet.GW_MONITORING);

        GroundwaterMonitorDataSetTableAdapters.SESSIONTableAdapter sessionTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.SESSIONTableAdapter();
        sessionTA.Update(changedGroundwaterMonitorDataSet.SESSION);
      }

      catch (Exception ex)
      {
        SetStatus("Error encountered");
        MessageBox.Show("Error running SaveGwMonData: " + ex.Message, "Error Updating Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      finally
      {
        SetStatus("Ready");
      }
    }

    /// <summary>
    /// Primary method for flagging data specified by the user as not being valid for graphing and/or analysis
    /// </summary>
    private void FlagGwMonData()
    {
      throw new NotImplementedException();
    }

    #region Export To Excel
    //public virtual event EventHandler WorkStart;
    //public virtual event EventHandler WorkFinished;

    //public void OnWorkStart(object sender, EventArgs e)
    //{
    //  if (WorkStart != null) { WorkStart(sender, e); }
    //}

    //public void OnWorkFinished(object sender, EventArgs e)
    //{
    //  if (WorkFinished != null) { WorkFinished(sender, e); }
    //}  
    
    ///// <summary>  
    ///// Manages a new thread used to Export from DataGridView to an Excel worksheet.  
    ///// This method must be used in combination with the methods:  
    ///// GetExcelReady()  
    ///// ToExcel()  
    ///// </summary>  
    ///// <param name="dGV">ExtendedDataGridView name.</param>  
    ///// <param name="initialRow">Number of Excel row where to start copying DataGridView titles.</param>  
    ///// <param name="initialCol">Number of Excel column where to start copying DataGridView titles.</param>  
    ///// <param name="exportTitles">True if you want to export DGV titles.</param>  
    ///// <param name="wksName">How do you want to name the new worksheet.</param>  
    //public void ExcelThread(DataGridView dGV, int initialRow, int initialCol, bool exportTitles, string wksName)
    //{
    //  Thread t1 = new Thread
    //      (
    //      delegate()
    //      {
    //        OnWorkStart(dGV, new EventArgs());

    //        // If exportTitles is set to false, change initial row value.  
    //        if (!exportTitles) { initialRow = initialRow - 1; }

    //        // Export Data.  
    //        GetExcelReady(dGV, initialRow, initialCol, exportTitles, wksName);

    //        OnWorkFinished(dGV, new EventArgs());
    //      }
    //      );
    //  t1.Start();
    //}

    ///// <summary>  
    ///// Sets a new Excel object where to export DataGridView.  
    ///// This method should be used with the methods:  
    ///// ExcelThread().  
    ///// ToExcel().  
    ///// </summary>  
    ///// <param name="dGV">ExtendedDataGridView name.</param>  
    ///// <param name="initialRow">Number of Excel row where to start copying DataGridView titles.</param>  
    ///// <param name="initialCol">Number of Excel column where to start copying DataGridView titles.</param>  
    ///// <param name="exportTitles">True if you want to export DGV titles.</param>  
    ///// <param name="wksName">How do you want to name the new worksheet.</param>  
    //void GetExcelReady(DataGridView dGV, int initialRow, int initialCol, bool exportTitles, string wksName)
    //{
    //  // Declare missing object.  
    //  Object oMissing = System.Reflection.Missing.Value;

    //  // Change current thread culture to ("en-US").  
    //  // System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");  

    //  // Create a new Excel instance.  
    //  Excel.Application oExcel = new Excel.Application();

    //  // Set Excel workbook to open with only 1 worsheet.  
    //  oExcel.SheetsInNewWorkbook = 1;

    //  // Set the UserControl property so Excel won't shut down.  
    //  oExcel.UserControl = true;

    //  // Add a workbook.  
    //  Excel.Workbook oBook = oExcel.Workbooks.Add(oMissing);

    //  // Get worksheets collection   
    //  Excel.Sheets oSheetsColl = oExcel.Worksheets;

    //  // Get Worksheet number 1  
    //  Excel.Worksheet oSheet = (Excel.Worksheet)oSheetsColl.get_Item(1);
    //  oSheet.Name = wksName;

    //  // Export dGV columns To Excel worksheet.  
    //  ToExcel(dGV, initialRow, initialCol, exportTitles, oSheet);

    //  // Make Excel visible to the user.  
    //  oExcel.Visible = true;

    //  // Release the variables.  
    //  //oBook.Close(false, oMissing, oMissing);  
    //  oBook = null;

    //  //oExcel.Quit();  
    //  oExcel = null;

    //  // Collect garbage.  
    //  GC.Collect();
    //}

    ///// <summary>  
    ///// Export from DataGridView to Excel worksheet.  
    ///// This method should be used in combination with the methods:  
    ///// ExcelThread().  
    ///// GetExcelReady().  
    ///// </summary>  
    ///// <param name="dGV">ExtendedDataGridView name.</param>  
    ///// <param name="initialRow">Number of Excel row where to start copying DataGridView titles.</param>  
    ///// <param name="initialCol">Number of Excel column where to start copying DataGridView titles.</param>  
    ///// <param name="exportTitles">True if you want to export DGV titles.</param>  
    ///// <param name="wksName">How do you want to name the new worksheet.</param>  
    ///// <param name="oSheet"></param>  
    //void ToExcel(DataGridView dGV, int initialRow, int initialCol, bool exportTitles, Microsoft.Office.Interop.Excel.Worksheet oSheet)
    //{
    //  int colIndex = 0;
    //  foreach (DataGridViewColumn column in dGV.Columns)
    //  {
    //    // Export only visible columns.  
    //    if (dGV.ExportVisibleColumnsOnly)
    //    {
    //      if (column.Visible)
    //      {
    //        // Export.  
    //        if (exportTitles) { oSheet.Cells[initialRow, colIndex + initialCol] = column.HeaderText; }
    //        for (int row = initialRow; row < initialRow + dGV.Rows.Count - 1; row++)
    //        { oSheet.Cells[row + 1, colIndex + initialCol] = dGV[colIndex, row - initialRow].Value; }
    //        colIndex++;
    //      }
    //    }
    //    else
    //    {
    //      // Export all columns.  
    //      if (exportTitles) { oSheet.Cells[initialRow, colIndex + initialCol] = column.HeaderText; }
    //      for (int row = initialRow; row < initialRow + dGV.Rows.Count - 1; row++)
    //      { oSheet.Cells[row + 1, colIndex + initialCol] = dGV[colIndex, row - initialRow].Value; }
    //      colIndex++;
    //    }
    //  }
    //}
    #endregion  

    #endregion

    #region Events

    /// <summary>
    /// Form load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void frmMain_Load(object sender, EventArgs e)
    {
      // TODO: This line of code loads data into the 'groundwaterMonitorDataSet.SESSION' table. You can move, or remove it, as needed.
      this.sESSIONTableAdapter.Fill(this.groundwaterMonitorDataSet.SESSION);
      // TODO: This line of code loads data into the 'groundwaterMonitorDataSet.GwMonUpdater' table. You can move, or remove it, as needed.
      //this.gwMonUpdaterTableAdapter.Fill(this.groundwaterMonitorDataSet.GwMonUpdater);
      // TODO: This line of code loads data into the 'groundwaterMonitorDataSet.MonitorList' table. You can move, or remove it, as needed.
      this.monitorListTableAdapter.Fill(this.groundwaterMonitorDataSet.MonitorList);
      UpdateMonitorList();
      ResetMonitorList();
      // TODO: This line of code loads data into the 'ProjectDataSet.GwMonUpdater' table. You can move, or remove it, as needed.
      groundwaterMonitorDataSet.EnforceConstraints = false;     
      chartGwData.Visible = false;
      //LoadMapControl();
    }

    /// <summary>
    /// Explorer main click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void expBarMain_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
    {
      if (e.Item.Key.Equals("dataConnection"))
      {
        UpdateGwMonDataConnection();
      }
      else
      {
        LoadTab(e.Item.Key);
      }
    }
    
    /// <summary>
    /// Load Button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnLoadGraph_Click(object sender, EventArgs e)
    {
        //clear existing data series
        chartGwData.Series.Clear();
        chartGwData.Visible = false;
        GraphGwMonData();
        btnExportGraphData.Visible = true;
    }

    /// <summary>
    /// Clear graph button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnClearChart_Click(object sender, EventArgs e)
    {
      chartGwData.Series.Clear();
      chartGwData.Visible = false;
      btnExportGraphData.Visible = false;
    }

    /// <summary>
    /// Load update button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnLoadUpdateFile_Click(object sender, EventArgs e)
    {
      SetStatus("Loading...");  
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
      dgvDataUpdates.Visible = true;      
      SetStatus("Ready");
    }
    
    /// <summary>
    /// Submit updates button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSubmitUpdates_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;

      if (!PrepareUpdateFile(txtUploadFilePath.Text))
      {
        tabControlMain.Tabs["updateFileErrors"].Visible = true;
        LoadTab("updateFileErrors");
        return;
      }

      UpdateGwMonTables();

      if (groundwaterMonitorDataSet.HasErrors)
      {
        groundwaterMonitorDataSet.RejectChanges();
        MessageBox.Show("Errors were found in the update table. No changes committed.", "Changes Rejected", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      SaveGwMonData();
      MessageBox.Show("All updates to the groundwater monitoring system have completed successfully." + "\n"
      + "To review changes from this edit session, return to the main page, and click on the 'Load Update History' button to load the desired edit session.",
      "GMonGr: Update Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

      Cursor.Current = Cursors.Default;
      RestartUpdate();
    }

    /// <summary>
    /// Cancel updates button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancelUpdates_Click(object sender, EventArgs e)
    {
      RestartUpdate();
    }

    /// <summary>
    /// Main toolbar manager tool click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void toolBarManagerMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
    {
      string fi;

      switch (e.Tool.Key)
      {
        case "aboutProgram":
          fi = @"\\Cassio\asm_apps\Apps\GMonGr\publish.htm";
          System.Diagnostics.Process.Start("IEXPLORE.EXE", fi);
          return;
        default:
          break;
      }

      switch (e.Tool.Key)
      {
        case "graphData":
          LoadTab("graphData");
          return;
        default:
          break;
      }
      switch (e.Tool.Key)
      {
        case "loadUpdate":
          LoadTab("loadUpdates");
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
        case "flagData":
          LoadTab("flagData");
          return;
        default:
          break;
      }
      switch (e.Tool.Key)
      {
        case "updateHistory":
          LoadTab("updateHistory");
          return;
        default:
          break;
      }
      switch (e.Tool.Key)
      {
        case "monitorMap":
          LoadTab("monitorMap");
          return;
        default:
          break;
      }
      switch (e.Tool.Key)
      {
        case "exitProgram":
          System.Windows.Forms.Application.Exit();
          return;
        default:
          MessageBox.Show("Tool '" + e.Tool.Key + "' not implemented");
          return;
      }
    }

    /// <summary>
    /// Monitor list (graph data tab) combo-box value changed event
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
      CheckRange();
    }

    /// <summary>
    /// Monitor list (data updates tab) combo-box value changed event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbxUpdateMonitorList_ValueChanged(object sender, EventArgs e)
    {
      if (cbxUpdateMonitorList.Value == null)
      {
        return;
      }
      else
      {
        txtUploadFilePath.Enabled = true;
        btnLoadUpdateFile.Enabled = true;
      }
    }    
    
    /// <summary>
    /// Cancel update button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCancelUpdateErrors_Click(object sender, EventArgs e)
    {
      RestartUpdate();
    }

    /// <summary>
    /// Export update errors button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnExportUpdateErrors_Click(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Export graph data button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnExportGraphData_Click(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Flag groundwater monitor data button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnFlagGwMonData_Click(object sender, EventArgs e)
    {
      string selectedMonitor;
      selectedMonitor = cbxFlagMonitorDataList.Value.ToString();
      string flagDataRangeStart;
      flagDataRangeStart = clndrFlagDataRangeStart.Value.ToShortDateString();
      string flagDataRangeEnd;
      flagDataRangeEnd = clndrFlagDataRangeEnd.Value.ToShortDateString();
      DialogResult dialResult = MessageBox.Show("Flag data for " + selectedMonitor + " monitor" + " from " + flagDataRangeStart + " to " + flagDataRangeEnd + "?", "Confirm Data Flagging Operation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

      if (dialResult != DialogResult.OK)
      {
        return;
      }

      FlagGwMonData();

      //throw new NotImplementedException();
    }

    #endregion
  }
}

public static class GwMonErrors
{
  public const string ReadingDateConflict = "Conflicting Reading Date";
  public const string PiezoReadingError = "No Piezometer Reading";
}
