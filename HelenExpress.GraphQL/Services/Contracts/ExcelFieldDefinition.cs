using System;

namespace HelenExpress.GraphQL.Services.Contracts
{
    public class ExcelFieldDefinition
    {
        public string PropName { get; set; }
        public string DisplayText { get; set; }
        public Type FieldType { get; set; }
    }
}