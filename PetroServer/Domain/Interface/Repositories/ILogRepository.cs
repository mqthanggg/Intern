using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface ILogRepository : IRepository<Log>
{
    Task<IReadOnlyList<LogResponse>> GetLogByStationIdAsync(Station entity);
    Task<int> UpdateLogTimeAsync(LogResponse entity);
    Task<IReadOnlyList<LogResponse>> GetFullLogByStationIdAsync(Station entity);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetPageLogByStationIdAsync(Station entity, int page, int pageSize);
    //========================= 1 CONDITION =========================
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserNameAsync(GetDispenserResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypeAsync(GetLogTypeResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelNameAsync(GetFuelResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodAsync(GetPeriodResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPriceAsync(GetPriceLiterResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByTotalAmountAsync(GetPriceLiterResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByTotalLiterAsync(GetPriceLiterResponse entity, int page, int pageSize);

    //========================= 2 CONDITIONS =========================
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenerFuelAsync(GetDispenerFuelResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenerLogAsync(GetDispenerLogResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerAsync(GetPeriodDispenserResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodFuelAsync(GetPeriodFuelResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogAsync(GetPeriodLogResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelLogAsync(GetFuelLogResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserPriceAsync(GetDipenserPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserTotalLiterAsync(GetDipenserPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserTotalAmountAsync(GetDipenserPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelPriceAsync(GetFuelPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelNameTotalLiterAsync(GetFuelPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelNameTotalAmountAsync(GetFuelPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypePriceAsync(GeLogTypePriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypeTotalLiterAsync(GeLogTypePriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogTypeTotalAmountAsync(GeLogTypePriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodPriceAsync(GetPeriodPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodTotalLiterAsync(GetPeriodPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodTotalAmountAsync(GetPeriodPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPriceTotalLiterAsync(GetPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPriceTotalAmountAsync(GetPriceLitterAmountResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByTotalLitersTotalAmountAsync(GetPriceLitterAmountResponse entity, int page, int pageSize);
    //========================= 3 CONDITIONS =========================
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerFuelAsync(GetPeriodDispenerFuelResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerLogAsync(GetPeriodDispenerLogResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogFuelAsync(GetPeriodFuelLogResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserFuelLogAsync(GetDipenserFuelLogResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserFuelPriceAsync(GetDipenserFuelPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserFuelAmountAsync(GetDipenserFuelPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserFuelLitersAsync(GetDipenserFuelPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserLogPriceAsync(GetDipenserLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserLogAmountAsync(GetDipenserLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetDispenserLogLitersAsync(GetDipenserLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerPriceAsync(GetPeriodDipenserPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerLitersAsync(GetPeriodDipenserPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodDispenerAmountAsync(GetPeriodDipenserPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserPriceAmountAsync(GetDipenserPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserPriceLitersAsync(GetDipenserPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByDispenserAmountLitersAsync(GetDipenserPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelLogPriceAsync(GetFuelLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelLogAmountAsync(GetFuelLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelLogLitersAsync(GetFuelLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodFuelPriceAsync(GetPeriodFuelPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodFuelAmountAsync(GetPeriodFuelPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodFuelLiterAsync(GetPeriodFuelPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogFuelPriceLiterAsync(GetFuelPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelPriceAmountAsync(GetFuelPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByFuelAmountLiterAsync(GetFuelPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogPriceAsync(GetPeriodLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogAmountAsync(GetPeriodLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodLogLitersAsync(GetPeriodLogPriceResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogPriceAmountAsync(GetLogPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogPriceLitersAsync(GetLogPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByLogAmountLitersAsync(GetLogPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodPriceAmountAsync(GetPeriodPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodPriceLitersAsync(GetPeriodPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPeriodAmountLitersAsync(GetPeriodPriceTotalResponse entity, int page, int pageSize);
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetLogByPriceAmountLitersAsync(GetPriceAmountLitersResponse entity, int page, int pageSize);
    //========================= 4 CONDITION =========================
    Task<(IReadOnlyList<LogResponse> Logs, int TotalCount)> GetFullConditionAsync(GetFullConditionResponse entity, int page, int pageSize);
}
