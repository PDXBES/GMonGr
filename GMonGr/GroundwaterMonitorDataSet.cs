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
    partial class SESSIONDataTable
    {
    }
  
    partial class GW_MONITOR_READINGDataTable
    {
    }
  
    partial class MonitorListDataTable
    {
    }

    partial class GwMonQcDataTable
    {
    }

    partial class MONITOR_LOCATIONSDataTable
    {
    }

    partial class GwMonUpdaterDataTable
    {

    }
  }
    
}

namespace GMonGr.GroundwaterMonitorDataSetTableAdapters
{
  public partial class GW_MONITOR_READINGTableAdapter
  {
    public int FillGwMonDataBySensorName(GroundwaterMonitorDataSet.GW_MONITOR_READINGDataTable gwMonDataTable, IEnumerable<string> sensorName)
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