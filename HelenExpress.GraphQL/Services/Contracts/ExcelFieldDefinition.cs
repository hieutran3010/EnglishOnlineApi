using System;

namespace HelenExpress.GraphQL.Services.Contracts
{
    public class ExcelFieldDefinition
    {
        public ExcelFieldDefinition(string displayName)
        {
            this.DisplayText = displayName;
        }

        public string DisplayText { get; set; }
        public Type FieldType { get; set; }
    }
}