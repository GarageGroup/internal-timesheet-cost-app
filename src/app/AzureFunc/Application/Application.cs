using System;
using GarageGroup.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrimeFuncPack;

namespace GarageGroup.Internal.Timesheet;

internal static partial class Application
{
    private const string DataverseSectionName = "Dataverse";

    private static Dependency<IDataverseApiClient> UseDataverseApi()
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging("DataverseApi")
        .UseTokenCredentialStandard()
        .UsePollyStandard()
        .UseDataverseApiClient(DataverseSectionName);

    private static Dependency<IHttpApi> UseDataverseHttpApi()
        =>
        PrimaryHandler.UseStandardSocketsHttpHandler()
        .UseLogging("DataverseHttpApi")
        .UseTokenCredentialStandard()
        .UsePollyStandard()
        .UseHttpApi(ResolveDataverseHttpApiOption);

    private static HttpApiOption ResolveDataverseHttpApiOption(IServiceProvider serviceProvider)
    {
        var option = serviceProvider.GetRequiredService<IConfiguration>().GetDataverseApiClientOption(DataverseSectionName);

        return new()
        {
            BaseAddress = new(option.ServiceUrl),
            Timeout = option.HttpTimeOut
        };
    }

    private static Dependency<ISqlApi> UseSqlApi()
        =>
        DataverseDbProvider.Configure("Dataverse").UseSqlApi();
}