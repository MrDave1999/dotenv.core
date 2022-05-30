namespace DotEnv.Core.Tests.Validator;

public class RequiredKeys
{
    private string ignore_2;
    public int IGNORE_1 { get; set; }
    public string SAMC_KEY { get; }
    public string API_KEY { get; }
    public string JWT_TOKEN { get; }
    public string JWT_TOKEN_ID { get; }
    public string SERVICE_ID { get; }
    public double IGNORE_2 { get; }
    public string IGNORE_3 { set => ignore_2 = value; }
}
