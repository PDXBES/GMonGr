namespace GMonGr {
    
    
    public partial class NeptuneDataSet 
    {
      partial class GlencoeRainDataTable
      {

      }

      public void InitGlencoeRainTable()
      {
        NeptuneDataSetTableAdapters.GlencoeRainTableAdapter glencoeRainTA;
        glencoeRainTA = new NeptuneDataSetTableAdapters.GlencoeRainTableAdapter();
        glencoeRainTA.Fill(tableGlencoeRain);
      }
    }
}
