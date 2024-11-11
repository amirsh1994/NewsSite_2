using System.Xml;
using DataAccess.Repositories;

namespace DataAccess.Services;

public interface IXmlLogService
{
    Task GenerateXmlLog(XmlModel model);
}