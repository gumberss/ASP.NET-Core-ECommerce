using System;

namespace Catalog.API.Settings
{
    public interface ICatalogDatabaseSettings
    {
        public String ConnectionString { get; set; }
        public String DatabaseName { get; set; }
        public String CollectionName { get; set; }
    }
}
