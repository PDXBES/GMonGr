using System;
using System.Data.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;

//namespace GMonGr
namespace GMonGr 
{    
    public partial class GroundwaterMonitorDataSet 
    {
      partial class GW_MONITORINGDataTable
      {
      }

      partial class MONITOR_LOCATIONSDataTable
      {
      }
    
      partial class P1402DataTable
      {
      }

      partial class P4501DataTable
      {
      }
      
      partial class P4502DataTable
      {
      }    
    
      partial class GwMonUpdaterDataTable
      {
      }
    
      public void InitTableP1401()
      {
        GroundwaterMonitorDataSetTableAdapters.P1401TableAdapter p1401TA;
        p1401TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P1401TableAdapter();
        p1401TA.Fill(P1401);
      }

      public void InitTableP1402()
      {
        GroundwaterMonitorDataSetTableAdapters.P1402TableAdapter p1402TA;
        p1402TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P1402TableAdapter();
        p1402TA.Fill(P1402);
      }

      public void InitTableP4501()
      {
        GroundwaterMonitorDataSetTableAdapters.P4501TableAdapter p4501TA;
        p4501TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P4501TableAdapter();
        p4501TA.Fill(P4501);
      }

      public void InitTableP4502()
      {
        GroundwaterMonitorDataSetTableAdapters.P4502TableAdapter p4502TA;
        p4502TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P4502TableAdapter();
        p4502TA.Fill(P4502);
      }

      public void InitTableP4503()
      {
        GroundwaterMonitorDataSetTableAdapters.P4503TableAdapter p4503TA;
        p4503TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P4503TableAdapter();
        p4503TA.Fill(P4503);
      }

      public void InitTableP4504()
      {
        GroundwaterMonitorDataSetTableAdapters.P4504TableAdapter p4504TA;
        p4504TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P4504TableAdapter();
        p4504TA.Fill(P4504);
      }

      public void InitTableP4505()
      {
        GroundwaterMonitorDataSetTableAdapters.P4505TableAdapter p4505TA;
        p4505TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P4505TableAdapter();
        p4505TA.Fill(P4505);
      }

      public void InitTableP5201()
      {
        GroundwaterMonitorDataSetTableAdapters.P5201TableAdapter p5201TA;
        p5201TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P5201TableAdapter();
        p5201TA.Fill(P5201);
      }

      public void InitTableP5202()
      {
        GroundwaterMonitorDataSetTableAdapters.P5202TableAdapter p5202TA;
        p5202TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P5202TableAdapter();
        p5202TA.Fill(P5202);
      }

      public void InitTableP5203()
      {
        GroundwaterMonitorDataSetTableAdapters.P5203TableAdapter p5203TA;
        p5203TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P5203TableAdapter();
        p5203TA.Fill(P5203);
      }

      public void InitTableP5204()
      {
        GroundwaterMonitorDataSetTableAdapters.P5204TableAdapter p5204TA;
        p5204TA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.P5204TableAdapter();
        p5204TA.Fill(P5204);
      }

      public void InitTableTGD1A()
      {
        GroundwaterMonitorDataSetTableAdapters.TGD1ATableAdapter tgd1ATA;
        tgd1ATA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.TGD1ATableAdapter();
        tgd1ATA.Fill(TGD1A);
      }

      public void InitTableTGD1B()
      {
        GroundwaterMonitorDataSetTableAdapters.TGD1BTableAdapter tgd1BTA;
        tgd1BTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.TGD1BTableAdapter();
        tgd1BTA.Fill(TGD1B);
      }

      public void InitTableTGD2A()
      {
        GroundwaterMonitorDataSetTableAdapters.TGD2ATableAdapter tgd2ATA;
        tgd2ATA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.TGD2ATableAdapter();
        tgd2ATA.Fill(TGD2A);
      }

      public void InitTableTGD2B()
      {
        GroundwaterMonitorDataSetTableAdapters.TGD2BTableAdapter tgd2BTA;
        tgd2BTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.TGD2BTableAdapter();
        tgd2BTA.Fill(TGD2B);
      }

      public void InitTableTGD3A()
       {
         GroundwaterMonitorDataSetTableAdapters.TGD3ATableAdapter tgd3ATA;
         tgd3ATA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.TGD3ATableAdapter();
         tgd3ATA.Fill(TGD3A);
       }

      public void InitTableTGD3B()
       {
         GroundwaterMonitorDataSetTableAdapters.TGD3BTableAdapter tgd3BTA;
         tgd3BTA = new GMonGr.GroundwaterMonitorDataSetTableAdapters.TGD3BTableAdapter();
         tgd3BTA.Fill(TGD3B);
       }
    }
}

namespace GMonGr.GroundwaterMonitorDataSetTableAdapters
{
  public partial class GW_MONITORINGTableAdapter
  {
    public int FillGwMonDataBySensorName(GroundwaterMonitorDataSet.GW_MONITORINGDataTable gwMonDataTable, IEnumerable<string> sensorName)
    {
      string originalCommandText =  CommandCollection[0].CommandText;
      int returnValue = 0;
      try
      {
        this.CommandCollection[0].CommandText += Utilities.CreateGwMonWhereClause(sensorName);
        returnValue = this.Fill(gwMonDataTable);
      }
      
      finally
      {
        this.CommandCollection[0].CommandText = originalCommandText;
      }
      return returnValue;
    }
  }

  public partial class MONITOR_LOCATIONSTableAdapter
  {
    public int FillMonLocationBySensorName(GroundwaterMonitorDataSet.MONITOR_LOCATIONSDataTable monLocationDataTable, IEnumerable<string> sensorName)
    {
      string originalCommandText = CommandCollection[0].CommandText;
      int returnValue = 0;
      try
      {
        this.CommandCollection[0].CommandText += Utilities.CreateMonLocWhereClause(sensorName);
        returnValue = this.Fill(monLocationDataTable);

      }
      finally
      {
        this.CommandCollection[0].CommandText = originalCommandText;
      }
      return returnValue;
    }
  }

  internal static class Utilities
  {
    internal static string CreateGwMonWhereClause(IEnumerable<string> sensName)
    {
      string whereClause;
      whereClause = " WHERE sensor_name IN ('";
      string delimeter = String.Empty;
      foreach (string sensorname in sensName)
      {
        whereClause += delimeter + sensorname;
        delimeter = ",";
      }
      whereClause += "')";
      return whereClause;
    }

    internal static string CreateMonLocWhereClause(IEnumerable<string> sensName)
    {
      string whereClause;
      whereClause = " WHERE sensor_name IN ('";
      string delimeter = String.Empty;
      foreach (string sensorname in sensName)
      {
        whereClause += delimeter + sensorname;
        delimeter = ",";
      }
      whereClause += "')";
      return whereClause;
    }
  }
}