namespace Employee_Leave_Management_System.Model;

public static class Departments
{
    public const string Research = "Research";
    public const string Engineering = "Engineering";
    public const string Product = "Product";
    public const string Design = "Design";
    public const string QualityAssurance = "Quality Assurance";
    public const string InformationTechnology = "Information Technology";
    public const string DataAnalytics = "Data Analytics";
    public const string BusinessDevelopment = "Business Development";
    public const string Marketing = "Marketing";
    public const string CustomerSupport = "Customer Support";
    public const string HumanResource = "Human Resource";
    public const string Finance = "Finance";
    public const string Compliance = "Compliance";
    public const string Operations = "Operations";
    
    public static readonly List<string> ValidDepartments =
    [
        Research,
        Engineering,
        Product,
        Design,
        QualityAssurance,
        InformationTechnology,
        DataAnalytics,
        BusinessDevelopment,
        Marketing,
        CustomerSupport,
        HumanResource,
        Finance,
        Compliance,
        Operations
    ];
}
