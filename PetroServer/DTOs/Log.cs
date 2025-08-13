public class LogResponse
{
    public required string Name { get; set; } = "";
    public required string FuelName { get; set; } = "";
    public required float TotalLiters { get; set; } = -1.0f;
    public required int Price { get; set; } = -1;
    public required int TotalAmount { get; set; } = -1;
    public int? LogType { get; set; } = -1;
    public string LogTypeName
    {
        get
        {
            return LogType switch
            {
                1 => "Bán lẻ",
                2 => "Công nợ",
                3 => "Khuyến mãi",
                4 => "Trả trước",
                _ => "Không xác định"
            };
        }
    }
    public required DateTime Time { get; set; }
}

//-----------------------------------------------
public class GetDispenserResponse
{
    public required int StationId { get; set; } = -1;
    public required int Name { get; set; } = -1;
}

public class GetFuelResponse
{
    public required int StationId { get; set; } = -1;
    public required string FuelName { get; set; } = "";
}

public class GetLogTypeResponse
{
    public required int StationId { get; set; } = -1;
    public required int LogType { get; set; } = -1;
}
public class GetPeriodResponse
{
    public required int StationId { get; set; } = -1;
    public required DateTime FromDate { get; set; }
    public required DateTime ToDate { get; set; }
}
public class GetPriceLiterResponse
{
    public required int StationId { get; set; } = -1;
    public required int From { get; set; } = -1;
    public required int To { get; set; } = -1;
}

//========================================================
public class GetDispenerFuelResponse
{
    public required int StationId { get; set; } = -1;
    public required int Name { get; set; } = -1;
    public required string FuelName { get; set; } = "";
}
public class GetDispenerLogResponse
{
    public required int StationId { get; set; } = -1;
    public required int Name { get; set; } = -1;
    public required int LogType { get; set; } = -1;
}
public class GetPeriodDispenserResponse : GetPeriodResponse
{
    public required int Name { get; set; } = -1;
}

public class GetPeriodFuelResponse : GetPeriodResponse
{
    public required string FuelName { get; set; } = "";
}
public class GetPeriodLogResponse : GetPeriodResponse
{
    public required int LogType { get; set; } = -1;
}
public class GetFuelLogResponse
{
    public required int StationId { get; set; } = -1;
    public required string FuelName { get; set; } = "";
    public required int LogType { get; set; } = -1; 
}
//========================================================
public class GetPeriodDispenerFuelResponse : GetPeriodResponse
{
    public required int Name { get; set; } = -1;
    public required string FuelName { get; set; } = "";
}

public class GetPeriodDispenerLogResponse : GetPeriodResponse
{
    public required int Name { get; set; } = -1;
    public required int LogType { get; set; } = -1;
}

public class GetPeriodFuelLogResponse : GetPeriodResponse
{
    public required String FuelName { get; set; } = "";
    public required int LogType { get; set; } = -1;
}

public class GetDipenserFuelLogResponse
{
    public required int StationId { get; set; } = -1;
    public required String FuelName { get; set; } = "";
    public required int LogType { get; set; } = -1;
    public required int Name { get; set; } = -1;
}

public class GetFullConditionResponse : GetPeriodResponse
{
    public required string FuelName { get; set; } = "";
    public required int LogType { get; set; } = -1;
    public required int Name { get; set; } = -1;
}
public class GetDipenserPriceLitterAmountResponse : GetPriceLiterResponse
{
    public required int Name { get; set; } = -1;
}
public class GetFuelPriceLitterAmountResponse : GetPriceLiterResponse
{
    public required string FuelName { get; set; } = "";
}
public class GeLogTypePriceLitterAmountResponse : GetPriceLiterResponse
{
    public required int LogType { get; set; } = -1;
}

public class GetPeriodPriceLitterAmountResponse : GetPriceLiterResponse
{
    public required DateTime FromDate { get; set; }
    public required DateTime ToDate { get; set; }
}

public class GetPriceLitterAmountResponse : GetPriceLiterResponse
{
    public required int FromTotal { get; set; } = -1;
    public required int ToTotal { get; set; } = -1;
}
//========================================================
public class GetDipenserFuelPriceResponse : GetPriceLiterResponse
{
    public required int Name { get; set; } = -1;
    public required string FuelName { get; set; } = "";
}

public class GetDipenserLogPriceResponse : GetPriceLiterResponse
{
    public required int Name { get; set; } = -1;
    public required int LogType { get; set; } = -1;
}

public class GetPeriodDipenserPriceResponse : GetPeriodDispenserResponse
{
    public required int To { get; set; } = -1;
    public required int From { get; set; } = -1;
}

public class GetDipenserPriceTotalResponse : GetPriceLiterResponse
{
    public required int Name { get; set; } = -1;
    public required int ToTotal { get; set; } = -1;
    public required int FromTotal { get; set; } = -1;
}
public class GetFuelLogPriceResponse : GetPriceLiterResponse
{
    public required string FuelName { get; set; } = "";
    public required int LogType { get; set; } = -1;
}

public class GetPeriodFuelPriceResponse : GetPeriodFuelResponse
{
    public required int To { get; set; } = -1;
    public required int From { get; set; } = -1;
}
public class GetFuelPriceTotalResponse : GetFuelPriceLitterAmountResponse
{
    public required int ToTotal { get; set; } = -1;
    public required int FromTotal { get; set; } = -1;
}
public class GetPeriodLogPriceResponse : GetPeriodLogResponse
{
    public required int To { get; set; } = -1;
    public required int From { get; set; } = -1;
}
public class GetLogPriceTotalResponse : GetPriceLiterResponse
{
    public required int LogType { get; set; } = -1;
    public required int ToTotal { get; set; } = -1;
    public required int FromTotal { get; set; } = -1;
}

public class GetPeriodPriceTotalResponse : GetPeriodPriceLitterAmountResponse
{
    public required int ToTotal { get; set; } = -1;
    public required int FromTotal { get; set; } = -1;
}
public class GetPriceAmountLitersResponse : GetPriceLitterAmountResponse
{
    public required int ToAmount { get; set; } = -1;
    public required int FromAmount { get; set; } = -1;
}