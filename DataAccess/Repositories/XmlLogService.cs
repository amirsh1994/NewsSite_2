using DataAccess.Services;
using System.Data;
using System.Xml;

namespace DataAccess.Repositories;

public class XmlLogService:IXmlLogService
{
    private readonly string _path = Path.Combine(Environment.CurrentDirectory, "XML.log", "myLog.xml");

    

    public Task GenerateXmlLog(XmlModel model)
    {
        var directory = Path.GetDirectoryName(_path);
        if (Directory.Exists(directory)==false && directory!=null)
        {
            Directory.CreateDirectory(directory);
        }
        if (File.Exists(_path) == false)
        {
            var ds = BuildLog(model);
            ds.WriteXml(_path);
        }
        else
        {
            var ds = new DataSet();
            ds.ReadXml(_path);
            var dataRow = ds.Tables["Errors"]?.NewRow();
            dataRow["Id"] = model.Id;
            dataRow["ErrorDate"] = model.Time;
            dataRow["ErrorMessage"] = model.Exception;
            ds.Tables["Errors"]?.Rows.Add(dataRow);
            ds.WriteXml(_path);

        }
        return Task.CompletedTask;
    }

    private DataSet BuildLog(XmlModel model)
    {
        var ds = new DataSet("ErrroList");
        var dtb = new DataTable("Errors");
        var dc = new DataColumn("Id", typeof(int));
        dtb.Columns.Add(dc);
        dc = new DataColumn("ErrorDate", typeof(DateTime));
        dtb.Columns.Add(dc);
        dc = new DataColumn("ErrorMessage", typeof(string));
        dtb.Columns.Add(dc);
        ds.Tables.Add(dtb);
        var dataRow = ds.Tables["Errors"]?.NewRow();
        dataRow["Id"] = model.Id;
        dataRow["ErrorDate"] = model.Time;
        dataRow["ErrorMessage"] = model.Exception;
        ds.Tables["Errors"]?.Rows.Add(dataRow);
        return ds;
    }

   

    
}

public class XmlModel
{
    public string Exception { get; set; } = "";

    public int Id { get; set; }

    public DateTime Time { get; set; } = DateTime.Now;
}